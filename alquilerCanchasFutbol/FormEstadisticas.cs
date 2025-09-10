using alquilerCanchasBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using alquilerCanchasBE;
using alquilerCanchasDAL;

namespace alquilerCanchasFutbol
{
    public partial class FormEstadisticas : Form
    {
        
        public FormEstadisticas()
        {
            InitializeComponent();
            CargarCompras();
        }
        private void CargarCompras()
        {
            CompraBLL compraBLL = new CompraBLL();
            var compras = compraBLL.ObtenerTodas();
            dgvCompras.DataSource = compras;
            MostrarTotal(compras);
        }

        private void FormEstadisticas_Load(object sender, EventArgs e)
        {

        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            string categoria = cmbCategoria.SelectedItem?.ToString();
            string nombre = txtNombreProducto.Text.Trim().ToLower();

            CompraBLL compraBLL = new CompraBLL();
            var compras = compraBLL.ObtenerTodas()
                .Where(c=>c.Detalles.Any(d=>(string.IsNullOrEmpty(categoria) || d.Categoria.Equals(categoria,StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(nombre) || d.NombreProducto.ToLower().Contains(nombre))))
                .ToList();
            dgvCompras.DataSource = compras;
            MostrarTotal(compras);
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            string cliente = txtNombreCliente.Text.Trim().ToLower();
            CompraBLL compraBLL =new CompraBLL();
            var compras = compraBLL.ObtenerTodas()
                .Where(c=>c.Cliente.ToLower().Contains(cliente))
                .ToList();
            dgvCompras.DataSource = compras;
            MostrarTotal(compras);
        }
        private void MostrarTotal(List<Compra>compras)
        {
            decimal total = compras.Sum(c => c.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario));
            lblTotalVentas.Text = $"Total vendido: ${total:N2}";
        }

        private void btnVerTicket_Click(object sender, EventArgs e)
        {
            if (dgvCompras.CurrentRow == null) return;
            Compra compra = (Compra)dgvCompras.CurrentRow.DataBoundItem;
            StringBuilder ticket = new StringBuilder();
            ticket.AppendLine($" TICKET DE COMPRA  ");
            ticket.AppendLine($"Cliente: {compra.Cliente}");
            ticket.AppendLine($"Fecha: {compra.Fecha}");
            ticket.AppendLine("---------------------------------");

            foreach(var item in compra.Detalles)
            {
                ticket.AppendLine($"{item.NombreProducto} x{item.Cantidad} - ${item.PrecioUnitario:N2}");
            }
            decimal total = compra.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            ticket.AppendLine("-----------------------------------------");
            ticket.AppendLine($"Total: ${total:N2}");

            MessageBox.Show(ticket.ToString(), "Detalle de compra");
        }
    }
}
