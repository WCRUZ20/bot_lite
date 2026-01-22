using BOT_LITE.Estilos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOT_LITE.Interfaces
{
    public partial class ResultadoProcesoForm : Form
    {
        private const string ColId = "Id";
        private const string ColUsuario = "Usuario";
        private const string ColNombre = "Nombre";
        private const string ColAnio = "Anio";
        private const string ColMes = "Mes";
        private const string ColDocumento = "Documento";
        private const string ColEstado = "Estado";
        private const string ColAccion = "Accion";

        private readonly Func<ResultadoProcesoItem, Task<EstadoResultado>> _reintentarAsync;
        private readonly Dictionary<Guid, ResultadoProcesoItem> _items;
        private readonly Dictionary<Guid, DataRow> _rows;
        private readonly DataTable _tabla;
        private readonly DataView _vista;

        public ResultadoProcesoForm(IEnumerable<ResultadoProcesoItem> resultados, Func<ResultadoProcesoItem, Task<EstadoResultado>> reintentarAsync)
        {
            InitializeComponent();
            _reintentarAsync = reintentarAsync ?? throw new ArgumentNullException(nameof(reintentarAsync));
            _items = resultados?.ToDictionary(item => item.Id) ?? throw new ArgumentNullException(nameof(resultados));
            _rows = new Dictionary<Guid, DataRow>();
            _tabla = CrearTabla();

            foreach (var item in _items.Values)
            {
                AgregarFila(item);
            }

            _vista = new DataView(_tabla);
            ConfigurarGrid();
            ConfigurarFiltros();
            AplicarFiltro();
        }

        private void ConfigurarGrid()
        {
            dtgResultados.AutoGenerateColumns = false;
            dtgResultados.AllowUserToAddRows = false;
            dtgResultados.AllowUserToDeleteRows = false;
            dtgResultados.AllowUserToResizeRows = false;
            dtgResultados.MultiSelect = false;
            dtgResultados.ReadOnly = true;
            dtgResultados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtgResultados.EditMode = DataGridViewEditMode.EditProgrammatically;
            dtgResultados.BackgroundColor = Color.White;
            dtgResultados.BorderStyle = BorderStyle.None;
            dtgResultados.RowHeadersVisible = false;
            dtgResultados.DoubleBuffered(true);

            dtgResultados.EnableHeadersVisualStyles = false;
            dtgResultados.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 37, 41);
            dtgResultados.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgResultados.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dtgResultados.ColumnHeadersHeight = 32;

            dtgResultados.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dtgResultados.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 225, 245);
            dtgResultados.RowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dtgResultados.Columns.Clear();
            dtgResultados.Columns.Add(CrearTextoColumna(ColUsuario, "Usuario", 110));
            dtgResultados.Columns.Add(CrearTextoColumna(ColNombre, "Nombre", 170));
            dtgResultados.Columns.Add(CrearTextoColumna(ColAnio, "Año", 60));
            dtgResultados.Columns.Add(CrearTextoColumna(ColMes, "Mes", 55));
            dtgResultados.Columns.Add(CrearTextoColumna(ColDocumento, "Documento", 120));
            dtgResultados.Columns.Add(CrearTextoColumna(ColEstado, "Estado", 90));

            var accion = new DataGridViewButtonColumn
            {
                Name = ColAccion,
                HeaderText = "Acción",
                DataPropertyName = ColAccion,
                UseColumnTextForButtonValue = false,
                Width = 110,
                FlatStyle = FlatStyle.Flat
            };
            dtgResultados.Columns.Add(accion);

            var oculto = new DataGridViewTextBoxColumn
            {
                Name = ColId,
                DataPropertyName = ColId,
                Visible = false
            };
            dtgResultados.Columns.Add(oculto);

            dtgResultados.DataSource = _vista;
        }

        private static DataGridViewTextBoxColumn CrearTextoColumna(string nombre, string header, int width)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = nombre,
                HeaderText = header,
                DataPropertyName = nombre,
                Width = width,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
        }

        private void ConfigurarFiltros()
        {
            chkExitoso.Checked = true;
            chkFallido.Checked = true;
            chkSinDatos.Checked = true;

            chkExitoso.CheckedChanged += (_, __) => AplicarFiltro();
            chkFallido.CheckedChanged += (_, __) => AplicarFiltro();
            chkSinDatos.CheckedChanged += (_, __) => AplicarFiltro();
            btnReintentarFallidos.Click += btnReintentarFallidos_Click;
            dtgResultados.CellFormatting += dtgResultados_CellFormatting;
            dtgResultados.CellContentClick += dtgResultados_CellContentClick;
        }

        private DataTable CrearTabla()
        {
            var tabla = new DataTable();
            tabla.Columns.Add(ColId, typeof(Guid));
            tabla.Columns.Add(ColUsuario, typeof(string));
            tabla.Columns.Add(ColNombre, typeof(string));
            tabla.Columns.Add(ColAnio, typeof(string));
            tabla.Columns.Add(ColMes, typeof(string));
            tabla.Columns.Add(ColDocumento, typeof(string));
            tabla.Columns.Add(ColEstado, typeof(string));
            tabla.Columns.Add(ColAccion, typeof(string));
            return tabla;
        }

        private void AgregarFila(ResultadoProcesoItem item)
        {
            var row = _tabla.NewRow();
            row[ColId] = item.Id;
            row[ColUsuario] = item.Usuario;
            row[ColNombre] = item.Nombre;
            row[ColAnio] = item.Anio;
            row[ColMes] = item.Mes;
            row[ColDocumento] = item.Documento;
            row[ColEstado] = ObtenerTextoEstado(item.Estado);
            row[ColAccion] = item.Estado == EstadoResultado.Fallido ? "Reintentar" : string.Empty;
            _tabla.Rows.Add(row);
            _rows[item.Id] = row;
        }

        private void AplicarFiltro()
        {
            var estados = new List<string>();
            if (chkExitoso.Checked)
                estados.Add("'Exitoso'");
            if (chkFallido.Checked)
                estados.Add("'Fallido'");
            if (chkSinDatos.Checked)
                estados.Add("'Sin datos'");

            _vista.RowFilter = estados.Count == 0
                ? "1=0"
                : $"{ColEstado} IN ({string.Join(",", estados)})";
        }

        private void dtgResultados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtgResultados.Columns[e.ColumnIndex].Name != ColEstado)
                return;

            var estado = e.Value?.ToString();
            if (string.Equals(estado, "Exitoso", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.BackColor = Color.FromArgb(212, 237, 218);
                e.CellStyle.ForeColor = Color.FromArgb(21, 87, 36);
            }
            else if (string.Equals(estado, "Fallido", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.BackColor = Color.FromArgb(248, 215, 218);
                e.CellStyle.ForeColor = Color.FromArgb(114, 28, 36);
            }
            else if (string.Equals(estado, "Sin datos", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 243, 205);
                e.CellStyle.ForeColor = Color.FromArgb(133, 100, 4);
            }
        }

        private async void dtgResultados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dtgResultados.Columns[e.ColumnIndex].Name != ColAccion)
                return;

            var rowView = dtgResultados.Rows[e.RowIndex].DataBoundItem as DataRowView;
            var id = ObtenerId(rowView);
            if (id == Guid.Empty || !_items.TryGetValue(id, out var item))
                return;

            if (item.Estado != EstadoResultado.Fallido)
                return;

            await ReintentarItemAsync(item);
        }

        private async void btnReintentarFallidos_Click(object sender, EventArgs e)
        {
            btnReintentarFallidos.Enabled = false;
            try
            {
                var fallidos = _items.Values.Where(item => item.Estado == EstadoResultado.Fallido).ToList();
                foreach (var item in fallidos)
                {
                    await ReintentarItemAsync(item);
                }
            }
            finally
            {
                btnReintentarFallidos.Enabled = true;
            }
        }

        private async Task ReintentarItemAsync(ResultadoProcesoItem item)
        {
            var estado = await _reintentarAsync(item);
            item.Estado = estado;
            ActualizarFila(item);
        }

        private void ActualizarFila(ResultadoProcesoItem item)
        {
            if (!_rows.TryGetValue(item.Id, out var row))
                return;

            row[ColEstado] = ObtenerTextoEstado(item.Estado);
            row[ColAccion] = item.Estado == EstadoResultado.Fallido ? "Reintentar" : string.Empty;
        }

        private Guid ObtenerId(DataRowView rowView)
        {
            if (rowView == null)
                return Guid.Empty;

            return rowView.Row.Field<Guid>(ColId);
        }

        private static string ObtenerTextoEstado(EstadoResultado estado)
        {
            return estado == EstadoResultado.SinDatos ? "Sin datos" : estado.ToString();
        }
    }
}
