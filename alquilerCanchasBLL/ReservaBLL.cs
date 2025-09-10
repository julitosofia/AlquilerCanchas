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
        private ReservaDAL dal = new ReservaDAL();

        public bool ReservarCancha(Reserva r, out string mensaje)
        {
            // Validación de disponibilidad
            bool disponible = dal.VerificarDisponibilidad(
                r.IdCancha,
                r.Fecha,
                r.HoraInicio,
                r.HoraFin);

            if (!disponible)
            {
                mensaje = "La cancha ya está reservada en ese horario.";
                return false;
            }

            // Validación de duplicado por usuario
            if (YaTieneReserva(r.IdUsuario, r.Fecha, r.HoraInicio, r.HoraFin))
            {
                mensaje = "Ya tenés una reserva en ese horario.";
                return false;
            }

            r.Estado = "ACTIVA";
            bool ok = dal.Insertar(r);
            mensaje = ok ? "Reserva confirmada." : "Error al registrar la reserva.";
            return ok;
        }

        public List<Reserva> ObtenerReservas() => dal.Listar();

        public bool TurnoDisponible(int idCancha, DateTime fecha, DateTime inicio, DateTime fin)
        {
            var reservas = dal.ObtenerReservasPorCanchaYFecha(idCancha, fecha)
                              .Where(r => r.Estado == "ACTIVA")
                              .ToList();

            foreach (var r in reservas)
            {
                if ((inicio < r.HoraFin) && (fin > r.HoraInicio))
                {
                    return false;
                }
            }
            return true;
        }

        public decimal CalcularPrecio(int idCancha, DateTime inicio, DateTime fin)
        {
            decimal tarifaPorHora = dal.ObtenerTarifaPorCancha(idCancha);
            double horas = (fin - inicio).TotalHours;
            return tarifaPorHora * (decimal)horas;
        }

        public bool RegistrarReserva(Reserva nueva)
        {
            nueva.Estado = "ACTIVA";
            return dal.Insertar(nueva);
        }

        public List<Reserva> ObtenerReservasPorUsuario(int idUsuario)
        {
            return dal.ObtenerReservasPorUsuario(idUsuario);
        }

        public bool YaTieneReserva(int idUsuario, DateTime fecha, DateTime inicio, DateTime fin)
        {
            var reservas = ObtenerReservasPorUsuario(idUsuario)
                           .Where(r => r.Estado == "ACTIVA" && r.Fecha.Date == fecha.Date)
                           .ToList();

            return reservas.Any(r =>
                (inicio >= r.HoraInicio && inicio < r.HoraFin) ||
                (fin > r.HoraInicio && fin <= r.HoraFin) ||
                (inicio <= r.HoraInicio && fin > r.HoraFin));
        }

        public bool CancelarReserva(int idReserva)
        {
            return dal.CancelarReserva(idReserva);
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

            bool ok = dal.CancelarReserva(idReserva);
            mensaje = ok ? "Reserva cancelada correctamente." : "Error al cancelar la reserva.";
            return ok;
        }
    }
}