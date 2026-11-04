using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Analizador_Lexico.Core;

namespace Analizador_Lexico.Views
{
    public partial class FrmAutomatas : Form
    {
        private const string Abecedario = "abcdefghijklmnopqrstuvwxyz";
        private const string ColEstado = "colEstado";

        private bool cargando = true;
        private ResultadoCadena ultimo; 

        public FrmAutomatas()
        {
            InitializeComponent();
            txtAlfabeto.Text = "a,b";
            cargando = false;
            ReconstruirTabla();
            AdaptarModo();
        }

        private bool EsAFND { get { return rbAFND.Checked; } }

        private void AdaptarModo()
        {
            chkEpsilon.Enabled = EsAFND;
            if (!EsAFND) chkEpsilon.Checked = false;
            if (EsAFND)
            {
                grpTabla.Text = "Tabla de transiciones — AFND";
                lblAyuda.Text = "AFND: cada celda admite uno o varios estados separados por coma (ej. q0,q1). "
                              + "Deje vacío si no hay transición." + (chkEpsilon.Checked ? " La columna ε es la transición vacía." : "");
            }
            else
            {
                grpTabla.Text = "Tabla de transiciones — AFD";
                lblAyuda.Text = "AFD: cada celda admite un solo estado (ej. q1). Una celda vacía significa que "
                              + "la cadena se bloquea (estado de error implícito).";
            }
            Text = "Autómatas finitos — " + (EsAFND ? "AFND" : "AFD");
        }

        private void rbTipo_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (cargando || (rb != null && !rb.Checked)) return;
            AdaptarModo();
            ReconstruirTabla();
            ActualizarDibujo(false);
        }

        private void chkEpsilon_CheckedChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            AdaptarModo();
            ReconstruirTabla();
            ActualizarDibujo(false);
        }

        private void nudEstados_ValueChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            ReconstruirTabla();
            ActualizarDibujo(false);
        }

        private void nudSimbolos_ValueChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            List<string> alfa = LeerAlfabeto();
            int m = (int)nudSimbolos.Value;
            while (alfa.Count > m) alfa.RemoveAt(alfa.Count - 1);
            foreach (char c in Abecedario)
            {
                if (alfa.Count >= m) break;
                if (!alfa.Contains(c.ToString())) alfa.Add(c.ToString());
            }
            cargando = true;
            txtAlfabeto.Text = string.Join(",", alfa);
            cargando = false;
            ReconstruirTabla();
            ActualizarDibujo(false);
        }

        private void txtAlfabeto_Leave(object sender, EventArgs e)
        {
            AplicarAlfabetoEscrito();
        }

        private void txtAlfabeto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; AplicarAlfabetoEscrito(); }
        }

        private void AplicarAlfabetoEscrito()
        {
            if (cargando) return;
            List<string> alfa = LeerAlfabeto();
            if (alfa.Count == 0)
            {
                MessageBox.Show(this, "Escriba al menos un símbolo, separados por coma (ej. a,b).", "Alfabeto",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            cargando = true;
            nudSimbolos.Value = Math.Min(nudSimbolos.Maximum, Math.Max(nudSimbolos.Minimum, alfa.Count));
            txtAlfabeto.Text = string.Join(",", alfa);
            cargando = false;
            ReconstruirTabla();
            ActualizarDibujo(false);
        }

        private List<string> LeerAlfabeto()
        {
            List<string> alfa = new List<string>();
            string texto = txtAlfabeto.Text;
            IEnumerable<string> partes = texto.Contains(",") || texto.Contains(";") || texto.Contains(" ")
                ? texto.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                : texto.Select(c => c.ToString());
            foreach (string p in partes)
            {
                string s = p.Trim();
                if (s.Length != 1 || s == Automata.Epsilon || alfa.Contains(s)) continue;
                alfa.Add(s);
            }
            return alfa;
        }
        private void ReconstruirTabla()
        {
            Dictionary<string, string> previo = new Dictionary<string, string>();
            foreach (DataGridViewRow fila in dgvTransiciones.Rows)
                foreach (DataGridViewColumn col in dgvTransiciones.Columns)
                    if (col.Name != ColEstado)
                        previo[Convert.ToString(fila.Cells[0].Value) + "\u0001" + col.HeaderText] =
                            Convert.ToString(fila.Cells[col.Index].Value ?? "");

            string inicialAnterior = cmbInicial.SelectedItem as string;
            HashSet<string> finalesAnteriores = new HashSet<string>(clbFinales.CheckedItems.Cast<string>());

            dgvTransiciones.Columns.Clear();
            dgvTransiciones.Rows.Clear();
            dgvTransiciones.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = ColEstado, HeaderText = "Estado", ReadOnly = true, Width = 62,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(235, 238, 248), Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            });

            List<string> simbolos = LeerAlfabeto();
            int m = (int)nudSimbolos.Value;
            while (simbolos.Count > m) simbolos.RemoveAt(simbolos.Count - 1);
            if (EsAFND && chkEpsilon.Checked) simbolos.Add(Automata.Epsilon);
            foreach (string s in simbolos)
                dgvTransiciones.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = s, Name = "col_" + s, SortMode = DataGridViewColumnSortMode.NotSortable,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 46
                });

            int n = (int)nudEstados.Value;
            cmbInicial.Items.Clear();
            clbFinales.Items.Clear();
            for (int i = 0; i < n; i++)
            {
                string nombre = "q" + i;
                int idx = dgvTransiciones.Rows.Add();
                dgvTransiciones.Rows[idx].Cells[0].Value = nombre;
                foreach (DataGridViewColumn col in dgvTransiciones.Columns)
                {
                    string valor;
                    if (col.Name != ColEstado && previo.TryGetValue(nombre + "\u0001" + col.HeaderText, out valor))
                        dgvTransiciones.Rows[idx].Cells[col.Index].Value = valor;
                }
                cmbInicial.Items.Add(nombre);
                clbFinales.Items.Add(nombre, finalesAnteriores.Contains(nombre));
            }
            if (inicialAnterior != null && cmbInicial.Items.Contains(inicialAnterior)) cmbInicial.SelectedItem = inicialAnterior;
            else if (cmbInicial.Items.Count > 0) cmbInicial.SelectedIndex = 0;
        }

        private void dgvTransiciones_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ActualizarDibujo(false);
        }

        private void cmbInicial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargando) ActualizarDibujo(false);
        }

        private void clbFinales_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!cargando) BeginInvoke(new Action(() => ActualizarDibujo(false)));
        }
        private Automata ConstruirModelo(out List<string> problemas)
        {
            Automata a = new Automata { EsAFND = EsAFND, UsaEpsilon = EsAFND && chkEpsilon.Checked };
            problemas = new List<string>();

            foreach (DataGridViewRow fila in dgvTransiciones.Rows)
                a.Estados.Add(Convert.ToString(fila.Cells[0].Value));
            foreach (DataGridViewColumn col in dgvTransiciones.Columns)
                if (col.Name != ColEstado && col.HeaderText != Automata.Epsilon) a.Alfabeto.Add(col.HeaderText);

            a.Inicial = cmbInicial.SelectedItem as string;
            foreach (object o in clbFinales.CheckedItems) a.Finales.Add((string)o);

            foreach (DataGridViewRow fila in dgvTransiciones.Rows)
            {
                string estado = Convert.ToString(fila.Cells[0].Value);
                foreach (DataGridViewColumn col in dgvTransiciones.Columns)
                {
                    if (col.Name == ColEstado) continue;
                    string texto = Convert.ToString(fila.Cells[col.Index].Value ?? "");
                    List<string> destinos = new List<string>();
                    foreach (string token in texto.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string t = token.Trim();
                        if (t == "-" || t == "∅" || t == "—") continue;
                        destinos.Add(NormalizarEstado(a, t));
                    }
                    a.FijarTransicion(estado, col.HeaderText, destinos);
                }
            }
            problemas = a.Validar();
            return a;
        }
        private static string NormalizarEstado(Automata a, string token)
        {
            if (a.Estados.Contains(token)) return token;
            foreach (string e in a.Estados)
                if (string.Equals(e, token, StringComparison.OrdinalIgnoreCase)) return e;
            if (a.Estados.Contains("q" + token)) return "q" + token;
            return token;
        }

        private void btnDibujar_Click(object sender, EventArgs e)
        {
            ActualizarDibujo(true);
        }

        private void ActualizarDibujo(bool avisar)
        {
            List<string> problemas;
            Automata a = ConstruirModelo(out problemas);
            pnlAutomata.HiloActivo = null;
            pnlAutomata.EstadosActivos = null;
            LimpiarResultado();
            if (problemas.Count > 0)
            {
                lblEstadoTabla.ForeColor = Color.Firebrick;
                lblEstadoTabla.Text = "⚠ " + problemas[0] + (problemas.Count > 1 ? "  (+" + (problemas.Count - 1) + " más)" : "");
                if (avisar)
                    MessageBox.Show(this, "La definición del autómata tiene problemas:\n\n• " + string.Join("\n• ", problemas),
                        "Revise la tabla", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                pnlAutomata.Modelo = ProblemasPermitenDibujar(problemas) ? a : null;
                return;
            }
            lblEstadoTabla.ForeColor = Color.SeaGreen;
            lblEstadoTabla.Text = "✔ Definición válida: " + a.Estados.Count + " estado(s), " + a.Alfabeto.Count + " símbolo(s), "
                + a.Finales.Count + " final(es).";
            pnlAutomata.Modelo = a;
        }
        private static bool ProblemasPermitenDibujar(List<string> problemas)
        {
            return problemas.All(p => p.StartsWith("AFD:") || p.StartsWith("Debe elegir"));
        }

        private void btnGuardarImagen_Click(object sender, EventArgs e)
        {
            if (pnlAutomata.Modelo == null)
            {
                MessageBox.Show(this, "Primero defina y dibuje un autómata válido.", "Guardar imagen",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Guardar dibujo del autómata";
                dlg.Filter = "Imagen PNG (*.png)|*.png";
                dlg.FileName = EsAFND ? "afnd" : "afd";
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    using (Bitmap bmp = pnlAutomata.Capturar(1400, 900))
                        bmp.Save(dlg.FileName, ImageFormat.Png);
                    MessageBox.Show(this, "Imagen guardada.", "Guardar imagen", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "No se pudo guardar la imagen:\n" + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnEjemplo_Click(object sender, EventArgs e)
        {
            cargando = true;
            string[,] celdas;
            int[] finales;
            if (!EsAFND)
            {
                nudEstados.Value = 3; nudSimbolos.Value = 2; txtAlfabeto.Text = "a,b";
                chkEpsilon.Checked = false;
                celdas = new[,] { { "q1", "q0" }, { "q1", "q2" }, { "q1", "q0" } };
                finales = new[] { 2 };
            }
            else if (!chkEpsilon.Checked)
            {
                nudEstados.Value = 3; nudSimbolos.Value = 2; txtAlfabeto.Text = "a,b";
                celdas = new[,] { { "q0,q1", "q0" }, { "", "q2" }, { "", "" } };
                finales = new[] { 2 };
            }
            else
            {
                nudEstados.Value = 4; nudSimbolos.Value = 2; txtAlfabeto.Text = "a,b";
                celdas = new[,] { { "q0", "q0", "q1" }, { "q2", "", "" }, { "", "q3", "" }, { "", "", "" } };
                finales = new[] { 3 };
            }
            cargando = false;
            ReconstruirTabla();
            for (int i = 0; i < celdas.GetLength(0); i++)
                for (int j = 0; j < celdas.GetLength(1) && j + 1 < dgvTransiciones.Columns.Count; j++)
                    dgvTransiciones.Rows[i].Cells[j + 1].Value = celdas[i, j];
            cargando = true;
            cmbInicial.SelectedIndex = 0;
            for (int i = 0; i < clbFinales.Items.Count; i++) clbFinales.SetItemChecked(i, finales.Contains(i));
            cargando = false;
            txtCadena.Text = "aab";
            ActualizarDibujo(false);
        }

        private void btnLimpiarTabla_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow fila in dgvTransiciones.Rows)
                for (int j = 1; j < dgvTransiciones.Columns.Count; j++) fila.Cells[j].Value = "";
            cargando = true;
            for (int i = 0; i < clbFinales.Items.Count; i++) clbFinales.SetItemChecked(i, false);
            cargando = false;
            txtCadena.Clear();
            ActualizarDibujo(false);
        }

        private void LimpiarResultado()
        {
            ultimo = null;
            lstHilos.Items.Clear();
            dgvPasos.Rows.Clear();
            lblResultado.Text = "Resultado: —";
            lblResultado.ForeColor = Color.Black;
            lblPaso.Text = "Paso: —";
            btnAnterior.Enabled = btnSiguiente.Enabled = false;
        }

        private void btnAnalizar_Click(object sender, EventArgs e)
        {
            List<string> problemas;
            Automata a = ConstruirModelo(out problemas);
            if (problemas.Count > 0)
            {
                MessageBox.Show(this, "Corrija la tabla antes de analizar una cadena:\n\n• " + string.Join("\n• ", problemas),
                    "Autómata no válido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            pnlAutomata.Modelo = a;
            ultimo = a.Analizar(txtCadena.Text);
            lstHilos.Items.Clear();
            dgvPasos.Rows.Clear();

            if (ultimo.Error != null)
            {
                lblResultado.ForeColor = Color.Firebrick;
                lblResultado.Text = "Resultado: ✗ " + ultimo.Error;
                pnlAutomata.HiloActivo = null;
                pnlAutomata.EstadosActivos = null;
                btnAnterior.Enabled = btnSiguiente.Enabled = false;
                return;
            }

            lblResultado.ForeColor = ultimo.Acepta ? Color.SeaGreen : Color.Firebrick;
            lblResultado.Text = "Resultado: " + (ultimo.Acepta ? "✔ CADENA ACEPTADA" : "✗ CADENA RECHAZADA")
                + "   (" + ultimo.Hilos.Count + (ultimo.Hilos.Count == 1 ? " hilo" : " hilos") + (ultimo.Truncado ? ", lista truncada" : "") + ")";

            int n = 1;
            foreach (Hilo h in ultimo.Hilos)
                lstHilos.Items.Add("Hilo " + (n++) + "  " + (h.Acepta ? "✓" : "✗") + " " + h.Resultado + ":  " + h);

            for (int k = 0; k < ultimo.ConjuntosPorPaso.Count; k++)
                dgvPasos.Rows.Add(k, k == 0 ? "(inicio)" : ultimo.SimbolosPorPaso[k - 1],
                    "{ " + string.Join(", ", ultimo.ConjuntosPorPaso[k]) + " }");
            int elegido = Math.Max(0, ultimo.Hilos.FindIndex(h => h.Acepta));
            if (lstHilos.Items.Count > 0) lstHilos.SelectedIndex = elegido;
        }

        private void lstHilos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ultimo == null || lstHilos.SelectedIndex < 0) return;
            Hilo h = ultimo.Hilos[lstHilos.SelectedIndex];
            pnlAutomata.HiloActivo = h;
            MostrarPaso(h.Pasos.Count);
        }

        private void MostrarPaso(int paso)
        {
            if (ultimo == null || lstHilos.SelectedIndex < 0) return;
            Hilo h = ultimo.Hilos[lstHilos.SelectedIndex];
            paso = Math.Max(0, Math.Min(h.Pasos.Count, paso));
            pnlAutomata.PasoActual = paso;
            pnlAutomata.EstadosActivos = new HashSet<string>(new[] { h.Estado(paso) }, StringComparer.Ordinal);
            btnAnterior.Enabled = paso > 0;
            btnSiguiente.Enabled = paso < h.Pasos.Count;
            lblPaso.Text = "Paso " + paso + "/" + h.Pasos.Count + " → " + h.Estado(paso)
                + (paso > 0 ? " (leyó '" + h.Pasos[paso - 1].Simbolo + "')" : " (inicio)");
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            MostrarPaso(pnlAutomata.PasoActual - 1);
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            MostrarPaso(pnlAutomata.PasoActual + 1);
        }
        private void dgvPasos_SelectionChanged(object sender, EventArgs e)
        {
            if (ultimo == null || dgvPasos.CurrentRow == null) return;
            int k = dgvPasos.CurrentRow.Index;
            if (k < 0 || k >= ultimo.ConjuntosPorPaso.Count) return;
            pnlAutomata.HiloActivo = null;
            pnlAutomata.EstadosActivos = new HashSet<string>(ultimo.ConjuntosPorPaso[k], StringComparer.Ordinal);
            lblPaso.Text = "Tras " + k + " símbolo(s): { " + string.Join(", ", ultimo.ConjuntosPorPaso[k]) + " }";
            btnAnterior.Enabled = btnSiguiente.Enabled = false;
        }
    }
}
