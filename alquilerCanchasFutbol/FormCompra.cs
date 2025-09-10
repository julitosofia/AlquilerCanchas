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
using alquilerCanchasBLL;
using alquilerCanchasDAL;

namespace alquilerCanchasFutbol
{
    public partial class FormCompra : Form
    {
        List<DetalleCompra> carrito = new List<DetalleCompra>();
        private ProductoBLL productoBLL = new ProductoBLL();
        private CompraBLL compraBLL = new CompraBLL();
        private Usuario usuario;
        public FormCompra(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            CargarProductos();
        }
        private void CargarProductos()
        {
            try
            {
                var disponibles = productoBLL.ObtenerTodos().Where(p => p.Stock > 0).ToList();
                dgvProductos.DataSource = disponibles;
            }
            catch(Exception ep)
            {
                MessageBox.Show("Error al cargar productos: " + ep.Message);
            }

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if(dgvProductos.CurrentRow == null) return;
            Producto producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;
            int cantidad = (int)nudCantidad.Value;

            if(cantidad<=0 || cantidad > producto.Stock)
            {
                MessageBox.Show("Cantidad invalida o stock insuficiente.");
                return;
            }
            var existente = carrito.FirstOrDefault(d => d.IdProducto == producto.IdProducto);
            if(existente!=null)
            {
                if(existente.Cantidad + cantidad>producto.Stock)
                {
                    MessageBox.Show("Cantidad total excede el stock disponible.");
                    return;
                }
                existente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new DetalleCompra
                {
                    IdProducto = producto.IdProducto,
                    NombreProducto = producto.Nombre,
                    Cantidad = cantidad,
                    PrecioUnitario = producto.Precio,
                    Categoria = producto.Categoria
                });
            }
            ActualizarVistaCarrito();
        }
        private void ActualizarVistaCarrito()
        {
            dgvCarrito.DataSource = null;
            dgvCarrito.DataSource = carrito;

            decimal total = carrito.Sum(d => d.Cantidad * d.PrecioUnitario);
            lblTotal.Text = $"Total: ${total:N2}";
        }

        private void btnConfirmarCompra_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtNombreCliente.Text))
            {
                MessageBox.Show("Ingrese el nombre del cliente.");
                return;
            }
            if(carrito.Count==0)
            {
                MessageBox.Show("El carrito esta vacio.");
                return;
            }
            Compra compra = new Compra
            {
                Cliente = txtNombreCliente.Text.Trim(),
                Fecha = DateTime.Now,
                Detalles = carrito
            };
            string mensaje;
            bool ok = compraBLL.RegistrarCompra(compra, out mensaje);
            MessageBox.Show(mensaje);

            if(ok)
            {
                MostrarTicket(compra);
                carrito.Clear();
                txtNombreCliente.Clear();
                ActualizarVistaCarrito();
                CargarProductos();
            }
        }
        private void MostrarTicket(Compra compra)
        {
            StringBuilder ticket = new StringBuilder();
            ticket.AppendLine($" TICKET COMPRA");
            ticket.AppendLine($"Cliente: {compra.Cliente}");
            ticket.AppendLine($"Fecha: {compra.Fecha}");
            ticket.AppendLine("-----------------------------------");

            foreach(var item in compra.Detalles)
            {
                ticket.AppendLine($"{item.NombreProducto} x{item.Cantidad} - ${item.PrecioUnitario:N2}");
            }

            decimal total = compra.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            ticket.AppendLine("---------------------------------------");
            ticket.AppendLine($"Total: ${total:N2}");

            MessageBox.Show(ticket.ToString(),"Compra confirmada");
        }

        private void FormCompra_Load(object sender, EventArgs e)
        {

        }
    }
}
