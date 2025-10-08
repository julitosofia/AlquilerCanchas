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

        public List<Cancha>Listar()
        {
            var lista = new List<Cancha>();
            using(SqlConnection cn = new SqlConnection(conexion.CadenaConexion))
            {
                cn.Open();
                using(SqlCommand cmd = new SqlCommand("SP_ListarCancha",cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            lista.Add(new Cancha
                            {
                                IdCancha = (int)dr["IdCancha"],
                                Nombre = dr["Nombre"].ToString(),
                                Tipo = dr["Tipo"].ToString(),
                                PrecioHora = (decimal)dr["PrecioHora"]
                            });
                        }
                    }
                }
                return lista;
            }
        }
        public Cancha ObtenerPorId(int id)
        {
            var parametros = new List<SqlParameter> { new SqlParameter("@IdCancha", id) };
            using(var reader= conexion.EjecutarReader("SP_ObtenerCanchaPorId",parametros))
            {
                if(reader.Read())
                {
                    return new Cancha
                    {
                        IdCancha = (int)reader["IdCancha"],
                        Nombre = reader["Nombre"].ToString(),
                        Tipo = reader["Tipo"].ToString(),
                        PrecioHora = (decimal)reader["PrecioHora"]
                    };
                }
            }
            return null;
        }
        public bool Insertar(Cancha cancha)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@Nombre",cancha.Nombre),
                new SqlParameter("@Tipo",cancha.Tipo),
                new SqlParameter("@PrecioHora",cancha.PrecioHora)
            };
            return conexion.EjecutarNonQuery("SP_InsertarCancha", parametros) > 0;
        }
        public bool Actualizar(Cancha cancha)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@IdCancha",cancha.IdCancha),
                new SqlParameter("@Nombre",cancha.Nombre),
                new SqlParameter("@Tipo",cancha.Tipo),
                new SqlParameter("@PrecioHora",cancha.PrecioHora)
            };
            return conexion.EjecutarNonQuery("SP_ActualizarCancha", parametros) > 0;
        }
        public bool Eliminar(int id)
        {
            var parametros = new List<SqlParameter> { new SqlParameter("@IdCancha", id) };
            return conexion.EjecutarNonQuery("SP_EliminarCancha", parametros) > 0;
        }
    }
}