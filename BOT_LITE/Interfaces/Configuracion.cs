using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOT_LITE.Interfaces
{
    public partial class Configuracion : Form
    {
        public Configuracion()
        {
            InitializeComponent();
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCamposObligatorios())
            {
                return;
            }

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;

            ActualizarSetting(settings, "SqlConnectionString", txtConn.Text?.Trim());
            ActualizarSetting(settings, "RutaArchivos", txtRuta.Text?.Trim());
            ActualizarSetting(settings, "UrlSri", txtUrlSri.Text?.Trim());
            ActualizarSetting(settings, "SupabaseUrl", txtSupabaseUrl.Text?.Trim());
            ActualizarSetting(settings, "SupabaseAnonKey", txtSupabaseKey.Text?.Trim());
            ActualizarSetting(settings, "LicenseUserCode", txtLicenseUser.Text?.Trim());
            ActualizarSetting(settings, "LicensePassword", txtLicensePassword.Text);

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Configuración guardada correctamente.", "OK",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Seleccione el directorio base para las descargas";
                dialog.ShowNewFolderButton = true;

                if (!string.IsNullOrWhiteSpace(txtRuta.Text))
                {
                    dialog.SelectedPath = txtRuta.Text;
                }

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtRuta.Text = dialog.SelectedPath;
                }
            }

        }

        private void Configuracion_Load(object sender, EventArgs e)
        {
            txtConn.Text = ConfigurationManager.AppSettings["SqlConnectionString"] ?? string.Empty;
            txtRuta.Text = ConfigurationManager.AppSettings["RutaArchivos"] ?? string.Empty;
            txtUrlSri.Text = ConfigurationManager.AppSettings["UrlSri"] ?? string.Empty;
            txtSupabaseUrl.Text = ConfigurationManager.AppSettings["SupabaseUrl"] ?? string.Empty;
            txtSupabaseKey.Text = ConfigurationManager.AppSettings["SupabaseAnonKey"] ?? string.Empty;
            txtLicenseUser.Text = ConfigurationManager.AppSettings["LicenseUserCode"] ?? string.Empty;
            txtLicensePassword.Text = ConfigurationManager.AppSettings["LicensePassword"] ?? string.Empty;

        }

        private void btnProbarConexion_Click(object sender, EventArgs e)
        {
            var connectionString = txtConn.Text?.Trim();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                MessageBox.Show("Ingrese una cadena de conexión válida para probar.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConn.Focus();
                return;
            }

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                }

                MessageBox.Show("Conexión exitosa.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo conectar: {ex.Message}", "Error de conexión",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ActualizarSetting(KeyValueConfigurationCollection settings, string key, string value)
        {
            if (settings[key] == null)
            {
                settings.Add(key, value ?? string.Empty);
                return;
            }

            settings[key].Value = value ?? string.Empty;
        }

        private bool ValidarCamposObligatorios()
        {
            if (string.IsNullOrWhiteSpace(txtConn.Text))
            {
                MessageBox.Show("La cadena de conexión es obligatoria.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConn.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRuta.Text))
            {
                MessageBox.Show("La ruta de archivos es obligatoria.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRuta.Focus();
                return false;
            }

            return true;
        }

    }
}
