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


        private readonly ConexionDAL conn = new ConexionDAL();


        private ProductoBLL productoBLL;
        private CompraBLL compraBLL;
        private VentaBLL ventaBll;

        private Usuario usuario;
        public FormCompra(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;

            var productoDAL = new ProductoDAL(conn);
            var compraDAL = new CompraDAL(conn);

            this.productoBLL = new ProductoBLL(productoDAL);
            this.compraBLL = new CompraBLL(compraDAL, productoDAL);
            this.ventaBll = new VentaBLL();
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
            Compra nuevaCompra;

            try
            {
                nuevaCompra = ObtenerDatosDeCompraDelFormulario();
            }

            catch (InvalidOperationException validationEx)
            {
                MessageBox.Show(validationEx.Message, "Validación Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception dataEx)
            {
                MessageBox.Show($"Error al preparar datos: {dataEx.Message}", "Error de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            conn.IniciarTransaccion();

            try
            {

                string mensaje;


                bool exito = compraBLL.RegistrarCompra(nuevaCompra, out mensaje);

                if (exito)
                {

                    conn.ConfirmarTransaccion();
                    MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MostrarTicket(nuevaCompra);

                }
                else
                {

                    conn.CancelarTransaccion();
                    MessageBox.Show(mensaje, "Fallo en la Compra", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                conn.CancelarTransaccion();
                MessageBox.Show($"Ocurrió un error crítico durante el registro: {ex.Message}", "Error Crítico del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private Compra ObtenerDatosDeCompraDelFormulario()
        {

            string nombreCliente = txtNombreCliente.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombreCliente))
            {
                throw new InvalidOperationException("Debe ingresar el nombre del cliente.");
            }

            var compra = new Compra
            {
                Cliente = nombreCliente,
                Fecha = DateTime.Now,
                Detalles = new List<DetalleCompra>()
            };

            var dgvDetallesCompra = (DataGridView)this.Controls.Find("dgvCarrito", true).FirstOrDefault() ?? new DataGridView();



            foreach (DataGridViewRow fila in dgvDetallesCompra.Rows)
            {

                if (!fila.IsNewRow && fila.Cells["IdProducto"].Value != null)
                {
                    try
                    {
                        var detalle = new DetalleCompra
                        {
                            IdProducto = Convert.ToInt32(fila.Cells["IdProducto"].Value),
                            NombreProducto = fila.Cells["NombreProducto"].Value.ToString(),

                            Cantidad = Convert.ToInt32(fila.Cells["Cantidad"].Value),
                            PrecioUnitario = Convert.ToDecimal(fila.Cells["PrecioUnitario"].Value),
                            Categoria = fila.Cells["Categoria"].Value?.ToString() ?? string.Empty
                        };

                        compra.Detalles.Add(detalle);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException($"Error de datos en la fila {fila.Index + 1} del carrito: {ex.Message}");
                    }
                }
            }


            if (compra.Detalles.Count == 0)
            {
                throw new InvalidOperationException("El carrito no contiene productos. Agregue al menos un producto.");
            }


            return compra;
        }

        private void btnExportarXml_Click(object sender, EventArgs e)
        {
            string mensaje;

            bool exito = this.ventaBll.ExportarVentasAXml(out mensaje);

            if (exito)
            {
                MessageBox.Show(mensaje, "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(mensaje, "Error de Exportación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportarXml_Click(object sender, EventArgs e)
        {
            try
            {
                var ventasDesdeXml = this.ventaBll.ImportarVentasDesdeXml();

                if (ventasDesdeXml != null && ventasDesdeXml.Count > 0)
                {
                    MessageBox.Show(
                        $"Se han cargado {ventasDesdeXml.Count} registros de VENTAS desde 'ventas.xml' para verificación. (No se modifica la compra actual)",
                        "Importación Exitosa de Ventas",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                }
                else
                {
                    MessageBox.Show(
                       "El archivo 'ventas.xml' no existe, está vacío o corrupto. No se pudo importar.",
                       "Error/Advertencia de Importación",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Warning
                   );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico al importar XML: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
