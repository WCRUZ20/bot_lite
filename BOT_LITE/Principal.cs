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
        private string nombre_empresa = "KARCHER";
        private string usuario = "0992793015001";
        private string ciAdicional = "0927098244";
        private string password = "Karcher2024*";
        private string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"DescargasBOT");
        private string _connectionString = "Server=localhost;Database=Prueba;User Id=sa;Password=B1Admin;";

        public Principal()
        {
            InitializeComponent();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            CargarClientes();
        }

        private async void btnProceso_Click(object sender, EventArgs e)
        {
            btnProceso.Enabled = false;

            try
            {
                await EjecutarProcesoAsync(url, false, usuario, ciAdicional, password, nombre_empresa);
                MessageBox.Show("Proceso finalizado correctamente", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnProceso.Enabled = true;
            }
        }

        private async Task EjecutarProcesoAsync(string pageUrl, bool headless, string usuario, string ciAdicional, string password, string _nombre)
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

                var parametros = new ParametrosConsulta
                {
                    Anio = "2026",
                    Mes = "1",
                    Dia = "0",
                    Tipo = CatalogoComprobantes.ObtenerPorValue("1")
                };

                // 1️⃣ LOGIN
                await IniciarSesionAsync(session, actions, usuario, ciAdicional, password, _nombre);

                // 2️⃣ CONSULTA + DESCARGA
                await EjecutarConsultaYDescargaAsync(session, actions, parametros, usuario, _nombre);
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

        private async Task EjecutarConsultaYDescargaAsync(BrowserSession session, PageActions actions, ParametrosConsulta parametros, string usuario, string nombreUsuario)
        {
            // SETEAR COMBOS
            await actions.SelectAsync("#frmPrincipal\\:ano", parametros.Anio);
            await actions.SelectAsync("#frmPrincipal\\:mes", parametros.Mes);
            await actions.SelectAsync("#frmPrincipal\\:dia", parametros.Dia);
            await actions.SelectAsync("#frmPrincipal\\:cmbTipoComprobante", parametros.Tipo.Value);

            // CONSULTAR
            await actions.ClickAsync("#frmPrincipal\\:btnBuscar");

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

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarClientes();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {

        }
    }
}
