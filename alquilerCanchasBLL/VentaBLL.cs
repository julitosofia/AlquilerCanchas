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
        {
            var lista = new List<Venta>();
            var reader = dal.ListarVenta();

            while (reader.Read())
            {
                lista.Add(new Venta
                {
                    IdVenta = (int)reader["IdVenta"],
                    Fecha = (DateTime)reader["Fecha"],
                    IdUsuario = (int)reader["IdUsuario"],
                    Total = (decimal)reader["Total"]
                });
            }

            reader.Close();
            return lista;
        }

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

    }
}