using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using alquilerCanchasBLL;
using alquilerCanchasBE;
using alquilerCanchasDAL;

namespace alquilerCanchasFutbol
{
    public partial class FormLogin : Form
    {
        private UsuarioBLL usuarioBLL= new UsuarioBLL();
        public FormLogin()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += FormLogin_KeyDown;
            txtClave.PasswordChar = '*';
            txtUsuario.MaxLength = 30;
            txtClave.MaxLength = 30;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string nombre = txtUsuario.Text.Trim();
            string clave = txtClave.Text;
            var resultado = usuarioBLL.ValidarLogin(nombre, clave);
            if(!resultado.EsValido)
            {
                MessageBox.Show(resultado.Mensaje);
                return;
            }
            Usuario usuario = resultado.Usuario;
            if(usuario.Rol == "Cliente")
            {
                new FormCliente(usuario).Show();
            }
            else if (usuario.Rol== "Empleado")
            {
                new FormEmpleado(usuario).Show();
            }
            else
            {
                MessageBox.Show("Rol no reconocido.");
                return;
            }
            this.Hide();
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            new FormRegistro().Show();
            this.Hide();
        }
        private void FormLogin_KeyDown(object sender,KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnIngresar.PerformClick();
            }
        }
    }
}
