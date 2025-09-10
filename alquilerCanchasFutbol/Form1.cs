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
using alquilerCanchasBLL;


namespace alquilerCanchasFutbol
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Size = new Size(200, 200);
            btnLogin.Text = "Iniciar Sesion";
            btnLogin.Size = new Size(150, 40);
            btnLogin.Location = new Point(100, 220);
            btnLogin.Click+=btnLogin_Click;
            this.Controls.Add(btnLogin);

            btnRegistro.Text = "Registrarse";
            btnRegistro.Size = new Size(150, 40);
            btnRegistro.Location = new Point(100, 270);
            btnRegistro.Click += btnRegistro_Click;
            this.Controls.Add(btnRegistro);

            btnSalir.Text = "Salir";
            btnSalir.Size = new Size(150, 40);
            btnSalir.Location = new Point(100, 320);
            btnSalir.Click += btnSalir_Click;
            this.Controls.Add(btnSalir);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Show();
            this.Hide();
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            FormRegistro registro = new FormRegistro();
            registro.Show();
            this.Hide();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
