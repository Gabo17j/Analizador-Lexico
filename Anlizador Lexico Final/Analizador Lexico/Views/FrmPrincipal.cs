using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Analizador_Lexico.Core;

namespace Analizador_Lexico.Views
{
    public partial class FrmPrincipal : Form
    {
        private const int MaximoFilas = 20000;

        private ResultadoAnalisis resultado;
        private string rutaActual;
        private bool bloqueado;
        private int lineasMostradas;

        public FrmPrincipal()
        {
            InitializeComponent();
            rtbCodigo.BringToFront();
            ActualizarLineas();
        }

        private void ActualizarLineas()
        {
            int total = rtbCodigo.GetLineFromCharIndex(rtbCodigo.TextLength) + 1;
            if (total != lineasMostradas)
            {
                lineasMostradas = total;
                int digitos = total.ToString().Length;
                StringBuilder sb = new StringBuilder();
                for (int i = 1; i <= total; i++)
                {
                    sb.Append(i.ToString().PadLeft(digitos));
                    if (i < total) sb.Append('\n');
                }
                rtbLineas.Text = sb.ToString();
                rtbLineas.Width = Math.Max(40, digitos * 13 + 18);
            }
            SincronizarScroll();
        }

        private void SincronizarScroll()
        {
            int primera = Nativos.SendMessage(rtbCodigo.Handle, Nativos.EM_GETFIRSTVISIBLELINE, 0, 0);
            int actual = Nativos.SendMessage(rtbLineas.Handle, Nativos.EM_GETFIRSTVISIBLELINE, 0, 0);
            if (primera != actual)
                Nativos.SendMessage(rtbLineas.Handle, Nativos.EM_LINESCROLL, 0, primera - actual);
        }

        private void ActualizarPosicion()
        {
            int indice = rtbCodigo.SelectionStart;
            int linea = rtbCodigo.GetLineFromCharIndex(indice);
            int inicio = rtbCodigo.GetFirstCharIndexFromLine(linea);
            lblPosicion.Text = "Ln " + (linea + 1) + ", Col " + (indice - inicio + 1);
        }

        private void MostrarEstado(string mensaje)
        {
            lblEstado.Text = mensaje;
        }

        private void ActualizarResumen(bool vigente)
        {
            if (resultado == null)
            {
                lblResumen.Text = "Sin análisis";
                return;
            }
            lblResumen.Text = resultado.TotalTokens + " tokens | " + resultado.TotalErrores + " errores | "
                + resultado.TotalLineas + " líneas | " + resultado.TiempoMs.ToString("0.00") + " ms"
                + (vigente ? "" : " (desactualizado, pulse F5)");
        }

        private void rtbCodigo_TextChanged(object sender, EventArgs e)
        {
            if (bloqueado) return;
            ActualizarLineas();
            ActualizarResumen(false);
        }

        private void rtbCodigo_VScroll(object sender, EventArgs e)
        {
            SincronizarScroll();
        }

        private void rtbCodigo_SelectionChanged(object sender, EventArgs e)
        {
            if (!bloqueado) ActualizarPosicion();
        }

        private void rtbCodigo_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void rtbCodigo_DragDrop(object sender, DragEventArgs e)
        {
            string[] archivos = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (archivos != null && archivos.Length > 0) CargarArchivo(archivos[0]);
        }

        private void FrmPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                e.Handled = true;
                Analizar();
            }
            else if (e.Control && e.KeyCode == Keys.O)
            {
                e.Handled = true;
                AbrirArchivo();
            }
            else if (e.Control && e.KeyCode == Keys.E)
            {
                e.Handled = true;
                Exportar();
            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            AbrirArchivo();
        }

        private void btnAnalizar_Click(object sender, EventArgs e)
        {
            Analizar();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            Exportar();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void AbrirArchivo()
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
                CargarArchivo(openFileDialog1.FileName);
        }

        private void CargarArchivo(string ruta)
        {
            try
            {
                FileInfo info = new FileInfo(ruta);
                if (!info.Exists)
                {
                    MessageBox.Show(this, "El archivo no existe:\n" + ruta, "Archivo no encontrado",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (info.Length > 5L * 1024 * 1024)
                {
                    DialogResult r = MessageBox.Show(this, "El archivo pesa más de 5 MB. ¿Desea cargarlo de todos modos?",
                        "Archivo grande", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (r != DialogResult.Yes) return;
                }
                string texto = File.ReadAllText(ruta, Encoding.UTF8);
                if (texto.IndexOf('\0') >= 0)
                {
                    MessageBox.Show(this, "El archivo parece binario. Solo se pueden analizar archivos de texto.",
                        "Archivo no válido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                rtbCodigo.Text = texto;
                rtbCodigo.SelectionStart = 0;
                rutaActual = ruta;
                resultado = null;
                LimpiarTablas();
                ActualizarResumen(false);
                Text = "Analizador Léxico - " + Path.GetFileName(ruta);
                MostrarEstado(texto.Length == 0 ? "Archivo vacío: " + ruta : "Archivo cargado: " + ruta);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(this, "No tiene permisos para leer el archivo.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, "No se pudo leer el archivo:\n" + ex.Message, "Error de lectura",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error inesperado al cargar el archivo:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpiar()
        {
            if (rtbCodigo.TextLength > 0)
            {
                DialogResult r = MessageBox.Show(this, "¿Desea borrar el código y los resultados?", "Limpiar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r != DialogResult.Yes) return;
            }
            rtbCodigo.Clear();
            rutaActual = null;
            resultado = null;
            LimpiarTablas();
            ActualizarResumen(false);
            Text = "Analizador Léxico";
            MostrarEstado("Editor y resultados limpiados.");
        }

        private void LimpiarTablas()
        {
            dgvTokens.Rows.Clear();
            dgvErrores.Rows.Clear();
            dgvSimbolos.Rows.Clear();
            txtEstadisticas.Clear();
            tabErrores.Text = "Errores";
        }

        private void Analizar()
        {
            string codigo = rtbCodigo.Text;
            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show(this, "No hay código para analizar.\nEscriba código o abra un archivo.",
                    "Nada que analizar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MostrarEstado("Sin código para analizar.");
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                AnalizadorLexico analizador = new AnalizadorLexico(codigo);
                analizador.Analizar();
                resultado = analizador.ObtenerResultado();

                LlenarTablas();
                if (rtbCodigo.TextLength <= 150000)
                {
                    bloqueado = true;
                    try { ResaltadorSintaxis.Aplicar(rtbCodigo, resultado); }
                    finally { bloqueado = false; }
                }
                ActualizarResumen(true);
                tabControl1.SelectedTab = resultado.TotalErrores > 0 ? tabErrores : tabTokens;
                MostrarEstado(resultado.TotalErrores == 0
                    ? "Análisis completado sin errores léxicos."
                    : "Análisis completado: " + resultado.TotalErrores + " error(es) léxico(s).");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Ocurrió un error durante el análisis:\n" + ex.Message, "Error de análisis",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                MostrarEstado("El análisis falló.");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void LlenarTablas()
        {
            LimpiarTablas();
            ResultadoAnalisis r = resultado;

            int n = 1;
            dgvTokens.SuspendLayout();
            foreach (Token t in r.Tokens)
            {
                if (n > MaximoFilas) break;
                dgvTokens.Rows.Add(n++, Reporte.Visible(t.Lexema), t.Tipo, t.Categoria, t.Linea, t.Columna);
            }
            dgvTokens.ResumeLayout();

            foreach (ErrorLexico er in r.Errores)
            {
                int fila = dgvErrores.Rows.Add(er.Regla, Reporte.Visible(er.Lexema), er.Linea, er.Columna, er.Descripcion);
                dgvErrores.Rows[fila].Tag = er;
                dgvErrores.Rows[fila].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                dgvErrores.Rows[fila].DefaultCellStyle.BackColor = System.Drawing.Color.MistyRose;
            }
            tabErrores.Text = r.TotalErrores > 0 ? "Errores (" + r.TotalErrores + ")" : "Errores";

            foreach (ElementoSimbolo s in r.Simbolos)
            {
                int fila = dgvSimbolos.Rows.Add(s.Id, s.Lexema, s.TipoToken, s.TipoDato,
                    s.LineaPrimera + "/" + s.ColumnaPrimera, s.Frecuencia,
                    s.Referencias.Count == 0 ? "-" : string.Join(", ", s.Referencias));
                dgvSimbolos.Rows[fila].Tag = s;
            }

            txtEstadisticas.Text = Reporte.Estadisticas(r).Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        }

        private void dgvTokens_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || resultado == null) return;
            int n = Convert.ToInt32(dgvTokens.Rows[e.RowIndex].Cells[0].Value);
            if (n < 1 || n > resultado.Tokens.Count) return;
            Token t = resultado.Tokens[n - 1];
            IrAPosicion(t.Linea, t.Columna, t.Lexema.Length);
        }

        private void dgvErrores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            ErrorLexico er = dgvErrores.Rows[e.RowIndex].Tag as ErrorLexico;
            if (er == null) return;
            string texto = rtbCodigo.Text;
            int off = ResaltadorSintaxis.Offset(ResaltadorSintaxis.InicioLineas(texto), texto, er.Linea, er.Columna);
            IrAPosicion(er.Linea, er.Columna, ResaltadorSintaxis.LongitudError(er, texto, off));
        }

        private void dgvSimbolos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            ElementoSimbolo s = dgvSimbolos.Rows[e.RowIndex].Tag as ElementoSimbolo;
            if (s == null) return;
            IrAPosicion(s.LineaPrimera, s.ColumnaPrimera, s.Lexema.Length);
        }

        private void IrAPosicion(int linea, int columna, int largo)
        {
            string texto = rtbCodigo.Text;
            int off = ResaltadorSintaxis.Offset(ResaltadorSintaxis.InicioLineas(texto), texto, linea, columna);
            if (off < 0 || off > texto.Length) return;
            largo = Math.Max(0, Math.Min(largo, texto.Length - off));
            rtbCodigo.Focus();
            rtbCodigo.Select(off, largo);
            rtbCodigo.ScrollToCaret();
        }

        private void Exportar()
        {
            if (resultado == null)
            {
                MessageBox.Show(this, "Todavía no hay resultados para exportar.\nPrimero ejecute el análisis (F5).",
                    "Nada que exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string nombre = rutaActual != null ? Path.GetFileNameWithoutExtension(rutaActual) : "codigo";
            saveFileDialog1.FileName = nombre + "_resultado";
            if (saveFileDialog1.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                string ext = Path.GetExtension(saveFileDialog1.FileName).ToLowerInvariant();
                string contenido;
                if (ext == ".csv")
                {
                    if (saveFileDialog1.FilterIndex == 3) contenido = Exportador.ErroresCsv(resultado);
                    else if (saveFileDialog1.FilterIndex == 4) contenido = Exportador.SimbolosCsv(resultado);
                    else contenido = Exportador.TokensCsv(resultado);
                }
                else if (ext == ".html" || ext == ".htm") contenido = Exportador.Html(nombre, resultado);
                else contenido = Reporte.Completo(nombre, resultado);

                Exportador.Guardar(saveFileDialog1.FileName, contenido);
                MostrarEstado("Resultados exportados a " + saveFileDialog1.FileName);

                DialogResult abrir = MessageBox.Show(this, "Resultados exportados correctamente.\n\n¿Desea abrir el archivo?",
                    "Exportación completada", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (abrir == DialogResult.Yes)
                {
                    try { Process.Start(new ProcessStartInfo(saveFileDialog1.FileName) { UseShellExecute = true }); }
                    catch (Exception) { MostrarEstado("No hay un programa asociado para abrir el archivo."); }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(this, "No tiene permisos para escribir en esa ubicación.", "Acceso denegado",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, "No se pudo escribir el archivo:\n" + ex.Message, "Error de escritura",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error al exportar:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
