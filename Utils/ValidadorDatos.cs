using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public static class ValidadorDatos
    {
        public static bool EsFechaFutura(DateTime fecha)
        {
            return fecha.Date > DateTime.Today;
        }

        public static bool EsRangoHorarioValido(DateTime inicio, DateTime fin)
        {
            return inicio < fin && (fin - inicio).TotalMinutes >= 30;
        }

        public static bool TieneStockSuficiente(int stockDisponible, int cantidadSolicitada)
        {
            return stockDisponible >= cantidadSolicitada;
        }

        public static bool EsMontoPositivo(decimal monto)
        {
            return monto >= 0;
        }

        public static bool EsIdValido(int id)
        {
            return id > 0;
        }

        public static bool EsTextoNoVacio(string texto)
        {
            return !string.IsNullOrWhiteSpace(texto);
        }

        public static bool EsCantidadValida(int cantidad)
        {
            return cantidad > 0;
        }

        public static bool EsDuracionMinima(DateTime inicio, DateTime fin, int minutosMinimos)
        {
            return (fin - inicio).TotalMinutes >= minutosMinimos;
        }


    }
}