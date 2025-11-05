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
        private readonly ConexionDAL conexion;

        public CompraDAL(ConexionDAL conexion)
        {
            this.conexion = conexion;
        }
        public SqlDataReader ObtenerTodasLasCompras()
            => conexion.EjecutarReader("SP_ObtenerTodasLasCompras", null);
        public SqlDataReader ObtenerDetallesPorCompra(int idCompra)
            => conexion.EjecutarReader("SP_ObtenerDetallesPorCompra",
                new List<SqlParameter> { new SqlParameter("@IdCompra", idCompra) });
        public SqlDataReader ListarCompra()
            => conexion.EjecutarReader("SP_ListarCompra", null);

        public bool RegistrarCompra(Compra compra)
        {
            var parametrosCompra = new List<SqlParameter>
            {
                new SqlParameter("@Cliente", compra.Cliente),
                new SqlParameter("@Fecha", compra.Fecha)
            };
            object resultadoEscalar = conexion.EjecutarEscalarTransaccion("RegistrarCompra", parametrosCompra);
            int idCompra = Convert.ToInt32(resultadoEscalar);

            foreach(var detalle in compra.Detalles)
            {
                var parametrosDetalle = new List<SqlParameter>
                {
                    new SqlParameter("@IdCompra", idCompra),
                    new SqlParameter("@NombreProducto",detalle.NombreProducto),
                    new SqlParameter("@IdProducto",detalle.IdProducto),
                    new SqlParameter("@Cantidad",detalle.Cantidad),
                    new SqlParameter("@PrecioUnitario",detalle.PrecioUnitario),
                    new SqlParameter("@Categoria",detalle.Categoria)
                };
                conexion.EjecutarNonQueryTransacciones("RegistrarDetalleCompra", parametrosDetalle);

                var parametrosStock = new List<SqlParameter>
                {
                    new SqlParameter("@IdProducto", detalle.IdProducto),
                    new SqlParameter("@Cantidad",detalle.Cantidad)
                };
                conexion.EjecutarNonQueryTransacciones("SP_ActualizarStock", parametrosStock);
            }
            return true;
        }
    }
}