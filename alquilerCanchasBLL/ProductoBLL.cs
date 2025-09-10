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
        private ProductoDAL dal = new ProductoDAL();

        public List<Producto> ObtenerTodos()
        {
            return dal.Listar();
        }
        public bool RegistrarVenta(int idProducto,int cantidadVendida, out string mensaje)
        {
            var producto = dal.Listar().FirstOrDefault(p => p.IdProducto == idProducto);
            if(producto == null)
            {
                mensaje = "Producto no encontrado.";
                return false;
            }
            if(producto.Stock<cantidadVendida)
            {
                mensaje = $"Stock insuficiente. Disponible: {producto.Stock}";
                return false;
            }
            bool actualizado = dal.ActualiarStock(idProducto, cantidadVendida);
            mensaje = actualizado ? "Venta registrada y stock actualizado." : "Error al actualizar el stock.";
            return actualizado;
        }
        public bool AgregarProducto(Producto p)
        {
            if(string.IsNullOrWhiteSpace(p.Nombre) || p.Precio<=0 || p.Stock<0 || string.IsNullOrWhiteSpace(p.Categoria))
            {
                return false;
            }
            return dal.Insertar(p);
        }
        public bool ModificarProducto(Producto p)
        {
            if(p.IdProducto<=0 || string.IsNullOrWhiteSpace(p.Nombre) || p.Precio<=0 || p.Stock<0 || string.IsNullOrWhiteSpace(p.Categoria))
            {
                return false;
            }
            return dal.Actualizar(p);
        }
        public bool EliminarProducto(int idProducto)
        {
            return dal.Eliminar(idProducto);
        }
    }
}