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
    public class CanchaBLL
    {
        private readonly CanchaDAL canchaDAL;

        public CanchaBLL(CanchaDAL _canchaDAL)
        {
            canchaDAL = _canchaDAL;
        }

        public List<Cancha> Listar()
        {
            return canchaDAL.Listar();
        }
        public List<Cancha> ObtenerTodas()
             => canchaDAL.Listar();


        public Cancha ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El Id de la cancha debe ser mayor a cero.");

            return canchaDAL.ObtenerPorId(id);
        }

        public bool Insertar(Cancha cancha)
        {
            if (string.IsNullOrWhiteSpace(cancha.Nombre))
                throw new ArgumentException("El nombre de la cancha es obligatorio.");

            return canchaDAL.Insertar(cancha);
        }

        public bool Actualizar(Cancha cancha)
        {
            if (cancha.IdCancha <= 0)
                throw new ArgumentException("Debe indicar un Id válido para actualizar.");

            return canchaDAL.Actualizar(cancha);
        }

        public bool Eliminar(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Debe indicar un Id válido para eliminar.");

            return canchaDAL.Eliminar(id);
        }


        public void ExportarCanchaXML(string rutaArchivo)
        {
            var lista = canchaDAL.Listar(); 
            canchaDAL.ExportarCanchaXML(lista, rutaArchivo);
        }

        public List<Cancha> ImportarCanchaXML(string rutaArchivo)
        {
            var lista = canchaDAL.ImportarCanchaXML(rutaArchivo);


            if (lista == null || lista.Count == 0)
                throw new InvalidOperationException("El archivo XML no contiene canchas válidas.");

            return lista;
        }
    }


}