using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class Cancha
    {
        public int IdCancha { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal PrecioHora { get; set; }
    }
}