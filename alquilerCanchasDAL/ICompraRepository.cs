using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using alquilerCanchasBE;

namespace alquilerCanchasDAL
{
    public interface ICompraRepository
    {
        bool RegistrarCompra(Compra compra);
        SqlDataReader ListarCompra();
        SqlDataReader ObtenerDetallesPorCompra(int idCompra);
        SqlDataReader ObtenerTodasLasCompras();
    }
}