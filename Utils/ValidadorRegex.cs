using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Utils
{
    public static class ValidadorRegex
    {
        public static bool EsUsuarioValido(string nombre)
        {
            return Regex.IsMatch(nombre, @"^[a-zA-Z0-9]{4,20}$");
        }

        public static bool EsClaveSegura(string clave)
        {
            return Regex.IsMatch(clave, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{6,}$");
        }

        public static bool EsNombreClienteValido(string nombre)
        {
            return Regex.IsMatch(nombre, @"^[A-ZÁÉÍÓÚÑ][a-záéíóúñ]+( [A-ZÁÉÍÓÚÑ][a-záéíóúñ]+)*$");
        }

        public static bool EsEmailValido(string email)
        {
            return Regex.IsMatch(email, @"^[\w\.-]+@[\w\.-]+\.\w{2,4}$");
        }

        public static bool EsCategoriaValida(string categoria)
        {
            return Regex.IsMatch(categoria, @"^[A-Za-z\s]{3,30}$");
        }


    }
}