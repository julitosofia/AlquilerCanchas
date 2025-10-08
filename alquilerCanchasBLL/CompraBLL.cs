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
        private readonly ICompraRepository compraRepo;
        private readonly IProductoRepository productoRepo;

        public CompraBLL(ICompraRepository compraRepo, IProductoRepository productoRepo)
        {
            this.compraRepo = compraRepo;
            this.productoRepo = productoRepo;
        }

        public bool RegistrarCompra(Compra compra, out string mensaje)
        {
            mensaje = "";

            if (compra == null || compra.Detalles == null || compra.Detalles.Count == 0)
            {
                mensaje = "La compra no contiene productos.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(compra.Cliente))
            {
                mensaje = "Debe especificar el cliente.";
                return false;
            }

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
            using (var reader = compraRepo.ListarCompra())
            {
                while (reader.Read())
                {
                    lista.Add(new Compra
                    {
                        IdCompra = (int)reader["IdVenta"],
                        Fecha = (DateTime)reader["Fecha"],
                        Cliente = reader["Usuario"].ToString()
                    });
                }
            }
            return lista;
        }

        public List<DetalleCompra> ObtenerDetalles(int idCompra)
        {
            var lista = new List<DetalleCompra>();
            using (var reader = compraRepo.ObtenerDetallesPorCompra(idCompra))
            {
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
            }
            return lista;
        }

        public List<Compra> ObtenerTodas()
        {
            var lista = new List<Compra>();
            using (var reader = compraRepo.ObtenerTodasLasCompras())
            {
                while (reader.Read())
                {
                    lista.Add(new Compra
                    {
                        IdCompra = (int)reader["IdVenta"],
                        Fecha = (DateTime)reader["Fecha"],
                        Cliente = reader["Usuario"].ToString()
                    });
                }
            }
            return lista;
        }


    }
}