using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using alquilerCanchasBE;
using alquilerCanchasDAL;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Utils;

namespace alquilerCanchasBLL
{
    public class VentaBLL
    {
        private readonly VentaDAL dal;

        public VentaBLL()
        {
            dal = new VentaDAL();
        }

        public bool RegistrarVenta(Venta venta, List<VentaDetalle> detalles, out string mensaje)
        {
            mensaje = ValidarVenta(venta, detalles);

            if (!string.IsNullOrEmpty(mensaje))
                return false;

            venta.Total = detalles.Sum(d => d.PrecioUnitario * d.Cantidad);

            bool ok = dal.InsertarVenta(venta, detalles);
            mensaje = ok ? "Venta registrada correctamente." : "Error al registrar la venta.";
            return ok;
        }

        public List<Venta> ObtenerVentas()
            => dal.ListarVenta();

        public bool ActualizarVenta(Venta venta, out string mensaje)
        {
            mensaje = string.Empty;

            if (venta == null || venta.IdVenta <= 0 || venta.IdUsuario <= 0)
            {
                mensaje = "Datos de venta inválidos.";
                return false;
            }

            return dal.ActualizarVenta(venta.IdVenta, venta.IdUsuario, venta.Fecha) > 0;
        }

        public bool ActualizarDetalle(VentaDetalle detalle, out string mensaje)
        {
            mensaje = string.Empty;

            if (detalle == null || detalle.IdDetalle <= 0 || detalle.Cantidad <= 0 || detalle.PrecioUnitario < 0)
            {
                mensaje = "Datos del detalle inválidos.";
                return false;
            }

            return dal.ActualizarDetalleVenta(detalle.IdDetalle, detalle.Cantidad, detalle.PrecioUnitario) > 0;
        }

        public bool EliminarVenta(int idVenta, out string mensaje)
        {
            mensaje = string.Empty;

            if (idVenta <= 0)
            {
                mensaje = "Id de venta inválido.";
                return false;
            }

            return dal.Eliminar(idVenta);
        }

        private string ValidarVenta(Venta venta, List<VentaDetalle> detalles)
        {
            if (venta == null)
                return "La venta no puede ser nula.";

            if (venta.IdUsuario <= 0)
                return "Debe especificar el usuario.";

            if (detalles == null || detalles.Count == 0)
                return "La venta no tiene productos.";

            foreach (var d in detalles)
            {
                if (d.IdProducto <= 0 || d.Cantidad <= 0 || d.PrecioUnitario < 0)
                    return $"Detalle inválido para el producto ID {d.IdProducto}.";
            }

            return string.Empty;
        }

        public bool ExportarVentasAXml(string rutaArchivo, out string mensaje)
        {
            var ventas = dal.ListarVenta();

            if (ventas == null || ventas.Count == 0)
            {
                mensaje = "No hay ventas para exportar.";
                return false;
            }

            try
            {
                dal.ExportarVentasXML(ventas, rutaArchivo);
                mensaje = $"Ventas exportadas a '{rutaArchivo}' correctamente.";
                return true;
            }
            catch (Exception ex)
            {
                mensaje = $"Error de sistema al exportar las ventas: {ex.Message}";
                return false;
            }
        }

        public List<Venta> ImportarVentasDesdeXml(string rutaArchivo)
        {
            var lista = dal.ImportarVentasXML(rutaArchivo);

            if (lista == null || lista.Count == 0)
                throw new InvalidOperationException("El archivo XML no contiene ventas válidas.");

            return lista;
        }


    }
}