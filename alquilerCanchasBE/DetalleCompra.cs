using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class DetalleCompra
    {
        private int idProducto;
        private string nombreProducto;
        private int cantidad;
        private decimal precioUnitario;
        private string categoria;

        public int IdProducto
        {
            get => idProducto;
            set => idProducto = value > 0 ? value : throw new ArgumentException("IdProducto invalido.");
        }
        public string NombreProducto
        {
            get => nombreProducto;
            set => nombreProducto = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Nombre de producto requerido");
        }
        public int Cantidad
        {
            get => cantidad;
            set => cantidad = value > 0 ? value : throw new ArgumentException("Cantidad no valida.");
        }
        public decimal PrecioUnitario
        {
            get => precioUnitario;
            set => precioUnitario = value >= 0 ? value : throw new ArgumentException("Precio unitario invalido");
        }
        public string Categoria
        {
            get => categoria;
            set => categoria = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Categoria requerida.");
        }

        public DetalleCompra() { }

        public DetalleCompra(int idProducto, string nombreProducto, int cantidad, decimal precioUnitario, string categoria)
        {
            IdProducto = idProducto;
            NombreProducto = nombreProducto;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            Categoria = categoria;
        }
    }
}