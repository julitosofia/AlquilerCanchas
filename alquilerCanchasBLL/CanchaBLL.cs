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
        private CanchaDAL dal = new CanchaDAL();

        public List<Cancha> ObtenerTodas() => dal.Listar();
        public bool AgregarCancha(Cancha c) => dal.Insertar(c);
        public bool ModificarCancha(Cancha c) => dal.Actualizar(c);
        public bool EliminarCanca(int idCancha) => dal.Elminar(idCancha);

    }
}