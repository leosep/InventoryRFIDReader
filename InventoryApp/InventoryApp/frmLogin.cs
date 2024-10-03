using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLL;
using InventoryApp.BLL;
using System.Web;
using System.Security;

namespace InventoryApp
{
    public partial class frmLogin : Form
    {
        InventoryApp.inventoryWebService.Service obj = new inventoryWebService.Service();
     
        string sqry = "";  

        public frmLogin()
        {
            InitializeComponent();
        }
        
        private void btnEntrar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Processes.CheckConection())
            {
                MD5Tools encriptar = new MD5Tools();

                obj.AuthHeaderValue = SecurityWebService.authData;

                String usuario = txtUsuario.Text.Trim();
                String contrasena = encriptar.CalcMD5Hash(txtContrasena.Text.Trim());

                DataSet ds = new DataSet();

                ds = obj.Login(usuario, contrasena);

                int cantidad = ds.Tables[0].AsEnumerable().Count(r => r.Field<String>("status") == "ACT");

                if (cantidad > 0)
                {
                    frmMenu frm = new frmMenu();
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid User");
                }
            }
            else
            {
                MessageBox.Show("Web service not available. Please check your network connection.");
            }
            Cursor.Current = Cursors.Default;
            
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void lblContrasena_ParentChanged(object sender, EventArgs e)
        {

        }
    }
}