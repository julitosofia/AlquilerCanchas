using alquilerCanchasBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using alquilerCanchasBE;

namespace alquilerCanchasFutbol
{
    public partial class FormReserva : Form
    {
        private Usuario usuario;
        public FormReserva(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private void btnVerDisponibilidad_Click(object sender, EventArgs e)
        {
            int idCancha = (int)cmbCancha.SelectedValue;
            DateTime fecha = dtpFecha.Value.Date;
            DateTime inicio = dtpHoraInicio.Value;
            DateTime fin = dtpHoraFin.Value;

            ReservaBLL reservaBLL = new ReservaBLL();
            bool disponible = reservaBLL.TurnoDisponible(idCancha, fecha, inicio, fin);

            lblEstado.Text=disponible ? "Disponible" : "Ocupado";
            lblEstado.ForeColor=disponible?Color.Green : Color.Red;

            if(disponible)
            {
                decimal precio = reservaBLL.CalcularPrecio(idCancha, inicio, fin);
                lblTotal.Text = $"Total: ${precio}";
            }
        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            if(lblEstado.Text!="Disponible")
            {
                MessageBox.Show("El turno no esta disponible.");
                return;
            }
            Reserva nueva = new Reserva
            {
                IdUsuario=usuario.IdUsuario,
                IdCancha = (int)cmbCancha.SelectedValue,
                Cliente = usuario.Nombre,
                Fecha = dtpFecha.Value.Date,
                HoraInicio = dtpHoraInicio.Value,
                HoraFin = dtpHoraFin.Value,
                Total = decimal.Parse(lblTotal.Text.Replace("Total: $", ""))
            };
            ReservaBLL reservaBLL = new ReservaBLL();
            bool ok = reservaBLL.RegistrarReserva(nueva);

            MessageBox.Show(ok ? "Reserva confirmada." : "Error al registrar la reserva.");
            if(ok) this.Close();
        }

        private void FormReserva_Load(object sender, EventArgs e)
        {
            CanchaBLL canchaBLL = new CanchaBLL();
            var canchas = canchaBLL.ObtenerTodas();

            cmbCancha.DataSource = canchas;
            cmbCancha.DisplayMember = "Nombre";
            cmbCancha.ValueMember = "IdCancha";
            dtpHoraInicio.Format = DateTimePickerFormat.Time;
            dtpHoraInicio.ShowUpDown = true;

            dtpHoraFin.Format = DateTimePickerFormat.Time;
            dtpHoraFin.ShowUpDown = true;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            FormCliente menu = new FormCliente(usuario);
            menu.Show();
            this.Close();
        }
    }
}
