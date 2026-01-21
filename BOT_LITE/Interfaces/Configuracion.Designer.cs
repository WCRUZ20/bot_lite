namespace BOT_LITE.Interfaces
{
    partial class Configuracion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.txtConn = new System.Windows.Forms.TextBox();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnguardar = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtUrlSri = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtConn
            // 
            this.txtConn.Location = new System.Drawing.Point(12, 31);
            this.txtConn.Name = "txtConn";
            this.txtConn.Size = new System.Drawing.Size(463, 20);
            this.txtConn.TabIndex = 0;
            // 
            // txtRuta
            // 
            this.txtRuta.Location = new System.Drawing.Point(12, 82);
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.Size = new System.Drawing.Size(425, 20);
            this.txtRuta.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cadena conexión";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ruta archivos";
            // 
            // btnguardar
            // 
            this.btnguardar.Location = new System.Drawing.Point(408, 155);
            this.btnguardar.Name = "btnguardar";
            this.btnguardar.Size = new System.Drawing.Size(75, 23);
            this.btnguardar.TabIndex = 6;
            this.btnguardar.Text = "Guardar";
            this.btnguardar.UseVisualStyleBackColor = true;
            this.btnguardar.Click += new System.EventHandler(this.btnguardar_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(443, 82);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(32, 20);
            this.btnBuscar.TabIndex = 7;
            this.btnBuscar.Text = "=";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtUrlSri
            // 
            this.txtUrlSri.Location = new System.Drawing.Point(12, 129);
            this.txtUrlSri.Name = "txtUrlSri";
            this.txtUrlSri.Size = new System.Drawing.Size(425, 20);
            this.txtUrlSri.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "URL SRI";
            // 
            // Configuracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 190);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUrlSri);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.btnguardar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRuta);
            this.Controls.Add(this.txtConn);
            this.Name = "Configuracion";
            this.Text = "Configuracion";
            this.Load += new System.EventHandler(this.Configuracion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtConn;
        private System.Windows.Forms.TextBox txtRuta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnguardar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.TextBox txtUrlSri;
        private System.Windows.Forms.Label label3;
    }
}