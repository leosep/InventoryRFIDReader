namespace InventoryApp
{
    partial class frmMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMenu));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.btnBarras = new System.Windows.Forms.Button();
            this.btnRfid = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnBarras
            // 
            this.btnBarras.Location = new System.Drawing.Point(8, 71);
            this.btnBarras.Name = "btnBarras";
            this.btnBarras.Size = new System.Drawing.Size(220, 25);
            this.btnBarras.TabIndex = 2;
            this.btnBarras.Text = "Barcodes";
            this.btnBarras.Click += new System.EventHandler(this.btnBarras_Click);
            // 
            // btnRfid
            // 
            this.btnRfid.Location = new System.Drawing.Point(8, 102);
            this.btnRfid.Name = "btnRfid";
            this.btnRfid.Size = new System.Drawing.Size(220, 24);
            this.btnRfid.TabIndex = 3;
            this.btnRfid.Text = "RFID";
            this.btnRfid.Click += new System.EventHandler(this.btnRfid_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(8, 132);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(220, 25);
            this.btnSalir.TabIndex = 4;
            this.btnSalir.Text = "Exit";
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular);
            this.lblTitulo.Location = new System.Drawing.Point(19, 48);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(213, 20);
            this.lblTitulo.Text = "Inventory Management";
            // 
            // frmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(238, 263);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnRfid);
            this.Controls.Add(this.btnBarras);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "frmMenu";
            this.Text = "Menu";
            this.Load += new System.EventHandler(this.frmMenu_Load);
            this.Closed += new System.EventHandler(this.frmMenu_Closed);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMenu_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBarras;
        private System.Windows.Forms.Button btnRfid;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Label lblTitulo;
    }
}