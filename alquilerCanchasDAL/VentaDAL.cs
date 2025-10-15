using alquilerCanchasBE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace alquilerCanchasDAL
{
    public class VentaDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public int ActualizarVenta(int idVenta, int idUsuario, DateTime fecha)
        {
            var parametros = new List<SqlParameter>
        {
            new SqlParameter("@IdVenta", idVenta),
            new SqlParameter("@IdUsuario", idUsuario),
            new SqlParameter("@Fecha", fecha)
        };
            return conexion.EjecutarNonQuery("SP_ActualizarVenta", parametros);
        }

        public int ActualizarDetalleVenta(int idDetalle, int cantidad, decimal precioUnitario)
        {
            var parametros = new List<SqlParameter>
        {
            new SqlParameter("@IdDetalle", idDetalle),
            new SqlParameter("@Cantidad", cantidad),
            new SqlParameter("@PrecioUnitario", precioUnitario)
        };
            return conexion.EjecutarNonQuery("SP_ActualizarDetalleVenta", parametros);
        }

        public SqlDataReader ListarVenta()
        {
            return conexion.EjecutarReader("SP_ListarVenta", null);
        }

        public bool Eliminar(int idVenta)
        {
            var cn = new SqlConnection(conexion.CadenaConexion);
            cn.Open();

            var cmd = new SqlCommand("SP_EliminarVenta", cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@IdVenta", idVenta);

            bool resultado = cmd.ExecuteNonQuery() > 0;
            cn.Close();
            return resultado;
        }

        public bool InsertarVenta(Venta v, List<VentaDetalle> detalles)
        {
            var cn = new SqlConnection(conexion.CadenaConexion);
            cn.Open();
            var tx = cn.BeginTransaction();

            try
            {
                int idVenta;

                var cmd = new SqlCommand("SP_InsertarVenta", cn, tx)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Fecha", v.Fecha);
                cmd.Parameters.AddWithValue("@IdUsuario", v.IdUsuario);
                cmd.Parameters.AddWithValue("@Total", v.Total);

                idVenta = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (var d in detalles)
                {
                    var cmdDetalle = new SqlCommand("SP_InsertarDetalleVenta", cn, tx)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmdDetalle.Parameters.AddWithValue("@IdVenta", idVenta);
                    cmdDetalle.Parameters.AddWithValue("@IdProducto", d.IdProducto);
                    cmdDetalle.Parameters.AddWithValue("@Cantidad", d.Cantidad);
                    cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", d.PrecioUnitario);

                    cmdDetalle.ExecuteNonQuery();
                }

                tx.Commit();
                cn.Close();
                return true;
            }
            catch
            {
                tx.Rollback();
                cn.Close();
                return false;
            }
        }


    }
}