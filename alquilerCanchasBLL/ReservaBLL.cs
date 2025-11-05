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

        public List<Reserva> ObtenerReservas()
        {
            return reservaRepo.Listar();
        }

        public List<Reserva> ObtenerReservasPorUsuario(string nombreCliente)
        {
            return reservaRepo.ObtenerReservasPorUsuario(nombreCliente);
        }

        public bool TurnoDisponible(int idCancha, DateTime fecha, DateTime inicio, DateTime fin)
        {
            var lista = new List<Reserva>();
            var reader = reservaRepo.ObtenerReservasPorCanchaYFecha(idCancha, fecha);

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

            reader.Close();

            return !lista.Any(r => (inicio < r.HoraFin) && (fin > r.HoraInicio));
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

        public decimal CalcularPrecio(int idCancha, DateTime inicio, DateTime fin)
        {
            decimal tarifaPorHora = reservaRepo.ObtenerTarifaPorCancha(idCancha);
            double horas = (fin - inicio).TotalHours;
            return tarifaPorHora * (decimal)horas;
        }

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

        public bool RegistrarReserva(Reserva nueva)
        {
            nueva.Estado = "ACTIVA";
            return reservaRepo.Insertar(nueva);
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

        public bool ExportarReservasAXml(out string mensaje)
        {
            var reservas = reservaRepo.Listar();
            if(reservas == null || reservas.Count == 0)
            {
                mensaje = "No hay reservas para exportar";
                return false;
            }
            try
            {
                var xmlManager = new XmlManager<Reserva>("reservas.xml");
                bool exito = xmlManager.Guardar(reservas);
                if(exito)
                {
                    mensaje = "Reservas exportadas a 'reservas.xml' correctamente.";
                    return true;
                }
                else
                {
                    mensaje = "Error desconocido al intentar guardar el archivo XML de reservas.";
                    return false;
                }
            }
            catch(Exception ep)
            {
                mensaje = $"Error de sistema al exportar las reservas: {ep.Message}";
                return false;
            }
        }
        public List<Reserva> ImportarReservasDesdeXml()
        {
            var xmlManager = new XmlManager<Reserva>("reservas.xml");
            return xmlManager.Cargar();
        }
    }
}