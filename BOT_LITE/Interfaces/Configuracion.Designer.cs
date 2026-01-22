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
            this.txtSupabaseUrl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSupabaseKey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLicenseUser = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLicensePassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
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
            this.btnguardar.Location = new System.Drawing.Point(400, 306);
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
            // txtSupabaseUrl
            // 
            this.txtSupabaseUrl.Location = new System.Drawing.Point(12, 178);
            this.txtSupabaseUrl.Name = "txtSupabaseUrl";
            this.txtSupabaseUrl.Size = new System.Drawing.Size(463, 20);
            this.txtSupabaseUrl.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Supabase URL";
            // 
            // txtSupabaseKey
            // 
            this.txtSupabaseKey.Location = new System.Drawing.Point(12, 226);
            this.txtSupabaseKey.Name = "txtSupabaseKey";
            this.txtSupabaseKey.Size = new System.Drawing.Size(463, 20);
            this.txtSupabaseKey.TabIndex = 12;
            this.txtSupabaseKey.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Supabase Anon Key";
            // 
            // txtLicenseUser
            // 
            this.txtLicenseUser.Location = new System.Drawing.Point(12, 272);
            this.txtLicenseUser.Name = "txtLicenseUser";
            this.txtLicenseUser.Size = new System.Drawing.Size(220, 20);
            this.txtLicenseUser.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 255);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Usuario licencia";
            // 
            // txtLicensePassword
            // 
            this.txtLicensePassword.Location = new System.Drawing.Point(255, 272);
            this.txtLicensePassword.Name = "txtLicensePassword";
            this.txtLicensePassword.Size = new System.Drawing.Size(220, 20);
            this.txtLicensePassword.TabIndex = 16;
            this.txtLicensePassword.UseSystemPasswordChar = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(257, 255);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Clave licencia";
            // 
            // Configuracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 341);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtLicensePassword);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtLicenseUser);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSupabaseKey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSupabaseUrl);
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
        private System.Windows.Forms.TextBox txtSupabaseUrl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSupabaseKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLicenseUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLicensePassword;
        private System.Windows.Forms.Label label7;
    }
}