using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using alquilerCanchasBE;
using System.Data;

namespace alquilerCanchasDAL
{
    public class CompraDAL : ICompraRepository
    {
        private readonly SqlConnection conexion;
        private readonly SqlTransaction transaccion;

        public CompraDAL(SqlConnection conexion, SqlTransaction transaccion)
        {
            this.conexion = conexion;
            this.transaccion = transaccion;
        }
        public CompraDAL()
        {
            var connDal = new ConexionDAL();
            this.conexion = connDal.Abrir();
        }

        public SqlDataReader ObtenerTodasLasCompras()
        {
            var cmd = new SqlCommand("SP_ObtenerTodasLasCompras", conexion, transaccion);
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd.ExecuteReader();
        }
        public SqlDataReader ObtenerDetallesPorCompra(int idCompra)
        {
            var cmd = new SqlCommand("SP_ObtenerDetallesPorCompra", conexion, transaccion);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdCompra", idCompra);
            return cmd.ExecuteReader();
        }
        public SqlDataReader ListarCompra()
        {
            var cmd = new SqlCommand("SP_ListarCompra", conexion, transaccion);
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd.ExecuteReader();
        }
        public bool RegistrarCompra(Compra compra)
        {
            int idCompra;

            var cmdCompra = new SqlCommand("RegistrarCompra", conexion, transaccion);
            cmdCompra.CommandType = CommandType.StoredProcedure;
            cmdCompra.Parameters.AddWithValue("@Cliente", compra.Cliente);
            cmdCompra.Parameters.AddWithValue("@Fecha", compra.Fecha);
            idCompra = Convert.ToInt32(cmdCompra.ExecuteScalar());

            foreach( var detalle in compra.Detalles)
            {
                var cmdDetalle = new SqlCommand("RegistrarDetalleCompra", conexion, transaccion);
                cmdDetalle.CommandType = CommandType.StoredProcedure;
                cmdDetalle.Parameters.AddWithValue("@IdCompra", idCompra);
                cmdDetalle.Parameters.AddWithValue("@NombreProducto", detalle.NombreProducto);
                cmdDetalle.Parameters.AddWithValue("@IdProducto", detalle.IdProducto);
                cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                cmdDetalle.Parameters.AddWithValue("@Categoria", detalle.Categoria);
                cmdDetalle.ExecuteNonQuery();

                var cmdStock = new SqlCommand("SP_ActualizarStock", conexion, transaccion);
                cmdStock.CommandType = CommandType.StoredProcedure;
                cmdStock.Parameters.AddWithValue("@IdProducto", detalle.IdProducto);
                cmdStock.Parameters.AddWithValue("@Cantidad",detalle.Cantidad);
                cmdStock.ExecuteNonQuery();
            }
            return true;
        }
    }
}