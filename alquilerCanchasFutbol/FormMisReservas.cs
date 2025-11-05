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


        private readonly ConexionDAL conexion = new ConexionDAL();


        private readonly ReservaBLL reservaBLL;
        public FormMisReservas(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            lblCliente.Text = $"Reservas de: {usuario.Nombre}";


            var reservaDAL = new ReservaDAL(conexion); 
            this.reservaBLL = new ReservaBLL(reservaDAL);

            CargarReservas();
        }
        private void CargarReservas()
        {
            var reservas = this.reservaBLL.ObtenerReservasPorUsuario(usuario.Nombre);
            dgvReservas.DataSource = reservas;
        }
        private void FormMisReservas_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvReservas.CurrentRow == null) return;
            Reserva reserva = (Reserva)dgvReservas.CurrentRow.DataBoundItem;

            if (reserva.Fecha < DateTime.Now)
            {
                MessageBox.Show("No se puede cancelar una reserva pasada.");
                return;
            }


            bool ok = this.reservaBLL.CancelarReserva(reserva.IdReserva);

            MessageBox.Show(ok ? "Reserva cancelada correctamente." : "Error al cancelar la reserva.");
            CargarReservas();
        }

        private void btnExportarXml_Click(object sender, EventArgs e)
        {
            string mensaje;
            bool exito = reservaBLL.ExportarReservasAXml(out mensaje);

            MessageBox.Show(mensaje, exito ? "Éxito" : "Error", MessageBoxButtons.OK, exito ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private void btnImportarXml_Click(object sender, EventArgs e)
        {
            try
            {
                List<Reserva> reservasImportadas = reservaBLL.ImportarReservasDesdeXml();

                if (reservasImportadas != null && reservasImportadas.Count > 0)
                {

                    MessageBox.Show($"Se cargaron {reservasImportadas.Count} reservas desde 'reservas.xml'.", "Importación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("No se encontraron reservas válidas en 'reservas.xml' o el archivo no existe.", "Importación Fallida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar importar: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
