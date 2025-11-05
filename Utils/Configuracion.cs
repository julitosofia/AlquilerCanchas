using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Utils
{
    public class Configuracion
    {
        public string CadenaConexion { get; set; } = "TuCadenaDeConexionPorDefecto";
        public decimal TarifaHoraBase { get; set; } = 1000.00m;
        public string VersionApp { get; set; } = "1.0.0";
    }
}