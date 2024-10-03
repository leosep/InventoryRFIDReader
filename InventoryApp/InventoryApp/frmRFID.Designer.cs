namespace InventoryApp
{
    partial class frmRFID
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRFID));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.lstActivos = new System.Windows.Forms.ListBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.lblScan = new System.Windows.Forms.Label();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.cboGrupo = new System.Windows.Forms.ComboBox();
            this.lblGrupo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstActivos
            // 
            this.lstActivos.Location = new System.Drawing.Point(3, 75);
            this.lstActivos.Name = "lstActivos";
            this.lstActivos.Size = new System.Drawing.Size(232, 146);
            this.lstActivos.TabIndex = 3;
            this.lstActivos.SelectedIndexChanged += new System.EventHandler(this.lstActivos_SelectedIndexChanged);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(163, 230);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(72, 24);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancel";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(85, 230);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(72, 24);
            this.btnEditar.TabIndex = 6;
            this.btnEditar.Text = "Edit";
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // lblScan
            // 
            this.lblScan.Location = new System.Drawing.Point(22, 55);
            this.lblScan.Name = "lblScan";
            this.lblScan.Size = new System.Drawing.Size(208, 20);
            this.lblScan.Text = "Please press the SCAN key";
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(4, 230);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(75, 24);
            this.btnLimpiar.TabIndex = 8;
            this.btnLimpiar.Text = "Clean";
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // cboGrupo
            // 
            this.cboGrupo.Location = new System.Drawing.Point(63, 30);
            this.cboGrupo.Name = "cboGrupo";
            this.cboGrupo.Size = new System.Drawing.Size(165, 23);
            this.cboGrupo.TabIndex = 11;
            // 
            // lblGrupo
            // 
            this.lblGrupo.Location = new System.Drawing.Point(11, 32);
            this.lblGrupo.Name = "lblGrupo";
            this.lblGrupo.Size = new System.Drawing.Size(100, 20);
            this.lblGrupo.Text = "Group";
            // 
            // frmRFID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(238, 263);
            this.Controls.Add(this.cboGrupo);
            this.Controls.Add(this.lblGrupo);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.lblScan);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.lstActivos);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "frmRFID";
            this.Text = "RFID";
            this.Load += new System.EventHandler(this.frmRFID_Load);
            this.Closed += new System.EventHandler(this.frmRFID_Closed);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmRFID_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstActivos;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Label lblScan;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.ComboBox cboGrupo;
        private System.Windows.Forms.Label lblGrupo;
    }
}