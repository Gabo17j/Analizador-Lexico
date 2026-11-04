using System;
using System.Windows.Forms;

namespace Analizador_Lexico.Views
{
    public enum OpcionExportar { Todo, Tokens, Errores, Simbolos, Estadisticas }
    public partial class DlgExportar : Form
    {
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
            InitializeComponent();
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {

        }
    }
}
