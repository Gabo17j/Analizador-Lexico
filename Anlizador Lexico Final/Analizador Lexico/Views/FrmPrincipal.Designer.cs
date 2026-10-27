namespace Analizador_Lexico.Views
{
    partial class FrmPrincipal
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

        private void InitializeComponent()
        {
            this.colNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLexema = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLinea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColumna = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrRegla = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrLexema = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrLinea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrColumna = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrDescripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSimId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSimLexema = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSimTipoToken = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSimTipoDato = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSimPrimera = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSimFrecuencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSimRefs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlSuperior = new System.Windows.Forms.Panel();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnExportar = new System.Windows.Forms.Button();
            this.btnAnalizar = new System.Windows.Forms.Button();
            this.btnAbrir = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rtbCodigo = new System.Windows.Forms.RichTextBox();
            this.rtbLineas = new System.Windows.Forms.RichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTokens = new System.Windows.Forms.TabPage();
            this.dgvTokens = new System.Windows.Forms.DataGridView();
            this.tabErrores = new System.Windows.Forms.TabPage();
            this.dgvErrores = new System.Windows.Forms.DataGridView();
            this.tabSimbolos = new System.Windows.Forms.TabPage();
            this.dgvSimbolos = new System.Windows.Forms.DataGridView();
            this.tabEstadisticas = new System.Windows.Forms.TabPage();
            this.txtEstadisticas = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblEstado = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPosicion = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblResumen = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pnlSuperior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabTokens.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTokens)).BeginInit();
            this.tabErrores.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrores)).BeginInit();
            this.tabSimbolos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSimbolos)).BeginInit();
            this.tabEstadisticas.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // colNum
            // 
            this.colNum.FillWeight = 8F;
            this.colNum.HeaderText = "#";
            this.colNum.MinimumWidth = 6;
            this.colNum.Name = "colNum";
            this.colNum.ReadOnly = true;
            // 
            // colLexema
            // 
            this.colLexema.FillWeight = 24F;
            this.colLexema.HeaderText = "Lexema";
            this.colLexema.MinimumWidth = 6;
            this.colLexema.Name = "colLexema";
            this.colLexema.ReadOnly = true;
            // 
            // colTipo
            // 
            this.colTipo.FillWeight = 26F;
            this.colTipo.HeaderText = "Tipo de token";
            this.colTipo.MinimumWidth = 6;
            this.colTipo.Name = "colTipo";
            this.colTipo.ReadOnly = true;
            // 
            // colCategoria
            // 
            this.colCategoria.FillWeight = 22F;
            this.colCategoria.HeaderText = "Categoría";
            this.colCategoria.MinimumWidth = 6;
            this.colCategoria.Name = "colCategoria";
            this.colCategoria.ReadOnly = true;
            // 
            // colLinea
            // 
            this.colLinea.FillWeight = 9F;
            this.colLinea.HeaderText = "Línea";
            this.colLinea.MinimumWidth = 6;
            this.colLinea.Name = "colLinea";
            this.colLinea.ReadOnly = true;
            // 
            // colColumna
            // 
            this.colColumna.FillWeight = 11F;
            this.colColumna.HeaderText = "Columna";
            this.colColumna.MinimumWidth = 6;
            this.colColumna.Name = "colColumna";
            this.colColumna.ReadOnly = true;
            // 
            // colErrRegla
            // 
            this.colErrRegla.FillWeight = 9F;
            this.colErrRegla.HeaderText = "Regla";
            this.colErrRegla.MinimumWidth = 6;
            this.colErrRegla.Name = "colErrRegla";
            this.colErrRegla.ReadOnly = true;
            // 
            // colErrLexema
            // 
            this.colErrLexema.FillWeight = 22F;
            this.colErrLexema.HeaderText = "Lexema";
            this.colErrLexema.MinimumWidth = 6;
            this.colErrLexema.Name = "colErrLexema";
            this.colErrLexema.ReadOnly = true;
            // 
            // colErrLinea
            // 
            this.colErrLinea.FillWeight = 9F;
            this.colErrLinea.HeaderText = "Línea";
            this.colErrLinea.MinimumWidth = 6;
            this.colErrLinea.Name = "colErrLinea";
            this.colErrLinea.ReadOnly = true;
            // 
            // colErrColumna
            // 
            this.colErrColumna.FillWeight = 10F;
            this.colErrColumna.HeaderText = "Columna";
            this.colErrColumna.MinimumWidth = 6;
            this.colErrColumna.Name = "colErrColumna";
            this.colErrColumna.ReadOnly = true;
            // 
            // colErrDescripcion
            // 
            this.colErrDescripcion.FillWeight = 50F;
            this.colErrDescripcion.HeaderText = "Descripción";
            this.colErrDescripcion.MinimumWidth = 6;
            this.colErrDescripcion.Name = "colErrDescripcion";
            this.colErrDescripcion.ReadOnly = true;
            // 
            // colSimId
            // 
            this.colSimId.FillWeight = 6F;
            this.colSimId.HeaderText = "Id";
            this.colSimId.MinimumWidth = 6;
            this.colSimId.Name = "colSimId";
            this.colSimId.ReadOnly = true;
            // 
            // colSimLexema
            // 
            this.colSimLexema.FillWeight = 20F;
            this.colSimLexema.HeaderText = "Lexema";
            this.colSimLexema.MinimumWidth = 6;
            this.colSimLexema.Name = "colSimLexema";
            this.colSimLexema.ReadOnly = true;
            // 
            // colSimTipoToken
            // 
            this.colSimTipoToken.FillWeight = 21F;
            this.colSimTipoToken.HeaderText = "Tipo de token";
            this.colSimTipoToken.MinimumWidth = 6;
            this.colSimTipoToken.Name = "colSimTipoToken";
            this.colSimTipoToken.ReadOnly = true;
            // 
            // colSimTipoDato
            // 
            this.colSimTipoDato.FillWeight = 16F;
            this.colSimTipoDato.HeaderText = "Tipo de dato";
            this.colSimTipoDato.MinimumWidth = 6;
            this.colSimTipoDato.Name = "colSimTipoDato";
            this.colSimTipoDato.ReadOnly = true;
            // 
            // colSimPrimera
            // 
            this.colSimPrimera.FillWeight = 12F;
            this.colSimPrimera.HeaderText = "1ª línea/col";
            this.colSimPrimera.MinimumWidth = 6;
            this.colSimPrimera.Name = "colSimPrimera";
            this.colSimPrimera.ReadOnly = true;
            // 
            // colSimFrecuencia
            // 
            this.colSimFrecuencia.FillWeight = 11F;
            this.colSimFrecuencia.HeaderText = "Frecuencia";
            this.colSimFrecuencia.MinimumWidth = 6;
            this.colSimFrecuencia.Name = "colSimFrecuencia";
            this.colSimFrecuencia.ReadOnly = true;
            // 
            // colSimRefs
            // 
            this.colSimRefs.FillWeight = 14F;
            this.colSimRefs.HeaderText = "Referencias";
            this.colSimRefs.MinimumWidth = 6;
            this.colSimRefs.Name = "colSimRefs";
            this.colSimRefs.ReadOnly = true;
            // 
            // pnlSuperior
            // 
            this.pnlSuperior.Controls.Add(this.btnLimpiar);
            this.pnlSuperior.Controls.Add(this.btnExportar);
            this.pnlSuperior.Controls.Add(this.btnAnalizar);
            this.pnlSuperior.Controls.Add(this.btnAbrir);
            this.pnlSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSuperior.Location = new System.Drawing.Point(0, 0);
            this.pnlSuperior.Name = "pnlSuperior";
            this.pnlSuperior.Size = new System.Drawing.Size(1257, 53);
            this.pnlSuperior.TabIndex = 0;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(453, 10);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(114, 34);
            this.btnLimpiar.TabIndex = 3;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.Location = new System.Drawing.Point(318, 10);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(126, 34);
            this.btnExportar.TabIndex = 2;
            this.btnExportar.Text = "Exportar";
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // btnAnalizar
            // 
            this.btnAnalizar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAnalizar.Location = new System.Drawing.Point(160, 10);
            this.btnAnalizar.Name = "btnAnalizar";
            this.btnAnalizar.Size = new System.Drawing.Size(149, 34);
            this.btnAnalizar.TabIndex = 1;
            this.btnAnalizar.Text = "Analizar (F5)";
            this.btnAnalizar.UseVisualStyleBackColor = true;
            this.btnAnalizar.Click += new System.EventHandler(this.btnAnalizar_Click);
            // 
            // btnAbrir
            // 
            this.btnAbrir.Location = new System.Drawing.Point(14, 10);
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(137, 34);
            this.btnAbrir.TabIndex = 0;
            this.btnAbrir.Text = "Abrir archivo";
            this.btnAbrir.UseVisualStyleBackColor = true;
            this.btnAbrir.Click += new System.EventHandler(this.btnAbrir_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 53);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.rtbCodigo);
            this.splitContainer1.Panel1.Controls.Add(this.rtbLineas);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1257, 615);
            this.splitContainer1.SplitterDistance = 548;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // rtbCodigo
            // 
            this.rtbCodigo.AcceptsTab = true;
            this.rtbCodigo.AllowDrop = true;
            this.rtbCodigo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbCodigo.DetectUrls = false;
            this.rtbCodigo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCodigo.Font = new System.Drawing.Font("Consolas", 11F);
            this.rtbCodigo.HideSelection = false;
            this.rtbCodigo.Location = new System.Drawing.Point(55, 0);
            this.rtbCodigo.Name = "rtbCodigo";
            this.rtbCodigo.Size = new System.Drawing.Size(493, 615);
            this.rtbCodigo.TabIndex = 0;
            this.rtbCodigo.Text = "";
            this.rtbCodigo.WordWrap = false;
            this.rtbCodigo.DragDrop += new System.Windows.Forms.DragEventHandler(this.rtbCodigo_DragDrop);
            this.rtbCodigo.DragEnter += new System.Windows.Forms.DragEventHandler(this.rtbCodigo_DragEnter);
            this.rtbCodigo.SelectionChanged += new System.EventHandler(this.rtbCodigo_SelectionChanged);
            this.rtbCodigo.VScroll += new System.EventHandler(this.rtbCodigo_VScroll);
            this.rtbCodigo.TextChanged += new System.EventHandler(this.rtbCodigo_TextChanged);
            // 
            // rtbLineas
            // 
            this.rtbLineas.BackColor = System.Drawing.Color.Gainsboro;
            this.rtbLineas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLineas.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbLineas.Dock = System.Windows.Forms.DockStyle.Left;
            this.rtbLineas.Font = new System.Drawing.Font("Consolas", 11F);
            this.rtbLineas.ForeColor = System.Drawing.Color.DimGray;
            this.rtbLineas.Location = new System.Drawing.Point(0, 0);
            this.rtbLineas.Name = "rtbLineas";
            this.rtbLineas.ReadOnly = true;
            this.rtbLineas.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbLineas.Size = new System.Drawing.Size(55, 615);
            this.rtbLineas.TabIndex = 1;
            this.rtbLineas.TabStop = false;
            this.rtbLineas.Text = "";
            this.rtbLineas.WordWrap = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTokens);
            this.tabControl1.Controls.Add(this.tabErrores);
            this.tabControl1.Controls.Add(this.tabSimbolos);
            this.tabControl1.Controls.Add(this.tabEstadisticas);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(704, 615);
            this.tabControl1.TabIndex = 0;
            // 
            // tabTokens
            // 
            this.tabTokens.Controls.Add(this.dgvTokens);
            this.tabTokens.Location = new System.Drawing.Point(4, 25);
            this.tabTokens.Name = "tabTokens";
            this.tabTokens.Size = new System.Drawing.Size(696, 586);
            this.tabTokens.TabIndex = 0;
            this.tabTokens.Text = "Tokens";
            this.tabTokens.UseVisualStyleBackColor = true;
            // 
            // dgvTokens
            // 
            this.dgvTokens.AllowUserToAddRows = false;
            this.dgvTokens.AllowUserToDeleteRows = false;
            this.dgvTokens.AllowUserToResizeRows = false;
            this.dgvTokens.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTokens.BackgroundColor = System.Drawing.Color.White;
            this.dgvTokens.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTokens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTokens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNum,
            this.colLexema,
            this.colTipo,
            this.colCategoria,
            this.colLinea,
            this.colColumna});
            this.dgvTokens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTokens.Location = new System.Drawing.Point(0, 0);
            this.dgvTokens.MultiSelect = false;
            this.dgvTokens.Name = "dgvTokens";
            this.dgvTokens.ReadOnly = true;
            this.dgvTokens.RowHeadersVisible = false;
            this.dgvTokens.RowHeadersWidth = 51;
            this.dgvTokens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTokens.Size = new System.Drawing.Size(696, 586);
            this.dgvTokens.TabIndex = 0;
            this.dgvTokens.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTokens_CellDoubleClick);
            // 
            // tabErrores
            // 
            this.tabErrores.Controls.Add(this.dgvErrores);
            this.tabErrores.Location = new System.Drawing.Point(4, 25);
            this.tabErrores.Name = "tabErrores";
            this.tabErrores.Size = new System.Drawing.Size(696, 590);
            this.tabErrores.TabIndex = 1;
            this.tabErrores.Text = "Errores";
            this.tabErrores.UseVisualStyleBackColor = true;
            // 
            // dgvErrores
            // 
            this.dgvErrores.AllowUserToAddRows = false;
            this.dgvErrores.AllowUserToDeleteRows = false;
            this.dgvErrores.AllowUserToResizeRows = false;
            this.dgvErrores.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvErrores.BackgroundColor = System.Drawing.Color.White;
            this.dgvErrores.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvErrores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvErrores.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colErrRegla,
            this.colErrLexema,
            this.colErrLinea,
            this.colErrColumna,
            this.colErrDescripcion});
            this.dgvErrores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvErrores.Location = new System.Drawing.Point(0, 0);
            this.dgvErrores.MultiSelect = false;
            this.dgvErrores.Name = "dgvErrores";
            this.dgvErrores.ReadOnly = true;
            this.dgvErrores.RowHeadersVisible = false;
            this.dgvErrores.RowHeadersWidth = 51;
            this.dgvErrores.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvErrores.Size = new System.Drawing.Size(696, 590);
            this.dgvErrores.TabIndex = 0;
            this.dgvErrores.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvErrores_CellDoubleClick);
            // 
            // tabSimbolos
            // 
            this.tabSimbolos.Controls.Add(this.dgvSimbolos);
            this.tabSimbolos.Location = new System.Drawing.Point(4, 25);
            this.tabSimbolos.Name = "tabSimbolos";
            this.tabSimbolos.Size = new System.Drawing.Size(696, 590);
            this.tabSimbolos.TabIndex = 2;
            this.tabSimbolos.Text = "Tabla de símbolos";
            this.tabSimbolos.UseVisualStyleBackColor = true;
            // 
            // dgvSimbolos
            // 
            this.dgvSimbolos.AllowUserToAddRows = false;
            this.dgvSimbolos.AllowUserToDeleteRows = false;
            this.dgvSimbolos.AllowUserToResizeRows = false;
            this.dgvSimbolos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSimbolos.BackgroundColor = System.Drawing.Color.White;
            this.dgvSimbolos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSimbolos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSimbolos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSimId,
            this.colSimLexema,
            this.colSimTipoToken,
            this.colSimTipoDato,
            this.colSimPrimera,
            this.colSimFrecuencia,
            this.colSimRefs});
            this.dgvSimbolos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSimbolos.Location = new System.Drawing.Point(0, 0);
            this.dgvSimbolos.MultiSelect = false;
            this.dgvSimbolos.Name = "dgvSimbolos";
            this.dgvSimbolos.ReadOnly = true;
            this.dgvSimbolos.RowHeadersVisible = false;
            this.dgvSimbolos.RowHeadersWidth = 51;
            this.dgvSimbolos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSimbolos.Size = new System.Drawing.Size(696, 590);
            this.dgvSimbolos.TabIndex = 0;
            this.dgvSimbolos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSimbolos_CellDoubleClick);
            // 
            // tabEstadisticas
            // 
            this.tabEstadisticas.Controls.Add(this.txtEstadisticas);
            this.tabEstadisticas.Location = new System.Drawing.Point(4, 25);
            this.tabEstadisticas.Name = "tabEstadisticas";
            this.tabEstadisticas.Size = new System.Drawing.Size(696, 590);
            this.tabEstadisticas.TabIndex = 3;
            this.tabEstadisticas.Text = "Estadísticas";
            this.tabEstadisticas.UseVisualStyleBackColor = true;
            // 
            // txtEstadisticas
            // 
            this.txtEstadisticas.BackColor = System.Drawing.Color.White;
            this.txtEstadisticas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEstadisticas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEstadisticas.Font = new System.Drawing.Font("Consolas", 10.5F);
            this.txtEstadisticas.Location = new System.Drawing.Point(0, 0);
            this.txtEstadisticas.Multiline = true;
            this.txtEstadisticas.Name = "txtEstadisticas";
            this.txtEstadisticas.ReadOnly = true;
            this.txtEstadisticas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEstadisticas.Size = new System.Drawing.Size(696, 590);
            this.txtEstadisticas.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblEstado,
            this.lblPosicion,
            this.lblResumen});
            this.statusStrip1.Location = new System.Drawing.Point(0, 668);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1257, 26);
            this.statusStrip1.TabIndex = 2;
            // 
            // lblEstado
            // 
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(1082, 20);
            this.lblEstado.Spring = true;
            this.lblEstado.Text = "Escriba código o abra un archivo y pulse Analizar.";
            this.lblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPosicion
            // 
            this.lblPosicion.Name = "lblPosicion";
            this.lblPosicion.Size = new System.Drawing.Size(77, 20);
            this.lblPosicion.Text = "Ln 1, Col 1";
            // 
            // lblResumen
            // 
            this.lblResumen.Name = "lblResumen";
            this.lblResumen.Size = new System.Drawing.Size(81, 20);
            this.lblResumen.Text = "Sin análisis";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Código fuente (*.txt;*.cs;*.cpp)|*.txt;*.cs;*.cpp|Todos los archivos (*.*)|*.*";
            this.openFileDialog1.Title = "Abrir código fuente";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Reporte completo (*.txt)|*.txt|Tokens (*.csv)|*.csv|Errores léxicos (*.csv)|*.csv" +
    "|Tabla de símbolos (*.csv)|*.csv|Reporte HTML (*.html)|*.html";
            this.saveFileDialog1.Title = "Exportar resultados";
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1257, 694);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlSuperior);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1026, 530);
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Analizador Léxico";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmPrincipal_KeyDown);
            this.pnlSuperior.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabTokens.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTokens)).EndInit();
            this.tabErrores.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvErrores)).EndInit();
            this.tabSimbolos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSimbolos)).EndInit();
            this.tabEstadisticas.ResumeLayout(false);
            this.tabEstadisticas.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel pnlSuperior;
        private System.Windows.Forms.Button btnAbrir;
        private System.Windows.Forms.Button btnAnalizar;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox rtbCodigo;
        private System.Windows.Forms.RichTextBox rtbLineas;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTokens;
        private System.Windows.Forms.TabPage tabErrores;
        private System.Windows.Forms.TabPage tabSimbolos;
        private System.Windows.Forms.TabPage tabEstadisticas;
        private System.Windows.Forms.DataGridView dgvTokens;
        private System.Windows.Forms.DataGridView dgvErrores;
        private System.Windows.Forms.DataGridView dgvSimbolos;
        private System.Windows.Forms.TextBox txtEstadisticas;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblEstado;
        private System.Windows.Forms.ToolStripStatusLabel lblPosicion;
        private System.Windows.Forms.ToolStripStatusLabel lblResumen;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLexema;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colColumna;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrRegla;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrLexema;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrLinea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrColumna;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrDescripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSimId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSimLexema;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSimTipoToken;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSimTipoDato;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSimPrimera;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSimFrecuencia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSimRefs;
    }
}
