using System;
using System.Drawing;
using System.Windows.Forms;

namespace Analizador_Lexico.Views
{
    public enum OpcionExportar { Todo, Tokens, Errores, Simbolos, Estadisticas }

    /// <summary>Pregunta qué exportar: todo o solo una de las cuatro secciones.</summary>
    public class DlgExportar : Form
    {
        private readonly RadioButton rbTodo = new RadioButton { Text = "Todo (reporte completo)", Checked = true, AutoSize = true };
        private readonly RadioButton rbTokens = new RadioButton { Text = "Solo tokens (dgvTokens)", AutoSize = true };
        private readonly RadioButton rbErrores = new RadioButton { Text = "Solo errores (dgvErrores)", AutoSize = true };
        private readonly RadioButton rbSimbolos = new RadioButton { Text = "Solo tabla de símbolos (dgvSimbolos)", AutoSize = true };
        private readonly RadioButton rbEstadisticas = new RadioButton { Text = "Solo estadísticas (txtEstadisticas)", AutoSize = true };

        public OpcionExportar Opcion
        {
            get
            {
                if (rbTokens.Checked) return OpcionExportar.Tokens;
                if (rbErrores.Checked) return OpcionExportar.Errores;
                if (rbSimbolos.Checked) return OpcionExportar.Simbolos;
                if (rbEstadisticas.Checked) return OpcionExportar.Estadisticas;
                return OpcionExportar.Todo;
            }
        }

        public DlgExportar()
        {
            Text = "Exportar resultados";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(340, 250);
            Font = new Font("Segoe UI", 9F);

            Label lbl = new Label { Text = "¿Qué desea exportar?", AutoSize = true, Location = new Point(16, 14), Font = new Font(Font, FontStyle.Bold) };
            RadioButton[] rbs = { rbTodo, rbTokens, rbErrores, rbSimbolos, rbEstadisticas };
            int y = 44;
            foreach (RadioButton rb in rbs)
            {
                rb.Location = new Point(24, y);
                y += 28;
                Controls.Add(rb);
            }

            Button ok = new Button { Text = "Continuar", DialogResult = DialogResult.OK, Location = new Point(140, 205), Size = new Size(90, 30) };
            Button cancel = new Button { Text = "Cancelar", DialogResult = DialogResult.Cancel, Location = new Point(236, 205), Size = new Size(90, 30) };
            Controls.Add(lbl);
            Controls.Add(ok);
            Controls.Add(cancel);
            AcceptButton = ok;
            CancelButton = cancel;
        }
    }
}
