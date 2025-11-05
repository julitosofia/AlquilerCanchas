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
using alquilerCanchasBLL;
using alquilerCanchasDAL;

namespace alquilerCanchasFutbol
{
    public partial class FormRegistro : Form
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        private readonly UsuarioBLL usuarioBLL;
        public FormRegistro()
        {
            InitializeComponent();


            var usuarioDAL = new UsuarioDAL(conexion); 
            this.usuarioBLL = new UsuarioBLL(usuarioDAL); 

            cmbRol.SelectedItem = "Cliente";
            cmbRol.Enabled = false;
            lblDisponibilidad.Text = "";
            lblDisponibilidad.ForeColor = Color.Black;
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            Usuario nuevo = new Usuario
            {
                Nombre = txtNombre.Text,
                Clave = txtClave.Text,
                Rol = "Cliente"
            };


            if (!this.usuarioBLL.NombreUsuarioDisponible(txtNombre.Text))
            {
                MessageBox.Show("El nombre de usuario ya esta en uso. Elegi otro.");
                return;
            }


            string mensaje;
            bool ok = this.usuarioBLL.RegistrarUsuario(nuevo, out mensaje);

            if (ok)
            {
                MessageBox.Show("Registro exitoso. Ahora podes iniciar sesion.");
                btnVolverLogin.Visible = true;
            }
            else
            {
                MessageBox.Show(mensaje);
            }
        }

        private void btnVolverLogin_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Show();
            this.Hide();
        }

        private void FormRegistro_Load(object sender, EventArgs e)
        {

        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                lblDisponibilidad.Text = "";
                return;
            }

            if (this.usuarioBLL.NombreUsuarioDisponible(nombre))
            {
                lblDisponibilidad.Text = "Nombre Disponible";
                lblDisponibilidad.ForeColor = Color.Green;
            }
            else
            {
                lblDisponibilidad.Text = "Nombre ya en uso";
                lblDisponibilidad.ForeColor = Color.Red;
            }
        }
    }
}
