using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public List<DetalleCompra>Detalles { get; set; }
    }
}