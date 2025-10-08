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

        private readonly SqlConnection cone;
        private readonly SqlTransaction transaccion;

        public ProductoDAL(SqlConnection cone, SqlTransaction transaccion)
        {
            this.cone = cone;
            this.transaccion = transaccion;
        }
        public ProductoDAL()
        {
            var connDal = new ConexionDAL();
            this.cone = new SqlConnection(connDal.CadenaConexion);
            this.cone.Open();
        }


        public List<Producto> Listar()
        {
            var lista = new List<Producto>();
            using (var cmd = new SqlCommand("SP_ListarProducto",cone,transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Producto
                        {
                            IdProducto = (int)reader["IdProducto"],
                            Nombre = reader["Nombre"].ToString(),
                            Precio = (decimal)reader["Precio"],
                            Stock = (int)reader["Stock"],
                            Categoria = reader["Categoria"].ToString()
                        });
                    }
                }

            }
            return lista;
        }

        public Producto ObtenerPorId(int idProducto)
        {
            using(var cmd = new SqlCommand("SP_ObtenerProductoPorId",cone,transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                using(var reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
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
                }
                return null;
            }
        }

        public bool Insertar(Producto producto)
        {
                using (SqlCommand cmd = new SqlCommand("SP_InsertarProductos", cone,transaccion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                    cmd.Parameters.AddWithValue("@Categoria", producto.Categoria);
                    return cmd.ExecuteNonQuery() > 0;
                }
        }

        public bool Actualizar(Producto producto)
        {
            using (var cmd = new SqlCommand("SP_ActualizarProducto", cone, transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                cmd.Parameters.AddWithValue("@Stock", producto.Stock);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Eliminar(int idProducto)
        {
                using (SqlCommand cmd = new SqlCommand("SP_EliminarProducto",cone,transaccion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    return cmd.ExecuteNonQuery() > 0;
                }
        }

        public int ActualizarStock(int idProducto, int cantidadVendida)
        {
            using (var cmd = new SqlCommand("SP_ActualizarStock", cone, transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                cmd.Parameters.AddWithValue("@Cantidad", cantidadVendida);
                return cmd.ExecuteNonQuery();
            }
        }


    }
}