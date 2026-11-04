namespace Analizador_Lexico.Views
{
    partial class FrmDiccionario
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
            this.dgvDiccionario = new System.Windows.Forms.DataGridView();
            this.colPalabra = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTipoDato = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pnlBarra = new System.Windows.Forms.FlowLayoutPanel();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnExportar = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pnlPie = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAplicar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiccionario)).BeginInit();
            this.pnlBarra.SuspendLayout();
            this.pnlPie.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDiccionario
            // 
            this.dgvDiccionario.AllowUserToAddRows = true;
            this.dgvDiccionario.AllowUserToDeleteRows = true;
            this.dgvDiccionario.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDiccionario.BackgroundColor = System.Drawing.Color.White;
            this.dgvDiccionario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiccionario.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPalabra,
            this.colTipoDato});
            this.dgvDiccionario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDiccionario.Location = new System.Drawing.Point(0, 44);
            this.dgvDiccionario.Name = "dgvDiccionario";
            this.dgvDiccionario.RowHeadersVisible = false;
            this.dgvDiccionario.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDiccionario.Size = new System.Drawing.Size(560, 370);
            this.dgvDiccionario.TabIndex = 0;
            // 
            // colPalabra
            // 
            this.colPalabra.FillWeight = 70F;
            this.colPalabra.HeaderText = "Palabra reservada";
            this.colPalabra.Name = "colPalabra";
            // 
            // colTipoDato
            // 
            this.colTipoDato.FillWeight = 30F;
            this.colTipoDato.HeaderText = "¿Es tipo de dato?";
            this.colTipoDato.Name = "colTipoDato";
            // 
            // pnlBarra
            // 
            this.pnlBarra.Controls.Add(this.btnEliminar);
            this.pnlBarra.Controls.Add(this.btnRestaurar);
            this.pnlBarra.Controls.Add(this.btnImportar);
            this.pnlBarra.Controls.Add(this.btnExportar);
            this.pnlBarra.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBarra.Location = new System.Drawing.Point(0, 0);
            this.pnlBarra.Name = "pnlBarra";
            this.pnlBarra.Padding = new System.Windows.Forms.Padding(6, 6, 6, 0);
            this.pnlBarra.Size = new System.Drawing.Size(560, 44);
            this.pnlBarra.TabIndex = 1;
            // 
            // btnEliminar
            // 
            this.btnEliminar.AutoSize = true;
            this.btnEliminar.Location = new System.Drawing.Point(9, 9);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(90, 30);
            this.btnEliminar.TabIndex = 0;
            this.btnEliminar.Text = "Eliminar fila";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnRestaurar
            // 
            this.btnRestaurar.AutoSize = true;
            this.btnRestaurar.Location = new System.Drawing.Point(105, 9);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(100, 30);
            this.btnRestaurar.TabIndex = 1;
            this.btnRestaurar.Text = "Restaurar 24";
            this.btnRestaurar.UseVisualStyleBackColor = true;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);
            // 
            // btnImportar
            // 
            this.btnImportar.AutoSize = true;
            this.btnImportar.Location = new System.Drawing.Point(211, 9);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(90, 30);
            this.btnImportar.TabIndex = 2;
            this.btnImportar.Text = "Importar...";
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.AutoSize = true;
            this.btnExportar.Location = new System.Drawing.Point(307, 9);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(90, 30);
            this.btnExportar.TabIndex = 3;
            this.btnExportar.Text = "Exportar...";
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblInfo.Location = new System.Drawing.Point(0, 414);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblInfo.Size = new System.Drawing.Size(560, 22);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.Text = "Escriba en la última fila para agregar una palabra (letras, dígitos y \'_\', sin iniciar con dígito).";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlPie
            // 
            this.pnlPie.Controls.Add(this.btnCancelar);
            this.pnlPie.Controls.Add(this.btnAplicar);
            this.pnlPie.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPie.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlPie.Location = new System.Drawing.Point(0, 436);
            this.pnlPie.Name = "pnlPie";
            this.pnlPie.Padding = new System.Windows.Forms.Padding(6);
            this.pnlPie.Size = new System.Drawing.Size(560, 44);
            this.pnlPie.TabIndex = 3;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(467, 9);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(84, 30);
            this.btnCancelar.TabIndex = 0;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAplicar
            // 
            this.btnAplicar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAplicar.Location = new System.Drawing.Point(377, 9);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(84, 30);
            this.btnAplicar.TabIndex = 1;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = true;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // FrmDiccionario
            // 
            this.AcceptButton = this.btnAplicar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(560, 480);
            this.Controls.Add(this.dgvDiccionario);
            this.Controls.Add(this.pnlBarra);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.pnlPie);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(576, 419);
            this.Name = "FrmDiccionario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Diccionario de palabras reservadas";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiccionario)).EndInit();
            this.pnlBarra.ResumeLayout(false);
            this.pnlBarra.PerformLayout();
            this.pnlPie.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDiccionario;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPalabra;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colTipoDato;
        private System.Windows.Forms.FlowLayoutPanel pnlBarra;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.FlowLayoutPanel pnlPie;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnAplicar;
    }
}
