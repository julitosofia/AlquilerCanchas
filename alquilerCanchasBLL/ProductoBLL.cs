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
            Producto producto = repo.ObtenerPorId(idProducto);

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

        public bool AgregarProducto(Producto p)
        {
            if (string.IsNullOrWhiteSpace(p.Nombre) || p.Precio <= 0 || p.Stock < 0 || string.IsNullOrWhiteSpace(p.Categoria))
                return false;

            return repo.Insertar(p);
        }

        public bool ModificarProducto(Producto p)
        {
            if (p.IdProducto <= 0 || string.IsNullOrWhiteSpace(p.Nombre) || p.Precio <= 0 || p.Stock < 0 || string.IsNullOrWhiteSpace(p.Categoria))
                return false;

            return repo.Actualizar(p);
        }

        public bool EliminarProducto(int idProducto)
        {
            return repo.Eliminar(idProducto);
        }


    }
}