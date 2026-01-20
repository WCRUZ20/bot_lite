using BOT_LITE.Automation;
using BOT_LITE.Automation.Models;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BOT_LITE
{
    public partial class Principal : Form
    {
        private string url = "https://srienlinea.sri.gob.ec/tuportal-internet/accederAplicacion.jspa?redireccion=57&idGrupo=55";
        //private string nombre_empresa = "KARCHER";
        //private string usuario = "0992793015001";
        //private string ciAdicional = "0927098244";
        //private string password = "Karcher2024*";
        private string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"DescargasBOT");
        private string _connectionString = "Server=localhost;Database=Prueba;User Id=sa;Password=B1Admin;";

        private DateTime? _ultimaEjecucionAutomatica;
        private bool _procesoEnCurso;

        private const int MaxReintentos = 3;

        private enum ResultadoConsulta
        {
            Descargado,
            SinDatos
        }

        public Principal()
        {
            InitializeComponent();
            InicializarProgramacion();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            CargarClientes();
        }

        private async void btnProceso_Click(object sender, EventArgs e)
        {
            await EjecutarProcesoManualAsync();
        }

        private async Task EjecutarProcesoConReintentosAsync(string pageUrl, bool headless, string usuario, string ciAdicional, string password, string nombre, ParametrosConsulta parametros)
        {
            for (int intento = 1; intento <= MaxReintentos; intento++)
            {
                try
                {
                    var resultado = await EjecutarProcesoAsync(pageUrl, headless, usuario, ciAdicional, password, nombre, parametros);
                    if (resultado == ResultadoConsulta.SinDatos)
                        return;

                    return;
                }
                catch
                {
                    if (intento == MaxReintentos)
                        throw;
                }
            }
        }

        private async Task<ResultadoConsulta> EjecutarProcesoAsync(string pageUrl, bool headless, string usuario, string ciAdicional, string password, string nombre, ParametrosConsulta parametros)
        {
            PlaywrightManager manager = null;
            BrowserSession session = null;

            try
            {
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
                15000
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

            bool sinDatos = await WaitHelper.ExistsAsync(
                session.Page,
                "text=No existen datos",
                3000
            );

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

                dtgClientes.AutoGenerateColumns = false;
                dtgClientes.Columns.Clear();

                foreach (DataColumn col in dt.Columns)
                {
                    // Detectar columnas Y / N
                    if (EsColumnaYN(dt, col.ColumnName))
                    {
                        DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn
                        {
                            Name = col.ColumnName,
                            HeaderText = col.ColumnName,
                            DataPropertyName = col.ColumnName,
                            TrueValue = "Y",
                            FalseValue = "N",
                            IndeterminateValue = "N",
                            Width = 70
                        };

                        dtgClientes.Columns.Add(chk);
                    }
                    else
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
                }

                dtgClientes.DataSource = dt;
                dtgClientes.AllowUserToAddRows = false;
                dtgClientes.ReadOnly = false;
                dtgClientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private bool EsColumnaYN(DataTable dt, string columnName)
        {
            foreach (DataRow row in dt.Rows)
            {
                var val = row[columnName]?.ToString();
                if (!string.IsNullOrEmpty(val) && val != "Y" && val != "N")
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
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {

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
            btnProceso.Enabled = false;

            try
            {
                var dt = dtgClientes.DataSource as DataTable;
                if (dt == null)
                    throw new InvalidOperationException("No hay clientes cargados para procesar.");

                foreach (DataRow row in dt.Rows)
                {
                    if (!EsValorYN(row, "Activo"))
                        continue;

                    string usuario = row["Usuario"]?.ToString();
                    string nombre = row["NombUsuario"]?.ToString();
                    string ciAdicional = row["ciAdicional"]?.ToString();
                    string password = row["Clave SRI"]?.ToString();

                    foreach (var periodo in ConstruirPeriodosConsulta(row))
                    {
                        foreach (var tipo in ObtenerTiposHabilitados(row))
                        {
                            var parametros = new ParametrosConsulta
                            {
                                Anio = periodo.Year.ToString(),
                                Mes = periodo.Month.ToString(),
                                Dia = "0",
                                Tipo = tipo
                            };

                            await EjecutarProcesoConReintentosAsync(url, false, usuario, ciAdicional, password, nombre, parametros);
                        }
                    }
                }

                if (mostrarMensajes)
                {
                    MessageBox.Show("Proceso finalizado correctamente", "OK",
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
                btnProceso.Enabled = true;
                _procesoEnCurso = false;
            }
        }


    }
}
