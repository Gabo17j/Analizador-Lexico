using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Analizador_Lexico.Core
{
    public static class Exportador
    {
        public static string CampoCsv(string valor)
        {
            string s = Reporte.Visible(valor ?? "");
            if (s.Length > 0 && (s[0] == '=' || s[0] == '+' || s[0] == '-' || s[0] == '@')) s = "'" + s;
            if (s.IndexOfAny(new[] { ',', '"', ';' }) >= 0 || s.StartsWith(" ") || s.EndsWith(" "))
                s = "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }

        private static string FilaCsv(params string[] campos)
        {
            return string.Join(",", campos.Select(CampoCsv));
        }

        public static string TokensCsv(ResultadoAnalisis r)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(FilaCsv("#", "Lexema", "Tipo de token", "Categoria", "Linea", "Columna"));
            int n = 1;
            foreach (Token t in r.Tokens)
                sb.AppendLine(FilaCsv((n++).ToString(), t.Lexema, t.Tipo, t.Categoria, t.Linea.ToString(), t.Columna.ToString()));
            return sb.ToString();
        }

        public static string ErroresCsv(ResultadoAnalisis r)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(FilaCsv("#", "Regla", "Lexema", "Linea", "Columna", "Descripcion"));
            int n = 1;
            foreach (ErrorLexico e in r.Errores)
                sb.AppendLine(FilaCsv((n++).ToString(), e.Regla.ToString(), e.Lexema, e.Linea.ToString(), e.Columna.ToString(), e.Descripcion));
            return sb.ToString();
        }

        public static string SimbolosCsv(ResultadoAnalisis r)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(FilaCsv("Id", "Lexema", "Tipo de token", "Tipo de dato", "Primera linea", "Primera columna", "Frecuencia", "Referencias"));
            foreach (ElementoSimbolo s in r.Simbolos)
                sb.AppendLine(FilaCsv(s.Id.ToString(), s.Lexema, s.TipoToken, s.TipoDato,
                    s.LineaPrimera.ToString(), s.ColumnaPrimera.ToString(), s.Frecuencia.ToString(),
                    s.Referencias.Count == 0 ? "" : string.Join(" ", s.Referencias)));
            return sb.ToString();
        }

        private static string H(string s)
        {
            return WebUtility.HtmlEncode(Reporte.Visible(s ?? ""));
        }

        private static string ClaseCategoria(string categoria)
        {
            switch (categoria)
            {
                case "Palabra Reservada": return "kw";
                case "Identificador": return "id";
                case "Número Entero":
                case "Número Decimal": return "num";
                case "Literal Cadena":
                case "Literal Carácter": return "str";
                case "Comentario": return "com";
                case "Delimitador": return "del";
                default: return "op";
            }
        }

        public static string Html(string titulo, ResultadoAnalisis r)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"es\"><head><meta charset=\"utf-8\">");
            sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
            sb.AppendLine("<title>Reporte léxico - " + H(titulo) + "</title>");
            sb.AppendLine("<style>");
            sb.AppendLine(":root{--bg:#f3f4f8;--card:#fff;--tx:#1e2233;--sub:#6a7089;--bd:#d8dbe6;--ac:#5b3fe0;--er:#d6204a;--kw:#7a3fc0;--id:#2a5db0;--num:#0b7285;--str:#b5651d;--com:#2e7d4f;--op:#9a6700;--del:#5b6475}");
            sb.AppendLine("@media(prefers-color-scheme:dark){:root{--bg:#14161f;--card:#1b1e2b;--tx:#e6e8f2;--sub:#8a90a8;--bd:#2c3042;--ac:#9a82ff;--er:#ff5c7a;--kw:#c792ea;--id:#82aaff;--num:#7fdbff;--str:#f7b267;--com:#6b9f7a;--op:#ffcb6b;--del:#89a3c0}}");
            sb.AppendLine("body{margin:0;font-family:'Segoe UI',system-ui,sans-serif;background:var(--bg);color:var(--tx)}");
            sb.AppendLine("header{padding:28px 32px;background:linear-gradient(120deg,var(--ac),#2a1f7a);color:#fff}");
            sb.AppendLine("header h1{margin:0;font-size:22px}header p{margin:6px 0 0;opacity:.85;font-size:13px}");
            sb.AppendLine("main{padding:24px 32px;max-width:1100px;margin:auto}");
            sb.AppendLine(".kpis{display:grid;grid-template-columns:repeat(auto-fit,minmax(150px,1fr));gap:12px;margin-bottom:24px}");
            sb.AppendLine(".kpi{background:var(--card);border:1px solid var(--bd);border-radius:12px;padding:14px 16px}");
            sb.AppendLine(".kpi b{display:block;font-size:26px}.kpi span{font-size:12px;color:var(--sub)}.kpi.err b{color:var(--er)}");
            sb.AppendLine("h2{font-size:16px;margin:28px 0 10px}");
            sb.AppendLine("table{width:100%;border-collapse:collapse;background:var(--card);border:1px solid var(--bd);border-radius:10px;overflow:hidden;font-size:13px}");
            sb.AppendLine("th,td{padding:7px 10px;text-align:left;border-bottom:1px solid var(--bd)}th{color:var(--sub);font-weight:600;font-size:12px}");
            sb.AppendLine("td.m{font-family:Consolas,monospace}tr.e td{color:var(--er)}");
            sb.AppendLine(".kw{color:var(--kw)}.id{color:var(--id)}.num{color:var(--num)}.str{color:var(--str)}.com{color:var(--com)}.op{color:var(--op)}.del{color:var(--del)}");
            sb.AppendLine(".bar{display:flex;align-items:center;gap:10px;margin:5px 0;font-size:13px}.bar i{display:block;height:12px;border-radius:6px;background:var(--ac)}");
            sb.AppendLine(".bar em{width:150px;font-style:normal;color:var(--sub)}");
            sb.AppendLine("footer{padding:20px 32px;color:var(--sub);font-size:12px;text-align:center}");
            sb.AppendLine("</style></head><body>");
            sb.AppendLine("<header><h1>Reporte del Analizador Léxico</h1><p>" + H(titulo) + " · generado el " +
                          DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " · USAC CUNOR · Lenguajes Formales y de Programación</p></header>");
            sb.AppendLine("<main>");

            sb.AppendLine("<div class=\"kpis\">");
            sb.AppendLine("<div class=\"kpi\"><b>" + r.TotalTokens + "</b><span>tokens</span></div>");
            sb.AppendLine("<div class=\"kpi" + (r.TotalErrores > 0 ? " err" : "") + "\"><b>" + r.TotalErrores + "</b><span>errores léxicos</span></div>");
            sb.AppendLine("<div class=\"kpi\"><b>" + r.TotalLineas + "</b><span>líneas procesadas</span></div>");
            sb.AppendLine("<div class=\"kpi\"><b>" + r.Simbolos.Count + "</b><span>identificadores distintos</span></div>");
            sb.AppendLine("<div class=\"kpi\"><b>" + r.TiempoMs.ToString("0.000") + " ms</b><span>tiempo de análisis</span></div>");
            sb.AppendLine("</div>");

            sb.AppendLine("<h2>Tokens por categoría</h2>");
            int max = r.TokensPorCategoria.Count == 0 ? 1 : r.TokensPorCategoria.Values.Max();
            foreach (KeyValuePair<string, int> kv in r.TokensPorCategoria.OrderByDescending(par => par.Value))
            {
                int ancho = Math.Max(2, (int)(kv.Value * 100.0 / max));
                sb.AppendLine("<div class=\"bar\"><em>" + H(kv.Key) + "</em><i style=\"width:" + ancho + "%\"></i><b>" + kv.Value + "</b></div>");
            }

            sb.AppendLine("<h2>Errores léxicos</h2>");
            if (r.Errores.Count == 0) sb.AppendLine("<p>Sin errores léxicos ✔</p>");
            else
            {
                sb.AppendLine("<table><tr><th>#</th><th>Regla</th><th>Lexema</th><th>Línea</th><th>Columna</th><th>Descripción</th></tr>");
                int n = 1;
                foreach (ErrorLexico e in r.Errores)
                    sb.AppendLine("<tr class=\"e\"><td>" + (n++) + "</td><td>" + e.Regla + "</td><td class=\"m\">" + H(e.Lexema) + "</td><td>" +
                                  e.Linea + "</td><td>" + e.Columna + "</td><td>" + H(e.Descripcion) + "</td></tr>");
                sb.AppendLine("</table>");
            }

            sb.AppendLine("<h2>Tabla de símbolos</h2>");
            sb.AppendLine("<table><tr><th>Id</th><th>Lexema</th><th>Tipo de dato</th><th>1ª línea/col</th><th>Frecuencia</th><th>Referencias</th></tr>");
            foreach (ElementoSimbolo s in r.Simbolos)
                sb.AppendLine("<tr><td>" + s.Id + "</td><td class=\"m id\">" + H(s.Lexema) + "</td><td>" + H(s.TipoDato) + "</td><td>" +
                              s.LineaPrimera + "/" + s.ColumnaPrimera + "</td><td>" + s.Frecuencia + "</td><td>" +
                              (s.Referencias.Count == 0 ? "-" : string.Join(", ", s.Referencias)) + "</td></tr>");
            sb.AppendLine("</table>");

            sb.AppendLine("<h2>Tokens reconocidos</h2>");
            sb.AppendLine("<table><tr><th>#</th><th>Lexema</th><th>Tipo de token</th><th>Categoría</th><th>Línea</th><th>Columna</th></tr>");
            int numToken = 1;
            foreach (Token t in r.Tokens)
                sb.AppendLine("<tr><td>" + (numToken++) + "</td><td class=\"m " + ClaseCategoria(t.Categoria) + "\">" + H(t.Lexema) + "</td><td>" +
                              H(t.Tipo) + "</td><td>" + H(t.Categoria) + "</td><td>" + t.Linea + "</td><td>" + t.Columna + "</td></tr>");
            sb.AppendLine("</table>");

            sb.AppendLine("</main><footer>Analizador léxico manual basado en autómata finito determinístico (sin Regex) · C# / Windows Forms</footer>");
            sb.AppendLine("</body></html>");
            return sb.ToString();
        }

        public static void Guardar(string ruta, string contenido)
        {
            File.WriteAllText(ruta, contenido, new UTF8Encoding(true));
        }
    }
}
