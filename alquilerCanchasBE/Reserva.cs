using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasBE
{
    public class Reserva
    {
        private int idReserva;
        private int idCancha;
        private string cliente;
        private DateTime fecha;
        private DateTime horaInicio;
        private DateTime horaFin;
        private decimal total;
        private string estado;
        private string nombreCancha;
        private int idUsuario;
        private string nombreCliente;

        public int IdReserva
        {
            get => idReserva;
            set => idReserva = value >= 0 ? value : throw new ArgumentException("IdReserva invalido.");
        }
        public int IdCancha
        {
            get => idCancha;
            set => idCancha = value > 0 ? value : throw new ArgumentException("IdCancha requerido");
        }
        public string Cliente
        {
            get => cliente;
            set => cliente = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Cliente requerido");
        }
        public DateTime Fecha
        {
            get => fecha;
            set => fecha = value;
        }
        public DateTime HoraInicio
        {
            get => horaInicio;
            set => horaInicio = value;
        }
        public DateTime HoraFin
        {
            get => horaFin;
            set => horaFin = value;
        }
        public decimal Total
        {
            get => total;
            set => total = value >= 0 ? value : throw new ArgumentException("Total invalido");
        }
        public string Estado
        {
            get => estado;
            set => estado = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException("Estado requerido");
        }
        public string NombreCancha
        {
            get => nombreCancha;
            set => nombreCancha = value ?? string.Empty;
        }
        public int IdUsuario
        {
            get => idUsuario;
            set => idUsuario = value > 0 ? value : throw new ArgumentException("IdUsuario requerido");
        }
        public string NombreCliente
        {
            get => nombreCliente;
            set => nombreCliente = value ?? string.Empty;
        }

        public Reserva() { }

        public Reserva(int idCancha, string cliente, DateTime fecha, DateTime horaInicio, DateTime horaFin, decimal total, string estado, int idUsuario)
        {
            IdCancha = idCancha;
            Cliente = cliente;
            Fecha = fecha;
            HoraInicio = horaInicio;
            HoraFin = horaFin;
            Total = total;
            Estado = estado;
            IdUsuario = idUsuario;
        }


    }
}