using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using alquilerCanchasBE;
using alquilerCanchasDAL;

namespace alquilerCanchasBLL
{
    public class UsuarioBLL
    {
        private UsuarioDAL dal = new UsuarioDAL();

        public LoginResultado ValidarLogin(string nombre,string clave)
        {
            if(string.IsNullOrWhiteSpace(nombre)  || string.IsNullOrWhiteSpace(clave))
            {
                return new LoginResultado
                {
                    EsValido = false,
                    Mensaje = "Todos los campos son obligatorios."
                };
            }
            Usuario usuario = dal.ObtenerPorCredenciales(nombre, clave);
            if(usuario == null)
            {
                return new LoginResultado
                {
                    EsValido = false,
                    Mensaje = "Usuario o clave incorrectos."
                };
            }
            return new LoginResultado
            {
                EsValido = true,
                Usuario = usuario
            };

        }
        public static bool RegistrarUsuario(Usuario nuevo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=BILARDO;Initial Catalog=AlquilerCanchas;Integrated Security=True;Encrypt=False"))
                {
                    string query = "INSERT INTO Usuario (Nombre,Clave,Rol) VALUES (@Nombre,@Clave,@Rol)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nombre", nuevo.Nombre);
                    cmd.Parameters.AddWithValue("@Clave", nuevo.Clave);
                    cmd.Parameters.AddWithValue("@Rol", nuevo.Rol);

                    conn.Open();
                    int rows=cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch
            {
                return false;
            }
        }
        public List<Usuario> ObtenerUsuario() => dal.Listar();

        public bool NombreUsuarioDisponible(string nombre)
        {
            return !dal.ExisteNombreUsuario(nombre);
        }
    }
}
