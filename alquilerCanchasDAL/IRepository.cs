using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alquilerCanchasDAL
{
    public interface IRepository<T>
    {
        List<T> Listar();
        T ObtenerPorId(int id);
        bool Insertar(T entidad);
        bool Actualizar(T entidad);
        bool Eliminar(int id);
    }
}