using alquilerCanchasBE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;

namespace alquilerCanchasDAL
{
    public class CanchaDAL : IRepository<Cancha>
    {
        private readonly ConexionDAL conexion = new ConexionDAL();
        public CanchaDAL(ConexionDAL _conexion)
        {
            conexion = _conexion;
        }
        public List<Cancha>Listar()
        {
            var lista = new List<Cancha>();
            var dr = conexion.EjecutarReader("SP_ListarCancha", null);
            try
            {
                while(dr.Read())
                {
                    lista.Add(MapearCancha(dr));
                }
            }
            finally
            {
                if(dr!=null && !dr.IsClosed)
                {
                    dr.Close();
                }
            }
            return lista;
        }

        public Cancha ObtenerPorId(int id)
        {
            Cancha cancha = null;
            var parametros = new List<SqlParameter> { new SqlParameter("@IdCancha", id) };
            var reader = conexion.EjecutarReader("SP_ObtenerCanchaPorId", parametros);
            try
            {
                if(reader.Read())
                {
                    cancha = MapearCancha(reader);
                }
            }
            finally
            {
                if(reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return cancha;
        }
        public bool Insertar(Cancha cancha)
            => conexion.EjecutarNonQuery("SP_InsertarCancha", new List<SqlParameter>
            {
                new SqlParameter("@Nombre", cancha.Nombre),
                new SqlParameter("@Tipo", cancha.Tipo),
                new SqlParameter("@PrecioHora", cancha.PrecioHora)
            }) > 0;
        public bool Actualizar(Cancha cancha)
            => conexion.EjecutarNonQuery("SP_ActualizarCancha", new List<SqlParameter>
            {
                new SqlParameter("@IdCancha",cancha.IdCancha),
                new SqlParameter("@Nombre", cancha.Nombre),
                new SqlParameter("@Tipo", cancha.Tipo),
                new SqlParameter("@PrecioHora",cancha.PrecioHora)
            }) > 0;
        public bool Eliminar(int id)
            => conexion.EjecutarNonQuery("SP_EliminarCancha", new List<SqlParameter>
            {
                new SqlParameter("@IdCancha", id)
            }) > 0;

        private Cancha MapearCancha(SqlDataReader dr)
        {
            return new Cancha
            {
                IdCancha = (int)dr["IdCancha"],
                Nombre = dr["Nombre"].ToString(),
                Tipo = dr["Tipo"].ToString(),
                PrecioHora = (decimal)dr["PrecioHora"]
            };
        }
    }
}