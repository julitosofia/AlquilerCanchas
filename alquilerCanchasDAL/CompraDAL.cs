using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using alquilerCanchasBE;

namespace alquilerCanchasDAL
{
    public class CompraDAL
    {
        private string connectionString = Conexion.Cadena;

        public bool RegistrarCompra(Compra compra)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                SqlTransaction tx = cn.BeginTransaction();
                try
                {
                    string queryCompra = @"INSERT INTO Compras (Cliente, Fecha) 
                                       VALUES (@Cliente, @Fecha); 
                                       SELECT SCOPE_IDENTITY();";

                    int idCompra;
                    using (SqlCommand cmdCompra = new SqlCommand(queryCompra, cn, tx))
                    {
                        cmdCompra.Parameters.AddWithValue("@Cliente", compra.Cliente);
                        cmdCompra.Parameters.AddWithValue("@Fecha", compra.Fecha);
                        idCompra = Convert.ToInt32(cmdCompra.ExecuteScalar());
                    }

                    foreach (var detalle in compra.Detalles)
                    {
                        string queryDetalle = @"INSERT INTO DetalleCompra 
                        (IdCompra, IdProducto, NombreProducto, Cantidad, PrecioUnitario, Categoria) 
                        VALUES (@IdCompra, @IdProducto, @NombreProducto, @Cantidad, @PrecioUnitario, @Categoria)";

                        using (SqlCommand cmdDetalle = new SqlCommand(queryDetalle, cn, tx))
                        {
                            cmdDetalle.Parameters.AddWithValue("@IdCompra", idCompra);
                            cmdDetalle.Parameters.AddWithValue("@IdProducto", detalle.IdProducto);
                            cmdDetalle.Parameters.AddWithValue("@NombreProducto", detalle.NombreProducto);
                            cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                            cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                            cmdDetalle.Parameters.AddWithValue("@Categoria", detalle.Categoria);
                            cmdDetalle.ExecuteNonQuery();
                        }

                        string queryStock = @"UPDATE Producto 
                                          SET Stock = Stock - @Cantidad 
                                          WHERE IdProducto = @IdProducto AND Stock >= @Cantidad";

                        using (SqlCommand cmdStock = new SqlCommand(queryStock, cn, tx))
                        {
                            cmdStock.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                            cmdStock.Parameters.AddWithValue("@IdProducto", detalle.IdProducto);
                            cmdStock.ExecuteNonQuery();
                        }
                    }

                    tx.Commit();
                    return true;
                }
                catch
                {
                    tx.Rollback();
                    return false;
                }
            }
        }

        public List<Compra> ListarCompras()
        {
            var lista = new List<Compra>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = "SELECT IdCompra, Cliente, Fecha FROM Compras ORDER BY Fecha DESC";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Compra
                        {
                            IdCompra = (int)dr["IdCompra"],
                            Cliente = dr["Cliente"].ToString(),
                            Fecha = (DateTime)dr["Fecha"]
                        });
                    }
                }
            }
            return lista;
        }

        public List<DetalleCompra> ObtenerDetallesPorCompra(int idCompra)
        {
            var lista = new List<DetalleCompra>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = @"SELECT IdProducto, NombreProducto, Cantidad, PrecioUnitario, Categoria 
                             FROM DetalleCompra WHERE IdCompra = @IdCompra";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@IdCompra", idCompra);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetalleCompra
                            {
                                IdProducto = (int)dr["IdProducto"],
                                NombreProducto = dr["NombreProducto"].ToString(),
                                Cantidad = (int)dr["Cantidad"],
                                PrecioUnitario = (decimal)dr["PrecioUnitario"],
                                Categoria = dr["Categoria"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public List<Compra> ObtenerTodasLasCompras()
        {
            var lista = new List<Compra>();
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string queryCompra = "SELECT IdCompra, Cliente, Fecha FROM Compras ORDER BY Fecha DESC";
                using (SqlCommand cmdCompra = new SqlCommand(queryCompra, cn))
                using (SqlDataReader drCompra = cmdCompra.ExecuteReader())
                {
                    while (drCompra.Read())
                    {
                        var compra = new Compra
                        {
                            IdCompra = (int)drCompra["IdCompra"],
                            Cliente = drCompra["Cliente"].ToString(),
                            Fecha = (DateTime)drCompra["Fecha"],
                            Detalles = new List<DetalleCompra>()
                        };
                        lista.Add(compra);
                    }
                }

                foreach (var compra in lista)
                {
                    string queryDetalle = @"SELECT IdProducto, NombreProducto, Cantidad, PrecioUnitario, Categoria 
                                        FROM DetalleCompra WHERE IdCompra = @IdCompra";

                    using (SqlCommand cmdDetalle = new SqlCommand(queryDetalle, cn))
                    {
                        cmdDetalle.Parameters.AddWithValue("@IdCompra", compra.IdCompra);
                        using (SqlDataReader drDetalle = cmdDetalle.ExecuteReader())
                        {
                            while (drDetalle.Read())
                            {
                                compra.Detalles.Add(new DetalleCompra
                                {
                                    IdProducto = (int)drDetalle["IdProducto"],
                                    NombreProducto = drDetalle["NombreProducto"].ToString(),
                                    Cantidad = (int)drDetalle["Cantidad"],
                                    PrecioUnitario = (decimal)drDetalle["PrecioUnitario"],
                                    Categoria = drDetalle["Categoria"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return lista;
        }
    }
}