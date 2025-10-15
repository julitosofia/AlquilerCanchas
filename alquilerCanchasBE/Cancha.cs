using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class Cancha
    {
        private int idCancha;
        private string nombre;
        private string tipo;
        private decimal precioHora;

        public int IdCancha
        {
            get => idCancha;
            set => idCancha = value > 0 ? value : throw new ArgumentException("IdCancha invalido.");
        }
        public string Nombre
        {
            get => nombre;
            set => nombre = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Nombre no puede estar vacio.");
        }
        public string Tipo
        {
            get => tipo;
            set => tipo = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Tipo invalido.");
        }
        public decimal PrecioHora
        {
            get => precioHora;
            set => precioHora = value >= 0 ? value : throw new ArgumentException("Precio hora invalido.");
        }
        public Cancha() { }
        public Cancha(string nombre, string tipo, decimal precioHora)
        {
            Nombre = nombre;
            Tipo = tipo;
            PrecioHora = precioHora;
        }
    }
}