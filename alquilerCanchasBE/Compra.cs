using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class Compra
    {
        private int idCompra;
        private string cliente;
        private DateTime fecha;

        public int IdCompra
        {
            get => idCompra;
            set => idCompra = value >= 0 ? value : throw new ArgumentException("IdCompra invalido.");
        }
        public string Cliente
        {
            get => cliente;
            set => cliente = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Cliente invalido");
        }
        public DateTime Fecha
        {
            get => fecha;
            set => fecha = value;
        }
        public List<DetalleCompra> Detalles { get; set; } = new List<DetalleCompra>();

        public Compra() { }

        public Compra(string cliente, DateTime fecha)
        {
            Cliente = cliente;
            Fecha = fecha;
        }
    }
}