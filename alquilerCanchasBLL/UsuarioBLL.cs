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
            Usuario usuario = dal.ObtenerPorCredencialesPlano(nombre, clave);

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
            UsuarioDAL dal = new UsuarioDAL();
            return dal.RegistrarUsuario(nuevo);
        }
        public List<Usuario> ObtenerUsuario() => dal.Listar();

        public bool NombreUsuarioDisponible(string nombre)
        {
            return !dal.ExisteNombreUsuario(nombre);
        }
    }
}
