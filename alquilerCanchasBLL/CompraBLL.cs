using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using alquilerCanchasDAL;
using alquilerCanchasBE;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Utils;

namespace alquilerCanchasBLL
{
    public class CompraBLL
    {
        private readonly ICompraRepository compraRepo;
        private readonly IProductoRepository productoRepo;

        public CompraBLL(ICompraRepository compraRepo, IProductoRepository productoRepo)
        {
            this.compraRepo = compraRepo;
            this.productoRepo = productoRepo;
        }

        public bool RegistrarCompra(Compra compra, out string mensaje)
        {
            mensaje = string.Empty;

            if (!ValidarCompra(compra, out mensaje))
                return false;

            foreach (var detalle in compra.Detalles)
            {
                var producto = productoRepo.ObtenerPorId(detalle.IdProducto);
                if (producto == null)
                {
                    mensaje = $"Producto con ID {detalle.IdProducto} no existe.";
                    return false;
                }

                if (producto.Stock < detalle.Cantidad)
                {
                    mensaje = $"Stock insuficiente para el producto '{producto.Nombre}'. Disponible: {producto.Stock}, solicitado: {detalle.Cantidad}";
                    return false;
                }
            }

            bool resultado = compraRepo.RegistrarCompra(compra);
            mensaje = resultado ? "Compra registrada correctamente." : "Error al registrar la compra.";
            return resultado;
        }

        public List<Compra> ListarCompras()
            => compraRepo.ListarCompra();

        public List<DetalleCompra> ObtenerDetalles(int idCompra)
            => compraRepo.ObtenerDetallesPorCompra(idCompra);

        public List<Compra> ObtenerTodas()
            => compraRepo.ObtenerTodasLasCompras();

        private bool ValidarCompra(Compra compra, out string mensaje)
        {
            mensaje = string.Empty;

            if (compra == null)
            {
                mensaje = "La compra no puede ser nula.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(compra.Cliente))
            {
                mensaje = "Debe especificar el cliente.";
                return false;
            }

            if (compra.Detalles == null || compra.Detalles.Count == 0)
            {
                mensaje = "La compra no contiene productos.";
                return false;
            }

            return true;
        }

        // --- Métodos XML delegados a DAL ---
        public bool ExportarComprasAXml(string rutaArchivo, out string mensaje)
        {
            var compras = ObtenerTodas();

            if (compras == null || compras.Count == 0)
            {
                mensaje = "No hay compras para exportar.";
                return false;
            }

            try
            {
                compraRepo.ExportarComprasXML(compras, rutaArchivo);
                mensaje = $"Compras exportadas a '{rutaArchivo}' correctamente.";
                return true;
            }
            catch (Exception ex)
            {
                mensaje = $"Error de sistema al exportar las compras: {ex.Message}";
                return false;
            }
        }

        public List<Compra> ImportarComprasDesdeXml(string rutaArchivo)
        {
            var lista = compraRepo.ImportarComprasXML(rutaArchivo);

            if (lista == null || lista.Count == 0)
                throw new InvalidOperationException("El archivo XML no contiene compras válidas.");

            return lista;
        }



    }
}