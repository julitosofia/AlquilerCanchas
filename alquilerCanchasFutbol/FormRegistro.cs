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
        public FormRegistro()
        {
            InitializeComponent();
            cmbRol.SelectedItem = "Cliente";
            cmbRol.Enabled=false;
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
            UsuarioBLL usuarioBLL=new UsuarioBLL();
            if(!usuarioBLL.NombreUsuarioDisponible(txtNombre.Text))
            {
                MessageBox.Show("El nombre de usuario ya esta en uso.Elegi otro.");
                return;
            }
            if(string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }
            if(txtClave.Text.Length<4)
            {
                MessageBox.Show("La clave debe tener al menos 4 caracteres.");
                return;
            }

            bool ok = UsuarioBLL.RegistrarUsuario(nuevo);
            if(ok)
            {
                MessageBox.Show("Registro exitoso.Ahora podes iniciar sesion.");
                btnVolverLogin.Visible = true;
            }
            else
            {
                MessageBox.Show("Error al registrar usuario.");
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
            UsuarioBLL usuarioBLL = new UsuarioBLL();
            string nombre = txtNombre.Text.Trim();
            if(string.IsNullOrWhiteSpace(nombre))
            {
                lblDisponibilidad.Text = "";
                return;
            }
            if(usuarioBLL.NombreUsuarioDisponible(nombre))
            {
                lblDisponibilidad.Text = "Nombre Disponible";
                lblDisponibilidad.ForeColor= Color.Green;
            }
            else
            {
                lblDisponibilidad.Text = "Nombre ya en uso";
                lblDisponibilidad.ForeColor=Color.Red;
            }

        }
    }
}
