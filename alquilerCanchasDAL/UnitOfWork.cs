using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace alquilerCanchasDAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _conexion;
        private SqlTransaction _transaccion;

        public IReservaRepository Reservas { get; }
        public IProductoRepository Productos { get; }
        public ICompraRepository Compras { get; }

        public UnitOfWork(string cadenaConexion)
        {
            _conexion = new SqlConnection(cadenaConexion);
            _conexion.Open();
            _transaccion = _conexion.BeginTransaction();

            Reservas = new ReservaDAL(_conexion,  _transaccion);
            Productos = new ProductoDAL(_conexion, _transaccion);
            Compras = new CompraDAL(_conexion, _transaccion);
        }
        public void Commit()
        {
            _transaccion?.Commit();
            _conexion?.Close();
        }
        public void Rollback()
        {
            _transaccion?.Rollback();
            _conexion?.Close();
        }
        public void Dispose()
        {
            _transaccion?.Dispose();
            _conexion?.Dispose();
        }
    }
}