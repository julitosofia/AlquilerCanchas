using alquilerCanchasBE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace alquilerCanchasDAL
{
    public class ReservaDAL : IReservaRepository
    {
        private readonly ConexionDAL conexion;

        public ReservaDAL(ConexionDAL conexion)
        {
            this.conexion = conexion;
        }

        private Reserva MapearReservaBase(SqlDataReader reader)
        {
            return new Reserva
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
            };
        }

        private Reserva MapearReservaDetallada(SqlDataReader dr)
        {
            var reserva = MapearReservaBase(dr);
            reserva.NombreCliente = dr["NombreCliente"].ToString();
            reserva.NombreCancha = dr["NombreCancha"].ToString();
            return reserva;
        }

        public List<Reserva> Listar()
        {
            var lista = new List<Reserva>();
            var reader = conexion.EjecutarReader("SP_ListarReserva", null);
            try
            {
                while (reader.Read())
                {
                    lista.Add(MapearReservaDetallada(reader));
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
            }
            return lista;
        }

        public List<Reserva> ObtenerReservasPorUsuario(string nombreCliente)
        {
            var lista = new List<Reserva>();
            var parametros = new List<SqlParameter> { new SqlParameter("@NombreCliente", nombreCliente) };
            var dr = conexion.EjecutarReader("SP_ObtenerReservasPorUsuario", parametros);
            try
            {
                while (dr.Read())
                {
                    lista.Add(MapearReservaBase(dr));
                }
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
            }
            return lista;
        }

        public List<Reserva> ObtenerReservasPorCanchaYFecha(int idCancha, DateTime fecha)
        {
            var lista = new List<Reserva>();
            var reader = conexion.EjecutarReader("SP_ObtenerReservasPorCanchaYFecha", new List<SqlParameter>
        {
            new SqlParameter("@IdCancha", idCancha),
            new SqlParameter("@Fecha", fecha)
        });

            try
            {
                while (reader.Read())
                {
                    lista.Add(MapearReservaBase(reader));
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
            }

            return lista;
        }
        public List<Reserva> VerificarDisponibilidad(int idCancha, DateTime fecha, DateTime horaInicio, DateTime horaFin)
        {
            var lista = new List<Reserva>();
            var reader = conexion.EjecutarReader("SP_VerificarDisponibilidadReserva", new List<SqlParameter>
        {
            new SqlParameter("@IdCancha", idCancha),
            new SqlParameter("@Fecha", fecha),
            new SqlParameter("@HoraInicio", horaInicio),
            new SqlParameter("@HoraFin", horaFin)
        });

            try
            {
                while (reader.Read())
                {
                    lista.Add(MapearReservaBase(reader));
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
            }

            return lista;
        }

        public decimal ObtenerTarifaPorCancha(int idCancha)
        {
            object result = conexion.EjecutarEscalarTransaccion("SP_ObtenerTarifaPorCancha", new List<SqlParameter>
        {
            new SqlParameter("@IdCancha", idCancha)
        });
            return result != null ? Convert.ToDecimal(result) : 0m;
        }

        public bool Actualizar(Reserva r)
            => ActualizarReserva(r.IdReserva, r.Fecha, r.HoraInicio, r.HoraFin, r.Total, r.Estado) > 0;

        public int ActualizarReserva(int idReserva, DateTime fecha, DateTime horaInicio, DateTime horaFin, decimal total, string estado)
            => conexion.EjecutarNonQueryTransacciones("SP_ActualizarReserva", new List<SqlParameter>
            {
            new SqlParameter("@IdReserva", idReserva),
            new SqlParameter("@Fecha", fecha),
            new SqlParameter("@HoraInicio", horaInicio),
            new SqlParameter("@HoraFin", horaFin),
            new SqlParameter("@Total", total),
            new SqlParameter("@Estado", estado)
            });

        public int CancelarReserva(int idReserva)
            => conexion.EjecutarNonQueryTransacciones("SP_CancelarReserva", new List<SqlParameter>
            {
            new SqlParameter("@IdReserva", idReserva)
            });

        public bool Insertar(Reserva r)
            => conexion.EjecutarNonQueryTransacciones("SP_InsertaarReservas", new List<SqlParameter>
            {
            new SqlParameter("@IdCancha", r.IdCancha),
            new SqlParameter("@IdUsuario", r.IdUsuario),
            new SqlParameter("@Cliente", r.Cliente),
            new SqlParameter("@Fecha", r.Fecha),
            new SqlParameter("@Horainicio", r.HoraInicio),
            new SqlParameter("@HoraFin", r.HoraFin),
            new SqlParameter("@Total", r.Total),
            new SqlParameter("@Estado", r.Estado)
            }) > 0;

        public Reserva ObtenerPorId(int id)
        {
            throw new NotImplementedException("Este método no está implementado porque no se requiere actualmente.");
        }

        public bool Eliminar(int id)
        {
            throw new NotImplementedException("Este método no está implementado porque no se requiere actualmente.");
        }

        public List<Reserva> ObtenerReservasParaExportar()
        {
            var lista = new List<Reserva>();
            var reader = conexion.EjecutarReader("SP_ObtenerReservasParaExportar", null);
            try
            {
                while (reader.Read())
                {
                    lista.Add(MapearReservaBase(reader));
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
            }
            return lista;
        }



        public void ExportarReservasXML(List<Reserva> reservas, string rutaArchivo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Reserva>));
            using (var writer = new StreamWriter(rutaArchivo))
            {
                serializer.Serialize(writer, reservas);
            }
        }

        public List<Reserva> ImportarReservasXML(string rutaArchivo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Reserva>));
            using (var reader = new StreamReader(rutaArchivo))
            {
                return (List<Reserva>)serializer.Deserialize(reader);
            }
        }


    }
}