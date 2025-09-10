using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using alquilerCanchasDAL;
using alquilerCanchasBE;

namespace alquilerCanchasBLL
{
    public class CompraBLL
    {
        private CompraDAL compraDAL = new CompraDAL();
        private ProductoDAL productoDAL = new ProductoDAL();

        public bool RegistrarCompra(Compra compra,out string mensaje)
        {
            mensaje = "";
            if(compra == null || compra.Detalles == null || compra.Detalles.Count == 0)
            {
                mensaje = "La compra no contiene productos.";
                return false;
            }
            if(string.IsNullOrWhiteSpace(compra.Cliente))
            {
                mensaje = "Debe especificar el cliente.";
                return false;
            }
            foreach(var detalle in compra.Detalles)
            {
                var producto = productoDAL.ObtenerProductoPorId(detalle.IdProducto);
                if(producto == null)
                {
                    mensaje = $"Producto con ID {detalle.IdProducto} no existe.";
                    return false;
                }
                if(producto.Stock < detalle.Cantidad)
                {
                    mensaje =$"Stock insuficiente para el producto '{producto.Nombre}'. Disponible: {producto.Stock}, solicitado: {detalle.Cantidad}";
                    return false;
                }
            }
            bool resultado = compraDAL.RegistrarCompra(compra);
            mensaje = resultado ? "Compra registrada correctamente." : "Error al registrar la compra.";
            return resultado;
        }
        public List<Compra>ListarCompras()
        {
            return compraDAL.ListarCompras();
        }
        public List<DetalleCompra>ObtenerDetalles(int idCompra)
        {
            return compraDAL.ObtenerDetallesPorCompra(idCompra);
        }
        public List<Compra>ObtenerTodas()
        {
            return compraDAL.ObtenerTodasLasCompras();
        }
    }
}