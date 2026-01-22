namespace BOT_LITE.Interfaces
{
    partial class ResultadoProcesoForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dtgResultados = new System.Windows.Forms.DataGridView();
            this.chkExitoso = new System.Windows.Forms.CheckBox();
            this.chkFallido = new System.Windows.Forms.CheckBox();
            this.chkSinDatos = new System.Windows.Forms.CheckBox();
            this.btnReintentarFallidos = new System.Windows.Forms.Button();
            this.lblFiltro = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtgResultados)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgResultados
            // 
            this.dtgResultados.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgResultados.Location = new System.Drawing.Point(24, 74);
            this.dtgResultados.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dtgResultados.Name = "dtgResultados";
            this.dtgResultados.Size = new System.Drawing.Size(936, 394);
            this.dtgResultados.TabIndex = 0;
            // 
            // chkExitoso
            // 
            this.chkExitoso.AutoSize = true;
            this.chkExitoso.Location = new System.Drawing.Point(117, 29);
            this.chkExitoso.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkExitoso.Name = "chkExitoso";
            this.chkExitoso.Size = new System.Drawing.Size(65, 17);
            this.chkExitoso.TabIndex = 1;
            this.chkExitoso.Text = "Exitoso";
            this.chkExitoso.UseVisualStyleBackColor = true;
            // 
            // chkFallido
            // 
            this.chkFallido.AutoSize = true;
            this.chkFallido.Location = new System.Drawing.Point(196, 29);
            this.chkFallido.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkFallido.Name = "chkFallido";
            this.chkFallido.Size = new System.Drawing.Size(59, 17);
            this.chkFallido.TabIndex = 2;
            this.chkFallido.Text = "Fallido";
            this.chkFallido.UseVisualStyleBackColor = true;
            // 
            // chkSinDatos
            // 
            this.chkSinDatos.AutoSize = true;
            this.chkSinDatos.Location = new System.Drawing.Point(268, 29);
            this.chkSinDatos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkSinDatos.Name = "chkSinDatos";
            this.chkSinDatos.Size = new System.Drawing.Size(74, 17);
            this.chkSinDatos.TabIndex = 3;
            this.chkSinDatos.Text = "Sin datos";
            this.chkSinDatos.UseVisualStyleBackColor = true;
            // 
            // btnReintentarFallidos
            // 
            this.btnReintentarFallidos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReintentarFallidos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnReintentarFallidos.FlatAppearance.BorderSize = 0;
            this.btnReintentarFallidos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReintentarFallidos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnReintentarFallidos.ForeColor = System.Drawing.Color.White;
            this.btnReintentarFallidos.Location = new System.Drawing.Point(750, 23);
            this.btnReintentarFallidos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnReintentarFallidos.Name = "btnReintentarFallidos";
            this.btnReintentarFallidos.Size = new System.Drawing.Size(210, 30);
            this.btnReintentarFallidos.TabIndex = 4;
            this.btnReintentarFallidos.Text = "Reintentar fallidos";
            this.btnReintentarFallidos.UseVisualStyleBackColor = false;
            // 
            // lblFiltro
            // 
            this.lblFiltro.AutoSize = true;
            this.lblFiltro.Location = new System.Drawing.Point(24, 30);
            this.lblFiltro.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFiltro.Name = "lblFiltro";
            this.lblFiltro.Size = new System.Drawing.Size(83, 13);
            this.lblFiltro.TabIndex = 5;
            this.lblFiltro.Text = "Filtrar estado:";
            // 
            // ResultadoProcesoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 491);
            this.Controls.Add(this.lblFiltro);
            this.Controls.Add(this.btnReintentarFallidos);
            this.Controls.Add(this.chkSinDatos);
            this.Controls.Add(this.chkFallido);
            this.Controls.Add(this.chkExitoso);
            this.Controls.Add(this.dtgResultados);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ResultadoProcesoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resumen del proceso";
            ((System.ComponentModel.ISupportInitialize)(this.dtgResultados)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgResultados;
        private System.Windows.Forms.CheckBox chkExitoso;
        private System.Windows.Forms.CheckBox chkFallido;
        private System.Windows.Forms.CheckBox chkSinDatos;
        private System.Windows.Forms.Button btnReintentarFallidos;
        private System.Windows.Forms.Label lblFiltro;
    }
}