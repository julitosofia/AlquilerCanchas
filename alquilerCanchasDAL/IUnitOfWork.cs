using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using alquilerCanchasBE;

namespace alquilerCanchasDAL
{
    public interface IUnitOfWork : IDisposable
    {
        IReservaRepository Reservas { get; }
        IProductoRepository Productos { get; }
        ICompraRepository Compras { get; }

        void Commit();
        void Rollback();
    }
}