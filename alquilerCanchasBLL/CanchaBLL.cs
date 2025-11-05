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
        private readonly IRepository<Cancha> dal;

        public CanchaBLL(IRepository<Cancha> dal)
        {
            this.dal = dal;
        }

        public List<Cancha> ObtenerTodas()
        {
            return dal.Listar();
        }

        public bool AgregarCancha(Cancha cancha, out string mensaje)
        {
            mensaje = ValidarCancha(cancha);

            if (!string.IsNullOrEmpty(mensaje))
                return false;

            return dal.Insertar(cancha);
        }

        public bool ModificarCancha(Cancha cancha, out string mensaje)
        {
            if (cancha.IdCancha <= 0)
            {
                mensaje = "Id de cancha inválido.";
                return false;
            }

            mensaje = ValidarCancha(cancha);

            if (!string.IsNullOrEmpty(mensaje))
                return false;

            return dal.Actualizar(cancha);
        }

        public bool EliminarCancha(int idCancha, out string mensaje)
        {
            mensaje = string.Empty;

            if (idCancha <= 0)
            {
                mensaje = "Id inválido.";
                return false;
            }

            return dal.Eliminar(idCancha);
        }

        private string ValidarCancha(Cancha cancha)
        {
            if (string.IsNullOrWhiteSpace(cancha.Nombre))
                return "El nombre de la cancha es obligatorio.";

            if (string.IsNullOrWhiteSpace(cancha.Tipo))
                return "El tipo de cancha es obligatorio.";

            if (cancha.PrecioHora < 0)
                return "El precio por hora no puede ser negativo.";

            return string.Empty;
        }

        public bool ExportarCanchasAXml(out string mensaje)
        {

            var canchas = ObtenerTodas();

            if (canchas == null || canchas.Count == 0)
            {
                mensaje = "No hay canchas para exportar.";
                return false;
            }

            try
            {

                var xmlManager = new XmlManager<Cancha>("canchas.xml");


                bool exito = xmlManager.Guardar(canchas);

                if (exito)
                {
                    mensaje = "Canchas exportadas a 'canchas.xml' correctamente.";
                    return true;
                }
                else
                {

                    mensaje = "Error desconocido al intentar guardar el archivo XML de canchas.";
                    return false;
                }
            }
            catch (Exception ex)
            {

                mensaje = $"Error de sistema al exportar las canchas: {ex.Message}";
                return false;
            }
        }
        public List<Cancha> ImportarCanchasDesdeXml()
        {
            var xmlManager = new XmlManager<Cancha>("canchas.xml");
            return xmlManager.Cargar();
        }
    }
}