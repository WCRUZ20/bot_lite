namespace BOT_LITE
{
    partial class Principal
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
            this.components = new System.ComponentModel.Container();
            this.btnProceso = new System.Windows.Forms.Button();
            this.dtgClientes = new System.Windows.Forms.DataGridView();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnConfig = new System.Windows.Forms.Button();
            this.chkAutoEjecutar = new System.Windows.Forms.CheckBox();
            this.dtpHoraProgramada = new System.Windows.Forms.DateTimePicker();
            this.lblHoraProgramada = new System.Windows.Forms.Label();
            this.timerProgramacion = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dtgClientes)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProceso
            // 
            this.btnProceso.BackColor = System.Drawing.Color.LimeGreen;
            this.btnProceso.Location = new System.Drawing.Point(681, 33);
            this.btnProceso.Name = "btnProceso";
            this.btnProceso.Size = new System.Drawing.Size(90, 28);
            this.btnProceso.TabIndex = 0;
            this.btnProceso.Text = "Ejecutar";
            this.btnProceso.UseVisualStyleBackColor = false;
            this.btnProceso.Click += new System.EventHandler(this.btnProceso_Click);
            // 
            // dtgClientes
            // 
            this.dtgClientes.BackgroundColor = System.Drawing.Color.White;
            this.dtgClientes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dtgClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgClientes.Location = new System.Drawing.Point(30, 79);
            this.dtgClientes.Name = "dtgClientes";
            this.dtgClientes.Size = new System.Drawing.Size(741, 346);
            this.dtgClientes.TabIndex = 1;
            // 
            // btnActualizar
            // 
            this.btnActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnActualizar.Location = new System.Drawing.Point(30, 33);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(83, 28);
            this.btnActualizar.TabIndex = 2;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfig.Location = new System.Drawing.Point(134, 34);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(39, 27);
            this.btnConfig.TabIndex = 3;
            this.btnConfig.Text = "⚙️";
            this.btnConfig.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnConfig.UseVisualStyleBackColor = false;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // chkAutoEjecutar
            // 
            this.chkAutoEjecutar.AutoSize = true;
            this.chkAutoEjecutar.Location = new System.Drawing.Point(270, 39);
            this.chkAutoEjecutar.Name = "chkAutoEjecutar";
            this.chkAutoEjecutar.Size = new System.Drawing.Size(89, 17);
            this.chkAutoEjecutar.TabIndex = 4;
            this.chkAutoEjecutar.Text = "Auto ejecutar";
            this.chkAutoEjecutar.UseVisualStyleBackColor = true;
            // 
            // dtpHoraProgramada
            // 
            this.dtpHoraProgramada.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpHoraProgramada.Location = new System.Drawing.Point(490, 37);
            this.dtpHoraProgramada.Name = "dtpHoraProgramada";
            this.dtpHoraProgramada.ShowUpDown = true;
            this.dtpHoraProgramada.Size = new System.Drawing.Size(90, 20);
            this.dtpHoraProgramada.TabIndex = 6;
            // 
            // lblHoraProgramada
            // 
            this.lblHoraProgramada.AutoSize = true;
            this.lblHoraProgramada.Location = new System.Drawing.Point(367, 39);
            this.lblHoraProgramada.Name = "lblHoraProgramada";
            this.lblHoraProgramada.Size = new System.Drawing.Size(100, 13);
            this.lblHoraProgramada.TabIndex = 5;
            this.lblHoraProgramada.Text = "Hora programación:";
            // 
            // timerProgramacion
            // 
            this.timerProgramacion.Tick += new System.EventHandler(this.timerProgramacion_Tick);
            //  
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dtpHoraProgramada);
            this.Controls.Add(this.lblHoraProgramada);
            this.Controls.Add(this.chkAutoEjecutar);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.dtgClientes);
            this.Controls.Add(this.btnProceso);
            this.Name = "Principal";
            this.Text = "DOCSY";
            this.Load += new System.EventHandler(this.Principal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgClientes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProceso;
        private System.Windows.Forms.DataGridView dtgClientes;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.CheckBox chkAutoEjecutar;
        private System.Windows.Forms.DateTimePicker dtpHoraProgramada;
        private System.Windows.Forms.Label lblHoraProgramada;
        private System.Windows.Forms.Timer timerProgramacion;
    }
}

