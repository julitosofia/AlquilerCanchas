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

        List<Compra> ListarCompra();
        List<DetalleCompra> ObtenerDetallesPorCompra(int idCompra);
        List<Compra> ObtenerTodasLasCompras();

        void ExportarComprasXML(List<Compra> compras, string rutaArchivo);
        List<Compra> ImportarComprasXML(string rutaArchivo);

    }
}