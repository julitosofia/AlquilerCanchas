using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class Reserva
    {
        public int IdReserva { get; set; }
        public int IdCancha { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public string NombreCancha { get; set; }
        public int IdUsuario { get; set; }
        public string NombreCliente { get; set; }
    }
}