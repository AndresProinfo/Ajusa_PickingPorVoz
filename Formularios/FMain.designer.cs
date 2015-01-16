namespace ServicioPPV
{
    partial class FMain
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.tIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.iList = new System.Windows.Forms.ImageList(this.components);
            this.iListREG = new System.Windows.Forms.ImageList(this.components);
            this.chFecha = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDescripcion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lREG = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tIcon
            // 
            this.tIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("tIcon.Icon")));
            this.tIcon.Text = "Voice Dispatcher";
            this.tIcon.Visible = true;
            this.tIcon.DoubleClick += new System.EventHandler(this.tIcon_DoubleClick);
            // 
            // iList
            // 
            this.iList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iList.ImageStream")));
            this.iList.TransparentColor = System.Drawing.Color.Fuchsia;
            this.iList.Images.SetKeyName(0, "Play.bmp");
            this.iList.Images.SetKeyName(1, "Stop.bmp");
            this.iList.Images.SetKeyName(2, "Configure.bmp");
            this.iList.Images.SetKeyName(3, "Exit.bmp");
            this.iList.Images.SetKeyName(4, "Zetes.bmp");
            // 
            // iListREG
            // 
            this.iListREG.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iListREG.ImageStream")));
            this.iListREG.TransparentColor = System.Drawing.Color.Fuchsia;
            this.iListREG.Images.SetKeyName(0, "0.bmp");
            this.iListREG.Images.SetKeyName(1, "1.bmp");
            this.iListREG.Images.SetKeyName(2, "2.bmp");
            this.iListREG.Images.SetKeyName(3, "3.bmp");
            // 
            // chFecha
            // 
            this.chFecha.Text = "Fecha";
            this.chFecha.Width = 152;
            // 
            // chDescripcion
            // 
            this.chDescripcion.Text = "Descripción";
            this.chDescripcion.Width = 500;
            // 
            // lREG
            // 
            this.lREG.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lREG.Location = new System.Drawing.Point(-1, 56);
            this.lREG.Name = "lREG";
            this.lREG.Size = new System.Drawing.Size(668, 420);
            this.lREG.SmallImageList = this.iListREG;
            this.lREG.TabIndex = 4;
            this.lREG.UseCompatibleStateImageBehavior = false;
            this.lREG.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Fecha";
            this.columnHeader1.Width = 152;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Descripción";
            this.columnHeader2.Width = 500;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(131, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Arrancar Servicio";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(286, 27);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(129, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Parar Servicio";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 511);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lREG);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FMain";
            this.Text = "Servidor Tramas Ajusa";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FMain_FormClosing);
            this.Shown += new System.EventHandler(this.FMain_Shown);
            this.Resize += new System.EventHandler(this.FMain_Resize);
            this.ResumeLayout(false);

        }

        #endregion

      private System.Windows.Forms.NotifyIcon tIcon;
      private System.Windows.Forms.ImageList iList;
      private System.Windows.Forms.ImageList iListREG;
			private System.Windows.Forms.ColumnHeader chFecha;
			private System.Windows.Forms.ColumnHeader chDescripcion;
			private System.Windows.Forms.ListView lREG;
			private System.Windows.Forms.ColumnHeader columnHeader1;
      private System.Windows.Forms.ColumnHeader columnHeader2;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Button button2;
    }
}

