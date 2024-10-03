using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;
using System.Threading;
using System.Runtime.InteropServices;

using NordicId;
using InventoryApp.BLL;
using InventoryApp.Models;

namespace InventoryApp
{
    public partial class frmRFID : Form
    {
        // We import the FindWindow and ShowWindow commands so we can show and hide the taskbar
        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern uint FindWindowW(String ClassName, String WindowName);
        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern uint ShowWindow(uint hwnd, int cmd);

        // Virtual key constant for F12
        private const int VK_F12 = 123;
        // Invalid identifier value constant (0xFFFFFFFF)
        private const int INVALID_HANDLE_VALUE = -1;
        // Constants ShowWindow
        private const int SW_SHOW = 5;
        private const int SW_HIDE = 0;

        // MHL object
        MHL MhlCore = new MHL();

        // captura Hotkey 
        HotkeyWindow HotKeyWnd = new HotkeyWindow();

        // MHL
        int hRfid = 0;
        int hKeyb = 0;
        int hKbbl = 0;

        // TaskBar control
        uint taskbar_hwnd;

        //Web Service
        inventoryWebService.Service obj = new inventoryWebService.Service();  

        public frmRFID()
        {
            InitializeComponent();

            lstActivos.SelectedValue = DBNull.Value;

            hRfid = MhlCore.OpenDrv("RFID");

            if (hRfid != INVALID_HANDLE_VALUE)
            {
                MhlCore.SetString(hRfid, "RFID.TagType", "EPC C1G2");
            }

            // Open "Keyboard"  driver
            hKeyb = MhlCore.OpenDrv("Keyboard");
            if (hKeyb != INVALID_HANDLE_VALUE)
            {
                // Save keyboard map
                MhlCore.SaveProfile(hKeyb, "MHL_UHFTest1_SAVE");
                // Assign the "Scan" button to VK_F12 (0x7B)
                MhlCore.SetDword(hKeyb, "SpecialKey.Scan.All.VK", VK_F12);
                // Reload map
                MhlCore.SetDword(hKeyb, "Keyboard.Reload", 1);
            }

            // open "KeyBacklight" driver
            luzTrasera(100);

            // Assign callback HotKeyWnd 
            HotKeyWnd.callback = new HotkeyCallbackFunc(HotkeyCallback);
            // Register VK_F12 to HotKeyWnd
            HotKeyWnd.RegisterKey(VK_F12, KeyModifiers.None);
           
        }

        // Register hotkey click
        public void HotkeyCallback(int vk)
        {
            // F12
            if (vk == VK_F12)
            {
                PerformScan();
            }
        }

        private void PerformScan()
        {
            lstActivos.SelectedValue = DBNull.Value;
            lstActivos.DataSource = null;
            

            // Create worker
            Thread ScannerThread = new Thread(new ThreadStart(this.ScannerWorkerThreadFunction));
            ScannerThread.Start();
        }

        private void ScannerWorkerThreadFunction()
        {
            uint error = 0;
            int tag_count = 0;
            

            MhlCore.Execute(hRfid, "RFID.Inventory");
            error = MhlCore.GetDword(hRfid, "RFID.ExecError");
            tag_count = MhlCore.GetInt(hRfid, "RFID.TagsCount");
            string[] codigos = new string[16];


            if (tag_count > 0 && error == 0)
            {
                byte[] id_list = MhlCore.GetBin(hRfid, "RFID.IdList");

                for (uint i = 0; i != 16; i++)
                {
                    MhlCore.SetDword(hRfid, "RFID.CurrentId", i);
                    MhlCore.SetBool(hRfid, "RFID.Separator", false);

                    string serial = MhlCore.GetString(hRfid, "RFID.SerialString").ToString();
                    
                    int n;
                    if (Processes.parseInt(Processes.hexToASCII(serial).Trim(), out n))
                    {
                        codigos[i] = serial.ToString().Trim();                       
                    }   
                }                  
                
                codigos = codigos.Where(x => !string.IsNullOrEmpty(x)).ToArray();
               
                SetThreadText(codigos);
            }
            else
            {
                //SetThreadText("RFID Read Failed, reason: " + error.ToString("d"));
            }
        }

        private delegate void SetThreadTextDelegate(string[] codigos);

        // Used to set text in UI component from another thread
        private void SetThreadText(string[] codigos)
        {

            if (this.InvokeRequired)
            {                
                this.Invoke(new SetThreadTextDelegate(SetThreadText), new object[] { codigos });
                return;
            }
            Cursor.Current = Cursors.WaitCursor;

            if (Processes.CheckConection())
            {
                obj.AuthHeaderValue = SecurityWebService.authData;

                DataSet ds = new DataSet();
                DataTable selectedTable = new DataTable();
                ds = obj.getActivoList(codigos);

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

                    lstActivos.SelectedValue = DBNull.Value;
                    lstActivos.DataSource = null;

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
                MessageBox.Show("Web service not available. Please check your network connection.");
            }
            Cursor.Current = Cursors.Default;

        }

        private delegate void SetScanStateDelegate(Boolean scanning);

        // Used to set text in UI component from another thread
        private void SetScanState(Boolean scanning)
        {
            if (this.InvokeRequired)
            {                
                this.Invoke(new SetScanStateDelegate(SetScanState), new object[] { scanning });
                return;
            }

            lstActivos.Enabled = !scanning;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            // Close driver RFID
            if (hRfid != INVALID_HANDLE_VALUE)
            {
                MhlCore.CloseDrv(hRfid);
            }

            // Close keyboard driver 
            if (hKeyb != INVALID_HANDLE_VALUE)
            {
                // Restore saved keymap
                MhlCore.LoadProfile(hKeyb, "MHL_UHFTest1_SAVE");
                MhlCore.SetDword(hKeyb, "Keyboard.Reload", 1);
                MhlCore.CloseDrv(hKeyb);
            }

            // Close KeyBacklight driver 
            if (hKbbl != INVALID_HANDLE_VALUE)
            {
                MhlCore.SetDword(hKbbl, "KeyBacklight.scan", 1);
                MhlCore.CloseDrv(hKbbl);
            }

            // Show taskbar
            if (taskbar_hwnd != 0)
                ShowWindow(taskbar_hwnd, SW_SHOW);
        }

        private void lstActivos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
       

         private void btnCancelar_Click(object sender, EventArgs e)
         {
             this.Close();            
             luzTrasera(0);

         }

         private void btnEditar_Click(object sender, EventArgs e)
         {
             if (lstActivos.SelectedIndex.ToString() != "-1" && lstActivos.GetItemText(lstActivos.SelectedItem) != "El código no existe.")
             {
                 Globals.codigoActivo = lstActivos.SelectedValue.ToString();
                 frmActivo frm = new frmActivo();
                 frm.ShowDialog();
             }         
         }

         private void btnLimpiar_Click(object sender, EventArgs e)
         {
             lstActivos.SelectedValue = DBNull.Value;
             lstActivos.DataSource = null;
         }

         private void frmRFID_Load(object sender, EventArgs e)
         {
             Cursor.Current = Cursors.WaitCursor;

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

         private void frmRFID_Closed(object sender, EventArgs e)
         {
             luzTrasera(0);
         }

         private void frmRFID_Closing(object sender, CancelEventArgs e)
         {
             luzTrasera(0);
         }

         private void luzTrasera(uint valor) {
             // Cerrar el driver "KeyBacklight"
             hKbbl = MhlCore.OpenDrv("KeyBacklight");
             if (hKbbl != INVALID_HANDLE_VALUE)
             {
                 MhlCore.SetDword(hKbbl, "KeyBacklight.scan", valor);
             }
         }

       
    }
}