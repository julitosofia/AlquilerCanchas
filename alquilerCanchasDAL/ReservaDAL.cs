using alquilerCanchasBE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace alquilerCanchasDAL
{
    public class ReservaDAL : IReservaRepository
    {
        private readonly SqlConnection conexion;
        private readonly SqlTransaction transaccion;

        public ReservaDAL(SqlConnection conexion,SqlTransaction transaccion)
        {
            this.conexion = conexion;
            this.transaccion = transaccion;
        }
        public ReservaDAL()
        {
            var connDal = new ConexionDAL();
            this.conexion = new SqlConnection(connDal.CadenaConexion);
            this.conexion.Open();
        }

        public List<Reserva> Listar()
        {
            var lista = new List<Reserva>();
            using (var cmd = new SqlCommand("SP_ListarReserva",conexion,transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Reserva
                        {
                            IdReserva = (int)reader["IdReserva"],
                            IdCancha = (int)reader["IdCancha"],
                            IdUsuario = (int)reader["IdUsuario"],
                            Cliente = reader["Cliente"].ToString(),
                            Fecha = (DateTime)reader["Fecha"],
                            HoraInicio = (DateTime)reader["HoraInicio"],
                            HoraFin = (DateTime)reader["HoraFin"],
                            Total = (decimal)reader["Total"],
                            Estado = reader["Estado"].ToString()
                        });
                    }
                }

            }
            return lista;
        }
        public int ActualizarReserva(int idReserva, DateTime fecha, DateTime horaInicio,DateTime horaFin, decimal total, string estado)
        {
            using(var cmd = new SqlCommand("SP_ActualizarReserva",conexion,transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdReserva", idReserva);
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@HoraInicio", horaInicio);
                cmd.Parameters.AddWithValue("@HoraFin", horaFin);
                cmd.Parameters.AddWithValue("@Total", total);
                cmd.Parameters.AddWithValue("@Estado", estado);
                return cmd.ExecuteNonQuery();
            }
        }
        public int CancelarReserva(int idReserva)
        {
            using(var cmd = new SqlCommand("SP_CancelarReserva",conexion,transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdReserva", idReserva);
                return cmd.ExecuteNonQuery();
            }
        }
        public SqlDataReader VerificarDisponibilidad(int idCancha, DateTime fecha, DateTime horaInicio, DateTime horaFin)
        {
            using (var cmd = new SqlCommand("SP_VerificarDisponibilidadReserva", conexion, transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCancha", idCancha);
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                cmd.Parameters.AddWithValue("@HoraInicio", horaInicio);
                cmd.Parameters.AddWithValue("@HoraFin", horaFin);
                return cmd.ExecuteReader();
            }

        }
        public SqlDataReader ObtenerReservasPorCanchaYFecha(int idCancha, DateTime fecha )
        {
            using (var cmd = new SqlCommand("SP_ObtenerReservasPorCanchaYFecha", conexion, transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCancha", idCancha);
                cmd.Parameters.AddWithValue("@Fecha", fecha);
                return cmd.ExecuteReader();
            }
        }
        public bool Insertar(Reserva r)
        {
            using (var cmd = new SqlCommand("SP_InsertaarReservas", conexion, transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCancha", r.IdCancha);
                cmd.Parameters.AddWithValue("@IdUsuario", r.IdUsuario);
                cmd.Parameters.AddWithValue("@Cliente", r.Cliente);
                cmd.Parameters.AddWithValue("@Fecha", r.Fecha);
                cmd.Parameters.AddWithValue("@Horainicio", r.HoraInicio);
                cmd.Parameters.AddWithValue("@HoraFin", r.HoraFin);
                cmd.Parameters.AddWithValue("@Total", r.Total);
                cmd.Parameters.AddWithValue("@Estado", r.Estado);
                return cmd.ExecuteNonQuery() > 0;
            }

        }
        public decimal ObtenerTarifaPorCancha(int idCancha)
        {
            using (var cmd = new SqlCommand("SP_ObtenerTarifaPorCancha", conexion, transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCancha", idCancha);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0m;
            }
        }
        public List<Reserva> ObtenerReservasPorUsuario(int idUsuario)
        {
            var lista = new List<Reserva>();
            using (var cmd = new SqlCommand("SP_ObtenerReservasPorUsuarios", conexion, transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                using (var dr = cmd.ExecuteReader())
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
        public Reserva ObtenerPorId(int id)
        {
            // Si no tenés el SP, devolvé null o lanzá NotImplementedException
            throw new NotImplementedException("Este método no está implementado porque no se requiere actualmente.");
        }

        public bool Actualizar(Reserva r)
        {
            // Podés usar ActualizarReserva si querés mapearlo
            return ActualizarReserva(r.IdReserva, r.Fecha, r.HoraInicio, r.HoraFin, r.Total, r.Estado) > 0;
        }

        public bool Eliminar(int id)
        {
            // Si no tenés SP, devolvé false o lanzá excepción
            throw new NotImplementedException("Este método no está implementado porque no se requiere actualmente.");
        }


    }
}