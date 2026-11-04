namespace Analizador_Lexico.Views
{
    partial class DlgExportar
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

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.lblPregunta = new System.Windows.Forms.Label();
            this.rbTodo = new System.Windows.Forms.RadioButton();
            this.rbTokens = new System.Windows.Forms.RadioButton();
            this.rbErrores = new System.Windows.Forms.RadioButton();
            this.rbSimbolos = new System.Windows.Forms.RadioButton();
            this.rbEstadisticas = new System.Windows.Forms.RadioButton();
            this.btnContinuar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPregunta
            // 
            this.lblPregunta.AutoSize = true;
            this.lblPregunta.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPregunta.Location = new System.Drawing.Point(16, 14);
            this.lblPregunta.Name = "lblPregunta";
            this.lblPregunta.Size = new System.Drawing.Size(159, 20);
            this.lblPregunta.TabIndex = 0;
            this.lblPregunta.Text = "¿Qué desea exportar?";
            // 
            // rbTodo
            // 
            this.rbTodo.AutoSize = true;
            this.rbTodo.Checked = true;
            this.rbTodo.Location = new System.Drawing.Point(24, 44);
            this.rbTodo.Name = "rbTodo";
            this.rbTodo.Size = new System.Drawing.Size(195, 24);
            this.rbTodo.TabIndex = 1;
            this.rbTodo.TabStop = true;
            this.rbTodo.Text = "Todo (reporte completo)";
            this.rbTodo.UseVisualStyleBackColor = true;
            // 
            // rbTokens
            // 
            this.rbTokens.AutoSize = true;
            this.rbTokens.Location = new System.Drawing.Point(24, 72);
            this.rbTokens.Name = "rbTokens";
            this.rbTokens.Size = new System.Drawing.Size(191, 24);
            this.rbTokens.TabIndex = 2;
            this.rbTokens.TabStop = true;
            this.rbTokens.Text = "Solo tokens (dgvTokens)";
            this.rbTokens.UseVisualStyleBackColor = true;
            // 
            // rbErrores
            // 
            this.rbErrores.AutoSize = true;
            this.rbErrores.Location = new System.Drawing.Point(24, 100);
            this.rbErrores.Name = "rbErrores";
            this.rbErrores.Size = new System.Drawing.Size(195, 24);
            this.rbErrores.TabIndex = 3;
            this.rbErrores.TabStop = true;
            this.rbErrores.Text = "Solo errores (dgvErrores)";
            this.rbErrores.UseVisualStyleBackColor = true;
            // 
            // rbSimbolos
            // 
            this.rbSimbolos.AutoSize = true;
            this.rbSimbolos.Location = new System.Drawing.Point(24, 128);
            this.rbSimbolos.Name = "rbSimbolos";
            this.rbSimbolos.Size = new System.Drawing.Size(284, 24);
            this.rbSimbolos.TabIndex = 4;
            this.rbSimbolos.TabStop = true;
            this.rbSimbolos.Text = "Solo tabla de símbolos (dgvSimbolos)";
            this.rbSimbolos.UseVisualStyleBackColor = true;
            // 
            // rbEstadisticas
            // 
            this.rbEstadisticas.AutoSize = true;
            this.rbEstadisticas.Location = new System.Drawing.Point(24, 156);
            this.rbEstadisticas.Name = "rbEstadisticas";
            this.rbEstadisticas.Size = new System.Drawing.Size(247, 24);
            this.rbEstadisticas.TabIndex = 5;
            this.rbEstadisticas.TabStop = true;
            this.rbEstadisticas.Text = "Solo estadísticas (txtEstadisticas)";
            this.rbEstadisticas.UseVisualStyleBackColor = true;
            // 
            // btnContinuar
            // 
            this.btnContinuar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnContinuar.Location = new System.Drawing.Point(140, 205);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new System.Drawing.Size(90, 30);
            this.btnContinuar.TabIndex = 6;
            this.btnContinuar.Text = "Continuar";
            this.btnContinuar.UseVisualStyleBackColor = true;
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(236, 205);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(90, 30);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // DlgExportar
            // 
            this.AcceptButton = this.btnContinuar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(340, 250);
            this.Controls.Add(this.lblPregunta);
            this.Controls.Add(this.rbTodo);
            this.Controls.Add(this.rbTokens);
            this.Controls.Add(this.rbErrores);
            this.Controls.Add(this.rbSimbolos);
            this.Controls.Add(this.rbEstadisticas);
            this.Controls.Add(this.btnContinuar);
            this.Controls.Add(this.btnCancelar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DlgExportar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Exportar resultados";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPregunta;
        private System.Windows.Forms.RadioButton rbTodo;
        private System.Windows.Forms.RadioButton rbTokens;
        private System.Windows.Forms.RadioButton rbErrores;
        private System.Windows.Forms.RadioButton rbSimbolos;
        private System.Windows.Forms.RadioButton rbEstadisticas;
        private System.Windows.Forms.Button btnContinuar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
