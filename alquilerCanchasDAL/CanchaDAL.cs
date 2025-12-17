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
    public class CanchaDAL : IRepository<Cancha>
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public CanchaDAL(ConexionDAL _conexion)
        {
            conexion = _conexion;
        }

        // --- Métodos SQL (ya los tenés) ---
        public List<Cancha> Listar()
        {
            var lista = new List<Cancha>();
            var dr = conexion.EjecutarReader("SP_ListarCancha", null);
            try
            {
                while (dr.Read())
                {
                    lista.Add(MapearCancha(dr));
                }
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                    dr.Close();
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
                if (reader.Read())
                {
                    cancha = MapearCancha(reader);
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            return cancha;
        }
            public List<Cancha> ObtenerTodas()
    {
        var lista = new List<Cancha>();
        var reader = conexion.EjecutarReader("SP_ObtenerTodasLasCanchas", null);
        try
        {
            while (reader.Read())
            {
                lista.Add(MapearCancha(reader));
            }
        }
        finally
        {
            if (reader != null && !reader.IsClosed)
                reader.Close();
        }
        return lista;
    }



        public bool Insertar(Cancha cancha) =>
            conexion.EjecutarNonQuery("SP_InsertarCancha", new List<SqlParameter>
            {
            new SqlParameter("@Nombre", cancha.Nombre),
            new SqlParameter("@Tipo", cancha.Tipo),
            new SqlParameter("@PrecioHora", cancha.PrecioHora)
            }) > 0;

        public bool Actualizar(Cancha cancha) =>
            conexion.EjecutarNonQuery("SP_ActualizarCancha", new List<SqlParameter>
            {
            new SqlParameter("@IdCancha", cancha.IdCancha),
            new SqlParameter("@Nombre", cancha.Nombre),
            new SqlParameter("@Tipo", cancha.Tipo),
            new SqlParameter("@PrecioHora", cancha.PrecioHora)
            }) > 0;

        public bool Eliminar(int id) =>
            conexion.EjecutarNonQuery("SP_EliminarCancha", new List<SqlParameter>
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

        // --- Métodos XML en DAL ---
        public void ExportarCanchaXML(List<Cancha> lista, string rutaArchivo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Cancha>));
            using (var writer = new StreamWriter(rutaArchivo))
            {
                serializer.Serialize(writer, lista);
            }
        }

        public List<Cancha> ImportarCanchaXML(string rutaArchivo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Cancha>));
            using (var reader = new StreamReader(rutaArchivo))
            {
                return (List<Cancha>)serializer.Deserialize(reader);
            }
        }
    }



}