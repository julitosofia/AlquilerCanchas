using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using alquilerCanchasBE;

namespace alquilerCanchasDAL
{
    public class UsuarioDAL
    {
        private string connectionString = Conexion.Cadena;
        public Usuario ObtenerPorCredenciales(string usuario, string clave)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = @"SELECT * FROM Usuario WHERE Nombre = @u AND Clave=@c";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@u", usuario);
                    cmd.Parameters.AddWithValue("@c", clave);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new Usuario
                            {
                                IdUsuario = (int)dr["IdUsuario"],
                                Nombre = dr["Nombre"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Rol = dr["Rol"].ToString()

                            };
                        }
                    }
                }
                return null;
            }
        }
        public List<Usuario> Listar()
        {
            var lista = new List<Usuario>();
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "SELECT * FROM Usuario";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Usuario
                        {
                            IdUsuario = (int)dr["IdUsuario"],
                            Nombre = dr["Nombre"].ToString(),
                            Rol = dr["Rol"].ToString(),
                            Clave = dr["Clave"].ToString()
                        });
                    }
                }
                return lista;
            }
        }

        public bool Insertar(Usuario u)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "INSERT INTO Usuario (IdUsuario,Nombre,Rol,Clave) VALUES(@IdUsuario,@Nombre,@Rol,@Clave)";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", u.IdUsuario);
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Rol", u.Rol);
                    cmd.Parameters.AddWithValue("@Clave", u.Clave);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public Usuario Login(string nombre, string clave)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "SELECT * FROM Usuario WHERE Nombre = @Nombre AND Clave =@Clave";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Clave", clave);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new Usuario
                            {
                                IdUsuario = (int)dr["IdUsuario"],
                                Nombre = dr["Nombre"].ToString(),
                                Rol = dr["Rol"].ToString(),
                                Clave = dr["Clave"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
        public bool ExisteNombreUsuario(string nombre)
        {
            using(SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "SELECT COUNT (*) FROM Usuario WHERE Nombre = @Nombre";
                using(SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
