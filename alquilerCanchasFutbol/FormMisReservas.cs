using alquilerCanchasBE;
using alquilerCanchasBLL;
using alquilerCanchasDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace alquilerCanchasFutbol
{
    public partial class FormMisReservas : Form
    {
        private Usuario usuario;
        public FormMisReservas(Usuario usuario)
        {
            InitializeComponent();
            this.usuario=usuario;
            lblCliente.Text = $"Reservas de: {usuario.Nombre}";
            CargarReservas();
        }
        private void CargarReservas()
        {
            ReservaBLL reservaBLL = new ReservaBLL(new ReservaDAL());
            var reservas = reservaBLL.ObtenerReservasPorUsuario(usuario.IdUsuario);
            dgvReservas.DataSource=reservas;
        }
        private void FormMisReservas_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvReservas.CurrentRow == null) return;
            Reserva reserva = (Reserva)dgvReservas.CurrentRow.DataBoundItem;
            if(reserva.Fecha<DateTime.Now)
            {
                MessageBox.Show("No se puede cancelar una reserva pasada.");
                return;
            }
            ReservaBLL reservaBLL = new ReservaBLL(new ReservaDAL());
            bool ok = reservaBLL.CancelarReserva(reserva.IdReserva);
            MessageBox.Show(ok ? "Reserva cancelada correctamente." : "Error al cancelar la reserva.");
            CargarReservas();

        }
    }
}
