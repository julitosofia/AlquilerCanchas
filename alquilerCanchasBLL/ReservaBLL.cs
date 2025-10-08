using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using alquilerCanchasBE;
using alquilerCanchasDAL;

namespace alquilerCanchasBLL
{
    public class ReservaBLL
    {
        private readonly IReservaRepository reservaRepo;

        public ReservaBLL(IReservaRepository reservaRepo)
        {
            this.reservaRepo = reservaRepo;
        }

        public List<Reserva> ObtenerReservas()
        {
            return reservaRepo.Listar();
        }

        public bool TurnoDisponible(int idCancha, DateTime fecha, DateTime inicio, DateTime fin)
        {
            var lista = new List<Reserva>();
            using (var reader = reservaRepo.ObtenerReservasPorCanchaYFecha(idCancha, fecha))
            {
                while (reader.Read())
                {
                    var reserva = new Reserva
                    {
                        HoraInicio = (DateTime)reader["HoraInicio"],
                        HoraFin = (DateTime)reader["HoraFin"],
                        Estado = reader["Estado"].ToString()
                    };
                    if (reserva.Estado == "ACTIVA")
                        lista.Add(reserva);
                }
            }

            return !lista.Any(r => (inicio < r.HoraFin) && (fin > r.HoraInicio));
        }

        public bool YaTieneReserva(int idUsuario, DateTime fecha, DateTime inicio, DateTime fin)
        {
            var reservas = reservaRepo.ObtenerReservasPorUsuario(idUsuario)
                                      .Where(r => r.Estado == "ACTIVA" && r.Fecha.Date == fecha.Date)
                                      .ToList();

            return reservas.Any(r =>
                (inicio >= r.HoraInicio && inicio < r.HoraFin) ||
                (fin > r.HoraInicio && fin <= r.HoraFin) ||
                (inicio <= r.HoraInicio && fin > r.HoraFin));
        }

        public decimal CalcularPrecio(int idCancha, DateTime inicio, DateTime fin)
        {
            decimal tarifaPorHora = reservaRepo.ObtenerTarifaPorCancha(idCancha);
            double horas = (fin - inicio).TotalHours;
            return tarifaPorHora * (decimal)horas;
        }

        public bool ReservarCancha(Reserva r, out string mensaje)
        {
            bool disponible = TurnoDisponible(r.IdCancha, r.Fecha, r.HoraInicio, r.HoraFin);

            if (!disponible)
            {
                mensaje = "La cancha ya está reservada en ese horario.";
                return false;
            }

            if (YaTieneReserva(r.IdUsuario, r.Fecha, r.HoraInicio, r.HoraFin))
            {
                mensaje = "Ya tenés una reserva en ese horario.";
                return false;
            }

            r.Estado = "ACTIVA";
            bool ok = reservaRepo.Insertar(r);
            mensaje = ok ? "Reserva confirmada." : "Error al registrar la reserva.";
            return ok;
        }

        public bool RegistrarReserva(Reserva nueva)
        {
            nueva.Estado = "ACTIVA";
            return reservaRepo.Insertar(nueva);
        }

        public List<Reserva> ObtenerReservasPorUsuario(int idUsuario)
        {
            return reservaRepo.ObtenerReservasPorUsuario(idUsuario);
        }

        public bool CancelarReserva(int idReserva)
        {
            return reservaRepo.CancelarReserva(idReserva) > 0;
        }

        public bool CancelarReserva(int idReserva, out string mensaje)
        {
            var reserva = ObtenerReservas().FirstOrDefault(r => r.IdReserva == idReserva);

            if (reserva == null)
            {
                mensaje = "Reserva no encontrada.";
                return false;
            }

            if (reserva.Estado != "ACTIVA")
            {
                mensaje = "La reserva ya fue cancelada.";
                return false;
            }

            if (reserva.Fecha.Date < DateTime.Today)
            {
                mensaje = "No se puede cancelar una reserva pasada.";
                return false;
            }

            bool ok = reservaRepo.CancelarReserva(idReserva) > 0;
            mensaje = ok ? "Reserva cancelada correctamente." : "Error al cancelar la reserva.";
            return ok;
        }


    }
}