using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class Venta
    {
        private int idVenta;
        private DateTime fecha;
        private int idUsuario;
        private decimal total;
        private string nombreUsuario;

        public int IdVenta
        {
            get => idVenta;
            set => idVenta = value >= 0 ? value : throw new ArgumentException("IdVenta invalido.");
        }
        public DateTime Fecha
        {
            get => fecha;
            set => fecha = value;
        }
        public int IdUsuario
        {
            get => idUsuario;
            set => idUsuario = value > 0 ? value : throw new ArgumentException("IdUsuario requerido.");
        }
        public decimal Total
        {
            get => total;
            set => total = value >= 0 ? value : throw new ArgumentException("Total invalido.");
        }
        public List<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();

        public string NombreUsuario
        {
            get => nombreUsuario;
            set => nombreUsuario = value ?? string.Empty;
        }
        public Venta() { }

        public Venta(DateTime fecha, int idUsuario, decimal total)
        {
            Fecha = fecha;
            IdUsuario = idUsuario;
            Total = total;
        }
    }
}