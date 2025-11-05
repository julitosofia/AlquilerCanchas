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
        SqlDataReader VerificarDisponibilidad(int idCancha, DateTime fecha, DateTime inicio, DateTime fin);
        decimal ObtenerTarifaPorCancha(int idCancha);
        SqlDataReader ObtenerReservasPorCanchaYFecha(int idCancha, DateTime fecha);
        int CancelarReserva(int idReserva);

    }
}