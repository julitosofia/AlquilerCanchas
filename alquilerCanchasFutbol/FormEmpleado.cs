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
    public partial class FormEmpleado : Form
    {
        public FormEmpleado(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            lblBienvenida.Text = $"Bienvenido, {usuario.Nombre} (Empleado)";
        }

        private void FormEmpleado_Load(object sender, EventArgs e)
        {
            btnProductos = new Button { Text = "Gestionar Productos", Location = new Point(50, 80) };
            btnEstadisticas = new Button { Text = "Ver Estadisticas", Location = new Point(50, 130) };

            btnProductos.Click += btnProductos_Click;
            btnEstadisticas.Click += btnEstadisticas_Click;

            Controls.Add(btnProductos);
            Controls.Add(btnEstadisticas);
        }
        private Usuario usuario;


        private void btnProductos_Click(object sender, EventArgs e)
        {
            FormProductos form = new FormProductos(usuario);
            form.ShowDialog();
        }

        private void btnEstadisticas_Click(object sender, EventArgs e)
        {
            FormEstadisticas form = new FormEstadisticas();
            form.ShowDialog();
        }
    }
}
