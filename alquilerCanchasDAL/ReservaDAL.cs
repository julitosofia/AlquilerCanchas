using alquilerCanchasBE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace alquilerCanchasDAL
{
    public class ReservaDAL
    {
        public List<Reserva> Listar()
        {
            var lista = new List<Reserva>();
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = @"SELECT r.*, c.Nombre AS NombreCancha, u.Nombre AS NombreCliente
                             FROM Reserva r
                             JOIN Cancha c ON r.IdCancha = c.IdCancha
                             JOIN Usuario u ON r.IdUsuario = u.IdUsuario";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Reserva
                        {
                            IdReserva = (int)dr["IdReserva"],
                            IdCancha = (int)dr["IdCancha"],
                            IdUsuario = (int)dr["IdUsuario"],
                            NombreCliente = dr["NombreCliente"].ToString(),
                            Fecha = (DateTime)dr["Fecha"],
                            HoraInicio = (DateTime)dr["HoraInicio"],
                            HoraFin = (DateTime)dr["HoraFin"],
                            Total = (decimal)dr["Total"],
                            NombreCancha = dr["NombreCancha"].ToString(),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public bool Insertar(Reserva r)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = @"INSERT INTO Reserva (IdReserva,IdCancha, IdUsuario,Cliente, Fecha, HoraInicio, HoraFin, Total, Estado)
                             VALUES (@IdReserva,@IdCancha, @IdUsuario,@Cliente, @Fecha, @HoraInicio, @HoraFin, @Total, @Estado)";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdReserva", r.IdReserva);
                cmd.Parameters.AddWithValue("@IdCancha", r.IdCancha);
                cmd.Parameters.AddWithValue("@IdUsuario", r.IdUsuario);
                cmd.Parameters.AddWithValue("@Cliente", r.Cliente);
                cmd.Parameters.AddWithValue("@Fecha", r.Fecha);
                cmd.Parameters.AddWithValue("@HoraInicio", r.HoraInicio);
                cmd.Parameters.AddWithValue("@HoraFin", r.HoraFin);
                cmd.Parameters.AddWithValue("@Total", r.Total);
                cmd.Parameters.AddWithValue("@Estado", r.Estado);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Actualizar(Reserva r)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = @"UPDATE Reserva SET IdCancha = @IdCancha, IdUsuario = @IdUsuario, Fecha = @Fecha,
                             HoraInicio = @HoraInicio, HoraFin = @HoraFin, Total = @Total, Estado = @Estado
                             WHERE IdReserva = @IdReserva";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdCancha", r.IdCancha);
                cmd.Parameters.AddWithValue("@IdUsuario", r.IdUsuario);
                cmd.Parameters.AddWithValue("@Fecha", r.Fecha);
                cmd.Parameters.AddWithValue("@HoraInicio", r.HoraInicio);
                cmd.Parameters.AddWithValue("@HoraFin", r.HoraFin);
                cmd.Parameters.AddWithValue("@Total", r.Total);
                cmd.Parameters.AddWithValue("@Estado", r.Estado);
                cmd.Parameters.AddWithValue("@IdReserva", r.IdReserva);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Eliminar(int idReserva)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "DELETE FROM Reserva WHERE IdReserva = @IdReserva";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdReserva", idReserva);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CancelarReserva(int idReserva)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "UPDATE Reserva SET Estado = 'CANCELADA' WHERE IdReserva = @IdReserva";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdReserva", idReserva);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool VerificarDisponibilidad(int idCancha, DateTime fecha, DateTime inicio, DateTime fin)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = @"SELECT COUNT(*) FROM Reserva
                             WHERE IdCancha = @id AND Fecha = @f AND Estado = 'ACTIVA'
                             AND ((HoraInicio < @fin AND HoraFin > @inicio))";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id", idCancha);
                    cmd.Parameters.AddWithValue("@f", fecha.Date);
                    cmd.Parameters.AddWithValue("@inicio", inicio);
                    cmd.Parameters.AddWithValue("@fin", fin);
                    int count = (int)cmd.ExecuteScalar();
                    return count == 0;
                }
            }
        }

        public List<Reserva> ObtenerReservasPorCanchaYFecha(int idCancha, DateTime fecha)
        {
            var lista = new List<Reserva>();
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = @"SELECT r.*, u.Nombre AS NombreCliente
                             FROM Reserva r
                             JOIN Usuario u ON r.IdUsuario = u.IdUsuario
                             WHERE r.IdCancha = @id AND r.Fecha = @fecha AND r.Estado = 'ACTIVA'";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id", idCancha);
                    cmd.Parameters.AddWithValue("@fecha", fecha.Date);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Reserva
                            {
                                IdReserva = (int)dr["IdReserva"],
                                IdCancha = (int)dr["IdCancha"],
                                IdUsuario = (int)dr["IdUsuario"],
                                NombreCliente = dr["NombreCliente"].ToString(),
                                Fecha = (DateTime)dr["Fecha"],
                                HoraInicio = (DateTime)dr["HoraInicio"],
                                HoraFin = (DateTime)dr["HoraFin"],
                                Total = (decimal)dr["Total"],
                                Estado = dr["Estado"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public decimal ObtenerTarifaPorCancha(int idCancha)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "SELECT PrecioHora FROM Cancha WHERE IdCancha = @id";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id", idCancha);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToDecimal(result) : 0m;
                }
            }
        }

        public List<Reserva> ObtenerReservasPorUsuario(int idUsuario)
        {
            var lista = new List<Reserva>();
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = @"SELECT r.*, c.Nombre AS NombreCancha, u.Nombre AS NombreCliente
                             FROM Reserva r
                             JOIN Cancha c ON r.IdCancha = c.IdCancha
                             JOIN Usuario u ON r.IdUsuario = u.IdUsuario
                             WHERE r.IdUsuario = @idUsuario";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Reserva
                            {
                                IdReserva = (int)dr["IdReserva"],
                                IdCancha = (int)dr["IdCancha"],
                                IdUsuario = (int)dr["IdUsuario"],
                                NombreCliente = dr["NombreCliente"].ToString(),
                                Fecha = (DateTime)dr["Fecha"],
                                HoraInicio = (DateTime)dr["HoraInicio"],
                                HoraFin = (DateTime)dr["HoraFin"],
                                Total = (decimal)dr["Total"],
                                NombreCancha = dr["NombreCancha"].ToString(),
                                Estado = dr["Estado"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}