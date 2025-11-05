using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Security;

namespace Utils
{
    public class ConfiguracionManager
    {
        private readonly XmlSerializer serializer = new XmlSerializer(typeof(Configuracion));
        private readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuracion.xml");

        public Configuracion Cargar()
        {
            if(!File.Exists(_path))
            {
                return new Configuracion();
            }
            try
            {
                using(var reader = new StreamReader(_path))
                {
                    return serializer.Deserialize(reader) as Configuracion;
                }
            }
            catch
            {
                return new Configuracion();
            }
        }
        public void Guardar(Configuracion config)
        {
            try
            {
                using(var writer = new StreamWriter(_path))
                {
                    serializer.Serialize(writer, config);
                }
            }
            catch
            {

            }
        }
    }
}