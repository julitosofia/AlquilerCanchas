using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class LoginResultado
    {
        public bool EsValido { get; set; }
        public string Mensaje { get; set; }
        public Usuario Usuario { get; set; }

    }
}