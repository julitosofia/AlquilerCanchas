using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using alquilerCanchasBE;
using alquilerCanchasDAL;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Utils;

namespace alquilerCanchasBLL
{
    public class ReservaBLL
    {
        private readonly IReservaRepository reservaRepo;

        public ReservaBLL(IReservaRepository reservaRepo)
        {
            this.reservaRepo = reservaRepo;
        }

        // Listados
        public List<Reserva> ObtenerReservas()
            => reservaRepo.Listar();

        public List<Reserva> ObtenerReservasPorUsuario(string nombreCliente)
            => reservaRepo.ObtenerReservasPorUsuario(nombreCliente);

        // Reglas de disponibilidad sin SqlDataReader (DAL devuelve listas)
        public bool TurnoDisponible(int idCancha, DateTime fecha, DateTime inicio, DateTime fin)
        {
            var reservas = reservaRepo.ObtenerReservasPorCanchaYFecha(idCancha, fecha)
                                      .Where(r => r.Estado == "ACTIVA")
                                      .ToList();

            // Hay solapamiento si inicio < fin_reserva y fin > inicio_reserva
            return !reservas.Any(r => (inicio < r.HoraFin) && (fin > r.HoraInicio));
        }

        public bool YaTieneReserva(string nombreCliente, DateTime fecha, DateTime inicio, DateTime fin)
        {
            var reservas = reservaRepo.ObtenerReservasPorUsuario(nombreCliente)
                                      .Where(r => r.Estado == "ACTIVA" && r.Fecha.Date == fecha.Date)
                                      .ToList();

            return reservas.Any(r =>
                (inicio >= r.HoraInicio && inicio < r.HoraFin) ||
                (fin > r.HoraInicio && fin <= r.HoraFin) ||
                (inicio <= r.HoraInicio && fin > r.HoraFin));
        }

        // Precio
        public decimal CalcularPrecio(int idCancha, DateTime inicio, DateTime fin)
        {
            decimal tarifaPorHora = reservaRepo.ObtenerTarifaPorCancha(idCancha);
            double horas = (fin - inicio).TotalHours;
            return tarifaPorHora * (decimal)horas;
        }

        // RegistrarReserva pedido explícitamente (para tu error CS1061)
        public bool RegistrarReserva(Reserva nueva)
        {
            var msg = ValidarReserva(nueva);
            if (!string.IsNullOrEmpty(msg)) return false;

            // Reglas de solapamiento
            if (!TurnoDisponible(nueva.IdCancha, nueva.Fecha, nueva.HoraInicio, nueva.HoraFin))
                return false;

            if (YaTieneReserva(nueva.Cliente, nueva.Fecha, nueva.HoraInicio, nueva.HoraFin))
                return false;

            nueva.Estado = "ACTIVA";
            return reservaRepo.Insertar(nueva);
        }

        // Variante con mensaje (útil en UI)
        public bool ReservarCancha(Reserva r, out string mensaje)
        {
            mensaje = ValidarReserva(r);
            if (!string.IsNullOrEmpty(mensaje))
                return false;

            if (!TurnoDisponible(r.IdCancha, r.Fecha, r.HoraInicio, r.HoraFin))
            {
                mensaje = "La cancha ya está reservada en ese horario.";
                return false;
            }

            if (YaTieneReserva(r.Cliente, r.Fecha, r.HoraInicio, r.HoraFin))
            {
                mensaje = "Ya tenés una reserva en ese horario.";
                return false;
            }

            r.Estado = "ACTIVA";
            bool ok = reservaRepo.Insertar(r);
            mensaje = ok ? "Reserva confirmada." : "Error al registrar la reserva.";
            return ok;
        }

        // Cancelaciones
        public bool CancelarReserva(int idReserva)
            => reservaRepo.CancelarReserva(idReserva) > 0;

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

        // Validación
        private string ValidarReserva(Reserva r)
        {
            if (r == null)
                return "La reserva no puede ser nula.";

            if (r.IdCancha <= 0)
                return "Debe seleccionar una cancha.";

            if (r.IdUsuario <= 0)
                return "Usuario inválido.";

            if (string.IsNullOrWhiteSpace(r.Cliente))
                return "Debe ingresar el nombre del cliente.";

            if (r.HoraInicio >= r.HoraFin)
                return "La hora de inicio debe ser anterior a la hora de fin.";

            return string.Empty;
        }
        public List<Reserva> ObtenerReservasParaExportar()
        {
            return reservaRepo.ObtenerReservasParaExportar();
        }



        // XML delegados a DAL
        public bool ExportarReservasAXml(string rutaArchivo, out string mensaje)
        {
            var reservas = reservaRepo.ObtenerReservasParaExportar();

            if (reservas == null || reservas.Count == 0)
            {
                mensaje = "No hay reservas para exportar.";
                return false;
            }

            try
            {
                reservaRepo.ExportarReservasXML(reservas, rutaArchivo);
                mensaje = $"Reservas exportadas a '{rutaArchivo}' correctamente.";
                return true;
            }
            catch (Exception ep)
            {
                mensaje = $"Error de sistema al exportar las reservas: {ep.Message}";
                return false;
            }
        }

        public List<Reserva> ImportarReservasDesdeXml(string rutaArchivo)
        {
            var lista = reservaRepo.ImportarReservasXML(rutaArchivo);

            if (lista == null || lista.Count == 0)
                throw new InvalidOperationException("El archivo XML no contiene reservas válidas.");

            return lista;
        }


    }
}