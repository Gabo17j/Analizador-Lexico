using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analizador_Lexico.Core
{
    public static class Reporte
    {
        public static string Visible(string s)
        {
            return (s ?? "").Replace("\r", "").Replace("\n", "\\n").Replace("\t", "\\t");
        }

        private static string Fila(int[] anchos, params string[] celdas)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < celdas.Length; i++)
            {
                string t = Visible(celdas[i]);
                if (i < celdas.Length - 1)
                {
                    if (t.Length > anchos[i]) t = t.Substring(0, anchos[i] - 3) + "...";
                    sb.Append(t.PadRight(anchos[i] + 1));
                }
                else sb.Append(t);
            }
            return sb.ToString();
        }

        public static string Tokens(IEnumerable<Token> tokens)
        {
            int[] a = { 5, 26, 20, 18, 6 };
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Fila(a, "#", "Lexema", "Tipo de token", "Categoría", "Línea/Col"));
            int n = 1;
            foreach (Token t in tokens)
                sb.AppendLine(Fila(a, n++.ToString(), t.Lexema, t.Tipo, t.Categoria, t.Linea + "/" + t.Columna));
            return sb.ToString();
        }

        public static string Simbolos(IEnumerable<ElementoSimbolo> simbolos)
        {
            int[] a = { 4, 18, 17, 16, 10, 5, 12 };
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Fila(a, "Id", "Lexema", "Tipo de token", "Tipo de dato", "1ª Lín/Col", "Frec", "Referencias"));
            foreach (ElementoSimbolo s in simbolos)
                sb.AppendLine(Fila(a, s.Id.ToString(), s.Lexema, s.TipoToken, s.TipoDato,
                    s.LineaPrimera + "/" + s.ColumnaPrimera, s.Frecuencia.ToString(),
                    s.Referencias.Count == 0 ? "-" : string.Join(",", s.Referencias)));
            return sb.ToString();
        }

        public static string Errores(IEnumerable<ErrorLexico> errores)
        {
            List<ErrorLexico> lista = errores.ToList();
            if (lista.Count == 0) return "Sin errores léxicos.\n";
            int[] a = { 4, 6, 24, 10, 60 };
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Fila(a, "#", "Regla", "Lexema", "Lín/Col", "Descripción"));
            int n = 1;
            foreach (ErrorLexico e in lista)
                sb.AppendLine(Fila(a, n++.ToString(), e.Regla.ToString(), e.Lexema, e.Linea + "/" + e.Columna, e.Descripcion));
            return sb.ToString();
        }

        public static string Estadisticas(ResultadoAnalisis r)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Líneas procesadas : " + r.TotalLineas);
            sb.AppendLine("Total de tokens   : " + r.TotalTokens);
            sb.AppendLine("Total de errores  : " + r.TotalErrores);
            sb.AppendLine("Identificadores distintos: " + r.Simbolos.Count);
            sb.AppendLine("Tiempo de análisis: " + r.TiempoMs.ToString("0.000") + " ms");
            sb.AppendLine("Tokens por categoría:");
            foreach (KeyValuePair<string, int> kv in r.TokensPorCategoria.OrderBy(k => k.Key))
                sb.AppendLine("  " + kv.Key.PadRight(20) + kv.Value);
            return sb.ToString();
        }

        public static string Completo(string titulo, ResultadoAnalisis r)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==================== " + titulo + " ====================");
            sb.AppendLine("--- TOKENS ---");
            sb.Append(Tokens(r.Tokens));
            sb.AppendLine();
            sb.AppendLine("--- TABLA DE SÍMBOLOS ---");
            sb.Append(Simbolos(r.Simbolos));
            sb.AppendLine();
            sb.AppendLine("--- ERRORES LÉXICOS ---");
            sb.Append(Errores(r.Errores));
            sb.AppendLine();
            sb.AppendLine("--- ESTADÍSTICAS ---");
            sb.Append(Estadisticas(r));
            return sb.ToString();
        }
    }
}
