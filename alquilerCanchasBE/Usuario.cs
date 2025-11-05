using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alquilerCanchasBE
{
    public class Usuario
    {
        private int idUsuario;
        private string nombre;
        private string rol;
        private string clave;

        public int IdUsuario
        {
            get => idUsuario;
            set => idUsuario = value > 0 ? value : throw new ArgumentException("IdUsuario invalido.");
        }
        public string Nombre
        {
            get => nombre;
            set => nombre = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Nombre invalido.");
        }
        public string Rol
        {
            get => rol;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Rol requerido");

                string upperValue = value.Trim().ToUpperInvariant();

                if (upperValue != "CLIENTE" && upperValue != "EMPLEADO")
                {
                    throw new ArgumentException("Rol invalido");
                }
                rol = upperValue;
            }
        }
        public string Clave
        {
            get => clave;
            set => clave = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Clave requerido");
        }

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();

        public Usuario() { }
        public Usuario(string nombre, string rol, string clave)
        {
            Nombre = nombre;
            Rol = rol;
            Clave = clave;
        }
    }
}
