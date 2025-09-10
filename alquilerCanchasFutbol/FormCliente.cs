using alquilerCanchasBE;
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
    public partial class FormCliente : Form
    {
        private Usuario usuario;
        public FormCliente(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            lblBienvenidaCliente.Text = $"Bienvenido, {usuario.Nombre} (Cliente)";
        }

        private void FormCliente_Load(object sender, EventArgs e)
        {

        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            FormReserva reserva = new FormReserva(usuario);
            reserva.Show();
            this.Hide();
        }

        private void btnMisReservas_Click(object sender, EventArgs e)
        {
            new FormMisReservas(usuario).Show();
            this.Hide();
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            new FormCompra(usuario).Show();
            this.Hide();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            new FormLogin().Show();
            this.Close();
        }
    }
}
