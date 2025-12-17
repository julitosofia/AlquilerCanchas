using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using alquilerCanchasBE;

namespace alquilerCanchasDAL
{
    public interface IProductoRepository
    {
        List<Producto> Listar();
        Producto ObtenerPorId(int idProducto);
        bool Insertar(Producto producto);
        bool Actualizar(Producto producto);
        bool Eliminar(int idProducto);
        int ActualizarStock(int idProducto, int cantidadVendida);

        void ExportarProductosXML(List<Producto> productos, string rutaArchivo);
        List<Producto> ImportarProductosXML(string rutaArchivo);


    }
}