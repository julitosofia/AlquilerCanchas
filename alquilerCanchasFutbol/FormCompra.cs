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
        private ProductoBLL productoBLL = new ProductoBLL(new ProductoDAL());
        private CompraBLL compraBLL = new CompraBLL(new CompraDAL(), new ProductoDAL());
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
            Compra nuevaCompra;
            try
            {
                nuevaCompra = ObtenerDatosDeCompraDelFormulario();
            }
            catch (InvalidOperationException validationEx)
            {
                MessageBox.Show(validationEx.Message, "Validación Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Salir si falla la validación del formulario.
            }
            catch (Exception dataEx)
            {
                MessageBox.Show($"Error al preparar datos: {dataEx.Message}", "Error de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Paso 2: Iniciar la Unidad de Trabajo (Transacción)
            var connDal = new ConexionDAL();
            string cadenaConexion = connDal.CadenaConexion;

            using (var uow = new UnitOfWork(cadenaConexion)) // La UoW abre la conexión e inicia la transacción
            {
                try
                {
                    // Instanciar la BLL inyectando los repositorios con la transacción activa de la UoW
                    var compraBLL = new CompraBLL(uow.Compras, uow.Productos);
                    string mensaje;

                    // Paso 3: Ejecutar la lógica de negocio (validación de stock, etc.)
                    bool exito = compraBLL.RegistrarCompra(nuevaCompra, out mensaje);

                    if (exito)
                    {
                        // Paso 4a: Lógica exitosa -> CONFIRMAR
                        uow.Commit();
                        MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MostrarTicket(nuevaCompra); // Muestra el ticket si todo fue bien
                                                    // LimpiarFormulario(); // Función para resetear campos
                    }
                    else
                    {
                        // Paso 4b: Fallo de lógica de negocio (ej. stock insuficiente, validado en BLL) -> REVERTIR
                        uow.Rollback();
                        MessageBox.Show(mensaje, "Fallo en la Compra", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    // Paso 4c: Fallo del sistema (ej. error SQL, conexión, o error no capturado en DAL) -> REVERTIR
                    // Este catch asegura que cualquier excepción lanzada por CompraDAL (ya sin try/catch interno) 
                    // sea manejada aquí con un Rollback, resolviendo el error de "SqlTransaction completed".
                    uow.Rollback();
                    MessageBox.Show($"Ocurrió un error crítico durante el registro: {ex.Message}", "Error Crítico del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            // ... (Se asume que txtNombreCliente es el nombre correcto del TextBox)
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

            // ❌ LÍNEA(S) A ELIMINAR O COMENTAR (ESTO ES LO QUE ESTÁ CAUSANDO EL ERROR) ❌
            /* if (dgvDetallesCompra.Rows.Count == 0 || (dgvDetallesCompra.Rows.Count == 1 && dgvDetallesCompra.AllowUserToAddRows))
            {
                throw new InvalidOperationException("El carrito no contiene productos. Agregue al menos un producto.");
            }
            */
            // -------------------------------------------------------------------------------------------------------

            foreach (DataGridViewRow fila in dgvDetallesCompra.Rows)
            {
                // Esta condición es la que correctamente ignora la fila de agregar
                if (!fila.IsNewRow && fila.Cells["IdProducto"].Value != null)
                {
                    try
                    {
                        var detalle = new DetalleCompra
                        {
                            IdProducto = Convert.ToInt32(fila.Cells["IdProducto"].Value),
                            NombreProducto = fila.Cells["NombreProducto"].Value.ToString(),
                            // MUY IMPORTANTE: Asegúrate de que los valores de Cantidad y Precio no sean DBNull o nulos.
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

            // ✅ LÍNEA(S) A AGREGAR O MOVER (CORRECCIÓN: Validar si la lista final tiene algo) ✅
            if (compra.Detalles.Count == 0)
            {
                throw new InvalidOperationException("El carrito no contiene productos. Agregue al menos un producto.");
            }
            // -----------------------------------------------------------------------------------------

            return compra;
        }
    }
}
