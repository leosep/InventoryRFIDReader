using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InventoryApp.BLL;

using System.Threading;

using NordicId;
using InventoryApp.Models;

namespace InventoryApp
{
    public partial class frmCodBarras : Form
    {

        // Virtual key constant for F12
        private const int VK_F12 = 123;

        // Invalid identifier value constant (0xFFFFFFFF)
        private const int INVALID_HANDLE_VALUE = -1;

        // MHL main object
        MHL MhlCore = new MHL();

        // Hotkey Capture
        HotkeyWindow HotKeyWnd = new HotkeyWindow();

        // Change to True, if canceled
        bool ScanCancelled = false;

        // MHL handles
        int hScan = 0;
        int hKeyb = 0;       

        public frmCodBarras()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Open keyboard driver
            hKeyb = MhlCore.OpenDrv("Keyboard");
            if (hKeyb > 0)
            {
                // Save current keymap
                MhlCore.SaveProfile(hKeyb, "MHL_CSTest_SAVE");
                // Assign the "Scan" button to VK_F12 (0x7B)
                MhlCore.SetDword(hKeyb, "SpecialKey.Scan.All.VK", VK_F12);
                // Reload map
                MhlCore.SetDword(hKeyb, "Keyboard.Reload", 1);
            }

            // Open "Scanner" driver
            hScan = MhlCore.OpenDrv("Scanner");

            // Assign HotKeyWnd callback
            HotKeyWnd.callback = new HotkeyCallbackFunc(HotkeyCallback);
            // Register VK_F12 as HotKeyWnd
            HotKeyWnd.RegisterKey(VK_F12, KeyModifiers.None);

            //Timer
            _tmrTicker = new System.Windows.Forms.Timer();
            _tmrTicker.Interval = 1000;
            _tmrTicker.Enabled = false;
            _tmrTicker.Tick += new EventHandler(_tmrTicker_Tick);
        }

        ~frmCodBarras() 
        {
            // Free the Scanner driver
            if (hScan != INVALID_HANDLE_VALUE) 
            {
                MhlCore.CloseDrv(hScan);
            }
            // Free the Keyboard driver
            if (hKeyb != INVALID_HANDLE_VALUE) 
            {
                // Restore saved map
                MhlCore.LoadProfile(hKeyb, "MHL_CSTest_SAVE");
                MhlCore.SetDword(hKeyb, "Keyboard.Reload", 1);
                MhlCore.CloseDrv(hKeyb);
            }
        }		

        inventoryWebService.Service obj = new inventoryWebService.Service();

        // Registered hotkey clicked
        public void HotkeyCallback(int vk)
        {
            // F12
            if (vk == VK_F12)
            {
                PerformScan();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            PerformScan();
            if (txtCodigo.Text != "")
            {                
                //buscarCodigo(txtCodigo.Text);
            }
        }
       
        private void txtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                if (txtCodigo.Text != "")
                {
                    //buscarCodigo(txtCodigo.Text);
                }
            }  
        }

        private void buscarCodigo(string bcode)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Processes.CheckConection())
            {
                lstActivos.SelectedValue = DBNull.Value;
                lstActivos.DataSource = null;

                obj.AuthHeaderValue = SecurityWebService.authData;

                DataSet ds = new DataSet();
                DataTable selectedTable = new DataTable();

                if (Processes.IsNumeric(bcode))
                {
                    ds = obj.getActivo(Convert.ToInt32(bcode));

                    if (Convert.ToInt32(cboGrupo.SelectedValue.ToString()) > -1)
                    {
                        int cantidadRows = ds.Tables[0].AsEnumerable()
                                        .Count(a => a.Field<int>("grupo_activo") == Convert.ToInt32(cboGrupo.SelectedValue.ToString()));
                        if (cantidadRows > 0)
                        {
                            selectedTable = ds.Tables[0].AsEnumerable()
                                            .Where(a => a.Field<int>("grupo_activo") == Convert.ToInt32(cboGrupo.SelectedValue.ToString()))
                                            .CopyToDataTable();
                        }
                    }
                    else
                    {
                        selectedTable = ds.Tables[0];
                    }

                    int cantidad = selectedTable.AsEnumerable().Count();

                    if (cantidad > 0)
                    {
                        lstActivos.DisplayMember = "descrip";
                        lstActivos.ValueMember = "cia_activo";
                        lstActivos.DataSource = selectedTable;
                    }
                    else
                    {
                        lstActivos.Items.Clear();
                        lstActivos.Items.Add("The code does not exist.");

                    }
                }
                else
                {
                    MessageBox.Show("The code is not a number.");
                }
            }
            else
            {
                MessageBox.Show("Web service not available. Please check your network connection.");
            }
            Cursor.Current = Cursors.Default;
        }

        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                buscarCodigo(txtCodigo.Text);
            }
        }

        private void PerformScan()
        {
            // If the 'txtCodigo' control is disabled, we continue scanning.
            if (txtCodigo.Enabled == false)
                return;

            // We create a worker thread to scan
            Thread ScannerThread = new Thread(new ThreadStart(this.ScannerWorkerThreadFunction));
            ScannerThread.Start();
        }

        private void ScannerWorkerThreadFunction()
        {
            SetScanState(true);
            SetThreadText("");

            if (hScan > 0)
            {
                // Start scan
                MhlCore.SetDword(hScan, "Scanner.Scan", 2);

                // If the scan is canceled, we exit
                if (ScanCancelled)
                {
                    return;
                }

                if (MhlCore.GetLastError() == 0)
                {
                    // We have something
                    SetThreadText(MhlCore.GetString(hScan, "Scanner.ScanResultString"));
                }
                else
                {
                    // We cannot decode, show message
                    SetThreadText(MhlCore.GetString(hScan, "Scanner.ScanResultInfo"));
                }
            }
            else
            {
                SetThreadText("Scanner not available");
            }
            // Enable controls
            SetScanState(false);
        }

        private delegate void SetScanStateDelegate(Boolean scanning);

        // Used to set text to UI component from another thread
        private void SetScanState(Boolean scanning)
        {
            if (this.InvokeRequired)
            {                
                this.Invoke(new SetScanStateDelegate(SetScanState), new object[] { scanning });
                return;
            }
           
            btnBuscar.Enabled = !scanning;
            txtCodigo.Enabled = !scanning;
        }

        private delegate void SetThreadTextDelegate(String message);

        private void SetThreadText(String message)
        {
            if (this.InvokeRequired)
            {               
                this.Invoke(new SetThreadTextDelegate(SetThreadText), new object[] { message });
                return;
            }

            this.txtCodigo.Text = message;           
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lstActivos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (lstActivos.SelectedIndex.ToString() != "-1" && lstActivos.GetItemText(lstActivos.SelectedItem) != "The code does not exist.")
            {
                Globals.codigoActivo = lstActivos.SelectedValue.ToString();
                frmActivo frm = new frmActivo();
                frm.ShowDialog();
            }     
        }

        private void lstActivos_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        void _tmrTicker_Tick(object sender, EventArgs e)
        {
            
            int n;
            if (Processes.parseInt(new String(_barcode.ToArray()), out n) && new String(_barcode.ToArray()).Length >= 0)
            {

                txtCodigo.Text = new String(_barcode.ToArray());
                _barcode.Clear();
                if (_tmrTicker.Enabled == true) _tmrTicker.Enabled = false;

            }
        }

        private System.Windows.Forms.Timer _tmrTicker = null; 
        DateTime _lastKeystroke = new DateTime(0);
        List<char> _barcode = new List<char>(10);
        
        private void frmCodBarras_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (_tmrTicker.Enabled == false) _tmrTicker.Enabled = true;

            // Check time (keystrokes within 100ms)
            TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
            if (elapsed.TotalMilliseconds > 100)
            {
                _barcode.Clear();
            }
            else { 
            
            }

            // Record keystroke and timestamp
            _barcode.Add(e.KeyChar);
            _lastKeystroke = DateTime.Now;
        }


        private void frmCodBarras_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmCodBarras_KeyPress);
            cboGrupo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmCodBarras_KeyPress);
            txtCodigo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmCodBarras_KeyPress);
            btnBuscar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmCodBarras_KeyPress);
            lstActivos.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmCodBarras_KeyPress);
            btnEditar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmCodBarras_KeyPress);
            btnCancelar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmCodBarras_KeyPress);           

            if (Processes.CheckConection())
            {
                obj.AuthHeaderValue = SecurityWebService.authData;

                DataSet ds = new DataSet();
                ds = obj.getGrupos();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    List<Grupos> objList = new List<Grupos>();
                    foreach (DataRow _dataRow in ds.Tables[0].Rows)
                    {
                        Grupos grupos = new Grupos();
                        grupos.grupo_activo = Convert.ToInt32(_dataRow["grupo_activo"]);
                        grupos.descrip = Convert.ToString(_dataRow["descrip"]);                        
                        objList.Add(grupos);
                    }

                    objList.Insert(0, new Grupos { grupo_activo = -1, descrip = "--SELECT--" });

                    cboGrupo.DisplayMember = "descrip";
                    cboGrupo.ValueMember = "grupo_activo";
                    cboGrupo.DataSource = objList;
                }
            }
            else
            {
                MessageBox.Show("Web service not available. Please check your network connection.");
                this.Close();
            }
            Cursor.Current = Cursors.Default;
        }
    }
}