using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private bool llenandoEstadisticas;
        private bool estadisticasEditadas;

        public FrmPrincipal()
        {
            InitializeComponent();
            rtbCodigo.HideSelection = false;
            txtEstadisticas.TextChanged += txtEstadisticas_TextChanged;
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
            llenandoEstadisticas = true;
            txtEstadisticas.Clear();
            llenandoEstadisticas = false;
            estadisticasEditadas = false;
            tabErrores.Text = "Errores";
        }

        private void Analizar(bool cambiarPestana = true)
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
                VerificadorEstructura.Verificar(resultado);

                LlenarTablas();
                if (rtbCodigo.TextLength <= 150000)
                {
                    bloqueado = true;
                    try { ResaltadorSintaxis.Aplicar(rtbCodigo, resultado); }
                    finally { bloqueado = false; }
                }
                ActualizarResumen(true);
                if (cambiarPestana)
                    tabControl1.SelectedTab = resultado.TotalErrores > 0 ? tabErrores : tabTokens;
                MostrarEstado(resultado.TotalErrores == 0
                    ? "Análisis completado sin errores."
                    : "Análisis completado: " + resultado.TotalErrores + " error(es) (léxicos y de estructura).");
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

            llenandoEstadisticas = true;
            txtEstadisticas.Text = Reporte.Estadisticas(r).Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
            llenandoEstadisticas = false;
            estadisticasEditadas = false;
        }

        private void NavegarToken(int fila, bool enfocar)
        {
            if (fila < 0 || resultado == null) return;
            int n = Convert.ToInt32(dgvTokens.Rows[fila].Cells[0].Value);
            if (n < 1 || n > resultado.Tokens.Count) return;
            Token t = resultado.Tokens[n - 1];
            IrAPosicion(t.Linea, t.Columna, t.Lexema.Length, enfocar);
        }

        private void NavegarError(int fila, bool enfocar)
        {
            if (fila < 0) return;
            ErrorLexico er = dgvErrores.Rows[fila].Tag as ErrorLexico;
            if (er == null) return;
            string texto = rtbCodigo.Text;
            int off = ResaltadorSintaxis.Offset(ResaltadorSintaxis.InicioLineas(texto), texto, er.Linea, er.Columna);
            IrAPosicion(er.Linea, er.Columna, ResaltadorSintaxis.LongitudError(er, texto, off), enfocar);
        }

        private void NavegarSimbolo(int fila, bool enfocar)
        {
            if (fila < 0) return;
            ElementoSimbolo s = dgvSimbolos.Rows[fila].Tag as ElementoSimbolo;
            if (s == null) return;
            IrAPosicion(s.LineaPrimera, s.ColumnaPrimera, s.Lexema.Length, enfocar);
        }
        private void dgvTokens_CellClick(object sender, DataGridViewCellEventArgs e) { NavegarToken(e.RowIndex, false); }
        private void dgvErrores_CellClick(object sender, DataGridViewCellEventArgs e) { NavegarError(e.RowIndex, false); }
        private void dgvSimbolos_CellClick(object sender, DataGridViewCellEventArgs e) { NavegarSimbolo(e.RowIndex, false); }

        private void dgvTokens_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvTokens.Columns[e.ColumnIndex].ReadOnly) NavegarToken(e.RowIndex, true);
        }

        private void dgvErrores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvErrores.Columns[e.ColumnIndex].ReadOnly) NavegarError(e.RowIndex, true);
        }

        private void dgvSimbolos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvSimbolos.Columns[e.ColumnIndex].ReadOnly) NavegarSimbolo(e.RowIndex, true);
        }
        private static string Desescapar(string original, string escrito)
        {
            if (original.IndexOfAny(new[] { '\r', '\n', '\t' }) < 0) return escrito;
            return escrito.Replace("\\n", "\n").Replace("\\t", "\t");
        }

        private void Reemplazar(int offset, int largo, string nuevo)
        {
            bloqueado = true;
            try
            {
                rtbCodigo.Select(offset, largo);
                rtbCodigo.SelectedText = nuevo;
            }
            finally { bloqueado = false; }
            ActualizarLineas();
        }

        private bool ReemplazarEnPosicion(int linea, int columna, int largo, string nuevo)
        {
            string texto = rtbCodigo.Text;
            int off = ResaltadorSintaxis.Offset(ResaltadorSintaxis.InicioLineas(texto), texto, linea, columna);
            if (off < 0 || off > texto.Length) return false;
            largo = Math.Max(0, Math.Min(largo, texto.Length - off));
            Reemplazar(off, largo, nuevo);
            return true;
        }
        private void ReanalizarTrasEdicion(string mensaje)
        {
            BeginInvoke(new Action(() =>
            {
                Analizar(false);
                if (mensaje != null) MostrarEstado(mensaje);
            }));
        }

        private void dgvTokens_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || resultado == null) return;
            DataGridViewRow fila = dgvTokens.Rows[e.RowIndex];
            int n = Convert.ToInt32(fila.Cells[0].Value);
            if (n < 1 || n > resultado.Tokens.Count) return;
            Token t = resultado.Tokens[n - 1];
            string escrito = Convert.ToString(fila.Cells[e.ColumnIndex].Value ?? "");
            if (escrito == Reporte.Visible(t.Lexema)) return;
            if (ReemplazarEnPosicion(t.Linea, t.Columna, t.Lexema.Length, Desescapar(t.Lexema, escrito)))
                ReanalizarTrasEdicion("Token editado: el código y las tablas se actualizaron.");
        }

        private void dgvErrores_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow fila = dgvErrores.Rows[e.RowIndex];
            ErrorLexico er = fila.Tag as ErrorLexico;
            if (er == null) return;
            string escrito = Convert.ToString(fila.Cells[e.ColumnIndex].Value ?? "");
            if (escrito == Reporte.Visible(er.Lexema)) return;
            string texto = rtbCodigo.Text;
            int off = ResaltadorSintaxis.Offset(ResaltadorSintaxis.InicioLineas(texto), texto, er.Linea, er.Columna);
            int largo = ResaltadorSintaxis.LongitudError(er, texto, off);
            if (ReemplazarEnPosicion(er.Linea, er.Columna, largo, Desescapar(er.Lexema, escrito)))
                ReanalizarTrasEdicion("Error editado: el código se corrigió y se volvió a analizar.");
        }

        private void dgvSimbolos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || resultado == null) return;
            DataGridViewRow fila = dgvSimbolos.Rows[e.RowIndex];
            ElementoSimbolo s = fila.Tag as ElementoSimbolo;
            if (s == null) return;
            string escrito = (Convert.ToString(fila.Cells[e.ColumnIndex].Value ?? "")).Trim();

            if (e.ColumnIndex == colSimLexema.Index)
            {
                if (escrito == s.Lexema) return;
                if (!PalabrasReservadas.EsIdentificadorValido(escrito) || PalabrasReservadas.EsReservada(escrito))
                {
                    MessageBox.Show(this, "'" + escrito + "' no es un identificador válido (o es una palabra reservada).",
                        "Renombrar identificador", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ReanalizarTrasEdicion(null);
                    return;
                }
                if (resultado.Simbolos.Any(x => x.Lexema == escrito))
                {
                    DialogResult r = MessageBox.Show(this, "Ya existe un identificador llamado '" + escrito + "'. Ambos quedarían unidos. ¿Continuar?",
                        "Renombrar identificador", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (r != DialogResult.Yes) { ReanalizarTrasEdicion(null); return; }
                }
                string texto = rtbCodigo.Text;
                List<int> inicios = ResaltadorSintaxis.InicioLineas(texto);
                List<int> offsets = new List<int>();
                foreach (Token t in resultado.Tokens)
                {
                    if (t.Tipo != TiposToken.Identificador || t.Lexema != s.Lexema) continue;
                    int off = ResaltadorSintaxis.Offset(inicios, texto, t.Linea, t.Columna);
                    if (off >= 0) offsets.Add(off);
                }
                for (int k = offsets.Count - 1; k >= 0; k--) Reemplazar(offsets[k], s.Lexema.Length, escrito);
                ReanalizarTrasEdicion("Identificador renombrado en " + offsets.Count + " lugar(es) del código.");
            }
            else if (e.ColumnIndex == colSimTipoDato.Index)
            {
                if (escrito == s.TipoDato) return;
                if (!PalabrasReservadas.EsTipoDeDato(escrito))
                {
                    MessageBox.Show(this, "'" + escrito + "' no es un tipo de dato del diccionario (int, float, double, char, bool, string).",
                        "Cambiar tipo de dato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ReanalizarTrasEdicion(null);
                    return;
                }
                Token decl = null;
                List<Token> tk = resultado.Tokens;
                for (int k = 1; k < tk.Count; k++)
                {
                    if (tk[k].Tipo == TiposToken.Identificador && tk[k].Lexema == s.Lexema
                        && tk[k - 1].Tipo == TiposToken.PalabraReservada && PalabrasReservadas.EsTipoDeDato(tk[k - 1].Lexema))
                    { decl = tk[k - 1]; break; }
                }
                if (decl == null)
                {
                    MessageBox.Show(this, "'" + s.Lexema + "' no tiene una declaración con tipo en el código; no hay nada que cambiar.",
                        "Cambiar tipo de dato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ReanalizarTrasEdicion(null);
                    return;
                }
                if (ReemplazarEnPosicion(decl.Linea, decl.Columna, decl.Lexema.Length, escrito))
                    ReanalizarTrasEdicion("Tipo de dato cambiado en la declaración de '" + s.Lexema + "'.");
            }
        }

        private void txtEstadisticas_TextChanged(object sender, EventArgs e)
        {
            if (!llenandoEstadisticas) estadisticasEditadas = true;
        }
        private void btnAutomatas_Click(object sender, EventArgs e)
        {
            using (FrmAutomatas frm = new FrmAutomatas())
                frm.ShowDialog(this);
        }

        private void btnDiccionario_Click(object sender, EventArgs e)
        {
            using (FrmDiccionario dlg = new FrmDiccionario())
            {
                dlg.ShowDialog(this);
                if (!dlg.Modificado) return;
            }
            MostrarEstado("Diccionario actualizado (" + PalabrasReservadas.Cantidad + " palabras).");
            if (!string.IsNullOrWhiteSpace(rtbCodigo.Text)) Analizar(false);
        }

        private void IrAPosicion(int linea, int columna, int largo, bool enfocar = true)
        {
            string texto = rtbCodigo.Text;
            int off = ResaltadorSintaxis.Offset(ResaltadorSintaxis.InicioLineas(texto), texto, linea, columna);
            if (off < 0 || off > texto.Length) return;
            largo = Math.Max(0, Math.Min(largo, texto.Length - off));
            if (enfocar) rtbCodigo.Focus();
            rtbCodigo.Select(off, largo);
            rtbCodigo.ScrollToCaret();
        }

        private string ContenidoExportacion(OpcionExportar op, string ext, string nombre)
        {
            string estadisticas = estadisticasEditadas ? txtEstadisticas.Text : null;
            bool csv = ext == ".csv";
            switch (op)
            {
                case OpcionExportar.Tokens:
                    return csv ? Exportador.TokensCsv(resultado) : "--- TOKENS ---" + Environment.NewLine + Reporte.Tokens(resultado.Tokens);
                case OpcionExportar.Errores:
                    return csv ? Exportador.ErroresCsv(resultado) : "--- ERRORES LÉXICOS ---" + Environment.NewLine + Reporte.Errores(resultado.Errores);
                case OpcionExportar.Simbolos:
                    return csv ? Exportador.SimbolosCsv(resultado) : "--- TABLA DE SÍMBOLOS ---" + Environment.NewLine + Reporte.Simbolos(resultado.Simbolos);
                case OpcionExportar.Estadisticas:
                    return "--- ESTADÍSTICAS ---" + Environment.NewLine + txtEstadisticas.Text;
                default:
                    if (ext == ".html" || ext == ".htm") return Exportador.Html(nombre, resultado, estadisticas);
                    return Reporte.Completo(nombre, resultado, estadisticas);
            }
        }

        private void Exportar()
        {
            if (resultado == null)
            {
                MessageBox.Show(this, "Todavía no hay resultados para exportar.\nPrimero ejecute el análisis (F5).",
                    "Nada que exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            OpcionExportar opcion;
            using (DlgExportar dlg = new DlgExportar())
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                opcion = dlg.Opcion;
            }

            string nombre = rutaActual != null ? Path.GetFileNameWithoutExtension(rutaActual) : "codigo";
            string sufijo;
            switch (opcion)
            {
                case OpcionExportar.Tokens:
                    saveFileDialog1.Filter = "CSV (*.csv)|*.csv|Texto (*.txt)|*.txt"; sufijo = "_tokens"; break;
                case OpcionExportar.Errores:
                    saveFileDialog1.Filter = "CSV (*.csv)|*.csv|Texto (*.txt)|*.txt"; sufijo = "_errores"; break;
                case OpcionExportar.Simbolos:
                    saveFileDialog1.Filter = "CSV (*.csv)|*.csv|Texto (*.txt)|*.txt"; sufijo = "_simbolos"; break;
                case OpcionExportar.Estadisticas:
                    saveFileDialog1.Filter = "Texto (*.txt)|*.txt"; sufijo = "_estadisticas"; break;
                default:
                    saveFileDialog1.Filter = "Reporte completo (*.txt)|*.txt|Reporte HTML (*.html)|*.html"; sufijo = "_resultado"; break;
            }
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.FileName = nombre + sufijo;
            if (saveFileDialog1.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                string ext = Path.GetExtension(saveFileDialog1.FileName).ToLowerInvariant();
                string contenido = ContenidoExportacion(opcion, ext, nombre);
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
