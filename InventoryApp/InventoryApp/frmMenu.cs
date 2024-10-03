using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {

        }

        private void frmMenu_Closed(object sender, EventArgs e)
        {
            Application.Exit();
            this.Close();
        }

        private void frmMenu_Closing(object sender, CancelEventArgs e)
        {
            Application.Exit();
            this.Close();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
            this.Close();
        }

        private void btnBarras_Click(object sender, EventArgs e)
        {
            frmCodBarras frm = new frmCodBarras();
            frm.ShowDialog();
        }

        private void btnRfid_Click(object sender, EventArgs e)
        {
            frmRFID frm = new frmRFID();
            frm.ShowDialog();
        }
    }
}