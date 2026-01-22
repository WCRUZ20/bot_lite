using BOT_LITE.Automation;
using BOT_LITE.Automation.Models;
using BOT_LITE.Estilos;
using BOT_LITE.Interfaces;
using BOT_LITE.Licensing;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BOT_LITE
{
    public partial class Principal : Form
    {
        private const string DownloadFolderName = "DescargasBOT";
        private const string LogFolderName = "Logs";
        private string url = "https://srienlinea.sri.gob.ec/tuportal-internet/accederAplicacion.jspa?redireccion=57&idGrupo=55";
        //private string nombre_empresa = "KARCHER";
        //private string usuario = "0992793015001";
        //private string ciAdicional = "0927098244";
        //private string password = "Karcher2024*";
        private string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), DownloadFolderName);
        private string _connectionString = "Server=localhost;Database=Prueba;User Id=sa;Password=B1Admin;";
        private string supabaseurl_ = "https://hgwbwaisngbyzaatwndb.supabase.co";
        private string annonkey_ = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Imhnd2J3YWlzbmdieXphYXR3bmRiIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTcwNDIwMDYsImV4cCI6MjA3MjYxODAwNn0.WgHmnqOwKCvzezBM1n82oSpAMYCT5kNCb8cLGRMIsbk";

        private DateTime? _ultimaEjecucionAutomatica;
        private bool _procesoEnCurso;
        private CancellationTokenSource _cancellationTokenSource;
        private DateTime? _ultimaValidacionLicencia;
        private LicenseValidationResult _estadoLicencia;

        private const int MaxReintentos = 3;
        private const int MinutosCacheLicencia = 10;
        private const int NumeroHilosProceso = 4;

        private enum ResultadoConsulta
        {
            Descargado,
            SinDatos
        }

        private sealed class ClienteProcesable
        {
            public string Usuario { get; set; }
            public string Nombre { get; set; }
            public string CiAdicional { get; set; }
            public string Password { get; set; }
            public List<DateTime> Periodos { get; set; }
            public List<TipoComprobante> Tipos { get; set; }
        }

        public Principal()
        {
            InitializeComponent();
            InicializarProgramacion();
            CargarConfiguracion();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            CargarClientes();
            EstilizarGridClientes();
        }

        private void CargarConfiguracion()
        {
            var configUrl = ConfigurationManager.AppSettings["UrlSri"];
            if (!string.IsNullOrWhiteSpace(configUrl))
            {
                url = configUrl;
            }

            var configConnection = ConfigurationManager.AppSettings["SqlConnectionString"];
            if (!string.IsNullOrWhiteSpace(configConnection))
            {
                _connectionString = configConnection;
            }

            var basePath = ConfigurationManager.AppSettings["RutaArchivos"];
            if (string.IsNullOrWhiteSpace(basePath))
            {
                downloadPath = Path.Combine(basePath, DownloadFolderName);
            }

        }

        private async void btnProceso_Click(object sender, EventArgs e)
        {
            if (_procesoEnCurso)
            {
                SolicitarCancelacionProceso();
                return;
            }

            await EjecutarProcesoManualAsync();
        }

        private async Task<bool> EjecutarProcesoConReintentosAsync(string pageUrl, bool headless, string usuario, string ciAdicional, string password, string nombre, ParametrosConsulta parametros, CancellationToken cancellationToken)
        {
            Exception lastEx = null;

            for (int intento = 1; intento <= MaxReintentos; intento++)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var resultado = await EjecutarProcesoAsync(pageUrl, headless, usuario, ciAdicional, password, nombre, parametros, cancellationToken);

                    if (resultado == ResultadoConsulta.SinDatos)
                    {
                        LogCliente(usuario, nombre, $"¨Sin datos {intento}/{MaxReintentos} ({parametros.Anio}-{parametros.Mes}, {parametros.Tipo?.Value})");
                        return true; // OK: no hay datos, no es error
                    }

                    LogCliente(usuario, nombre, $"¨Descarga exitosa {intento}/{MaxReintentos} ({parametros.Anio}-{parametros.Mes}, {parametros.Tipo?.Value})");
                    return true; // OK: descargó
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    lastEx = ex;

                    LogCliente(usuario, nombre, $"Falló intento {intento}/{MaxReintentos} ({parametros.Anio}-{parametros.Mes}, {parametros.Tipo?.Value}): {ex.Message}");
                }
            }

            LogCliente(usuario, nombre, $"❌ Falló la combinación luego de {MaxReintentos} intentos: {parametros.Anio}-{parametros.Mes} Tipo={parametros.Tipo?.Value}. Error: {lastEx?.Message}");

            return false;
        }

        private async Task<ResultadoConsulta> EjecutarProcesoAsync(string pageUrl, bool headless, string usuario, string ciAdicional, string password, string nombre, ParametrosConsulta parametros, CancellationToken cancellationToken)
        {
            PlaywrightManager manager = null;
            BrowserSession session = null;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var config = new BrowserConfig
                {
                    Url = pageUrl,
                    Headless = headless
                };

                manager = new PlaywrightManager();
                await manager.InitializeAsync(config);

                session = new BrowserSession(manager.Browser, config);
                await session.StartAsync();

                var actions = new PageActions(session.Page);

                // 1️⃣ LOGIN
                await IniciarSesionAsync(session, actions, usuario, ciAdicional, password, nombre);

                cancellationToken.ThrowIfCancellationRequested();

                // 2️⃣ CONSULTA + DESCARGA
                return await EjecutarConsultaYDescargaAsync(session, actions, parametros, usuario, nombre);
            }
            finally
            {
                // 3️⃣ CERRAR SESIÓN (intento)
                if (session != null)
                {
                    try
                    {
                        await CerrarSesionAsync(session);
                    }
                    catch
                    {
                        // no romper el cierre general
                    }

                    await session.DisposeAsync();
                }

                if (manager != null)
                    await manager.DisposeAsync();
            }
        }

        private async Task IniciarSesionAsync(BrowserSession session, PageActions actions, string usuario, string ciAdicional, string password, string _nombre)
        {
            await session.Page.WaitForSelectorAsync("#usuario", new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Visible
            });

            await actions.SetTextAsync("#usuario", usuario);
            await actions.SetTextAsync("#ciAdicional", ciAdicional);
            await actions.SetTextAsync("#password", password);

            await actions.ClickAsync("#kc-login");

            // Esperas seguras SRI
            await session.Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await session.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            bool loginExitoso = await WaitHelper.ExistsAsync(
                session.Page,
                "a[tooltip='Cerrar sesión']",
                25000
            );

            if (!loginExitoso)
                throw new Exception("❌ No se detectó el botón Cerrar sesión (login fallido)");
        }

        private async Task<ResultadoConsulta> EjecutarConsultaYDescargaAsync(BrowserSession session, PageActions actions, ParametrosConsulta parametros, string usuario, string nombreUsuario)
        {
            // SETEAR COMBOS
            await actions.SelectAsync("#frmPrincipal\\:ano", parametros.Anio);
            await actions.SelectAsync("#frmPrincipal\\:mes", parametros.Mes);
            await actions.SelectAsync("#frmPrincipal\\:dia", parametros.Dia);
            await actions.SelectAsync("#frmPrincipal\\:cmbTipoComprobante", parametros.Tipo.Value);

            // CONSULTAR
            await actions.ClickAsync("#frmPrincipal\\:btnBuscar");
            // CONSULTAR (con detección y recuperación de "captcha incorrecta")
            //await ConsultarConRecuperacionCaptchaAsync(session.Page, actions, "#frmPrincipal\\:btnBuscar");

            bool sinDatos = await WaitHelper.ExistsAsync(
                session.Page,
                "text=No existen datos",
                3000
            );

            bool captchaincorrecta = await WaitHelper.ExistsAsync(
                session.Page,
                "text=Captcha incorrecta",
                3000
            );

            if (captchaincorrecta)
                await ConsultarConRecuperacionCaptchaAsync(session.Page, actions, "#frmPrincipal\\:btnBuscar");

            if (sinDatos)
                return ResultadoConsulta.SinDatos;

            bool tablaCargada = await WaitHelper.ExistsAsync(
                session.Page,
                "#frmPrincipal\\:tablaCompRecibidos",
                10000
            );

            if (!tablaCargada)
                throw new Exception("❌ No se cargó la tabla de comprobantes electrónicos");

            string rutaUsuario = PrepararRutaDescarga(downloadPath, usuario, nombreUsuario);

            // DESCARGA CONTROLADA
            var download = await session.Page.RunAndWaitForDownloadAsync(async () =>
            {
                await actions.ClickAsync("#frmPrincipal\\:lnkTxtlistado");
            });

            string extension = Path.GetExtension(download.SuggestedFilename);

            string nombreArchivo =
                $"{parametros.Tipo.PrefijoArchivo}_" +
                $"{parametros.Mes.PadLeft(2, '0')}_" +
                $"{parametros.Anio}{extension}";

            string rutaFinal = Path.Combine(rutaUsuario, nombreArchivo);

            await download.SaveAsAsync(rutaFinal);
            return ResultadoConsulta.Descargado;
        }

        private async Task CerrarSesionAsync(BrowserSession session)
        {
            bool existeCerrarSesion = await WaitHelper.ExistsAsync(
                session.Page,
                "a[tooltip='Cerrar sesión']",
                5000
            );

            if (!existeCerrarSesion)
                return;

            await session.Page.ClickAsync("a[tooltip='Cerrar sesión']");

            await session.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        private string PrepararRutaDescarga(string basePath, string usuario, string _nombre)
        {
            string userFolder = Path.Combine(basePath, $"{usuario} - {_nombre}");

            if (!Directory.Exists(userFolder))
                Directory.CreateDirectory(userFolder);

            return userFolder;
        }

        private void CargarClientes()
        {
            string sql = "select \"orden\" \"Orden\", \"usuario\" As \"Usuario\", \"NombUsuario\", \"ci_adicional\" \"ciAdicional\", \"clave\" \"Clave SRI\", \"dias\" \"Días permitidos\", \"clave_ws\" \"Clave EDOC\", \"Activo\", \"meses_ante\" \"Meses consulta\", \"factura\", \"notacredito\", \"retencion\", \"liquidacioncompra\", \"notadebito\", \"carga\", \"descarga\", \"consultar_mes_actual\" \"Consulta mes actual\" from Clientes_BOT order by \"orden\"";

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                dtgClientes.AutoGenerateColumns = true;
                dtgClientes.Columns.Clear();

                foreach (DataColumn col in dt.Columns)
                {
                    DataGridViewTextBoxColumn txt = new DataGridViewTextBoxColumn
                    {
                        Name = col.ColumnName,
                        HeaderText = col.ColumnName,
                        DataPropertyName = col.ColumnName,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    };

                    dtgClientes.Columns.Add(txt);
                }

                dtgClientes.DataSource = dt;
                //dtgClientes.AllowUserToAddRows = false;
                //dtgClientes.ReadOnly = false;
                //dtgClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            }
        }

        private bool EsColumnaYN(DataTable dt, string columnName)
        {
            foreach (DataRow row in dt.Rows)
            {
                var val = row[columnName]?.ToString();
                if (!string.Equals(val?.Trim(), "Y", StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return true;
        }

        private bool EsValorYN(DataRow row, string columnName)
        {
            var value = row[columnName]?.ToString();
            return string.Equals(value?.Trim(), "Y", StringComparison.OrdinalIgnoreCase);
        }

        private int ObtenerEntero(DataRow row, string columnName)
        {
            if (int.TryParse(row[columnName]?.ToString(), out int valor))
                return valor;

            return 0;
        }

        private IEnumerable<DateTime> ConstruirPeriodosConsulta(DataRow row)
        {
            DateTime hoy = DateTime.Today;
            DateTime inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var periodos = new List<DateTime>();

            if (EsValorYN(row, "Consulta mes actual"))
                periodos.Add(inicioMes);

            int mesesAnteriores = ObtenerEntero(row, "Meses consulta");
            int diasPermitidos = ObtenerEntero(row, "Días permitidos");

            if (mesesAnteriores > 0 && (hoy.Day <= diasPermitidos || diasPermitidos == 0))
            {
                for (int i = 1; i <= mesesAnteriores; i++)
                    periodos.Add(inicioMes.AddMonths(-i));
            }

            return periodos;
        }

        private IEnumerable<TipoComprobante> ObtenerTiposHabilitados(DataRow row)
        {
            var tipos = new Dictionary<string, string>
            {
                { "factura", "1" },
                { "liquidacioncompra", "2" },
                { "notacredito", "3" },
                { "notadebito", "4" },
                { "retencion", "6" }
            };

            foreach (var tipo in tipos)
            {
                if (!EsValorYN(row, tipo.Key))
                    continue;

                var encontrado = CatalogoComprobantes.ObtenerPorValue(tipo.Value);
                if (encontrado != null)
                    yield return encontrado;
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarClientes();
            EstilizarGridClientes();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            using (var frm = new Configuracion())
            {
                frm.ShowDialog(this); // bloquea el form padre
            }
        }

        private void InicializarProgramacion()
        {
            dtpHoraProgramada.Format = DateTimePickerFormat.Time;
            dtpHoraProgramada.ShowUpDown = true;
            timerProgramacion.Interval = 30000;
            timerProgramacion.Start();
        }

        private async void timerProgramacion_Tick(object sender, EventArgs e)
        {
            if (!chkAutoEjecutar.Checked || _procesoEnCurso)
                return;

            DateTime ahora = DateTime.Now;
            DateTime horaProgramada = dtpHoraProgramada.Value;
            DateTime objetivo = new DateTime(ahora.Year, ahora.Month, ahora.Day, horaProgramada.Hour, horaProgramada.Minute, 0);

            if (ahora < objetivo)
                return;

            if (_ultimaEjecucionAutomatica.HasValue && _ultimaEjecucionAutomatica.Value.Date == ahora.Date)
                return;

            await EjecutarProcesoAutomaticoAsync();
        }

        private async Task EjecutarProcesoManualAsync()
        {
            await EjecutarProcesoAsync(true);
        }

        private async Task EjecutarProcesoAutomaticoAsync()
        {
            await EjecutarProcesoAsync(false);
            _ultimaEjecucionAutomatica = DateTime.Now;
        }

        private async Task EjecutarProcesoAsync(bool mostrarMensajes)
        {
            if (_procesoEnCurso)
                return;

            _procesoEnCurso = true;
            _cancellationTokenSource = new CancellationTokenSource();
            ActualizarEstadoBotonProceso(true);

            try
            {
                if (!await ValidarLicenciaAsync(mostrarMensajes))
                {
                    return;
                }

                var dt = dtgClientes.DataSource as DataTable;
                if (dt == null)
                    throw new InvalidOperationException("No hay clientes cargados para procesar.");

                var clientes = ConstruirClientesProcesables(dt);
                var tareas = Enumerable.Range(0, NumeroHilosProceso)
                    .Select(indice => ProcesarClientesAsync(clientes, indice, _cancellationTokenSource.Token))
                    .ToList();

                await Task.WhenAll(tareas);

                if (mostrarMensajes)
                {
                    MessageBox.Show("Proceso finalizado correctamente", "OK",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(OperationCanceledException)
            {
                if (mostrarMensajes)
                {
                    MessageBox.Show("Proceso cancelado por el usuario.", "Cancelado",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (mostrarMensajes)
                {
                    MessageBox.Show(ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Ejecución automática fallida: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                ActualizarEstadoBotonProceso(false);
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                _procesoEnCurso = false;
            }
        }

        private List<ClienteProcesable> ConstruirClientesProcesables(DataTable dt)
        {
            var clientes = new List<ClienteProcesable>();

            foreach (DataRow row in dt.Rows)
            {
                if (!EsValorYN(row, "Activo"))
                    continue;

                clientes.Add(new ClienteProcesable
                {
                    Usuario = row["Usuario"]?.ToString(),
                    Nombre = row["NombUsuario"]?.ToString(),
                    CiAdicional = row["ciAdicional"]?.ToString(),
                    Password = row["Clave SRI"]?.ToString(),
                    Periodos = ConstruirPeriodosConsulta(row).ToList(),
                    Tipos = ObtenerTiposHabilitados(row).ToList()
                });
            }

            return clientes;
        }

        private async Task ProcesarClientesAsync(IReadOnlyList<ClienteProcesable> clientes, int indiceHilo, CancellationToken cancellationToken)
        {
            for (int i = indiceHilo; i < clientes.Count; i += NumeroHilosProceso)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var cliente = clientes[i];

                foreach (var periodo in cliente.Periodos)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    foreach (var tipo in cliente.Tipos)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var parametros = new ParametrosConsulta
                        {
                            Anio = periodo.Year.ToString(),
                            Mes = periodo.Month.ToString(),
                            Dia = "0",
                            Tipo = tipo
                        };

                        await EjecutarProcesoConReintentosAsync(url, false, cliente.Usuario, cliente.CiAdicional, cliente.Password, cliente.Nombre, parametros, cancellationToken);
                    }
                }
            }
        }

        private void SolicitarCancelacionProceso()
        {
            if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
                return;

            _cancellationTokenSource.Cancel();
        }

        private void ActualizarEstadoBotonProceso(bool enEjecucion)
        {
            if (enEjecucion)
            {
                btnProceso.BackColor = Color.Red;
                btnProceso.Text = "Cancelar";
                btnProceso.Enabled = true;
                return;
            }

            btnProceso.BackColor = Color.LimeGreen;
            btnProceso.Text = "Ejecutar";
            btnProceso.Enabled = true;
        }

        private async Task ConsultarConRecuperacionCaptchaAsync(IPage page, PageActions actions, string btnBuscarSelector)
        {
            // 1) Intento normal
            await actions.ClickAsync(btnBuscarSelector);

            // 2) Espera corta para ver si aparece "captcha incorrecta"
            //    (importante: que el DOM tenga tiempo de pintar el mensaje)
            await page.WaitForTimeoutAsync(6000);

            // 3) Si detecta mensaje de captcha incorrecta, forzar estrategias
            bool captchaIncorrecta = await ExisteCaptchaIncorrectaAsync(page, 6000);
            if (!captchaIncorrecta)
                return;

            // Estrategias más efectivas: intentos cortos con esperas, sin “romper” el flujo
            var estrategias = ConstruirEstrategiasForzadas(btnBuscarSelector);

            for (int i = 0; i < estrategias.Count; i++)
            {
                // Pequeña espera entre estrategias (deja respirar a PrimeFaces/AJAX)
                await page.WaitForTimeoutAsync(6000);

                await estrategias[i](page);

                // Esperar a que el request/DOM reaccione
                await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                await page.WaitForTimeoutAsync(9000);

                // Si ya no está el mensaje, salir
                captchaIncorrecta = await ExisteCaptchaIncorrectaAsync(page, 1200);
                if (!captchaIncorrecta)
                    return;
            }

            // Si llega aquí, sigue con captcha incorrecta:
            // NO lanzamos excepción aquí; dejamos que tu lógica global de reintentos maneje el fallo si aplica
        }

        private async Task<bool> ExisteCaptchaIncorrectaAsync(IPage page, int timeoutMs)
        {
            // Buscamos texto que “contenga” captcha incorrecta (case-insensitive)
            // Espera hasta timeoutMs para capturar el render del mensaje
            try
            {
                // Preferimos locator con regex por robustez (a veces hay espacios o tildes)
                var locator = page.Locator("text=/captcha\\s+incorrecta/i");
                await locator.First.WaitForAsync(new LocatorWaitForOptions
                {
                    Timeout = timeoutMs,
                    State = WaitForSelectorState.Visible
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        private List<Func<IPage, Task>> ConstruirEstrategiasForzadas(string btnBuscarSelector)
        {
            return new List<Func<IPage, Task>>
            {
                // 1) Scroll + focus + click Playwright
                async (page) =>
                {
                    await page.EvaluateAsync(@"(sel) => {
                        const el = document.querySelector(sel);
                        if (!el) return;
                        el.scrollIntoView({ block: 'center', inline: 'center' });
                        el.focus();
                    }", btnBuscarSelector);

                    await page.ClickAsync(btnBuscarSelector, new PageClickOptions { Force = true, Timeout = 5000 });
                },

                // 2) Click con JS directo
                async (page) =>
                {
                    await page.EvaluateAsync(@"(sel) => {
                        const el = document.querySelector(sel);
                        if (!el) return;
                        el.scrollIntoView({ block: 'center', inline: 'center' });
                        el.click();
                    }", btnBuscarSelector);
                },

                // 3) Disparar MouseEvents (mousedown/up/click) para forzar listeners
                async (page) =>
                {
                    await page.EvaluateAsync(@"(sel) => {
                        const el = document.querySelector(sel);
                        if (!el) return;
                        el.scrollIntoView({ block: 'center', inline: 'center' });

                        const fire = (type) => el.dispatchEvent(new MouseEvent(type, { bubbles: true, cancelable: true, view: window }));
                        fire('mousedown');
                        fire('mouseup');
                        fire('click');
                    }", btnBuscarSelector);
                },

                // 4) Enter sobre el botón (a veces dispara submit/PrimeFaces)
                async (page) =>
                {
                    await page.FocusAsync(btnBuscarSelector);
                    await page.Keyboard.PressAsync("Enter");
                },

                // 5) Submit del formulario más cercano
                async (page) =>
                {
                    await page.EvaluateAsync(@"(sel) => {
                        const el = document.querySelector(sel);
                        if (!el) return;

                        const form = el.closest('form');
                        if (!form) return;

                        // Si existe requestSubmit, es lo más “real”
                        if (typeof form.requestSubmit === 'function') {
                            form.requestSubmit();
                        } else {
                            form.submit();
                        }
                    }", btnBuscarSelector);
                },

                // 6) Intento PrimeFaces.ab si el onclick lo contiene
                async (page) =>
                {
                    await page.EvaluateAsync(@"(sel) => {
                        const el = document.querySelector(sel);
                        if (!el) return;

                        const onclick = el.getAttribute('onclick') || '';
                        if (onclick.includes('PrimeFaces.ab')) {
                            try { eval(onclick); } catch (e) {}
                        } else {
                            // fallback: click
                            el.click();
                        }
                    }", btnBuscarSelector);
                },
            };
        }

        private void LogCliente(string usuario, string nombre, string mensaje)
        {
            string logDirectory = PrepararRutaLog(downloadPath);
            string sanitizedNombre = SanitizarNombreArchivo(nombre);
            string fecha = DateTime.Now.ToString("yyyyMMdd");
            string logFile = $"{usuario}_{sanitizedNombre}_{fecha}.log";
            string rutaLog = Path.Combine(logDirectory, logFile);
            string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {mensaje}{Environment.NewLine}";
            File.AppendAllText(rutaLog, entry, Encoding.UTF8);
        }

        private string PrepararRutaLog(string basePath)
        {
            string logFolder = Path.Combine(basePath, LogFolderName);

            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            return logFolder;
        }

        private string SanitizarNombreArchivo(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return "sin_nombre";

            var invalidChars = Path.GetInvalidFileNameChars();
            var builder = new StringBuilder(nombre.Length);

            foreach (char ch in nombre)
            {
                builder.Append(invalidChars.Contains(ch) ? '_' : ch);
            }

            return builder.ToString().Trim();
        }

        private async Task<bool> ValidarLicenciaAsync(bool mostrarMensajes)
        {
            if (_ultimaValidacionLicencia.HasValue
                && _estadoLicencia != null
                && (DateTime.Now - _ultimaValidacionLicencia.Value).TotalMinutes < MinutosCacheLicencia)
            {
                return _estadoLicencia.IsValid && _estadoLicencia.IsActive;
            }

            var supabaseUrl = supabaseurl_;
            var supabaseAnonKey = annonkey_;
            var userCode = ConfigurationManager.AppSettings["LicenseUserCode"];
            var password = ConfigurationManager.AppSettings["LicensePassword"];

            if (string.IsNullOrWhiteSpace(userCode) || string.IsNullOrWhiteSpace(password))
            {
                if (mostrarMensajes)
                {
                    MessageBox.Show("Configura Supabase y la licencia en Configuración antes de ejecutar.",
                        "Licencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                return false;
            }

            try
            {
                var client = new SupabaseLicenseClient(supabaseUrl, supabaseAnonKey);
                _estadoLicencia = await client.ValidateAsync(userCode, password);
                _ultimaValidacionLicencia = DateTime.Now;

                if (!_estadoLicencia.IsValid || !_estadoLicencia.IsActive)
                {
                    if (mostrarMensajes)
                    {
                        MessageBox.Show(_estadoLicencia.Message ?? "Licencia inactiva.",
                            "Licencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (mostrarMensajes)
                {
                    MessageBox.Show($"No se pudo validar la licencia: {ex.Message}",
                        "Licencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
        }

        private void EstilizarGridClientes()
        {
            var g = dtgClientes;

            // Comportamiento / UX
            g.ReadOnly = true; // cámbialo según tu necesidad
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.AllowUserToResizeRows = false;
            g.MultiSelect = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.EditMode = DataGridViewEditMode.EditOnEnter;

            // Rendimiento / Flicker
            g.DoubleBuffered(true);

            // Layout
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            g.RowTemplate.Height = 34;
            g.ColumnHeadersHeight = 38;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Bordes y líneas (minimalista)
            g.BorderStyle = BorderStyle.None;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.GridColor = Color.FromArgb(230, 230, 230);

            // Encabezados
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 248);
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(45, 45, 45);
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            // Celdas
            g.DefaultCellStyle.BackColor = Color.White;
            g.DefaultCellStyle.ForeColor = Color.FromArgb(33, 33, 33);
            g.DefaultCellStyle.Font = new Font("Segoe UI", 8.0f, FontStyle.Regular);
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254); // azul suave
            g.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 33, 33);
            g.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            g.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            // Zebra sutil
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            // Row header (la columna gris de la izquierda) off
            g.RowHeadersVisible = false;

            // Scrollbar más limpia
            g.ScrollBars = ScrollBars.Both;

            // Si quieres que no “salte” el último header al ordenar
            g.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
        }
    }
}
