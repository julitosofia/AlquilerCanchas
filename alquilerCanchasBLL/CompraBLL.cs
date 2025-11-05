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
        {
            var lista = new List<Compra>();
            var reader = compraRepo.ListarCompra();

            while (reader.Read())
            {
                lista.Add(new Compra
                {
                    IdCompra = (int)reader["IdVenta"],
                    Fecha = (DateTime)reader["Fecha"],
                    Cliente = reader["Usuario"].ToString()
                });
            }

            reader.Close();
            return lista;
        }

        public List<DetalleCompra> ObtenerDetalles(int idCompra)
        {
            var lista = new List<DetalleCompra>();
            var reader = compraRepo.ObtenerDetallesPorCompra(idCompra);

            while (reader.Read())
            {
                lista.Add(new DetalleCompra
                {
                    IdProducto = (int)reader["IdProducto"],
                    NombreProducto = reader["Nombre"].ToString(),
                    Cantidad = (int)reader["Cantidad"],
                    PrecioUnitario = (decimal)reader["PrecioUnitario"],
                    Categoria = reader["Categoria"].ToString()
                });
            }

            reader.Close();
            return lista;
        }

        public List<Compra> ObtenerTodas()
        {
            var lista = new List<Compra>();
            var reader = compraRepo.ObtenerTodasLasCompras();

            while (reader.Read())
            {
                lista.Add(new Compra
                {
                    IdCompra = (int)reader["IdVenta"],
                    Fecha = (DateTime)reader["Fecha"],
                    Cliente = reader["Usuario"].ToString()
                });
            }

            reader.Close();
            return lista;
        }

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
        public bool ExportarComprasAXml(out string mensaje)
        {

            var compras = ObtenerTodas();

            if (compras == null || compras.Count == 0)
            {
                mensaje = "No hay compras para exportar.";
                return false;
            }

            try
            {

                var xmlManager = new XmlManager<Compra>("compras.xml");


                bool exito = xmlManager.Guardar(compras);

                if (exito)
                {
                    mensaje = "Compras exportadas a 'compras.xml' correctamente.";
                    return true;
                }
                else
                {

                    mensaje = "Error desconocido al intentar guardar el archivo XML de compras.";
                    return false;
                }
            }
            catch (Exception ex)
            {

                mensaje = $"Error de sistema al exportar las compras: {ex.Message}";
                return false;
            }
        }
        public List<Compra> ImportarComprasDesdeXml()
        {
            var xmlManager = new XmlManager<Compra>("compras.xml");
            return xmlManager.Cargar();
        }
    }
}