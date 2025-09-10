using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsuario { get; set; }
        public decimal Total { get; set; }

        public List<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();
        public string NombreUsuario { get; set; }
    }
}