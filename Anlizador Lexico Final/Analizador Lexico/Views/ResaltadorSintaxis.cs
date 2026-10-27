using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Analizador_Lexico.Core;

namespace Analizador_Lexico.Views
{
    public static class ResaltadorSintaxis
    {
        private static readonly Color ColorPalabra = Color.FromArgb(0, 0, 200);
        private static readonly Color ColorCadena = Color.FromArgb(163, 21, 21);
        private static readonly Color ColorComentario = Color.FromArgb(0, 128, 0);
        private static readonly Color ColorNumero = Color.FromArgb(9, 134, 88);
        private static readonly Color ColorError = Color.Red;
        private static readonly Color FondoError = Color.FromArgb(255, 220, 220);

        public static List<int> InicioLineas(string texto)
        {
            List<int> inicios = new List<int> { 0 };
            for (int i = 0; i < texto.Length; i++)
                if (texto[i] == '\n') inicios.Add(i + 1);
            return inicios;
        }

        public static int Offset(List<int> inicios, string texto, int linea, int columna)
        {
            if (linea < 1 || linea > inicios.Count || columna < 1) return -1;
            int offset = inicios[linea - 1] + (columna - 1);
            return offset <= texto.Length ? offset : -1;
        }

        public static int LongitudError(ErrorLexico e, string texto, int offset)
        {
            if (offset < 0 || offset >= texto.Length) return 0;
            if (e.Lexema.Length > 0 && !e.Lexema.EndsWith("...")
                && offset + e.Lexema.Length <= texto.Length
                && string.CompareOrdinal(texto, offset, e.Lexema, 0, e.Lexema.Length) == 0)
                return e.Lexema.Length;

            if (e.Regla == 3) return texto.Length - offset;
            int fin = texto.IndexOf('\n', offset);
            if (fin < 0) fin = texto.Length;
            return Math.Max(1, fin - offset);
        }

        private static Color ColorDe(string categoria, Color normal)
        {
            switch (categoria)
            {
                case "Palabra Reservada": return ColorPalabra;
                case "Número Entero":
                case "Número Decimal": return ColorNumero;
                case "Literal Cadena":
                case "Literal Carácter": return ColorCadena;
                case "Comentario": return ColorComentario;
                default: return normal;
            }
        }

        public static void Aplicar(RichTextBox rtb, ResultadoAnalisis r)
        {
            string texto = rtb.Text;
            List<int> inicios = InicioLineas(texto);
            int selInicio = rtb.SelectionStart;
            int selLargo = rtb.SelectionLength;
            Point scroll = new Point();
            Nativos.SendMessage(rtb.Handle, Nativos.EM_GETSCROLLPOS, 0, ref scroll);
            Nativos.SendMessage(rtb.Handle, Nativos.WM_SETREDRAW, 0, 0);
            Color normal = rtb.ForeColor;
            Font negrita = new Font(rtb.Font, FontStyle.Bold);
            try
            {
                rtb.SelectAll();
                rtb.SelectionColor = normal;
                rtb.SelectionBackColor = rtb.BackColor;
                rtb.SelectionFont = rtb.Font;

                foreach (Token t in r.Tokens)
                {
                    int off = Offset(inicios, texto, t.Linea, t.Columna);
                    if (off < 0 || off + t.Lexema.Length > texto.Length) continue;
                    Color color = ColorDe(t.Categoria, normal);
                    bool reservada = t.Categoria == "Palabra Reservada";
                    if (color == normal) continue;
                    rtb.Select(off, t.Lexema.Length);
                    rtb.SelectionColor = color;
                    if (reservada) rtb.SelectionFont = negrita;
                }

                foreach (ErrorLexico e in r.Errores)
                {
                    int off = Offset(inicios, texto, e.Linea, e.Columna);
                    int largo = LongitudError(e, texto, off);
                    if (off < 0 || largo <= 0) continue;
                    rtb.Select(off, largo);
                    rtb.SelectionBackColor = FondoError;
                    rtb.SelectionColor = ColorError;
                }
            }
            finally
            {
                rtb.Select(selInicio, selLargo);
                Nativos.SendMessage(rtb.Handle, Nativos.EM_SETSCROLLPOS, 0, ref scroll);
                Nativos.SendMessage(rtb.Handle, Nativos.WM_SETREDRAW, 1, 0);
                negrita.Dispose();
                rtb.Invalidate();
            }
        }
    }
}
