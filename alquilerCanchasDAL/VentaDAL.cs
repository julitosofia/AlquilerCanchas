using alquilerCanchasBE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace alquilerCanchasDAL
{
    public class VentaDAL
    {
        public bool InsertarVenta(Venta v, List<VentaDetalle> detalles)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                SqlTransaction tx = cn.BeginTransaction();
                try
                {
                    string insertVenta = @"INSERT INTO Venta (Fecha,IdUsuario,Total) OUTPUT INSERTED.IdVenta VALUES (@f,@u,@t)";
                    int idVenta;
                    using (SqlCommand cmd = new SqlCommand(insertVenta, cn, tx))
                    {
                        cmd.Parameters.AddWithValue("@f", v.Fecha);
                        cmd.Parameters.AddWithValue("@u", v.IdUsuario);
                        cmd.Parameters.AddWithValue("@t", v.Total);
                        idVenta = (int)cmd.ExecuteScalar();
                    }
                    foreach (var d in detalles)
                    {
                        string insertDetalle = @"INSERT INTO VentaDetalle (IdVenta,IdProducto,Cantidad,PrecioUnitario) VALUES(@v,@p,@c,@pu)";
                        using (SqlCommand cme = new SqlCommand(insertDetalle, cn, tx))
                        {
                            cme.Parameters.AddWithValue("@v", idVenta);
                            cme.Parameters.AddWithValue("@p", d.IdProducto);
                            cme.Parameters.AddWithValue("@c", d.Cantidad);
                            cme.Parameters.AddWithValue("@pu", d.PrecioUnitario);
                            cme.ExecuteNonQuery();
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
        public List<Venta> Listar()
        {
            var lista = new List<Venta>();
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = @"SELECT v.IdVenta,v.Fecha,v.Total,v.IdUsuario,u.Nombre AS NombreUsuario,
                               vd.IdProducto,vd.Cantidad,vd.PrecioUnitario,p.Nombre AS NombreProducto
                               FROM Venta v
                               JOIN Usuario u ON v.IdUsuario = u.IdUsuario
                               JOIN VentaDetalle vd ON v.IdVenta = vd.IdVenta
                               JOIN Producto p ON vd.IdProducto = p.IdProducto
                               ORDER BY v.IdVenta";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    Dictionary<int, Venta> ventas = new Dictionary<int, Venta>();
                    while (dr.Read())
                    {
                        int idVenta = (int)dr["IdVenta"];
                        if (!ventas.ContainsKey(idVenta))
                        {
                            ventas[idVenta] = new Venta
                            {
                                IdVenta = idVenta,
                                Fecha = (DateTime)dr["Fecha"],
                                Total = (decimal)dr["Total"],
                                IdUsuario = (int)dr["IdUsuario"],
                                NombreUsuario = dr["NombreUsuario"].ToString(),
                                Detalles = new List<VentaDetalle>()
                            };
                        }
                        ventas[idVenta].Detalles.Add(new VentaDetalle
                        {
                            IdProducto = (int)dr["IdProducto"],
                            Cantidad = (int)dr["Cantidad"],
                            PrecioUnitario = (decimal)dr["PrecioUnitario"],
                            NombreProducto = dr["NombreProducto"].ToString(),
                            IdVenta = idVenta,
                        });
                    }
                    lista = ventas.Values.ToList();
                }
            }
            return lista;
        }
        public bool Actualizar(Venta v)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = @"UPDATE Venta
                               SET Fecha =@Fecha,Total=@Total,IdUsuario=@IdUsuario
                               WHERE IdVenta = @IdVenta";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Fecha", v.Fecha);
                cmd.Parameters.AddWithValue("@Total", v.Total);
                cmd.Parameters.AddWithValue("@IdUsuario", v.IdUsuario);
                cmd.Parameters.AddWithValue("@IdVenta", v.IdVenta);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool ActualizarDetalleVenta(VentaDetalle detalle)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = @"UPDATE VentaDetalle
                               SET Cantidad=@Cantidad, PrecioUnitario=@PrecioUnitario
                               WHERE IdVenta = @IdVenta AND IdProducto = @IdProducto";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                cmd.Parameters.AddWithValue("@IdVenta", detalle.IdVenta);
                cmd.Parameters.AddWithValue("@IdProducto", detalle.IdProducto);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool Eliminar(int idVenta)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "DELETE FROM Venta WHERE IdVenta = @IdVenta";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdVenta", idVenta);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}