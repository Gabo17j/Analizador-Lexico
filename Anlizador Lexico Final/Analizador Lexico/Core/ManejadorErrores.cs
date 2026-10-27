using System.Collections.Generic;
using System.Text;

namespace Analizador_Lexico.Core
{
    public class ManejadorErrores
    {
        private readonly List<ErrorLexico> errores = new List<ErrorLexico>();

        public IReadOnlyList<ErrorLexico> Errores => errores;
        public int Total => errores.Count;

        public void AgregarError(string lexema, string descripcion, int linea, int columna, int regla)
        {
            errores.Add(new ErrorLexico(lexema, descripcion, linea, columna, regla));
        }

        public void Limpiar()
        {
            errores.Clear();
        }

        public string MostrarReporte()
        {
            if (errores.Count == 0) return "Sin errores léxicos.";
            StringBuilder sb = new StringBuilder();
            foreach (ErrorLexico e in errores) sb.AppendLine(e.ToString());
            return sb.ToString();
        }
    }
}
