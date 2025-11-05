using alquilerCanchasBE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;

namespace alquilerCanchasDAL
{
    public class ProductoDAL : IProductoRepository
    {
        private readonly ConexionDAL conexion;
        public ProductoDAL(ConexionDAL conexion)
        {
            this.conexion = conexion;
        }

        private Producto MapearProducto(SqlDataReader reader)
        {
            return new Producto
            {
                IdProducto = (int)reader["IdProducto"],
                Nombre = reader["Nombre"].ToString(),
                Precio = (decimal)reader["Precio"],
                Stock = (int)reader["Stock"],
                Categoria = reader["Categoria"].ToString()
            };
        }
        public List<Producto>Listar()
        {
            var lista = new List<Producto>();
            var reader = conexion.EjecutarReader("SP_ListarProducto", null);

            try
            {
                while (reader.Read())
                {
                    lista.Add(MapearProducto(reader));
                }
            }
            finally
            {
                if(reader!=null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return lista;
        }
        public Producto ObtenerPorId(int idProducto)
        {
            Producto producto = null;
            var parametros = new List<SqlParameter> { new SqlParameter("@IdProducto", idProducto) };
            var reader = conexion.EjecutarReader("SP_ObtenerProductoPorId", parametros);
            try
            {
                if(reader.Read())
                {
                    producto = MapearProducto(reader);
                }
            }
            finally
            {
                if(reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return producto;
        }

        public bool Insertar(Producto producto)
            => conexion.EjecutarNonQueryTransacciones("SP_InsertarProductos", new List<SqlParameter>
            {
                new SqlParameter("@Nombre",producto.Nombre),
                new SqlParameter("@Precio", producto.Precio),
                new SqlParameter("@Stock", producto.Stock),
                new SqlParameter("@Categoria",producto.Categoria)
            }) > 0;

        public bool Actualizar(Producto producto)
            => conexion.EjecutarNonQueryTransacciones("SP_ModificarProducto", new List<SqlParameter>
            {
                new SqlParameter("@IdProducto", producto.IdProducto),
                new SqlParameter("@Nombre",producto.Nombre),
                new SqlParameter("@Precio",producto.Precio),
                new SqlParameter("@Stock",producto.Stock),
                new SqlParameter("@Categoria",producto.Categoria)
            }) > 0;
        public bool Eliminar(int idProducto)
            => conexion.EjecutarNonQueryTransacciones("SP_EliminarProducto", new List<SqlParameter>
            {
                new SqlParameter("@IdProducto", idProducto)
            }) > 0;
        public int ActualizarStock(int idProducto, int cantidadVendida)
            => conexion.EjecutarNonQueryTransacciones("SP_ActualizarStock", new List<SqlParameter>
            {
                new SqlParameter("@IdProducto",idProducto),
                new SqlParameter("@Cantidad",cantidadVendida)
            });
    }
}