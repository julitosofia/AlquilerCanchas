using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class VentaDetalle
    {
        private int idDetalle;
        private int idVenta;
        private int idProducto;
        private int cantidad;
        private decimal precioUnitario;
        private string nombreProducto;

        public int IdDetalle
        {
            get => idDetalle;
            set => idDetalle = value >= 0 ? value : throw new ArgumentException("IdDetalle invalido");
        }
        public int IdVenta
        {
            get => idVenta;
            set => idVenta = value > 0 ? value : throw new ArgumentException("IdVenta requerido");
        }
        public int IdProducto
        {
            get => IdProducto;
            set => IdProducto = value > 0 ? value : throw new ArgumentException("IdProducto requerido");
        }
        public int Cantidad
        {
            get => cantidad;
            set => cantidad = value > 0 ? value : throw new ArgumentException("Cantidad debe ser mayor a cero");
        }
        public decimal PrecioUnitario
        {
            get => precioUnitario;
            set => precioUnitario = value >= 0 ? value : throw new ArgumentException("Precio invalido.");
        }
        public string NombreProducto
        {
            get => nombreProducto;
            set => nombreProducto = value ?? string.Empty;
        }
        public VentaDetalle() { }

        public VentaDetalle(int idProducto, int cantidad, decimal precioUnitario )
        {
            IdProducto = idProducto;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }
    }
}