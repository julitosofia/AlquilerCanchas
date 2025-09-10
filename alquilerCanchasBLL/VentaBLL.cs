using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using alquilerCanchasBE;
using alquilerCanchasDAL;

namespace alquilerCanchasBLL
{
    public class VentaBLL
    {
        private VentaDAL dal = new VentaDAL();
        public bool RegistrarVenta(Venta v, List<VentaDetalle> detalles, out string mensaje)
        {
            if (detalles == null || detalles.Count == 0)
            {
                mensaje = "La venta no tiene productos.";
                return false;
            }
            v.Total = detalles.Sum(d => d.PrecioUnitario * d.Cantidad);
            bool ok = dal.InsertarVenta(v, detalles);
            mensaje = ok ? "Venta registrada correctamente." : "Error al registrar la venta.";
            return ok;
        }
        public List<Venta> ObtenerVentas()
        {
            return dal.Listar();
        }
        public bool ActualizarVenta(Venta v)
        {
            return dal.Actualizar(v);
        }
        public bool ActualizarDetalle(VentaDetalle detalle)
        {
            return dal.ActualizarDetalleVenta(detalle);
        }
        public bool EliminarVenta(int idVenta)
        {
            return dal.Eliminar(idVenta);
        }
    }
}