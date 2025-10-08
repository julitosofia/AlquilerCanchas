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
        public FormProductos(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void FormProductos_Load(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCategoria.Text) || !decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio))
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
            ProductoBLL productoBLL = new ProductoBLL(new ProductoDAL());
            bool ok = productoBLL.AgregarProducto(producto);
            MessageBox.Show(ok ? "Producto agregado correctamente." : "Error al agregar producto.");
            if (ok) CargarInventario();
        }


        private void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            if (dgvInventario.CurrentRow == null) return;
            Producto producto = (Producto)dgvInventario.CurrentRow.DataBoundItem;

            ProductoBLL productoBLL = new ProductoBLL(new ProductoDAL());
            bool ok = productoBLL.EliminarProducto(producto.IdProducto);
            MessageBox.Show(ok ? "Producto eliminado correctamente." : "Error al elminar producto.");
            if (ok) CargarInventario();
        }

        private void CargarInventario()
        {
            ProductoBLL productoBLL = new ProductoBLL(new ProductoDAL());
            dgvInventario.DataSource = null;
            dgvInventario.DataSource = productoBLL.ObtenerTodos();
        }

        private void btnModificarProducto_Click(object sender, EventArgs e)
        {
            if (dgvInventario.CurrentRow == null) return;
            Producto producto = (Producto)dgvInventario.CurrentRow.DataBoundItem;

            producto.Nombre = txtNombre.Text.Trim();
            producto.Precio = decimal.Parse(txtPrecio.Text.Trim());
            producto.Stock = (int)nudCantidad.Value;
            producto.Categoria = txtCategoria.Text.Trim();

            ProductoBLL productoBLL = new ProductoBLL(new ProductoDAL());
            bool ok = productoBLL.ModificarProducto(producto);
            MessageBox.Show(ok ? "Producto modificado correctamente." : "Error al modificar el producto.");
            if(ok) CargarInventario();
        }
    }
}
