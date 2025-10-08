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
        private readonly VentaDAL dal = new VentaDAL();

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
            var lista = new List<Venta>();
            using (var reader = dal.ListarVenta())
            {
                while (reader.Read())
                {
                    lista.Add(new Venta
                    {
                        IdVenta = (int)reader["IdVenta"],
                        Fecha = (DateTime)reader["Fecha"],
                        IdUsuario = (int)reader["IdUsuario"],
                        Total = (decimal)reader["Total"],
                    });
                }
            }
            return lista;
        }

        public bool ActualizarVenta(Venta v)
        {
            return dal.ActualizarVenta(v.IdVenta, v.IdUsuario, v.Fecha) > 0;
        }

        public bool ActualizarDetalle(VentaDetalle detalle)
        {
            return dal.ActualizarDetalleVenta(detalle.IdDetalle, detalle.Cantidad, detalle.PrecioUnitario) > 0;
        }

        public bool EliminarVenta(int idVenta)
        {
            return dal.Eliminar(idVenta);
        }


    }
}