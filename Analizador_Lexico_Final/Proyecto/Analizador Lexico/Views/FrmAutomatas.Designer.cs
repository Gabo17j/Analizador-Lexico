namespace Analizador_Lexico.Views
{
    partial class FrmAutomatas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAutomatas));
            this.lblProyecto = new System.Windows.Forms.Label();
            this.pnlIzq = new System.Windows.Forms.Panel();
            this.grpTabla = new System.Windows.Forms.GroupBox();
            this.lblAyuda = new System.Windows.Forms.Label();
            this.dgvTransiciones = new System.Windows.Forms.DataGridView();
            this.lblEstadoTabla = new System.Windows.Forms.Label();
            this.grpEstados = new System.Windows.Forms.GroupBox();
            this.lblInicial = new System.Windows.Forms.Label();
            this.cmbInicial = new System.Windows.Forms.ComboBox();
            this.lblFinales = new System.Windows.Forms.Label();
            this.clbFinales = new System.Windows.Forms.CheckedListBox();
            this.grpDef = new System.Windows.Forms.GroupBox();
            this.lblEstados = new System.Windows.Forms.Label();
            this.nudEstados = new System.Windows.Forms.NumericUpDown();
            this.lblSimbolos = new System.Windows.Forms.Label();
            this.nudSimbolos = new System.Windows.Forms.NumericUpDown();
            this.lblAlfabeto = new System.Windows.Forms.Label();
            this.txtAlfabeto = new System.Windows.Forms.TextBox();
            this.grpTipo = new System.Windows.Forms.GroupBox();
            this.rbAFD = new System.Windows.Forms.RadioButton();
            this.rbAFND = new System.Windows.Forms.RadioButton();
            this.chkEpsilon = new System.Windows.Forms.CheckBox();
            this.pnlBotones = new System.Windows.Forms.Panel();
            this.btnEjemplo = new System.Windows.Forms.Button();
            this.btnLimpiarTabla = new System.Windows.Forms.Button();
            this.btnDibujar = new System.Windows.Forms.Button();
            this.btnGuardarImagen = new System.Windows.Forms.Button();
            this.pnlDer = new System.Windows.Forms.Panel();
            this.pnlAutomata = new Analizador_Lexico.Views.PanelAutomata();
            this.pnlSim = new System.Windows.Forms.Panel();
            this.lblCadena = new System.Windows.Forms.Label();
            this.txtCadena = new System.Windows.Forms.TextBox();
            this.btnAnalizar = new System.Windows.Forms.Button();
            this.btnAnterior = new System.Windows.Forms.Button();
            this.btnSiguiente = new System.Windows.Forms.Button();
            this.lblPaso = new System.Windows.Forms.Label();
            this.lblResultado = new System.Windows.Forms.Label();
            this.lstHilos = new System.Windows.Forms.ListBox();
            this.dgvPasos = new System.Windows.Forms.DataGridView();
            this.colPasoN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPasoSim = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPasoEstados = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlIzq.SuspendLayout();
            this.grpTabla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransiciones)).BeginInit();
            this.grpEstados.SuspendLayout();
            this.grpDef.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEstados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSimbolos)).BeginInit();
            this.grpTipo.SuspendLayout();
            this.pnlBotones.SuspendLayout();
            this.pnlDer.SuspendLayout();
            this.pnlSim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPasos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProyecto
            // 
            this.lblProyecto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(45)))), ((int)(((byte)(86)))));
            this.lblProyecto.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProyecto.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblProyecto.ForeColor = System.Drawing.Color.White;
            this.lblProyecto.Location = new System.Drawing.Point(0, 0);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblProyecto.Size = new System.Drawing.Size(1250, 30);
            this.lblProyecto.TabIndex = 2;
            this.lblProyecto.Text = "Proyecto";
            this.lblProyecto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlIzq
            // 
            this.pnlIzq.Controls.Add(this.grpTabla);
            this.pnlIzq.Controls.Add(this.grpEstados);
            this.pnlIzq.Controls.Add(this.grpDef);
            this.pnlIzq.Controls.Add(this.grpTipo);
            this.pnlIzq.Controls.Add(this.pnlBotones);
            this.pnlIzq.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlIzq.Location = new System.Drawing.Point(0, 30);
            this.pnlIzq.Name = "pnlIzq";
            this.pnlIzq.Size = new System.Drawing.Size(460, 666);
            this.pnlIzq.TabIndex = 1;
            // 
            // grpTabla
            // 
            this.grpTabla.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTabla.Controls.Add(this.lblAyuda);
            this.grpTabla.Controls.Add(this.dgvTransiciones);
            this.grpTabla.Controls.Add(this.lblEstadoTabla);
            this.grpTabla.Location = new System.Drawing.Point(8, 256);
            this.grpTabla.Name = "grpTabla";
            this.grpTabla.Size = new System.Drawing.Size(444, 358);
            this.grpTabla.TabIndex = 3;
            this.grpTabla.TabStop = false;
            this.grpTabla.Text = "Tabla de transiciones";
            // 
            // lblAyuda
            // 
            this.lblAyuda.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.lblAyuda.Location = new System.Drawing.Point(10, 18);
            this.lblAyuda.Name = "lblAyuda";
            this.lblAyuda.Size = new System.Drawing.Size(424, 44);
            this.lblAyuda.TabIndex = 0;
            this.lblAyuda.Text = "Ayuda";
            // 
            // dgvTransiciones
            // 
            this.dgvTransiciones.AllowUserToAddRows = false;
            this.dgvTransiciones.AllowUserToDeleteRows = false;
            this.dgvTransiciones.AllowUserToResizeRows = false;
            this.dgvTransiciones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTransiciones.BackgroundColor = System.Drawing.Color.White;
            this.dgvTransiciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransiciones.Location = new System.Drawing.Point(10, 66);
            this.dgvTransiciones.MultiSelect = false;
            this.dgvTransiciones.Name = "dgvTransiciones";
            this.dgvTransiciones.RowHeadersVisible = false;
            this.dgvTransiciones.RowHeadersWidth = 51;
            this.dgvTransiciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTransiciones.Size = new System.Drawing.Size(424, 250);
            this.dgvTransiciones.TabIndex = 1;
            this.dgvTransiciones.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTransiciones_CellEndEdit);
            // 
            // lblEstadoTabla
            // 
            this.lblEstadoTabla.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEstadoTabla.Location = new System.Drawing.Point(10, 320);
            this.lblEstadoTabla.Name = "lblEstadoTabla";
            this.lblEstadoTabla.Size = new System.Drawing.Size(424, 32);
            this.lblEstadoTabla.TabIndex = 2;
            // 
            // grpEstados
            // 
            this.grpEstados.Controls.Add(this.lblInicial);
            this.grpEstados.Controls.Add(this.cmbInicial);
            this.grpEstados.Controls.Add(this.lblFinales);
            this.grpEstados.Controls.Add(this.clbFinales);
            this.grpEstados.Location = new System.Drawing.Point(8, 142);
            this.grpEstados.Name = "grpEstados";
            this.grpEstados.Size = new System.Drawing.Size(444, 112);
            this.grpEstados.TabIndex = 2;
            this.grpEstados.TabStop = false;
            this.grpEstados.Text = "Estado inicial y estados finales";
            // 
            // lblInicial
            // 
            this.lblInicial.AutoSize = true;
            this.lblInicial.Location = new System.Drawing.Point(14, 25);
            this.lblInicial.Name = "lblInicial";
            this.lblInicial.Size = new System.Drawing.Size(100, 20);
            this.lblInicial.TabIndex = 0;
            this.lblInicial.Text = "Estado inicial:";
            // 
            // cmbInicial
            // 
            this.cmbInicial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInicial.FormattingEnabled = true;
            this.cmbInicial.Location = new System.Drawing.Point(110, 22);
            this.cmbInicial.Name = "cmbInicial";
            this.cmbInicial.Size = new System.Drawing.Size(90, 28);
            this.cmbInicial.TabIndex = 1;
            this.cmbInicial.SelectedIndexChanged += new System.EventHandler(this.cmbInicial_SelectedIndexChanged);
            // 
            // lblFinales
            // 
            this.lblFinales.AutoSize = true;
            this.lblFinales.Location = new System.Drawing.Point(14, 54);
            this.lblFinales.Name = "lblFinales";
            this.lblFinales.Size = new System.Drawing.Size(110, 20);
            this.lblFinales.TabIndex = 2;
            this.lblFinales.Text = "Estados finales:";
            // 
            // clbFinales
            // 
            this.clbFinales.CheckOnClick = true;
            this.clbFinales.ColumnWidth = 62;
            this.clbFinales.FormattingEnabled = true;
            this.clbFinales.Location = new System.Drawing.Point(110, 52);
            this.clbFinales.MultiColumn = true;
            this.clbFinales.Name = "clbFinales";
            this.clbFinales.Size = new System.Drawing.Size(320, 48);
            this.clbFinales.TabIndex = 3;
            this.clbFinales.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbFinales_ItemCheck);
            // 
            // grpDef
            // 
            this.grpDef.Controls.Add(this.lblEstados);
            this.grpDef.Controls.Add(this.nudEstados);
            this.grpDef.Controls.Add(this.lblSimbolos);
            this.grpDef.Controls.Add(this.nudSimbolos);
            this.grpDef.Controls.Add(this.lblAlfabeto);
            this.grpDef.Controls.Add(this.txtAlfabeto);
            this.grpDef.Location = new System.Drawing.Point(8, 58);
            this.grpDef.Name = "grpDef";
            this.grpDef.Size = new System.Drawing.Size(444, 82);
            this.grpDef.TabIndex = 1;
            this.grpDef.TabStop = false;
            this.grpDef.Text = "Estados (n) y símbolos de entrada (m)";
            // 
            // lblEstados
            // 
            this.lblEstados.AutoSize = true;
            this.lblEstados.Location = new System.Drawing.Point(14, 24);
            this.lblEstados.Name = "lblEstados";
            this.lblEstados.Size = new System.Drawing.Size(85, 20);
            this.lblEstados.TabIndex = 0;
            this.lblEstados.Text = "n (estados):";
            // 
            // nudEstados
            // 
            this.nudEstados.Location = new System.Drawing.Point(96, 21);
            this.nudEstados.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudEstados.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEstados.Name = "nudEstados";
            this.nudEstados.Size = new System.Drawing.Size(60, 27);
            this.nudEstados.TabIndex = 1;
            this.nudEstados.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudEstados.ValueChanged += new System.EventHandler(this.nudEstados_ValueChanged);
            // 
            // lblSimbolos
            // 
            this.lblSimbolos.AutoSize = true;
            this.lblSimbolos.Location = new System.Drawing.Point(176, 24);
            this.lblSimbolos.Name = "lblSimbolos";
            this.lblSimbolos.Size = new System.Drawing.Size(99, 20);
            this.lblSimbolos.TabIndex = 2;
            this.lblSimbolos.Text = "m (símbolos):";
            // 
            // nudSimbolos
            // 
            this.nudSimbolos.Location = new System.Drawing.Point(262, 21);
            this.nudSimbolos.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSimbolos.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSimbolos.Name = "nudSimbolos";
            this.nudSimbolos.Size = new System.Drawing.Size(60, 27);
            this.nudSimbolos.TabIndex = 3;
            this.nudSimbolos.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudSimbolos.ValueChanged += new System.EventHandler(this.nudSimbolos_ValueChanged);
            // 
            // lblAlfabeto
            // 
            this.lblAlfabeto.AutoSize = true;
            this.lblAlfabeto.Location = new System.Drawing.Point(14, 53);
            this.lblAlfabeto.Name = "lblAlfabeto";
            this.lblAlfabeto.Size = new System.Drawing.Size(70, 20);
            this.lblAlfabeto.TabIndex = 4;
            this.lblAlfabeto.Text = "Alfabeto:";
            // 
            // txtAlfabeto
            // 
            this.txtAlfabeto.Location = new System.Drawing.Point(96, 50);
            this.txtAlfabeto.Name = "txtAlfabeto";
            this.txtAlfabeto.Size = new System.Drawing.Size(330, 27);
            this.txtAlfabeto.TabIndex = 5;
            this.txtAlfabeto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAlfabeto_KeyDown);
            this.txtAlfabeto.Leave += new System.EventHandler(this.txtAlfabeto_Leave);
            // 
            // grpTipo
            // 
            this.grpTipo.Controls.Add(this.rbAFD);
            this.grpTipo.Controls.Add(this.rbAFND);
            this.grpTipo.Controls.Add(this.chkEpsilon);
            this.grpTipo.Location = new System.Drawing.Point(8, 4);
            this.grpTipo.Name = "grpTipo";
            this.grpTipo.Size = new System.Drawing.Size(444, 52);
            this.grpTipo.TabIndex = 0;
            this.grpTipo.TabStop = false;
            this.grpTipo.Text = "Tipo de autómata";
            // 
            // rbAFD
            // 
            this.rbAFD.AutoSize = true;
            this.rbAFD.Checked = true;
            this.rbAFD.Location = new System.Drawing.Point(14, 22);
            this.rbAFD.Name = "rbAFD";
            this.rbAFD.Size = new System.Drawing.Size(58, 24);
            this.rbAFD.TabIndex = 0;
            this.rbAFD.TabStop = true;
            this.rbAFD.Text = "AFD";
            this.rbAFD.UseVisualStyleBackColor = true;
            this.rbAFD.CheckedChanged += new System.EventHandler(this.rbTipo_CheckedChanged);
            // 
            // rbAFND
            // 
            this.rbAFND.AutoSize = true;
            this.rbAFND.Location = new System.Drawing.Point(88, 22);
            this.rbAFND.Name = "rbAFND";
            this.rbAFND.Size = new System.Drawing.Size(69, 24);
            this.rbAFND.TabIndex = 1;
            this.rbAFND.Text = "AFND";
            this.rbAFND.UseVisualStyleBackColor = true;
            this.rbAFND.CheckedChanged += new System.EventHandler(this.rbTipo_CheckedChanged);
            // 
            // chkEpsilon
            // 
            this.chkEpsilon.AutoSize = true;
            this.chkEpsilon.Enabled = false;
            this.chkEpsilon.Location = new System.Drawing.Point(180, 22);
            this.chkEpsilon.Name = "chkEpsilon";
            this.chkEpsilon.Size = new System.Drawing.Size(249, 24);
            this.chkEpsilon.TabIndex = 2;
            this.chkEpsilon.Text = "Incluir transiciones ε (solo AFND)";
            this.chkEpsilon.UseVisualStyleBackColor = true;
            this.chkEpsilon.CheckedChanged += new System.EventHandler(this.chkEpsilon_CheckedChanged);
            // 
            // pnlBotones
            // 
            this.pnlBotones.Controls.Add(this.btnEjemplo);
            this.pnlBotones.Controls.Add(this.btnLimpiarTabla);
            this.pnlBotones.Controls.Add(this.btnDibujar);
            this.pnlBotones.Controls.Add(this.btnGuardarImagen);
            this.pnlBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBotones.Location = new System.Drawing.Point(0, 620);
            this.pnlBotones.Name = "pnlBotones";
            this.pnlBotones.Size = new System.Drawing.Size(460, 46);
            this.pnlBotones.TabIndex = 4;
            // 
            // btnEjemplo
            // 
            this.btnEjemplo.Location = new System.Drawing.Point(8, 8);
            this.btnEjemplo.Name = "btnEjemplo";
            this.btnEjemplo.Size = new System.Drawing.Size(108, 32);
            this.btnEjemplo.TabIndex = 0;
            this.btnEjemplo.Text = "Cargar ejemplo";
            this.btnEjemplo.UseVisualStyleBackColor = true;
            this.btnEjemplo.Click += new System.EventHandler(this.btnEjemplo_Click);
            // 
            // btnLimpiarTabla
            // 
            this.btnLimpiarTabla.Location = new System.Drawing.Point(120, 8);
            this.btnLimpiarTabla.Name = "btnLimpiarTabla";
            this.btnLimpiarTabla.Size = new System.Drawing.Size(104, 32);
            this.btnLimpiarTabla.TabIndex = 1;
            this.btnLimpiarTabla.Text = "Limpiar tabla";
            this.btnLimpiarTabla.UseVisualStyleBackColor = true;
            this.btnLimpiarTabla.Click += new System.EventHandler(this.btnLimpiarTabla_Click);
            // 
            // btnDibujar
            // 
            this.btnDibujar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDibujar.Location = new System.Drawing.Point(228, 8);
            this.btnDibujar.Name = "btnDibujar";
            this.btnDibujar.Size = new System.Drawing.Size(130, 32);
            this.btnDibujar.TabIndex = 2;
            this.btnDibujar.Text = "Dibujar autómata";
            this.btnDibujar.UseVisualStyleBackColor = true;
            this.btnDibujar.Click += new System.EventHandler(this.btnDibujar_Click);
            // 
            // btnGuardarImagen
            // 
            this.btnGuardarImagen.Location = new System.Drawing.Point(362, 8);
            this.btnGuardarImagen.Name = "btnGuardarImagen";
            this.btnGuardarImagen.Size = new System.Drawing.Size(90, 32);
            this.btnGuardarImagen.TabIndex = 3;
            this.btnGuardarImagen.Text = "Guardar PNG";
            this.btnGuardarImagen.UseVisualStyleBackColor = true;
            this.btnGuardarImagen.Click += new System.EventHandler(this.btnGuardarImagen_Click);
            // 
            // pnlDer
            // 
            this.pnlDer.Controls.Add(this.pnlAutomata);
            this.pnlDer.Controls.Add(this.pnlSim);
            this.pnlDer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDer.Location = new System.Drawing.Point(460, 30);
            this.pnlDer.Name = "pnlDer";
            this.pnlDer.Size = new System.Drawing.Size(790, 666);
            this.pnlDer.TabIndex = 0;
            // 
            // pnlAutomata
            // 
            this.pnlAutomata.BackColor = System.Drawing.Color.White;
            this.pnlAutomata.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAutomata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAutomata.EstadosActivos = ((System.Collections.Generic.HashSet<string>)(resources.GetObject("pnlAutomata.EstadosActivos")));
            this.pnlAutomata.HiloActivo = null;
            this.pnlAutomata.Location = new System.Drawing.Point(0, 0);
            this.pnlAutomata.Modelo = null;
            this.pnlAutomata.Name = "pnlAutomata";
            this.pnlAutomata.PasoActual = 0;
            this.pnlAutomata.Size = new System.Drawing.Size(790, 406);
            this.pnlAutomata.TabIndex = 0;
            // 
            // pnlSim
            // 
            this.pnlSim.Controls.Add(this.lblCadena);
            this.pnlSim.Controls.Add(this.txtCadena);
            this.pnlSim.Controls.Add(this.btnAnalizar);
            this.pnlSim.Controls.Add(this.btnAnterior);
            this.pnlSim.Controls.Add(this.btnSiguiente);
            this.pnlSim.Controls.Add(this.lblPaso);
            this.pnlSim.Controls.Add(this.lblResultado);
            this.pnlSim.Controls.Add(this.lstHilos);
            this.pnlSim.Controls.Add(this.dgvPasos);
            this.pnlSim.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSim.Location = new System.Drawing.Point(0, 406);
            this.pnlSim.Name = "pnlSim";
            this.pnlSim.Size = new System.Drawing.Size(790, 260);
            this.pnlSim.TabIndex = 1;
            // 
            // lblCadena
            // 
            this.lblCadena.AutoSize = true;
            this.lblCadena.Location = new System.Drawing.Point(10, 14);
            this.lblCadena.Name = "lblCadena";
            this.lblCadena.Size = new System.Drawing.Size(138, 20);
            this.lblCadena.TabIndex = 0;
            this.lblCadena.Text = "Cadena de entrada:";
            // 
            // txtCadena
            // 
            this.txtCadena.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtCadena.Location = new System.Drawing.Point(128, 10);
            this.txtCadena.Name = "txtCadena";
            this.txtCadena.Size = new System.Drawing.Size(190, 27);
            this.txtCadena.TabIndex = 1;
            // 
            // btnAnalizar
            // 
            this.btnAnalizar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAnalizar.Location = new System.Drawing.Point(326, 8);
            this.btnAnalizar.Name = "btnAnalizar";
            this.btnAnalizar.Size = new System.Drawing.Size(120, 28);
            this.btnAnalizar.TabIndex = 2;
            this.btnAnalizar.Text = "Analizar cadena";
            this.btnAnalizar.UseVisualStyleBackColor = true;
            this.btnAnalizar.Click += new System.EventHandler(this.btnAnalizar_Click);
            // 
            // btnAnterior
            // 
            this.btnAnterior.Enabled = false;
            this.btnAnterior.Location = new System.Drawing.Point(456, 8);
            this.btnAnterior.Name = "btnAnterior";
            this.btnAnterior.Size = new System.Drawing.Size(36, 28);
            this.btnAnterior.TabIndex = 3;
            this.btnAnterior.Text = "◀";
            this.btnAnterior.UseVisualStyleBackColor = true;
            this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);
            // 
            // btnSiguiente
            // 
            this.btnSiguiente.Enabled = false;
            this.btnSiguiente.Location = new System.Drawing.Point(496, 8);
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(36, 28);
            this.btnSiguiente.TabIndex = 4;
            this.btnSiguiente.Text = "▶";
            this.btnSiguiente.UseVisualStyleBackColor = true;
            this.btnSiguiente.Click += new System.EventHandler(this.btnSiguiente_Click);
            // 
            // lblPaso
            // 
            this.lblPaso.Location = new System.Drawing.Point(540, 8);
            this.lblPaso.Name = "lblPaso";
            this.lblPaso.Size = new System.Drawing.Size(244, 30);
            this.lblPaso.TabIndex = 5;
            this.lblPaso.Text = "Paso: —";
            this.lblPaso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblResultado
            // 
            this.lblResultado.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblResultado.Location = new System.Drawing.Point(10, 40);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(770, 24);
            this.lblResultado.TabIndex = 6;
            this.lblResultado.Text = "Resultado: —";
            this.lblResultado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lstHilos
            // 
            this.lstHilos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstHilos.Font = new System.Drawing.Font("Consolas", 9F);
            this.lstHilos.FormattingEnabled = true;
            this.lstHilos.HorizontalScrollbar = true;
            this.lstHilos.ItemHeight = 18;
            this.lstHilos.Location = new System.Drawing.Point(10, 68);
            this.lstHilos.Name = "lstHilos";
            this.lstHilos.Size = new System.Drawing.Size(430, 184);
            this.lstHilos.TabIndex = 7;
            this.lstHilos.SelectedIndexChanged += new System.EventHandler(this.lstHilos_SelectedIndexChanged);
            // 
            // dgvPasos
            // 
            this.dgvPasos.AllowUserToAddRows = false;
            this.dgvPasos.AllowUserToDeleteRows = false;
            this.dgvPasos.AllowUserToResizeRows = false;
            this.dgvPasos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPasos.BackgroundColor = System.Drawing.Color.White;
            this.dgvPasos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPasos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPasoN,
            this.colPasoSim,
            this.colPasoEstados});
            this.dgvPasos.Location = new System.Drawing.Point(448, 68);
            this.dgvPasos.MultiSelect = false;
            this.dgvPasos.Name = "dgvPasos";
            this.dgvPasos.ReadOnly = true;
            this.dgvPasos.RowHeadersVisible = false;
            this.dgvPasos.RowHeadersWidth = 51;
            this.dgvPasos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPasos.Size = new System.Drawing.Size(334, 186);
            this.dgvPasos.TabIndex = 8;
            this.dgvPasos.SelectionChanged += new System.EventHandler(this.dgvPasos_SelectionChanged);
            // 
            // colPasoN
            // 
            this.colPasoN.HeaderText = "Paso";
            this.colPasoN.MinimumWidth = 6;
            this.colPasoN.Name = "colPasoN";
            this.colPasoN.ReadOnly = true;
            this.colPasoN.Width = 46;
            // 
            // colPasoSim
            // 
            this.colPasoSim.HeaderText = "Símbolo";
            this.colPasoSim.MinimumWidth = 6;
            this.colPasoSim.Name = "colPasoSim";
            this.colPasoSim.ReadOnly = true;
            this.colPasoSim.Width = 66;
            // 
            // colPasoEstados
            // 
            this.colPasoEstados.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPasoEstados.HeaderText = "Estados activos";
            this.colPasoEstados.MinimumWidth = 6;
            this.colPasoEstados.Name = "colPasoEstados";
            this.colPasoEstados.ReadOnly = true;
            // 
            // FrmAutomatas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 696);
            this.Controls.Add(this.pnlDer);
            this.Controls.Add(this.pnlIzq);
            this.Controls.Add(this.lblProyecto);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1000, 640);
            this.Name = "FrmAutomatas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Autómatas finitos";
            this.pnlIzq.ResumeLayout(false);
            this.grpTabla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransiciones)).EndInit();
            this.grpEstados.ResumeLayout(false);
            this.grpEstados.PerformLayout();
            this.grpDef.ResumeLayout(false);
            this.grpDef.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEstados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSimbolos)).EndInit();
            this.grpTipo.ResumeLayout(false);
            this.grpTipo.PerformLayout();
            this.pnlBotones.ResumeLayout(false);
            this.pnlDer.ResumeLayout(false);
            this.pnlSim.ResumeLayout(false);
            this.pnlSim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPasos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblProyecto;
        private System.Windows.Forms.Panel pnlIzq;
        private System.Windows.Forms.GroupBox grpTipo;
        private System.Windows.Forms.RadioButton rbAFD;
        private System.Windows.Forms.RadioButton rbAFND;
        private System.Windows.Forms.CheckBox chkEpsilon;
        private System.Windows.Forms.GroupBox grpDef;
        private System.Windows.Forms.Label lblEstados;
        private System.Windows.Forms.NumericUpDown nudEstados;
        private System.Windows.Forms.Label lblSimbolos;
        private System.Windows.Forms.NumericUpDown nudSimbolos;
        private System.Windows.Forms.Label lblAlfabeto;
        private System.Windows.Forms.TextBox txtAlfabeto;
        private System.Windows.Forms.GroupBox grpEstados;
        private System.Windows.Forms.Label lblInicial;
        private System.Windows.Forms.ComboBox cmbInicial;
        private System.Windows.Forms.Label lblFinales;
        private System.Windows.Forms.CheckedListBox clbFinales;
        private System.Windows.Forms.GroupBox grpTabla;
        private System.Windows.Forms.Label lblAyuda;
        private System.Windows.Forms.DataGridView dgvTransiciones;
        private System.Windows.Forms.Label lblEstadoTabla;
        private System.Windows.Forms.Panel pnlBotones;
        private System.Windows.Forms.Button btnEjemplo;
        private System.Windows.Forms.Button btnLimpiarTabla;
        private System.Windows.Forms.Button btnDibujar;
        private System.Windows.Forms.Button btnGuardarImagen;
        private System.Windows.Forms.Panel pnlDer;
        private System.Windows.Forms.Panel pnlSim;
        private System.Windows.Forms.Label lblCadena;
        private System.Windows.Forms.TextBox txtCadena;
        private System.Windows.Forms.Button btnAnalizar;
        private System.Windows.Forms.Button btnAnterior;
        private System.Windows.Forms.Button btnSiguiente;
        private System.Windows.Forms.Label lblPaso;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.ListBox lstHilos;
        private System.Windows.Forms.DataGridView dgvPasos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPasoN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPasoSim;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPasoEstados;
        private Analizador_Lexico.Views.PanelAutomata pnlAutomata;
    }
}
