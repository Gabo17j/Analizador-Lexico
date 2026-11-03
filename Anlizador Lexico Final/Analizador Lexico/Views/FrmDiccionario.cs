using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Analizador_Lexico.Core;

namespace Analizador_Lexico.Views
{
    /// <summary>Diccionario editable de palabras reservadas, con importación y exportación (.csv / .txt).</summary>
    public class FrmDiccionario : Form
    {
        private readonly DataGridView dgv = new DataGridView();
        private readonly Label lblInfo = new Label();

        public bool Modificado { get; private set; }

        public FrmDiccionario()
        {
            Text = "Diccionario de palabras reservadas";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(560, 480);
            MinimumSize = new Size(560, 380);
            Font = new Font("Segoe UI", 9F);

            dgv.Dock = DockStyle.Fill;
            dgv.AllowUserToAddRows = true;
            dgv.AllowUserToDeleteRows = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.White;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPalabra", HeaderText = "Palabra reservada", FillWeight = 70 });
            dgv.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colTipoDato", HeaderText = "¿Es tipo de dato?", FillWeight = 30 });

            FlowLayoutPanel barra = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(6, 6, 6, 0) };
            barra.Controls.Add(Boton("Eliminar fila", (s, e) => EliminarSeleccion()));
            barra.Controls.Add(Boton("Restaurar 24", (s, e) => Cargar(PalabrasReservadas.PredeterminadasLista
                .Select(p => new KeyValuePair<string, bool>(p, new[] { "int", "float", "double", "char", "bool", "string" }.Contains(p))))));
            barra.Controls.Add(Boton("Importar...", (s, e) => Importar()));
            barra.Controls.Add(Boton("Exportar...", (s, e) => Exportar()));

            lblInfo.Dock = DockStyle.Bottom;
            lblInfo.Height = 22;
            lblInfo.TextAlign = ContentAlignment.MiddleLeft;
            lblInfo.Padding = new Padding(6, 0, 0, 0);

            FlowLayoutPanel pie = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 44, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(6) };
            Button cancelar = Boton("Cancelar", (s, e) => { DialogResult = DialogResult.Cancel; Close(); });
            Button aplicar = Boton("Aplicar", (s, e) => Aplicar());
            aplicar.Font = new Font(Font, FontStyle.Bold);
            pie.Controls.Add(cancelar);
            pie.Controls.Add(aplicar);

            Controls.Add(dgv);
            Controls.Add(barra);
            Controls.Add(lblInfo);
            Controls.Add(pie);
            AcceptButton = aplicar;
            CancelButton = cancelar;

            Cargar(PalabrasReservadas.Copiar());
        }

        private Button Boton(string texto, EventHandler accion)
        {
            Button b = new Button { Text = texto, AutoSize = true, Height = 30, Padding = new Padding(6, 0, 6, 0) };
            b.Click += accion;
            return b;
        }

        private void Cargar(IEnumerable<KeyValuePair<string, bool>> palabras)
        {
            dgv.Rows.Clear();
            foreach (KeyValuePair<string, bool> kv in palabras) dgv.Rows.Add(kv.Key, kv.Value);
            lblInfo.Text = "Escriba en la última fila para agregar una palabra. Solo letras, dígitos y '_' (sin iniciar con dígito).";
        }

        private void EliminarSeleccion()
        {
            foreach (DataGridViewRow fila in dgv.SelectedRows.Cast<DataGridViewRow>().ToList())
                if (!fila.IsNewRow) dgv.Rows.Remove(fila);
        }

        private List<KeyValuePair<string, bool>> Leer(out string problema)
        {
            problema = null;
            List<KeyValuePair<string, bool>> lista = new List<KeyValuePair<string, bool>>();
            HashSet<string> vistas = new HashSet<string>(StringComparer.Ordinal);
            foreach (DataGridViewRow fila in dgv.Rows)
            {
                if (fila.IsNewRow) continue;
                string palabra = Convert.ToString(fila.Cells[0].Value ?? "").Trim();
                bool tipo = fila.Cells[1].Value is bool && (bool)fila.Cells[1].Value;
                if (palabra.Length == 0) continue;
                if (!PalabrasReservadas.EsIdentificadorValido(palabra))
                {
                    problema = "'" + palabra + "' no es válida: use letras, dígitos y '_' y no inicie con dígito.";
                    return null;
                }
                if (!vistas.Add(palabra))
                {
                    problema = "'" + palabra + "' está repetida.";
                    return null;
                }
                lista.Add(new KeyValuePair<string, bool>(palabra, tipo));
            }
            return lista;
        }

        private void Aplicar()
        {
            dgv.EndEdit();
            string problema;
            List<KeyValuePair<string, bool>> lista = Leer(out problema);
            if (lista == null)
            {
                MessageBox.Show(this, problema, "Diccionario no válido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            PalabrasReservadas.Establecer(lista);
            Modificado = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Exportar()
        {
            dgv.EndEdit();
            string problema;
            List<KeyValuePair<string, bool>> lista = Leer(out problema);
            if (lista == null)
            {
                MessageBox.Show(this, problema, "Diccionario no válido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Exportar diccionario";
                dlg.Filter = "CSV (*.csv)|*.csv|Texto (*.txt)|*.txt";
                dlg.FileName = "diccionario";
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    StringBuilder sb = new StringBuilder();
                    bool csv = dlg.FilterIndex == 1;
                    if (csv) sb.AppendLine("Palabra,EsTipoDeDato");
                    foreach (KeyValuePair<string, bool> kv in lista)
                        sb.AppendLine(csv ? kv.Key + "," + (kv.Value ? "1" : "0") : kv.Key + (kv.Value ? "\t*" : ""));
                    Exportador.Guardar(dlg.FileName, sb.ToString());
                    MessageBox.Show(this, "Diccionario exportado (" + lista.Count + " palabras).", "Exportación completada",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "No se pudo guardar el archivo:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Importar()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Importar diccionario";
                dlg.Filter = "Diccionario (*.csv;*.txt)|*.csv;*.txt|Todos (*.*)|*.*";
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    List<KeyValuePair<string, bool>> lista = new List<KeyValuePair<string, bool>>();
                    foreach (string linea in File.ReadAllLines(dlg.FileName, Encoding.UTF8))
                    {
                        string l = linea.Trim();
                        if (l.Length == 0) continue;
                        string[] partes = l.Split(new[] { ',', ';', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        string palabra = partes[0].Trim();
                        if (palabra == "Palabra") continue;
                        bool tipo = partes.Length > 1 && (partes[1].Trim() == "1" || partes[1].Trim() == "*"
                            || partes[1].Trim().Equals("true", StringComparison.OrdinalIgnoreCase));
                        if (PalabrasReservadas.EsIdentificadorValido(palabra))
                            lista.Add(new KeyValuePair<string, bool>(palabra, tipo));
                    }
                    if (lista.Count == 0)
                    {
                        MessageBox.Show(this, "El archivo no contiene palabras válidas.", "Importar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Cargar(lista.GroupBy(k => k.Key, StringComparer.Ordinal).Select(g => g.First()));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "No se pudo leer el archivo:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
