using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using alquilerCanchasBE;
using alquilerCanchasDAL;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Utils;

namespace alquilerCanchasBLL
{
    public class UsuarioBLL
    {
        private readonly UsuarioDAL dal;
        private readonly ConexionDAL conexion = new ConexionDAL();
        public UsuarioBLL(UsuarioDAL usuarioDal)
        {
            this.dal = usuarioDal;
        }

        public LoginResultado ValidarLogin(string nombre, string clave)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(clave))
            {
                return new LoginResultado
                {
                    EsValido = false,
                    Mensaje = "Todos los campos son obligatorios."
                };
            }

            var usuario = dal.ObtenerPorCredencialesPlano(nombre, clave);

            if (usuario == null)
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

        public bool RegistrarUsuario(Usuario nuevo, out string mensaje)
        {
            mensaje = ValidarUsuario(nuevo);

            if (!string.IsNullOrEmpty(mensaje))
                return false;

            if (!NombreUsuarioDisponible(nuevo.Nombre))
            {
                mensaje = "El nombre de usuario ya está en uso.";
                return false;
            }

            return dal.RegistrarUsuario(nuevo);
        }

        public List<Usuario> ObtenerUsuarios()
        {
            return dal.Listar();
        }

        public bool NombreUsuarioDisponible(string nombre)
        {
            return !dal.ExisteNombreUsuario(nombre);
        }

        private string ValidarUsuario(Usuario u)
        {
            if (u == null)
                return "El usuario no puede ser nulo.";

            if (string.IsNullOrWhiteSpace(u.Nombre))
                return "El nombre de usuario es obligatorio.";

            if (string.IsNullOrWhiteSpace(u.Clave))
                return "La clave es obligatoria.";

            if (string.IsNullOrWhiteSpace(u.Rol))
                return "Debe especificar el rol.";

            return string.Empty;
        }
        public bool ExportarUsuariosAXml(out string mensaje)
        {

            var usuarios = dal.Listar();

            if (usuarios == null || usuarios.Count == 0)
            {
                mensaje = "No hay usuarios para exportar.";
                return false;
            }

            try
            {

                var xmlManager = new XmlManager<Usuario>("usuarios.xml");


                bool exito = xmlManager.Guardar(usuarios);

                if (exito)
                {
                    mensaje = "Usuarios exportados a 'usuarios.xml' correctamente.";
                    return true;
                }
                else
                {
                    mensaje = "Error desconocido al intentar guardar el archivo XML de usuarios.";
                    return false;
                }
            }
            catch (Exception ex)
            {

                mensaje = $"Error de sistema al exportar los usuarios: {ex.Message}";
                return false;
            }
        }
        public List<Usuario> ImportarUsuariosDesdeXml()
        {
            var xmlManager = new XmlManager<Usuario>("usuarios.xml");
            return xmlManager.Cargar();
        }
    }
}
