using alquilerCanchasBE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Xml.Serialization;

namespace alquilerCanchasDAL
{
    public class CompraDAL : ICompraRepository
    {
        private readonly ConexionDAL conexion;

        public CompraDAL(ConexionDAL conexion)
        {
            this.conexion = conexion;
        }

        public List<Compra> ObtenerTodasLasCompras()
        {
            var lista = new List<Compra>();
            var reader = conexion.EjecutarReader("SP_ObtenerTodasLasCompras", null);

            try
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
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }

            return lista;
        }

        public List<DetalleCompra> ObtenerDetallesPorCompra(int idCompra)
        {
            var lista = new List<DetalleCompra>();
            var reader = conexion.EjecutarReader("SP_ObtenerDetallesPorCompra",
                new List<SqlParameter> { new SqlParameter("@IdCompra", idCompra) });

            try
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
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }

            return lista;
        }

        public List<Compra> ListarCompra()
        {
            var lista = new List<Compra>();
            var reader = conexion.EjecutarReader("SP_ListarCompra", null);

            try
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
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }

            return lista;
        }

        public bool RegistrarCompra(Compra compra)
        {
            var parametrosCompra = new List<SqlParameter>
        {
            new SqlParameter("@Cliente", compra.Cliente),
            new SqlParameter("@Fecha", compra.Fecha)
        };

            object resultadoEscalar = conexion.EjecutarEscalarTransaccion("RegistrarCompra", parametrosCompra);
            int idCompra = Convert.ToInt32(resultadoEscalar);

            foreach (var detalle in compra.Detalles)
            {
                var parametrosDetalle = new List<SqlParameter>
            {
                new SqlParameter("@IdCompra", idCompra),
                new SqlParameter("@NombreProducto", detalle.NombreProducto),
                new SqlParameter("@IdProducto", detalle.IdProducto),
                new SqlParameter("@Cantidad", detalle.Cantidad),
                new SqlParameter("@PrecioUnitario", detalle.PrecioUnitario),
                new SqlParameter("@Categoria", detalle.Categoria)
            };
                conexion.EjecutarNonQueryTransacciones("RegistrarDetalleCompra", parametrosDetalle);

                var parametrosStock = new List<SqlParameter>
            {
                new SqlParameter("@IdProducto", detalle.IdProducto),
                new SqlParameter("@Cantidad", detalle.Cantidad)
            };
                conexion.EjecutarNonQueryTransacciones("SP_ActualizarStock", parametrosStock);
            }
            return true;
        }

        public void ExportarComprasXML(List<Compra> compras, string rutaArchivo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Compra>));
            using (var writer = new StreamWriter(rutaArchivo))
            {
                serializer.Serialize(writer, compras);
            }
        }

        public List<Compra> ImportarComprasXML(string rutaArchivo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Compra>));
            using (var reader = new StreamReader(rutaArchivo))
            {
                return (List<Compra>)serializer.Deserialize(reader);
            }
        }


    }
}