using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class LoginResultado
    {
        private bool esValido;
        private string mensaje;
        private Usuario usuario;

        public bool EsValido
        {
            get => esValido;
            set => esValido = value;
        }
        public string Mensaje
        {
            get => mensaje;
            set => mensaje = value ?? string.Empty;
        }
        public Usuario Usuario
        {
            get => usuario;
            set => usuario = value ?? new Usuario();
        }
        public LoginResultado() { }
        public LoginResultado(bool esValido, string mensaje,Usuario usuario)
        {
            EsValido = esValido;
            Mensaje = mensaje;
            Usuario = usuario;
        }
    }
}