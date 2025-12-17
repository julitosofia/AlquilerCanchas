using alquilerCanchasBE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace alquilerCanchasDAL
{
    public class UsuarioDAL
    {
        private readonly ConexionDAL conexion;

        public UsuarioDAL(ConexionDAL conexion)
        {
            this.conexion = conexion;
        }

        private Usuario MapearUsuario(SqlDataReader reader, bool incluirClave = true)
        {
            var usuario = new Usuario
            {
                IdUsuario = (int)reader["IdUsuario"],
                Nombre = reader["Nombre"].ToString(),
                Rol = reader["Rol"].ToString()
            };

            if (incluirClave && reader["Clave"] != DBNull.Value)
            {
                usuario.Clave = reader["Clave"].ToString();
            }

            return usuario;
        }

        public bool Login(string nombre, string clave)
        {
            var parametros = new List<SqlParameter>
        {
            new SqlParameter("@Nombre", nombre),
            new SqlParameter("@Clave", clave)
        };

            var reader = conexion.EjecutarReader("SP_Login", parametros);
            bool resultado = false;

            try
            {
                if (reader.Read())
                    resultado = Convert.ToInt32(reader["Autenticado"]) == 1;
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
            }
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

            try
            {
                if (reader.Read())
                {
                    usuario = MapearUsuario(reader, incluirClave: true);
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
            }
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

            try
            {
                if (reader.Read())
                    existe = Convert.ToInt32(reader["Existe"]) == 1;
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
            }
            return existe;
        }

        public List<Usuario> Listar()
        {
            var lista = new List<Usuario>();
            var reader = conexion.EjecutarReader("SP_ListarUsuarios", null);

            try
            {
                while (reader.Read())
                {
                    lista.Add(MapearUsuario(reader, incluirClave: false));
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
            }
            return lista;
        }

        public bool RegistrarUsuario(Usuario usuario)
            => conexion.EjecutarNonQuery("SP_InsertarUsuario", new List<SqlParameter>
            {
            new SqlParameter("@Nombre", usuario.Nombre),
            new SqlParameter("@Clave", usuario.Clave),
            new SqlParameter("@Rol", usuario.Rol)
            }) > 0;

        public void ExportarUsuariosXML(List<Usuario> usuarios, string rutaArchivo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Usuario>));
            using (var writer = new StreamWriter(rutaArchivo))
            {
                serializer.Serialize(writer, usuarios);
            }
        }

        public List<Usuario> ImportarUsuariosXML(string rutaArchivo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Usuario>));
            using (var reader = new StreamReader(rutaArchivo))
            {
                return (List<Usuario>)serializer.Deserialize(reader);
            }
        }


    }
}
