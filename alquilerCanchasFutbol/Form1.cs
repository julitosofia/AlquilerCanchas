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
using System.Security.Cryptography;


namespace alquilerCanchasFutbol
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
        }


        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FormLogin();
            frm.MdiParent = this;
            frm.Show();
        }

        private void registroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FormRegistro();
            frm.MdiParent = this;
            frm.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
