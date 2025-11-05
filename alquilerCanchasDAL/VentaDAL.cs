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
            => conexion.EjecutarNonQuery("SP_ActualizarVenta", new List<SqlParameter>
            {
            new SqlParameter("@IdVenta", idVenta),
            new SqlParameter("@IdUsuario", idUsuario),
            new SqlParameter("@Fecha", fecha)
            }); 

        public int ActualizarDetalleVenta(int idDetalle, int cantidad, decimal precioUnitario)
            => conexion.EjecutarNonQuery("SP_ActualizarDetalleVenta", new List<SqlParameter>
            {
            new SqlParameter("@IdDetalle", idDetalle),
            new SqlParameter("@Cantidad", cantidad),
            new SqlParameter("@PrecioUnitario", precioUnitario)
            }); 

        public SqlDataReader ListarVenta()
        {

            return conexion.EjecutarReader("SP_ListarVenta", null);
        }


        public bool Eliminar(int idVenta)
            => conexion.EjecutarNonQuery("SP_EliminarVenta", new List<SqlParameter>
            {
            new SqlParameter("@IdVenta", idVenta)
            }) > 0;



        public bool InsertarVenta(Venta v, List<VentaDetalle> detalles)
        {

            conexion.IniciarTransaccion();

            try
            {

                var parametrosVenta = new List<SqlParameter>
            {
                new SqlParameter("@Fecha", v.Fecha),
                new SqlParameter("@IdUsuario", v.IdUsuario),
                new SqlParameter("@Total", v.Total)
            };


                object resultadoEscalar = conexion.EjecutarEscalarTransaccion("SP_InsertarVenta", parametrosVenta);
                int idVenta = Convert.ToInt32(resultadoEscalar);


                foreach (var d in detalles)
                {
                    var parametrosDetalle = new List<SqlParameter>
                {
                    new SqlParameter("@IdVenta", idVenta),
                    new SqlParameter("@IdProducto", d.IdProducto),
                    new SqlParameter("@Cantidad", d.Cantidad),
                    new SqlParameter("@PrecioUnitario", d.PrecioUnitario)
                };
                    conexion.EjecutarNonQueryTransacciones("SP_InsertarDetalleVenta", parametrosDetalle);
                }


                conexion.ConfirmarTransaccion();
                return true;
            }
            catch
            {
                conexion.CancelarTransaccion();
                return false;
            }
        }
    }
}