using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using alquilerCanchasBE;
using alquilerCanchasDAL;

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


    }
}