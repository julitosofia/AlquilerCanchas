using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Utils
{
    public class XmlManager<T> where T : class
    {
        public readonly string _path;

        public XmlManager(string fileName)
        {
            _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public bool Guardar(List<T> list)
        {
            try
            {
                var serializar = new XmlSerializer(typeof(List<T>));
                using (var writer = new StreamWriter(_path))
                {
                    serializar.Serialize(writer, list);
                    return true;
                }
            }
            catch (Exception ep)
            {
                Console.WriteLine($"Error al guardar XML: {ep.Message}");
                return false;
            }
        }
        public List<T> Cargar()
        {
            if(!File.Exists(_path))
            {
                return new List<T>();
            }
            try
            {
                var serializer = new XmlSerializer(typeof(List<T>));
                using(var reader = new StreamReader(_path))
                {
                    var loadedList = serializer.Deserialize(reader) as List<T>;
                    return loadedList ?? new List<T>();
                }
            }
            catch(Exception ep)
            {
                Console.WriteLine($"Error al cargar XML: {ep.Message}");
                return new List<T>();
            }
        }

    }
}