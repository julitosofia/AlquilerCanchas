using alquilerCanchasBE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace alquilerCanchasDAL
{
    public class ProductoDAL
    {
        private string connectionString = Conexion.Cadena;
        public List<Producto>Listar()
        {
            var lista = new List<Producto>();
            using(SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = "SELECT * FROM Producto";
                using(SqlCommand cmd = new SqlCommand(query,cn))
                using(SqlDataReader dr = cmd.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        lista.Add(new Producto
                        {
                            IdProducto = (int)dr["IdProducto"],
                            Nombre = dr["Nombre"].ToString(),
                            Precio = (decimal)dr["Precio"],
                            Stock = (int)dr["Stock"],
                            Categoria = dr["Categoria"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
        public bool Insertar(Producto p)
        {
            using(SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = @"INSERT INTO Producto (Nombre,Precio,Stock,Categoria) VALUES(@Nombre,@Precio,@Stock,@Categoria)";
                using(SqlCommand cmd = new SqlCommand(query,cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                    cmd.Parameters.AddWithValue("@Precio",p.Precio);
                    cmd.Parameters.AddWithValue("@Stock", p.Stock);
                    cmd.Parameters.AddWithValue("@Categoria", p.Categoria);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Actualizar(Producto p)
        {
            using(SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = @"UPDATE Producto SET Nombre = @Nombre, Precio = @Precio, Stock=@Stock, Categoria =@Categoria WHERE IdProducto = @IdProducto";
                using(SqlCommand cmd = new SqlCommand(query,cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                    cmd.Parameters.AddWithValue("@Precio", p.Precio);
                    cmd.Parameters.AddWithValue("@Stock", p.Stock);
                    cmd.Parameters.AddWithValue("@Categoria", p.Categoria);
                    cmd.Parameters.AddWithValue("@IdProducto", p.IdProducto);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool Eliminar(int idProducto)
        {
            using(SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = "DELETE FROM Producto WHERE IdProducto = @IdProducto";
                using(SqlCommand cmd = new SqlCommand(query,cn))
                {
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool ActualiarStock(int idProducto, int cantidadVendida)
        {
            using(SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = @"UPDATE Producto SET Stock = Stock - @CantidadVendida WHERE IdProducto = @IdProducto AND Stock >= @CantidadVendida";
                using (SqlCommand cmd = new SqlCommand(query,cn))
                {
                    cmd.Parameters.AddWithValue("@CantidadVendida", cantidadVendida);
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public Producto ObtenerProductoPorId(int idProducto)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "SELECT * FROM Producto WHERE IdProducto = @IdProducto";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new Producto
                            {
                                IdProducto = (int)dr["IdProducto"],
                                Nombre = dr["Nombre"].ToString(),
                                Precio = (decimal)dr["Precio"],
                                Stock = (int)dr["Stock"],
                                Categoria = dr["Categoria"].ToString()
                            };
                        }
                    }
                }
                return null;
            }
        }
    }
}