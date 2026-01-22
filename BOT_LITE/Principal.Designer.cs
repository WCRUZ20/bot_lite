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
            this.btnProceso.BackColor = System.Drawing.Color.Green;
            this.btnProceso.FlatAppearance.BorderSize = 0;
            this.btnProceso.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnProceso.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProceso.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceso.ForeColor = System.Drawing.Color.White;
            this.btnProceso.Location = new System.Drawing.Point(794, 33);
            this.btnProceso.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnProceso.Name = "btnProceso";
            this.btnProceso.Size = new System.Drawing.Size(105, 28);
            this.btnProceso.TabIndex = 0;
            this.btnProceso.Text = "Ejecutar";
            this.btnProceso.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnProceso.UseVisualStyleBackColor = false;
            this.btnProceso.Click += new System.EventHandler(this.btnProceso_Click);
            // 
            // dtgClientes
            // 
            this.dtgClientes.BackgroundColor = System.Drawing.Color.White;
            this.dtgClientes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dtgClientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgClientes.Location = new System.Drawing.Point(35, 79);
            this.dtgClientes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dtgClientes.Name = "dtgClientes";
            this.dtgClientes.Size = new System.Drawing.Size(864, 346);
            this.dtgClientes.TabIndex = 1;
            // 
            // btnActualizar
            // 
            this.btnActualizar.BackColor = System.Drawing.Color.Gray;
            this.btnActualizar.FlatAppearance.BorderSize = 0;
            this.btnActualizar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActualizar.ForeColor = System.Drawing.Color.White;
            this.btnActualizar.Location = new System.Drawing.Point(35, 33);
            this.btnActualizar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(97, 28);
            this.btnActualizar.TabIndex = 2;
            this.btnActualizar.Text = "Refrescar";
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnConfig.FlatAppearance.BorderSize = 0;
            this.btnConfig.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfig.Location = new System.Drawing.Point(156, 33);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(46, 28);
            this.btnConfig.TabIndex = 3;
            this.btnConfig.Text = "🔧";
            this.btnConfig.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnConfig.UseVisualStyleBackColor = false;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // chkAutoEjecutar
            // 
            this.chkAutoEjecutar.AutoSize = true;
            this.chkAutoEjecutar.Location = new System.Drawing.Point(315, 39);
            this.chkAutoEjecutar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkAutoEjecutar.Name = "chkAutoEjecutar";
            this.chkAutoEjecutar.Size = new System.Drawing.Size(96, 17);
            this.chkAutoEjecutar.TabIndex = 4;
            this.chkAutoEjecutar.Text = "Auto ejecutar";
            this.chkAutoEjecutar.UseVisualStyleBackColor = true;
            // 
            // dtpHoraProgramada
            // 
            this.dtpHoraProgramada.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpHoraProgramada.Location = new System.Drawing.Point(572, 37);
            this.dtpHoraProgramada.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dtpHoraProgramada.Name = "dtpHoraProgramada";
            this.dtpHoraProgramada.ShowUpDown = true;
            this.dtpHoraProgramada.Size = new System.Drawing.Size(104, 22);
            this.dtpHoraProgramada.TabIndex = 6;
            // 
            // lblHoraProgramada
            // 
            this.lblHoraProgramada.AutoSize = true;
            this.lblHoraProgramada.Location = new System.Drawing.Point(428, 39);
            this.lblHoraProgramada.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHoraProgramada.Name = "lblHoraProgramada";
            this.lblHoraProgramada.Size = new System.Drawing.Size(111, 13);
            this.lblHoraProgramada.TabIndex = 5;
            this.lblHoraProgramada.Text = "Hora programación:";
            // 
            // timerProgramacion
            // 
            this.timerProgramacion.Tick += new System.EventHandler(this.timerProgramacion_Tick);
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(933, 450);
            this.Controls.Add(this.dtpHoraProgramada);
            this.Controls.Add(this.lblHoraProgramada);
            this.Controls.Add(this.chkAutoEjecutar);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.dtgClientes);
            this.Controls.Add(this.btnProceso);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Principal";
            this.Text = "Descarga masiva de documentos electrónicos";
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

