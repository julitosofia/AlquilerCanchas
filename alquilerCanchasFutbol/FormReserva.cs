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
using alquilerCanchasDAL;

namespace alquilerCanchasFutbol
{
    public partial class FormReserva : Form
    {
        private Usuario usuario;
        private readonly ConexionDAL conexion = new ConexionDAL();


        private readonly ReservaBLL reservaBLL;
        private readonly CanchaBLL canchaBLL; 
        public FormReserva(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;


            var reservaDAL = new ReservaDAL(conexion); 
                                                       
            var canchaDAL = new CanchaDAL(conexion); 


            this.reservaBLL = new ReservaBLL(reservaDAL);
            this.canchaBLL = new CanchaBLL(canchaDAL); 
        }

        private void btnVerDisponibilidad_Click(object sender, EventArgs e)
        {
            int idCancha = (int)cmbCancha.SelectedValue;
            DateTime fecha = dtpFecha.Value.Date;
            DateTime inicio = dtpHoraInicio.Value;
            DateTime fin = dtpHoraFin.Value;


            bool disponible = this.reservaBLL.TurnoDisponible(idCancha, fecha, inicio, fin);

            lblEstado.Text = disponible ? "Disponible" : "Ocupado";
            lblEstado.ForeColor = disponible ? Color.Green : Color.Red;

            if (disponible)
            {

                decimal precio = this.reservaBLL.CalcularPrecio(idCancha, inicio, fin);
                lblTotal.Text = $"Total: ${precio}";
            }
        }

        private void btnReservar_Click(object sender, EventArgs e)
        {
            if (lblEstado.Text != "Disponible")
            {
                MessageBox.Show("El turno no esta disponible.");
                return;
            }
            Reserva nueva = new Reserva
            {
                // ... (resto de la inicialización de la Reserva) ...
                IdUsuario = usuario.IdUsuario,
                IdCancha = (int)cmbCancha.SelectedValue,
                Cliente = usuario.Nombre,
                Fecha = dtpFecha.Value.Date,
                HoraInicio = dtpHoraInicio.Value,
                HoraFin = dtpHoraFin.Value,
                Total = decimal.Parse(lblTotal.Text.Replace("Total: $", ""))
            };

            // Antes: ReservaBLL reservaBLL = new ReservaBLL(new ReservaDAL());
            // Ahora: Usamos el campo de la clase.
            bool ok = this.reservaBLL.RegistrarReserva(nueva);

            MessageBox.Show(ok ? "Reserva confirmada." : "Error al registrar la reserva.");
            if (ok) this.Close();
        }

        private void FormReserva_Load(object sender, EventArgs e)
        {
            var canchas = this.canchaBLL.ObtenerTodas();

            cmbCancha.DataSource = canchas;
            cmbCancha.DisplayMember = "Nombre";
            cmbCancha.ValueMember = "IdCancha";

            // ... (resto de la configuración del DateTimePicker) ...
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

        private void btnExportarXml_Click(object sender, EventArgs e)
        {
            string mensaje;
            bool exito = reservaBLL.ExportarReservasAXml("reservas.xml", out mensaje);

            MessageBox.Show(mensaje, exito ? "Éxito" : "Error",
                MessageBoxButtons.OK,
                exito ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                List<Reserva> reservasImportadas = reservaBLL.ImportarReservasDesdeXml("reservas.xml");

                if (reservasImportadas != null && reservasImportadas.Count > 0)
                {
                    MessageBox.Show($"Se cargaron {reservasImportadas.Count} reservas desde 'reservas.xml'.",
                        "Importación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se encontraron reservas válidas en 'reservas.xml' o el archivo no existe.",
                        "Importación Fallida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar importar: {ex.Message}",
                    "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
