using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Analizador_Lexico.Core;

namespace Analizador_Lexico.Views
{
    public partial class FrmDiccionario : Form
    {
        public bool Modificado { get; private set; }

        public FrmDiccionario()
        {
            InitializeComponent();
            Cargar(PalabrasReservadas.Copiar());
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarSeleccion();
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            Cargar(PalabrasReservadas.PredeterminadasConTipo());
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            Importar();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            Exportar();
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            Aplicar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Cargar(IEnumerable<KeyValuePair<string, bool>> palabras)
        {
            dgvDiccionario.Rows.Clear();
            foreach (KeyValuePair<string, bool> kv in palabras) dgvDiccionario.Rows.Add(kv.Key, kv.Value);
        }

        private void EliminarSeleccion()
        {
            foreach (DataGridViewRow fila in dgvDiccionario.SelectedRows.Cast<DataGridViewRow>().ToList())
                if (!fila.IsNewRow) dgvDiccionario.Rows.Remove(fila);
        }

        private List<KeyValuePair<string, bool>> Leer(out string problema)
        {
            problema = null;
            List<KeyValuePair<string, bool>> lista = new List<KeyValuePair<string, bool>>();
            HashSet<string> vistas = new HashSet<string>(StringComparer.Ordinal);
            foreach (DataGridViewRow fila in dgvDiccionario.Rows)
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
            dgvDiccionario.EndEdit();
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
            dgvDiccionario.EndEdit();
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
