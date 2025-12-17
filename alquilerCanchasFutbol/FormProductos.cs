using alquilerCanchasBE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using alquilerCanchasBLL;
using alquilerCanchasDAL;
using System.Collections.Concurrent;


namespace alquilerCanchasFutbol
{
    public partial class FormProductos : Form
    {
        private Usuario usuario;
        private readonly ConexionDAL conexion = new ConexionDAL();


        private readonly ProductoBLL productoBLL;
        public FormProductos(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;


            var productoDAL = new ProductoDAL(conexion);


            this.productoBLL = new ProductoBLL(productoDAL);


            CargarInventario();
        }

        private void FormProductos_Load(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCategoria.Text) || !decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio))
            {
                MessageBox.Show("Todos los campos son obligatorios y deben tener formato valido.");
                return;
            }

            Producto producto = new Producto
            {
                Nombre = txtNombre.Text.Trim(),
                Precio = decimal.Parse(txtPrecio.Text.Trim()),
                Stock = (int)nudCantidad.Value,
                Categoria = txtCategoria.Text.Trim(),
            };


            string mensaje;
            bool ok = this.productoBLL.AgregarProducto(producto, out mensaje);


            MessageBox.Show(mensaje, ok ? "Éxito" : "Error");

            if (ok) CargarInventario();
        }


        private void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            if (dgvInventario.CurrentRow == null) return;
            Producto producto = (Producto)dgvInventario.CurrentRow.DataBoundItem;


            string mensaje;
            bool ok = this.productoBLL.EliminarProducto(producto.IdProducto, out mensaje);


            MessageBox.Show(mensaje, ok ? "Éxito" : "Error");

            if (ok) CargarInventario();
        }

        private void CargarInventario()
        {
            dgvInventario.DataSource = null;
            dgvInventario.DataSource = this.productoBLL.ObtenerTodos();
        }

        private void btnModificarProducto_Click(object sender, EventArgs e)
        {
            if (dgvInventario.CurrentRow == null) return;


            Producto producto = (Producto)dgvInventario.CurrentRow.DataBoundItem;


            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCategoria.Text) || !decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio))
            {
                MessageBox.Show("Todos los campos son obligatorios y deben tener formato válido para modificar.");
                return;
            }


            producto.Nombre = txtNombre.Text.Trim();
            producto.Precio = decimal.Parse(txtPrecio.Text.Trim());
            producto.Stock = (int)nudCantidad.Value;
            producto.Categoria = txtCategoria.Text.Trim();

            string mensaje;
            bool ok = this.productoBLL.ModificarProducto(producto, out mensaje);

            MessageBox.Show(mensaje, ok ? "Éxito" : "Error");

            if (ok) CargarInventario();
        }

        private void btnExportarXml_Click(object sender, EventArgs e)
        {
            string mensaje;
            bool exito = this.productoBLL.ExportarTodosAXml("productos.xml", out mensaje);

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
            var productosDesdeXml = this.productoBLL.ImportarTodosDesdeXml("productos.xml");

            if (productosDesdeXml != null && productosDesdeXml.Count > 0)
            {
                dgvInventario.DataSource = null;
                dgvInventario.DataSource = productosDesdeXml;

                MessageBox.Show(
                    $"Se han cargado {productosDesdeXml.Count} productos desde 'productos.xml' para visualización.",
                    "Importación Exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                MessageBox.Show(
                    "El archivo 'productos.xml' no existe o está vacío/corrupto. No se pudo importar.",
                    "Error/Advertencia",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}
