using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class Producto
    {
        private int idProducto;
        private string nombre;
        private decimal precio;
        private int stock;
        private string categoria;

        public int IdProducto
        {
            get => idProducto;
            set => idProducto = value >= 0 ? value : throw new ArgumentException("IdProducto invalido.");
        }
        public string Nombre
        {
            get => nombre;
            set => nombre = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Nombre requerido");
        }
        public decimal Precio
        {
            get => precio;
            set => precio = value >= 0 ? value : throw new ArgumentException("Precio invalido.");
        }
        public int Stock
        {
            get => stock;
            set => stock = value >= 0 ? value : throw new ArgumentException("Stock invalido.");
        }
        public string Categoria
        {
            get => categoria;
            set => categoria = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Categoria invalido.");
        }
        public Producto()
        {

        }
        public Producto(string nombre,decimal precio, int stock, string categoria)
        {
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
            Categoria = categoria;
        }
    }
}