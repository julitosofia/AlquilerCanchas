using alquilerCanchasBE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace alquilerCanchasDAL
{
    public class CanchaDAL
    {
        public List<Cancha> Listar()
        {
            var lista = new List<Cancha>();
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "SELECT * FROM Cancha";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
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

        public bool Insertar(Cancha cancha)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "INSERT INTO Cancha (Nombre,Tipo,PrecioHora) VALUE(@Nombre,@Tipo,@PrecioHora)";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Nombre", cancha.Nombre);
                cmd.Parameters.AddWithValue("@Tipo", cancha.Tipo);
                cmd.Parameters.AddWithValue("@PrecioHora", cancha.PrecioHora);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Actualizar(Cancha cancha)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "UPDATE Cancha SET Nombre = @Nombre, Tipo= @Tipo,PrecioHora=@PrecioHora WHERE IdCancha = @IdCancha";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Nombre", cancha.Nombre);
                cmd.Parameters.AddWithValue("@Tipo", cancha.Tipo);
                cmd.Parameters.AddWithValue("@PrecioHora", cancha.PrecioHora);
                cmd.Parameters.AddWithValue("@IdCancha", cancha.IdCancha);
                return cmd.ExecuteNonQuery() > 0;

            }
        }
        public bool Elminar(int idCancha)
        {
            using (SqlConnection cn = new SqlConnection(Conexion.Cadena))
            {
                cn.Open();
                string query = "DELETE FROM Cancha WHERE IdCancha = @IdCancha";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdCancha", idCancha);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

    }
}