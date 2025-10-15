using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using alquilerCanchasBE;
using alquilerCanchasDAL;

namespace alquilerCanchasBLL
{
    public class ProductoBLL
    {
        private readonly IProductoRepository repo;

        public ProductoBLL(IProductoRepository repo)
        {
            this.repo = repo;
        }

        public List<Producto> ObtenerTodos()
        {
            return repo.Listar();
        }

        public bool RegistrarVenta(int idProducto, int cantidadVendida, out string mensaje)
        {
            mensaje = string.Empty;

            if (idProducto <= 0 || cantidadVendida <= 0)
            {
                mensaje = "Datos inválidos para registrar la venta.";
                return false;
            }

            var producto = repo.ObtenerPorId(idProducto);

            if (producto == null)
            {
                mensaje = "Producto no encontrado.";
                return false;
            }

            if (producto.Stock < cantidadVendida)
            {
                mensaje = $"Stock insuficiente. Disponible: {producto.Stock}";
                return false;
            }

            int resultado = repo.ActualizarStock(idProducto, cantidadVendida);
            mensaje = resultado > 0 ? "Venta registrada y stock actualizado." : "Error al actualizar el stock.";
            return resultado > 0;
        }

        public bool AgregarProducto(Producto p, out string mensaje)
        {
            mensaje = ValidarProducto(p);

            if (!string.IsNullOrEmpty(mensaje))
                return false;

            return repo.Insertar(p);
        }

        public bool ModificarProducto(Producto p, out string mensaje)
        {
            if (p.IdProducto <= 0)
            {
                mensaje = "Id de producto inválido.";
                return false;
            }

            mensaje = ValidarProducto(p);

            if (!string.IsNullOrEmpty(mensaje))
                return false;

            return repo.Actualizar(p);
        }

        public bool EliminarProducto(int idProducto, out string mensaje)
        {
            mensaje = string.Empty;

            if (idProducto <= 0)
            {
                mensaje = "Id inválido.";
                return false;
            }

            return repo.Eliminar(idProducto);
        }

        private string ValidarProducto(Producto p)
        {
            if (string.IsNullOrWhiteSpace(p.Nombre))
                return "El nombre del producto es obligatorio.";

            if (p.Precio <= 0)
                return "El precio debe ser mayor a cero.";

            if (p.Stock < 0)
                return "El stock no puede ser negativo.";

            if (string.IsNullOrWhiteSpace(p.Categoria))
                return "La categoría es obligatoria.";

            return string.Empty;
        }


    }
}