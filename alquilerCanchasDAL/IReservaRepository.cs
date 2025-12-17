using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using alquilerCanchasBE;

namespace alquilerCanchasDAL
{
    public interface IReservaRepository : IRepository<Reserva>
    {
        List<Reserva> ObtenerReservasPorUsuario(string nombreCliente);
        List<Reserva> ObtenerReservasPorCanchaYFecha(int idCancha, DateTime fecha);
        List<Reserva> VerificarDisponibilidad(int idCancha, DateTime fecha, DateTime horaInicio, DateTime horaFin);


        int CancelarReserva(int idReserva);
        int ActualizarReserva(int idReserva, DateTime fecha, DateTime horaInicio, DateTime horaFin, decimal total, string estado);


        decimal ObtenerTarifaPorCancha(int idCancha);

        void ExportarReservasXML(List<Reserva> reservas, string rutaArchivo);
        List<Reserva> ImportarReservasXML(string rutaArchivo);

        List<Reserva> ObtenerReservasParaExportar();

    }
}