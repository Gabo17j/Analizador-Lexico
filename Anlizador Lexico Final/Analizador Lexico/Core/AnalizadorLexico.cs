using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Analizador_Lexico.Core
{
    public class AnalizadorLexico
    {
        private enum EstadoNumero { Entero, Decimal }
        private enum EstadoCadena { Dentro, Escape }

        private readonly string codigoFuente;
        private int posicion;
        private int linea;
        private int columna;

        private readonly List<Token> tokens = new List<Token>();
        private Token ultimoToken;
        private double tiempoMs;

        public TablaSimbolos TablaSimbolos { get; } = new TablaSimbolos();
        public ManejadorErrores Errores { get; } = new ManejadorErrores();

        public bool IncluirComentarios { get; set; } = true;

        public AnalizadorLexico(string codigoFuente)
        {
            string texto = codigoFuente ?? "";
            if (texto.Length > 0 && texto[0] == '\uFEFF') texto = texto.Substring(1);
            this.codigoFuente = texto;
        }

        public static AnalizadorLexico DesdeArchivo(string ruta)
        {
            return new AnalizadorLexico(File.ReadAllText(ruta, Encoding.UTF8));
        }

        public List<Token> Analizar()
        {
            Reiniciar();
            Stopwatch reloj = Stopwatch.StartNew();
            while (true)
            {
                SaltarEspacios();
                if (Fin) break;
                ReconocerLexema();
            }
            reloj.Stop();
            tiempoMs = reloj.Elapsed.TotalMilliseconds;
            return tokens;
        }

        public ResultadoAnalisis ObtenerResultado()
        {
            ResultadoAnalisis r = new ResultadoAnalisis();
            r.Tokens = new List<Token>(tokens);
            r.Simbolos = new List<ElementoSimbolo>(TablaSimbolos.Elementos);
            r.Errores = new List<ErrorLexico>(Errores.Errores);
            r.TotalLineas = ContarLineas();
            r.TiempoMs = tiempoMs;
            foreach (Token t in tokens)
            {
                int n;
                r.TokensPorCategoria.TryGetValue(t.Categoria, out n);
                r.TokensPorCategoria[t.Categoria] = n + 1;
            }
            return r;
        }

        private bool Fin => posicion >= codigoFuente.Length;

        private char Mirar(int desplazamiento = 0)
        {
            int i = posicion + desplazamiento;
            return i < codigoFuente.Length ? codigoFuente[i] : '\0';
        }

        private char SiguienteCaracter()
        {
            char c = codigoFuente[posicion++];
            if (c == '\n') { linea++; columna = 1; }
            else if (c != '\r') { columna++; }
            return c;
        }

        private bool Coincide(char esperado)
        {
            if (!Fin && Mirar() == esperado)
            {
                SiguienteCaracter();
                return true;
            }
            return false;
        }

        private void Reiniciar()
        {
            posicion = 0;
            linea = 1;
            columna = 1;
            tokens.Clear();
            ultimoToken = null;
            TablaSimbolos.Limpiar();
            Errores.Limpiar();
        }

        private void SaltarEspacios()
        {
            while (!Fin)
            {
                char c = Mirar();
                if (c == ' ' || c == '\t' || c == '\r' || c == '\n') SiguienteCaracter();
                else break;
            }
        }

        private int ContarLineas()
        {
            if (codigoFuente.Length == 0) return 0;
            int n = 1;
            foreach (char c in codigoFuente) if (c == '\n') n++;
            if (codigoFuente[codigoFuente.Length - 1] == '\n') n--;
            return n;
        }

        private static bool EsLetra(char c) { return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'); }
        private static bool EsDigito(char c) { return c >= '0' && c <= '9'; }
        private static bool EsLetraDigitoOGuion(char c) { return EsLetra(c) || EsDigito(c) || c == '_'; }

        private static bool EsEscapeValido(char c)
        {
            return c == 'n' || c == 't' || c == 'r' || c == '0' || c == '"' || c == '\'' || c == '\\';
        }

        private static string Resumir(string s)
        {
            int salto = s.IndexOfAny(new[] { '\r', '\n' });
            if (salto >= 0) s = s.Substring(0, salto) + "...";
            if (s.Length > 40) s = s.Substring(0, 40) + "...";
            return s;
        }

        private void Emitir(string tipo, string lexema, int lin, int col)
        {
            Token t = new Token(tipo, lexema, lin, col);
            if (tipo == TiposToken.ComLinea || tipo == TiposToken.ComBloque)
            {
                if (IncluirComentarios) tokens.Add(t);
                return;
            }
            tokens.Add(t);
            ultimoToken = t;
        }

        private void Error(string lexema, string descripcion, int lin, int col, int regla)
        {
            Errores.AgregarError(Resumir(lexema), descripcion, lin, col, regla);
        }

        private string TipoDatoPrevio()
        {
            if (ultimoToken != null
                && ultimoToken.Tipo == TiposToken.PalabraReservada
                && PalabrasReservadas.EsTipoDeDato(ultimoToken.Lexema))
                return ultimoToken.Lexema;
            return null;
        }

        private void ReconocerLexema()
        {
            int lin = linea;
            int col = columna;
            char c = Mirar();

            if (EsLetra(c) || c == '_') { EstadoS1_Identificador(lin, col); return; }
            if (EsDigito(c)) { EstadoS2_Numero(lin, col); return; }

            switch (c)
            {
                case '"': EstadoS3_Cadena(lin, col); break;
                case '\'': EstadoS3_Caracter(lin, col); break;
                default: EstadoS4_Simbolo(lin, col); break;
            }
        }

        private void EstadoS1_Identificador(int lin, int col)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SiguienteCaracter());
            while (!Fin && EsLetraDigitoOGuion(Mirar())) sb.Append(SiguienteCaracter());

            string lexema = sb.ToString();
            if (PalabrasReservadas.EsReservada(lexema))
            {
                Emitir(TiposToken.PalabraReservada, lexema, lin, col);
            }
            else
            {
                string tipoPrevio = TipoDatoPrevio();
                Emitir(TiposToken.Identificador, lexema, lin, col);
                TablaSimbolos.RegistrarIdentificador(lexema, lin, col, tipoPrevio);
            }
        }

        private void EstadoS2_Numero(int lin, int col)
        {
            StringBuilder sb = new StringBuilder();
            EstadoNumero estado = EstadoNumero.Entero;
            sb.Append(SiguienteCaracter());

            bool terminado = false;
            while (!terminado)
            {
                char c = Mirar();
                switch (estado)
                {
                    case EstadoNumero.Entero:
                        if (EsDigito(c))
                        {
                            sb.Append(SiguienteCaracter());
                        }
                        else if (c == '.' && EsDigito(Mirar(1)))
                        {
                            sb.Append(SiguienteCaracter());
                            estado = EstadoNumero.Decimal;
                        }
                        else terminado = true;
                        break;

                    case EstadoNumero.Decimal:
                        if (EsDigito(c)) sb.Append(SiguienteCaracter());
                        else terminado = true;
                        break;
                }
            }

            char sig = Mirar();

            if (estado == EstadoNumero.Decimal && sig == '.' && EsDigito(Mirar(1)))
            {
                while (EsLetraDigitoOGuion(Mirar()) || (Mirar() == '.' && EsDigito(Mirar(1))))
                    sb.Append(SiguienteCaracter());
                Error(sb.ToString(), "Número decimal mal formado (múltiples puntos decimales)", lin, col, 6);
            }
            else if (EsLetra(sig) || sig == '_')
            {
                while (EsLetraDigitoOGuion(Mirar())) sb.Append(SiguienteCaracter());
                if (estado == EstadoNumero.Entero)
                    Error(sb.ToString(), "Identificador mal formado (inicio numérico prohibido)", lin, col, 1);
                else
                    Error(sb.ToString(), "Número mal formado (sufijo no válido en literal numérico)", lin, col, 6);
            }
            else
            {
                Emitir(estado == EstadoNumero.Entero ? TiposToken.NumEntero : TiposToken.NumDecimal, sb.ToString(), lin, col);
            }
        }

        private void EstadoS3_Cadena(int lin, int col)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SiguienteCaracter());
            EstadoCadena estado = EstadoCadena.Dentro;
            bool escapeInvalido = false;

            while (true)
            {
                if (Fin || Mirar() == '\n' || Mirar() == '\r')
                {
                    Error(sb.ToString(), "Literal de cadena no finalizado", lin, col, 2);
                    return;
                }

                int colCaracter = columna;
                char c = SiguienteCaracter();
                sb.Append(c);

                switch (estado)
                {
                    case EstadoCadena.Dentro:
                        if (c == '"')
                        {
                            if (!escapeInvalido) Emitir(TiposToken.Cadena, sb.ToString(), lin, col);
                            return;
                        }
                        if (c == '\\') estado = EstadoCadena.Escape;
                        break;

                    case EstadoCadena.Escape:
                        if (!EsEscapeValido(c))
                        {
                            Error("\\" + c, "Secuencia de escape no válida en literal de cadena", linea, colCaracter - 1, 7);
                            escapeInvalido = true;
                        }
                        estado = EstadoCadena.Dentro;
                        break;
                }
            }
        }

        private bool FinDeLinea()
        {
            return Fin || Mirar() == '\n' || Mirar() == '\r';
        }

        private void EstadoS3_Caracter(int lin, int col)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SiguienteCaracter());

            if (FinDeLinea())
            {
                Error(sb.ToString(), "Literal carácter no finalizado", lin, col, 5);
                return;
            }
            if (Mirar() == '\'')
            {
                sb.Append(SiguienteCaracter());
                Error(sb.ToString(), "Literal carácter mal formado (vacío)", lin, col, 5);
                return;
            }

            bool escapeInvalido = false;
            char c = SiguienteCaracter();
            sb.Append(c);
            if (c == '\\')
            {
                if (FinDeLinea())
                {
                    Error(sb.ToString(), "Literal carácter no finalizado", lin, col, 5);
                    return;
                }
                char e = SiguienteCaracter();
                sb.Append(e);
                if (!EsEscapeValido(e)) escapeInvalido = true;
            }

            if (Mirar() == '\'' && !Fin)
            {
                sb.Append(SiguienteCaracter());
                if (escapeInvalido)
                    Error(sb.ToString(), "Secuencia de escape no válida en literal carácter", lin, col, 7);
                else
                    Emitir(TiposToken.Caracter, sb.ToString(), lin, col);
                return;
            }

            while (!FinDeLinea() && Mirar() != '\'' && Mirar() != ';') sb.Append(SiguienteCaracter());

            if (!Fin && Mirar() == '\'')
            {
                sb.Append(SiguienteCaracter());
                Error(sb.ToString(), "Literal carácter mal formado (más de un carácter)", lin, col, 5);
            }
            else
            {
                Error(sb.ToString(), "Literal carácter no finalizado (falta comilla simple de cierre)", lin, col, 5);
            }
        }

        private void EstadoS4_Simbolo(int lin, int col)
        {
            char c = SiguienteCaracter();
            switch (c)
            {
                case '+':
                    if (Coincide('+')) Emitir(TiposToken.OpInc, "++", lin, col);
                    else if (Coincide('=')) Emitir(TiposToken.OpAsigSuma, "+=", lin, col);
                    else Emitir(TiposToken.OpSuma, "+", lin, col);
                    break;
                case '-':
                    if (Coincide('-')) Emitir(TiposToken.OpDec, "--", lin, col);
                    else if (Coincide('=')) Emitir(TiposToken.OpAsigResta, "-=", lin, col);
                    else Emitir(TiposToken.OpResta, "-", lin, col);
                    break;
                case '*':
                    if (Coincide('=')) Emitir(TiposToken.OpAsigMult, "*=", lin, col);
                    else Emitir(TiposToken.OpMult, "*", lin, col);
                    break;
                case '/':
                    if (Coincide('/')) ComentarioLinea(lin, col);
                    else if (Coincide('*')) ComentarioBloque(lin, col);
                    else if (Coincide('=')) Emitir(TiposToken.OpAsigDiv, "/=", lin, col);
                    else Emitir(TiposToken.OpDiv, "/", lin, col);
                    break;
                case '%':
                    Emitir(TiposToken.OpModulo, "%", lin, col);
                    break;
                case '=':
                    if (Coincide('=')) Emitir(TiposToken.OpIgualdad, "==", lin, col);
                    else Emitir(TiposToken.OpAsignacion, "=", lin, col);
                    break;
                case '!':
                    if (Coincide('=')) Emitir(TiposToken.OpDiferente, "!=", lin, col);
                    else Emitir(TiposToken.OpNot, "!", lin, col);
                    break;
                case '<':
                    if (Coincide('=')) Emitir(TiposToken.OpMenorIgual, "<=", lin, col);
                    else Emitir(TiposToken.OpMenor, "<", lin, col);
                    break;
                case '>':
                    if (Coincide('=')) Emitir(TiposToken.OpMayorIgual, ">=", lin, col);
                    else Emitir(TiposToken.OpMayor, ">", lin, col);
                    break;
                case '&':
                    if (Coincide('&')) Emitir(TiposToken.OpAnd, "&&", lin, col);
                    else Error("&", "Símbolo no permitido en lenguaje (&): ¿quiso escribir '&&'?", lin, col, 4);
                    break;
                case '|':
                    if (Coincide('|')) Emitir(TiposToken.OpOr, "||", lin, col);
                    else Error("|", "Símbolo no permitido en lenguaje (|): ¿quiso escribir '||'?", lin, col, 4);
                    break;
                case '(': Emitir(TiposToken.ParIzq, "(", lin, col); break;
                case ')': Emitir(TiposToken.ParDer, ")", lin, col); break;
                case '[': Emitir(TiposToken.CorcheteIzq, "[", lin, col); break;
                case ']': Emitir(TiposToken.CorcheteDer, "]", lin, col); break;
                case '{': Emitir(TiposToken.LlaveIzq, "{", lin, col); break;
                case '}': Emitir(TiposToken.LlaveDer, "}", lin, col); break;
                case ';': Emitir(TiposToken.PuntoComa, ";", lin, col); break;
                case ',': Emitir(TiposToken.Coma, ",", lin, col); break;
                case '.': Emitir(TiposToken.Punto, ".", lin, col); break;
                case ':': Emitir(TiposToken.DosPuntos, ":", lin, col); break;
                default:

                    string visible = char.IsControl(c) ? "\\u" + ((int)c).ToString("X4") : c.ToString();
                    Error(visible, "Símbolo no permitido en lenguaje (" + visible + ")", lin, col, 4);
                    break;
            }
        }

        private void ComentarioLinea(int lin, int col)
        {
            StringBuilder sb = new StringBuilder("//");
            while (!FinDeLinea()) sb.Append(SiguienteCaracter());
            Emitir(TiposToken.ComLinea, sb.ToString(), lin, col);
        }

        private void ComentarioBloque(int lin, int col)
        {
            StringBuilder sb = new StringBuilder("/*");
            while (!Fin)
            {
                char c = SiguienteCaracter();
                sb.Append(c);
                if (c == '*' && Mirar() == '/' && !Fin)
                {
                    sb.Append(SiguienteCaracter());
                    Emitir(TiposToken.ComBloque, sb.ToString(), lin, col);
                    return;
                }
            }

            Error(sb.ToString(), "Comentario de bloque no cerrado que inicia en la línea " + lin, lin, col, 3);
        }
    }
}
