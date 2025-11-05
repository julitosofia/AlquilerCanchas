using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace alquilerCanchasDAL
{
    public class ConexionDAL
    {
        private readonly string cadena = @"Data Source=BILARDO;Initial Catalog=AlquilerCanchas;Integrated Security=True;";
        private SqlConnection conexion;
        private SqlTransaction transaccion;
        public string CadenaConexion => cadena;
        public SqlTransaction Transaccion => transaccion;
        public int EjecutarNonQuery(string nombreSP, List<SqlParameter> parametros)
        {
            using(var conn = new SqlConnection(cadena))
            {
                conn.Open();
                using(var cmd = new SqlCommand(nombreSP,conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if(parametros!=null)
                    {
                        cmd.Parameters.AddRange(parametros.ToArray());
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public SqlDataReader EjecutarReader(string nombreSP,List<SqlParameter>parametros)
        {
            var conn = new SqlConnection(cadena);
            conn.Open();
            var cmd = new SqlCommand(nombreSP, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (parametros != null)
                cmd.Parameters.AddRange(parametros.ToArray());
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public object EjecutarEscalar(string nombreSP, List<SqlParameter>parametros)
        {
            using(var conn = new SqlConnection(cadena))
            {
                conn.Open();
                using(var cmd = new SqlCommand(nombreSP,conn))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    if (parametros != null)
                        cmd.Parameters.AddRange(parametros.ToArray());
                    return cmd.ExecuteScalar();
                }
            }
        }
        public SqlConnection Abrir()
        {
            if (conexion == null)
                conexion = new SqlConnection(cadena);
            if(conexion.State != ConnectionState.Open)
                conexion.Open();
            return conexion;
        }
        public void Cerrar()
        {
            if (conexion != null && conexion.State == ConnectionState.Open)
                conexion.Close();
        }
        public void IniciarTransaccion()
        {
            if (conexion == null || conexion.State != ConnectionState.Open)
                Abrir();

            transaccion = conexion.BeginTransaction();
        }
        public void ConfirmarTransaccion()
        {
            transaccion?.Commit();
            Cerrar();
        }
        public void CancelarTransaccion()
        {
            transaccion?.Rollback();
            Cerrar();
        }
        public int EjecutarNonQueryTransacciones(string nombreSP, List<SqlParameter>parametros)
        {
            if (conexion == null || conexion.State != ConnectionState.Open)
            {
                Abrir();
            }

            using (var cmd = new SqlCommand(nombreSP, conexion, transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parametros != null)
                {
                    cmd.Parameters.AddRange(parametros.ToArray());
                }
                return cmd.ExecuteNonQuery();
            }
        }
        public object EjecutarEscalarTransaccion(string nombreSP, List<SqlParameter> parametros)
        {
            if (conexion == null || conexion.State != ConnectionState.Open)
            {
                Abrir();
            }

            using (var cmd = new SqlCommand(nombreSP, conexion, transaccion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parametros != null)
                {
                    cmd.Parameters.AddRange(parametros.ToArray());
                }
                return cmd.ExecuteScalar(); // Esta línea ahora usará una 'conexion' válida.
            }
        }
    }
}