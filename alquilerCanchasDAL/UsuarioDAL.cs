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
        private readonly ConexionDAL conexion = new ConexionDAL();

        public bool Login(string nombre, string clave)
        {
            var parametros = new List<SqlParameter>
        {
            new SqlParameter("@Nombre", nombre),
            new SqlParameter("@Clave", clave)
        };

            var reader = conexion.EjecutarReader("SP_Login", parametros);
            bool resultado = false;

            if (reader.Read())
                resultado = Convert.ToInt32(reader["Autenticado"]) == 1;

            reader.Close();
            return resultado;
        }

        public Usuario ObtenerPorCredencialesPlano(string nombre, string clave)
        {
            var parametros = new List<SqlParameter>
        {
            new SqlParameter("@Nombre", nombre),
            new SqlParameter("@Clave", clave)
        };

            var reader = conexion.EjecutarReader("SP_ObtenerPorCredencialesPlano", parametros);
            Usuario usuario = null;

            if (reader.Read())
            {
                usuario = new Usuario
                {
                    IdUsuario = (int)reader["IdUsuario"],
                    Nombre = reader["Nombre"].ToString(),
                    Clave = reader["Clave"].ToString(),
                    Rol = reader["Rol"].ToString()
                };
            }

            reader.Close();
            return usuario;
        }

        public bool ExisteNombreUsuario(string nombre)
        {
            var parametros = new List<SqlParameter>
        {
            new SqlParameter("@Nombre", nombre)
        };

            var reader = conexion.EjecutarReader("SP_ExisteNombreUsuario", parametros);
            bool existe = false;

            if (reader.Read())
                existe = Convert.ToInt32(reader["Existe"]) == 1;

            reader.Close();
            return existe;
        }

        public List<Usuario> Listar()
        {
            var lista = new List<Usuario>();
            var reader = conexion.EjecutarReader("SP_ListarUsuarios", null);

            while (reader.Read())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = (int)reader["IdUsuario"],
                    Nombre = reader["Nombre"].ToString(),
                    Rol = reader["Rol"].ToString()
                });
            }

            reader.Close();
            return lista;
        }

        public bool RegistrarUsuario(Usuario usuario)
        {
            var parametros = new List<SqlParameter>
        {
            new SqlParameter("@Nombre", usuario.Nombre),
            new SqlParameter("@Clave", usuario.Clave),
            new SqlParameter("@Rol", usuario.Rol)
        };

            return conexion.EjecutarNonQuery("SP_InsertarUsuario", parametros) > 0;
        }


    }
}
