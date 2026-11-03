using System;
using System.Collections.Generic;
using System.Linq;

namespace Analizador_Lexico.Core
{
    public static class VerificadorEstructura
    {
        public const int ReglaDelimitador = 8;
        public const int ReglaPuntoComa = 9;
        public const int ReglaEstructura = 10;

        private static readonly HashSet<string> Cabeceras = new HashSet<string>(StringComparer.Ordinal)
            { "if", "while", "for", "switch", "foreach", "using", "lock", "catch", "fixed" };

        private static readonly HashSet<string> ExigenParentesis = new HashSet<string>(StringComparer.Ordinal)
            { "if", "while", "for", "switch", "foreach" };

        private static readonly HashSet<string> Modificadores = new HashSet<string>(StringComparer.Ordinal)
            { "public", "private", "protected", "internal", "static", "abstract", "override", "virtual",
              "sealed", "readonly", "const", "async", "extern", "partial" };

        private static readonly HashSet<string> NoSonTipo = new HashSet<string>(StringComparer.Ordinal)
            { "new", "return", "throw", "await", "yield", "goto", "else", "do", "case", "default",
              "using", "namespace", "typeof", "sizeof", "ref", "out", "in", "params", "is", "as" };

        private static readonly HashSet<string> InicioDeInstruccion = new HashSet<string>(StringComparer.Ordinal)
            { "if", "else", "while", "for", "do", "switch", "case", "default", "break", "continue", "return",
              "int", "float", "double", "char", "bool", "string", "void", "class", "static", "public", "private" };

        private sealed class Delim
        {
            public Token Tok;
            public char Tipo;
            public bool Cabecera;
            public bool DoWhile;
            public string Metodo;
            public bool Inicializador;
            public bool PrecedidaPorDo;
        }

        public static void Verificar(ResultadoAnalisis r)
        {
            if (r == null) return;
            List<Token> toks = r.Tokens
                .Where(t => t.Tipo != TiposToken.ComLinea && t.Tipo != TiposToken.ComBloque)
                .ToList();
            List<ErrorLexico> nuevos = new Revision(toks, r.Errores).Ejecutar();
            if (nuevos.Count == 0) return;
            r.Errores = r.Errores.Concat(nuevos).OrderBy(e => e.Linea).ThenBy(e => e.Columna).ToList();
        }

        private sealed class Revision
        {
            private readonly List<Token> t;
            private readonly List<ErrorLexico> errores = new List<ErrorLexico>();
            private readonly List<Delim> pila = new List<Delim>();

            private int parenDepth, bracketDepth, initDepth;
            private int inicioSent;
            private bool pendienteDoWhile;
            private bool parenDoWhile;
            private bool cierreCabecera;
            private bool cierreDeclMetodo;

            private Token metodoCierre;
            private string metodoNombre;
            private Token claseToken;

            private readonly List<ErrorLexico> lexicos;
            private readonly bool comentarioSinCerrar;

            public Revision(List<Token> tokens, List<ErrorLexico> erroresLexicos)
            {
                t = tokens;
                lexicos = erroresLexicos;
                comentarioSinCerrar = erroresLexicos.Any(e => e.Regla == 3);
            }

            // Un error léxico entre dos tokens elimina lexemas: evita reportar un ';' que solo es consecuencia de eso.
            private bool HayErrorLexicoEntre(Token a, Token b)
            {
                foreach (ErrorLexico e in lexicos)
                {
                    bool despuesDeA = e.Linea > a.Linea || (e.Linea == a.Linea && e.Columna > a.Columna);
                    bool antesDeB = b == null || e.Linea < b.Linea || (e.Linea == b.Linea && e.Columna < b.Columna);
                    if (despuesDeA && antesDeB) return true;
                }
                return false;
            }

            private static bool L(Token x, string lex) { return x.Lexema == lex; }
            private static bool EsLiteral(Token x)
            {
                return x.Tipo == TiposToken.NumEntero || x.Tipo == TiposToken.NumDecimal
                    || x.Tipo == TiposToken.Cadena || x.Tipo == TiposToken.Caracter;
            }
            private static bool EsIdent(Token x) { return x.Tipo == TiposToken.Identificador; }
            private static bool EsPR(Token x) { return x.Tipo == TiposToken.PalabraReservada; }
            private static bool EsBoolLit(Token x) { return EsPR(x) && (x.Lexema == "true" || x.Lexema == "false"); }
            private static bool EsModificador(Token x)
            {
                return (EsPR(x) || EsIdent(x)) && Modificadores.Contains(x.Lexema);
            }
            private static bool EsTipo(Token x)
            {
                if (x.Tipo == TiposToken.PalabraReservada)
                    return PalabrasReservadas.EsTipoDeDato(x.Lexema) || x.Lexema == "void";
                return x.Tipo == TiposToken.Identificador && !NoSonTipo.Contains(x.Lexema) && !Modificadores.Contains(x.Lexema);
            }

            private void Agregar(Token tok, int regla, string descripcion)
            {
                string lex = tok.Lexema;
                if (lex.Length > 40) lex = lex.Substring(0, 40) + "...";
                errores.Add(new ErrorLexico(lex, descripcion, tok.Linea, tok.Columna, regla));
            }

            public List<ErrorLexico> Ejecutar()
            {
                for (int i = 0; i < t.Count; i++)
                {
                    Token tok = t[i];
                    VerificarPendientes(tok);

                    bool pendDo = pendienteDoWhile;
                    pendienteDoWhile = false;
                    cierreCabecera = false;
                    cierreDeclMetodo = false;

                    if (inicioSent < 0 || inicioSent > i) inicioSent = i;

                    ProcesarToken(i, tok, pendDo);
                    EvaluarPuntoComa(i);
                }

                if (metodoNombre != null)
                    Agregar(metodoCierre, ReglaEstructura, "Falta '{' para abrir el cuerpo del método '" + metodoNombre + "'");
                if (claseToken != null)
                    Agregar(claseToken, ReglaEstructura, "Falta '{' para abrir el cuerpo de la clase '" + claseToken.Lexema + "'");

                if (!comentarioSinCerrar)
                    foreach (Delim d in pila.AsEnumerable().Reverse())
                        Agregar(d.Tok, ReglaDelimitador, "Falta cerrar '" + d.Tipo + "' abierto en la línea " + d.Tok.Linea);

                return errores;
            }

            private void VerificarPendientes(Token tok)
            {
                if (metodoNombre != null)
                {
                    if (!(L(tok, "{") || L(tok, ";") || L(tok, "=") || L(tok, ":")))
                        Agregar(metodoCierre, ReglaEstructura, "Falta '{' para abrir el cuerpo del método '" + metodoNombre + "'");
                    metodoNombre = null;
                    metodoCierre = null;
                }
                if (claseToken != null && tok != claseToken)
                {
                    if (!(L(tok, "{") || L(tok, ":") || L(tok, "<")))
                        Agregar(claseToken, ReglaEstructura, "Falta '{' para abrir el cuerpo de la clase '" + claseToken.Lexema + "'");
                    claseToken = null;
                }
            }

            private void ProcesarToken(int i, Token tok, bool pendDo)
            {
                string lex = tok.Lexema;

                if (EsIdent(tok) && i > 0 && EsPR(t[i - 1]) && t[i - 1].Lexema == "class")
                    claseToken = tok;

                if (ExigenParentesis.Contains(lex) && (EsPR(tok) || EsIdent(tok)))
                {
                    bool sigueParen = i + 1 < t.Count && L(t[i + 1], "(");
                    if (!sigueParen)
                        Agregar(tok, ReglaEstructura, "Se esperaba '(' después de '" + lex + "'");
                    if (lex == "while" && pendDo) parenDoWhile = true;
                }

                if (tok.Tipo == TiposToken.ParIzq)
                {
                    Delim d = new Delim { Tok = tok, Tipo = '(' };
                    if (i > 0 && Cabeceras.Contains(t[i - 1].Lexema) && (EsPR(t[i - 1]) || EsIdent(t[i - 1])))
                    {
                        d.Cabecera = true;
                        d.DoWhile = parenDoWhile;
                    }
                    else if (parenDepth == 0 && bracketDepth == 0)
                    {
                        d.Metodo = DetectarMetodo(i);
                    }
                    parenDoWhile = false;
                    Empujar(d);
                }
                else if (tok.Tipo == TiposToken.CorcheteIzq)
                {
                    Empujar(new Delim { Tok = tok, Tipo = '[' });
                }
                else if (tok.Tipo == TiposToken.LlaveIzq)
                {
                    Token prev = i > 0 ? t[i - 1] : null;
                    Delim d = new Delim { Tok = tok, Tipo = '{' };
                    d.Inicializador = initDepth > 0
                        || (prev != null && (L(prev, "=") || L(prev, "]") || L(prev, ",") || L(prev, "(") || L(prev, "new")));
                    d.PrecedidaPorDo = prev != null && L(prev, "do");
                    Empujar(d);
                    inicioSent = i + 1;
                }
                else if (tok.Tipo == TiposToken.ParDer || tok.Tipo == TiposToken.CorcheteDer || tok.Tipo == TiposToken.LlaveDer)
                {
                    Cerrar(i, tok);
                }
                else if (tok.Tipo == TiposToken.PuntoComa)
                {
                    if (parenDepth == 0 && bracketDepth == 0) inicioSent = i + 1;
                }
                else if (tok.Tipo == TiposToken.DosPuntos)
                {
                    if (parenDepth == 0 && inicioSent < t.Count && inicioSent <= i
                        && (L(t[inicioSent], "case") || L(t[inicioSent], "default")))
                        inicioSent = i + 1;
                }
                else if (EsPR(tok) && (lex == "else" || lex == "do"))
                {
                    inicioSent = i + 1;
                }
            }

            private void Empujar(Delim d)
            {
                pila.Add(d);
                if (d.Tipo == '(') parenDepth++;
                else if (d.Tipo == '[') bracketDepth++;
                else if (d.Inicializador) initDepth++;
            }

            private Delim Sacar()
            {
                Delim d = pila[pila.Count - 1];
                pila.RemoveAt(pila.Count - 1);
                if (d.Tipo == '(') parenDepth--;
                else if (d.Tipo == '[') bracketDepth--;
                else if (d.Inicializador) initDepth--;
                return d;
            }

            private void Cerrar(int i, Token tok)
            {
                char cierre = tok.Lexema[0];
                char apertura = cierre == ')' ? '(' : cierre == ']' ? '[' : '{';

                int idx = -1;
                for (int k = pila.Count - 1; k >= 0; k--)
                    if (pila[k].Tipo == apertura) { idx = k; break; }

                if (idx < 0)
                {
                    Agregar(tok, ReglaDelimitador, "'" + cierre + "' sin su delimitador de apertura '" + apertura + "'");
                    return;
                }

                while (pila.Count - 1 > idx)
                {
                    Delim sobrante = Sacar();
                    Agregar(sobrante.Tok, ReglaDelimitador,
                        "Falta cerrar '" + sobrante.Tipo + "' (abierto en la línea " + sobrante.Tok.Linea
                        + ") antes de '" + cierre + "' de la línea " + tok.Linea);
                }

                Delim d = Sacar();

                if (cierre == ')')
                {
                    cierreCabecera = d.Cabecera && !d.DoWhile;
                    if (d.Cabecera && !d.DoWhile) inicioSent = i + 1;
                    if (d.Metodo != null)
                    {
                        cierreDeclMetodo = true;
                        metodoNombre = d.Metodo;
                        metodoCierre = tok;
                    }
                }
                else if (cierre == '}')
                {
                    pendienteDoWhile = d.PrecedidaPorDo;
                    inicioSent = i + 1;
                }
            }

            private string DetectarMetodo(int idxParen)
            {
                if (inicioSent < 0 || inicioSent >= idxParen) return null;
                int k = inicioSent;
                int mods = 0;
                while (k < idxParen && EsModificador(t[k])) { k++; mods++; }
                int resto = idxParen - k;
                if (resto == 1 && mods > 0 && EsIdent(t[k])) return t[k].Lexema;
                if (resto < 2) return null;
                if (!EsTipo(t[k])) return null;
                k++;
                while (k + 1 < idxParen && t[k].Tipo == TiposToken.CorcheteIzq && t[k + 1].Tipo == TiposToken.CorcheteDer) k += 2;
                if (k == idxParen - 1 && EsIdent(t[k]) && !Cabeceras.Contains(t[k].Lexema)) return t[k].Lexema;
                return null;
            }

            private bool TerminaExpresion(int i, Token sig)
            {
                Token x = t[i];
                if (EsIdent(x) || EsLiteral(x) || EsBoolLit(x)) return true;
                if (x.Tipo == TiposToken.ParDer || x.Tipo == TiposToken.CorcheteDer) return true;
                if (x.Tipo == TiposToken.OpInc || x.Tipo == TiposToken.OpDec)
                {
                    if (i == 0) return false;
                    Token p = t[i - 1];
                    return EsIdent(p) || p.Tipo == TiposToken.ParDer || p.Tipo == TiposToken.CorcheteDer;
                }
                if (EsPR(x) && (x.Lexema == "break" || x.Lexema == "continue")) return true;
                if (EsPR(x) && x.Lexema == "return") return sig == null || sig.Tipo == TiposToken.LlaveDer;
                return false;
            }

            private bool InicioDeNuevaInstruccion(Token n)
            {
                if (EsIdent(n) || EsLiteral(n) || EsBoolLit(n)) return true;
                if (n.Tipo == TiposToken.LlaveDer) return true;
                if (EsPR(n) && InicioDeInstruccion.Contains(n.Lexema)) return true;
                return false;
            }

            private static readonly HashSet<string> PrefijoIdent = new HashSet<string>(StringComparer.Ordinal)
                { "ref", "out", "in", "params", "is", "as", "new", "typeof", "sizeof", "default",
                  "await", "yield", "var", "const", "readonly", "get", "set", "using", "namespace", "try", "finally" };

            private bool CulpableMismaLinea(Token a, Token b)
            {
                if ((EsPR(b) && b.Lexema == "else") || b.Tipo == TiposToken.LlaveDer) return true;
                if (EsPR(b) && InicioDeInstruccion.Contains(b.Lexema) && b.Lexema != "case" && b.Lexema != "default")
                {
                    if (EsIdent(a) && PrefijoIdent.Contains(a.Lexema)) return false;
                    return true;
                }
                bool aLiteral = EsLiteral(a) || EsBoolLit(a) || a.Tipo == TiposToken.OpInc || a.Tipo == TiposToken.OpDec;
                if (aLiteral && (EsIdent(b) || EsLiteral(b) || EsBoolLit(b))) return true;
                if (EsIdent(a) && (EsLiteral(b) || EsBoolLit(b))) return !PrefijoIdent.Contains(a.Lexema);
                return false;
            }

            private bool InstruccionEsClase(int i)
            {
                for (int k = Math.Max(0, inicioSent); k <= i && k < t.Count; k++)
                    if (EsPR(t[k]) && t[k].Lexema == "class") return true;
                return false;
            }

            private void EvaluarPuntoComa(int i)
            {
                if (parenDepth > 0 || bracketDepth > 0 || initDepth > 0) return;
                Token a = t[i];
                Token b = i + 1 < t.Count ? t[i + 1] : null;

                if (!TerminaExpresion(i, b)) return;
                if (cierreCabecera || cierreDeclMetodo) return;

                bool culpable;
                if (b == null) culpable = true;
                else if (b.Linea > a.Linea) culpable = InicioDeNuevaInstruccion(b);
                else culpable = CulpableMismaLinea(a, b);
                if (!culpable) return;
                if (HayErrorLexicoEntre(a, b)) return;

                if (inicioSent >= 0 && inicioSent < t.Count && t[inicioSent].Tipo == TiposToken.CorcheteIzq) return;
                if (InstruccionEsClase(i)) return;

                Agregar(a, ReglaPuntoComa, "Falta ';' al final de la instrucción (después de '" + (a.Lexema.Length > 20 ? a.Lexema.Substring(0, 20) + "..." : a.Lexema) + "')");
            }
        }
    }
}
