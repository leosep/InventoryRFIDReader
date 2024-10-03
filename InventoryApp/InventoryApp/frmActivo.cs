using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InventoryApp.BLL;


namespace InventoryApp
{
    public partial class frmActivo : Form
    {
        inventoryWebService.Service obj = new inventoryWebService.Service();        
      
        String codigo_alternativo;
        String desc;
        int tipo;
        int grupo;
        String serie;

        public frmActivo()
        {
            InitializeComponent();
        }

        private void frmActivo_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (Processes.CheckConection())
            {
                obj.AuthHeaderValue = SecurityWebService.authData;

                DataSet ds = new DataSet();

                string[] palabras = Globals.codigoActivo.Split('_');
                int cia = Convert.ToInt32(palabras[0]);
                int activo = Convert.ToInt32(palabras[1]);

                ds = obj.getActivoDetalle(cia, activo);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cia = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                    activo = Convert.ToInt32(ds.Tables[0].Rows[0][1].ToString());
                    codigo_alternativo = ds.Tables[0].Rows[0][2].ToString();
                    desc = ds.Tables[0].Rows[0][3].ToString();
                    tipo = Convert.ToInt32(ds.Tables[0].Rows[0][4].ToString());
                    grupo = Convert.ToInt32(ds.Tables[0].Rows[0][5].ToString());
                    serie = ds.Tables[0].Rows[0][6].ToString();

                    cboTipo.DisplayMember = "descrip";
                    cboTipo.ValueMember = "tipo_activo";
                    cboTipo.DataSource = ds.Tables[1];

                    cboGrupo.DisplayMember = "descrip";
                    cboGrupo.ValueMember = "grupo_activo";
                    cboGrupo.DataSource = ds.Tables[2];

                    Globals.codigoAlt = codigo_alternativo;
                    txtCodigo.Text = activo.ToString();
                    txtDesc.Text = desc;
                    cboTipo.SelectedValue = tipo;
                    cboGrupo.SelectedValue = grupo;
                    txtSerie.Text = serie;

                }
                else
                {
                    MessageBox.Show("The fixed asset does not exist.");
                }
            }
            else
            {
                MessageBox.Show("Web service not available. Please check your network connection.");
                this.Close();
            }
            Cursor.Current = Cursors.Default;

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (Processes.CheckConection())
            {
                obj.AuthHeaderValue = SecurityWebService.authData;

                string[] palabras = Globals.codigoActivo.Split('_');
                int cia = Convert.ToInt32(palabras[0]);
                int activo = Convert.ToInt32(palabras[1]);

                codigo_alternativo = Globals.codigoAlt;
                desc = txtDesc.Text.Trim();
                tipo = Convert.ToInt32(cboTipo.SelectedValue.ToString());
                grupo = Convert.ToInt32(cboGrupo.SelectedValue.ToString());
                serie = txtSerie.Text.Trim();

                int respuesta = 0;
                respuesta = obj.updateActivo(cia, activo, codigo_alternativo, desc, tipo, grupo, serie);
                if (respuesta > 0)
                {
                    MessageBox.Show("Saved fixed asset.");
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Error.");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Web service not available. Please check your network connection.");
            }
            Cursor.Current = Cursors.Default;
        }
    }
}