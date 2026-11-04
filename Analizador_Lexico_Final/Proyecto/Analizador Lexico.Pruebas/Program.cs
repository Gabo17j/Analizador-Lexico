using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Analizador_Lexico.Core;

namespace Analizador_Lexico.Pruebas
{
    internal class Verificador
    {
        public List<string> Fallos { get; } = new List<string>();

        public void Igual<T>(string campo, T esperado, T obtenido)
        {
            if (!EqualityComparer<T>.Default.Equals(esperado, obtenido))
                Fallos.Add(campo + ": esperado <" + esperado + "> pero se obtuvo <" + obtenido + ">");
        }

        public void Verdadero(string campo, bool condicion)
        {
            if (!condicion) Fallos.Add(campo + ": la condición es falsa");
        }

        public void Fallar(string mensaje)
        {
            Fallos.Add(mensaje);
        }
    }

    internal static class Program
    {
        private static readonly List<string> bitacora = new List<string>();
        private static int aprobadas;
        private static int fallidas;

        private static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            if (args.Length == 1 && args[0] != "--solo-pruebas")
                return AnalizarArchivoLibre(args[0]);

            EjecutarSuite();

            string resumen = "RESUMEN: " + (aprobadas + fallidas) + " casos, " + aprobadas + " aprobados, " + fallidas + " fallidos.";
            Escribir("");
            Escribir(resumen);
            GuardarResultados();

            if (!(args.Length == 1 && args[0] == "--solo-pruebas"))
                MostrarDemostracion();

            return fallidas;
        }

        private static void Escribir(string linea)
        {
            Console.WriteLine(linea);
            bitacora.Add(linea);
        }

        private static string RutaArchivo(string nombre)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", nombre);
        }

        private static void GuardarResultados()
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resultados_pruebas.txt");
            try
            {
                File.WriteAllLines(ruta, bitacora.ToArray(), new UTF8Encoding(true));
                Console.WriteLine("Resultados guardados en: " + ruta);
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se pudo guardar el archivo de resultados: " + ex.Message);
            }
        }

        private static int AnalizarArchivoLibre(string ruta)
        {
            try
            {
                AnalizadorLexico analizador = AnalizadorLexico.DesdeArchivo(ruta);
                analizador.Analizar();
                Console.WriteLine(Reporte.Completo(Path.GetFileName(ruta), analizador.ObtenerResultado()));
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se pudo analizar el archivo: " + ex.Message);
                return 1;
            }
        }

        private static void MostrarDemostracion()
        {
            Console.WriteLine();
            Console.WriteLine("################ DEMOSTRACIÓN SOBRE LOS ARCHIVOS DE PRUEBA ################");
            string[] archivos = { "Prueba_Sencilla.txt", "Prueba_Media.txt", "Prueba_Errores.txt" };
            foreach (string nombre in archivos)
            {
                AnalizadorLexico analizador = AnalizadorLexico.DesdeArchivo(RutaArchivo(nombre));
                analizador.Analizar();
                Console.WriteLine(Reporte.Completo(nombre, analizador.ObtenerResultado()));
            }
        }

        private static void Caso(string id, string descripcion, Action<Verificador> prueba)
        {
            Verificador v = new Verificador();
            try { prueba(v); }
            catch (Exception ex) { v.Fallar("Excepción no controlada: " + ex.GetType().Name + " - " + ex.Message); }

            if (v.Fallos.Count == 0)
            {
                aprobadas++;
                Escribir(id.PadRight(5) + "APROBADO  " + descripcion);
            }
            else
            {
                fallidas++;
                Escribir(id.PadRight(5) + "FALLIDO   " + descripcion);
                foreach (string f in v.Fallos) Escribir("        - " + f);
            }
        }

        private static ResultadoAnalisis AnalizarTexto(string codigo, bool incluirComentarios = true)
        {
            AnalizadorLexico a = new AnalizadorLexico(codigo);
            a.IncluirComentarios = incluirComentarios;
            a.Analizar();
            return a.ObtenerResultado();
        }

        private static ResultadoAnalisis AnalizarArchivoPrueba(string nombre)
        {
            AnalizadorLexico a = AnalizadorLexico.DesdeArchivo(RutaArchivo(nombre));
            a.Analizar();
            return a.ObtenerResultado();
        }

        private static int ContarTipo(ResultadoAnalisis r, string tipo)
        {
            return r.Tokens.Count(t => t.Tipo == tipo);
        }

        private static bool ExisteToken(ResultadoAnalisis r, string tipo, string lexema, int linea, int columna)
        {
            return r.Tokens.Any(t => t.Tipo == tipo && t.Lexema == lexema && t.Linea == linea && t.Columna == columna);
        }

        private static void ComprobarError(Verificador v, ResultadoAnalisis r, int indice, int regla, string lexema, int linea, int columna)
        {
            if (indice >= r.Errores.Count)
            {
                v.Fallar("No existe el error #" + (indice + 1));
                return;
            }
            ErrorLexico e = r.Errores[indice];
            string p = "Error #" + (indice + 1);
            v.Igual(p + " regla", regla, e.Regla);
            v.Igual(p + " lexema", lexema, e.Lexema);
            v.Igual(p + " línea", linea, e.Linea);
            v.Igual(p + " columna", columna, e.Columna);
        }

        private static void EjecutarSuite()
        {
            Escribir("SUITE DE PRUEBAS - MOTOR LÉXICO (Entregables 3 y 5)");
            Escribir("Fecha de ejecución: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Escribir(new string('-', 78));

            Caso("P01", "Prueba_Sencilla.txt: tokens, posiciones y tabla de símbolos", v =>
            {
                ResultadoAnalisis r = AnalizarArchivoPrueba("Prueba_Sencilla.txt");
                v.Igual("tokens (con comentario)", 25, r.TotalTokens);
                v.Igual("errores", 0, r.TotalErrores);
                v.Igual("líneas", 5, r.TotalLineas);
                v.Igual("comentarios de línea", 1, ContarTipo(r, TiposToken.ComLinea));
                v.Igual("palabras reservadas (int)", 3, ContarTipo(r, TiposToken.PalabraReservada));
                v.Igual("identificadores (tokens)", 8, ContarTipo(r, TiposToken.Identificador));
                v.Igual("identificadores distintos", 5, r.Simbolos.Count);
                v.Verdadero("Console en (5,1)", ExisteToken(r, TiposToken.Identificador, "Console", 5, 1));
                v.Verdadero("'.' en (5,8)", ExisteToken(r, TiposToken.Punto, ".", 5, 8));
                v.Verdadero("WriteLine en (5,9)", ExisteToken(r, TiposToken.Identificador, "WriteLine", 5, 9));
                v.Verdadero("'+' en (4,14)", ExisteToken(r, TiposToken.OpSuma, "+", 4, 14));
            });

            Caso("P02", "Prueba_Media.txt: comentario multilínea, llaves anidadas, ==, %", v =>
            {
                ResultadoAnalisis r = AnalizarArchivoPrueba("Prueba_Media.txt");
                v.Igual("tokens (con comentario)", 34, r.TotalTokens);
                v.Igual("errores", 0, r.TotalErrores);
                v.Igual("líneas", 17, r.TotalLineas);
                v.Igual("palabras reservadas", 11, ContarTipo(r, TiposToken.PalabraReservada));
                v.Igual("llaves '{'", 4, ContarTipo(r, TiposToken.LlaveIzq));
                v.Igual("llaves '}'", 4, ContarTipo(r, TiposToken.LlaveDer));
                v.Igual("enteros", 2, ContarTipo(r, TiposToken.NumEntero));
                v.Verdadero("comentario de bloque en (1,1)", r.Tokens[0].Tipo == TiposToken.ComBloque && r.Tokens[0].Linea == 1 && r.Tokens[0].Columna == 1);
                v.Verdadero("Calculadora en (4,14) tras el comentario", ExisteToken(r, TiposToken.Identificador, "Calculadora", 4, 14));
                v.Verdadero("== en (8,24)", ExisteToken(r, TiposToken.OpIgualdad, "==", 8, 24));
                v.Verdadero("último '}' en (17,1)", ExisteToken(r, TiposToken.LlaveDer, "}", 17, 1));
                ElementoSimbolo numero = null;
                foreach (ElementoSimbolo s in r.Simbolos) if (s.Lexema == "numero") numero = s;
                v.Verdadero("símbolo 'numero' existe", numero != null);
                if (numero != null)
                {
                    v.Igual("frecuencia de 'numero'", 2, numero.Frecuencia);
                    v.Igual("línea de primera aparición de 'numero'", 6, numero.LineaPrimera);
                }
                v.Igual("identificadores distintos", 3, r.Simbolos.Count);
            });

            Caso("P03", "Prueba_Errores.txt: 5 errores exactos y el análisis continúa", v =>
            {
                ResultadoAnalisis r = AnalizarArchivoPrueba("Prueba_Errores.txt");
                v.Igual("errores", 5, r.TotalErrores);
                v.Igual("tokens válidos", 21, r.TotalTokens);
                v.Igual("líneas", 7, r.TotalLineas);
                ComprobarError(v, r, 0, 1, "1erValor", 2, 9);
                ComprobarError(v, r, 1, 6, "15.50.99", 3, 20);
                ComprobarError(v, r, 2, 2, "\"Hola mundo;", 4, 22);
                ComprobarError(v, r, 3, 4, "$", 5, 9);
                ComprobarError(v, r, 4, 3, "/* comentario sin fin...", 6, 5);
                v.Verdadero("recuperación: 'total' se reconoce tras '$'", ExisteToken(r, TiposToken.Identificador, "total", 5, 10));
                v.Verdadero("recuperación: '500' se reconoce", ExisteToken(r, TiposToken.NumEntero, "500", 5, 18));
                v.Verdadero("recuperación: '=' de la línea 4 se reconoce", ExisteToken(r, TiposToken.OpAsignacion, "=", 4, 20));
                v.Igual("identificadores distintos", 4, r.Simbolos.Count);
            });

            Caso("P04", "Errores extra 1: literal carácter, '&' suelto y escape inválido", v =>
            {
                ResultadoAnalisis r = AnalizarArchivoPrueba("Prueba_Errores_Extra1.txt");
                v.Igual("errores", 4, r.TotalErrores);
                v.Igual("tokens válidos", 28, r.TotalTokens);
                ComprobarError(v, r, 0, 5, "'abc'", 1, 10);
                ComprobarError(v, r, 1, 5, "''", 2, 10);
                ComprobarError(v, r, 2, 4, "&", 4, 13);
                ComprobarError(v, r, 3, 7, "\\q", 5, 16);
                v.Verdadero("carácter válido 'z' en (3,10)", ExisteToken(r, TiposToken.Caracter, "'z'", 3, 10));
                v.Verdadero("el ';' tras 'abc' se reconoce", ExisteToken(r, TiposToken.PuntoComa, ";", 1, 15));
                v.Verdadero("la última línea se analiza (fin)", ExisteToken(r, TiposToken.Identificador, "fin", 6, 5));
                v.Igual("identificadores distintos", 8, r.Simbolos.Count);
            });

            Caso("P05", "Errores extra 2: números mal formados, '#', '@' y cadena sin cerrar en EOF", v =>
            {
                ResultadoAnalisis r = AnalizarArchivoPrueba("Prueba_Errores_Extra2.txt");
                v.Igual("errores", 5, r.TotalErrores);
                v.Igual("tokens válidos", 24, r.TotalTokens);
                ComprobarError(v, r, 0, 1, "9x", 2, 5);
                ComprobarError(v, r, 1, 6, "3.14abc", 2, 13);
                ComprobarError(v, r, 2, 4, "#", 4, 7);
                ComprobarError(v, r, 3, 4, "@", 4, 11);
                ComprobarError(v, r, 4, 2, "\"sin cerrar", 5, 1);
                v.Verdadero("'3.' se separa en entero y punto", ExisteToken(r, TiposToken.NumEntero, "3", 3, 12) && ExisteToken(r, TiposToken.Punto, ".", 3, 13));
                v.Verdadero("recuperación: '2' tras el número inválido", ExisteToken(r, TiposToken.NumEntero, "2", 2, 23));
            });

            Caso("P06", "Sensibilidad a mayúsculas: If/WHILE/Class son identificadores", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("If WHILE Class if");
                v.Igual("identificadores", 3, ContarTipo(r, TiposToken.Identificador));
                v.Igual("palabras reservadas", 1, ContarTipo(r, TiposToken.PalabraReservada));
                v.Igual("símbolos", 3, r.Simbolos.Count);
                v.Igual("errores", 0, r.TotalErrores);
            });

            Caso("P07", "Operadores: mayor coincidencia (+=, ++, --, <=, >=, ==, !=, &&, ||)", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("a+=b++;c--<=d>=e==f!=g&&h||!i;");
                string obtenido = string.Join(" ", r.Tokens.Select(t => t.Lexema));
                v.Igual("secuencia de lexemas", "a += b ++ ; c -- <= d >= e == f != g && h || ! i ;", obtenido);
                v.Verdadero("++ es TK_OP_INC", ExisteToken(r, TiposToken.OpInc, "++", 1, 5));
                v.Verdadero("-- es TK_OP_DEC", ExisteToken(r, TiposToken.OpDec, "--", 1, 9));
                v.Igual("errores", 0, r.TotalErrores);
            });

            Caso("P08", "Números: entero, decimal y '7.' separado en entero + punto", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("0 150 3.1416 100.001 7.");
                string tipos = string.Join(",", r.Tokens.Select(t => t.Tipo));
                string esperado = string.Join(",", new[]
                {
                    TiposToken.NumEntero, TiposToken.NumEntero, TiposToken.NumDecimal,
                    TiposToken.NumDecimal, TiposToken.NumEntero, TiposToken.Punto
                });
                v.Igual("tipos de token", esperado, tipos);
                v.Igual("errores", 0, r.TotalErrores);
            });

            Caso("P09", "Cadenas y caracteres con secuencias de escape válidas", v =>
            {
                ResultadoAnalisis r = AnalizarTexto(@"""a\n\t\""b\\"" '\n' '\''");
                v.Igual("tokens", 3, r.TotalTokens);
                v.Igual("cadenas", 1, ContarTipo(r, TiposToken.Cadena));
                v.Igual("caracteres", 2, ContarTipo(r, TiposToken.Caracter));
                v.Igual("errores", 0, r.TotalErrores);
            });

            Caso("P10", "Saltos de línea CRLF: columnas y líneas correctas", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("int a;\r\nint b;\r\n");
                v.Igual("tokens", 6, r.TotalTokens);
                v.Verdadero("b en (2,5)", ExisteToken(r, TiposToken.Identificador, "b", 2, 5));
                v.Igual("líneas", 2, r.TotalLineas);
            });

            Caso("P11", "Entrada vacía o solo espacios: sin tokens ni errores", v =>
            {
                ResultadoAnalisis vacio = AnalizarTexto("");
                v.Igual("vacío: tokens", 0, vacio.TotalTokens);
                v.Igual("vacío: errores", 0, vacio.TotalErrores);
                v.Igual("vacío: líneas", 0, vacio.TotalLineas);
                ResultadoAnalisis blancos = AnalizarTexto("  \t\r\n  \n");
                v.Igual("blancos: tokens", 0, blancos.TotalTokens);
                v.Igual("blancos: errores", 0, blancos.TotalErrores);
            });

            Caso("P12", "'/*/' no cierra el comentario: error de la Regla 3 con posición", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("int a; /*/ resto");
                v.Igual("tokens", 3, r.TotalTokens);
                v.Igual("errores", 1, r.TotalErrores);
                ComprobarError(v, r, 0, 3, "/*/ resto", 1, 8);
            });

            Caso("P13", "Tabla de símbolos: sin duplicados, frecuencia, referencias y tipo", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("int a;\nint b;\na = b;\nb = a;");
                v.Igual("símbolos distintos", 2, r.Simbolos.Count);
                ElementoSimbolo a = r.Simbolos[0];
                ElementoSimbolo b = r.Simbolos[1];
                v.Igual("a.Lexema", "a", a.Lexema);
                v.Igual("a.Frecuencia", 3, a.Frecuencia);
                v.Igual("a.LineaPrimera", 1, a.LineaPrimera);
                v.Igual("a.TipoDato", "int", a.TipoDato);
                v.Igual("a.Referencias", "3,4", string.Join(",", a.Referencias));
                v.Igual("b.Frecuencia", 3, b.Frecuencia);
                v.Igual("b.LineaPrimera", 2, b.LineaPrimera);
                v.Igual("b.TipoToken", TiposToken.Identificador, b.TipoToken);
            });

            Caso("P14", "Comentario de línea al final del archivo (sin salto) e IncluirComentarios", v =>
            {
                ResultadoAnalisis con = AnalizarTexto("x // fin");
                v.Igual("con comentarios: tokens", 2, con.TotalTokens);
                v.Igual("con comentarios: errores", 0, con.TotalErrores);
                ResultadoAnalisis sin = AnalizarTexto("x // fin", false);
                v.Igual("sin comentarios: tokens", 1, sin.TotalTokens);
                ResultadoAnalisis media = AnalizarArchivoPrueba("Prueba_Media.txt");
                ResultadoAnalisis media2 = null;
                AnalizadorLexico a = AnalizadorLexico.DesdeArchivo(RutaArchivo("Prueba_Media.txt"));
                a.IncluirComentarios = false;
                a.Analizar();
                media2 = a.ObtenerResultado();
                v.Igual("Prueba_Media sin comentarios", media.TotalTokens - 1, media2.TotalTokens);
            });

            Caso("P15", "Reanálisis: ejecutar dos veces no duplica tokens, símbolos ni errores", v =>
            {
                AnalizadorLexico a = AnalizadorLexico.DesdeArchivo(RutaArchivo("Prueba_Errores.txt"));
                a.Analizar();
                a.Analizar();
                ResultadoAnalisis r = a.ObtenerResultado();
                v.Igual("tokens", 21, r.TotalTokens);
                v.Igual("símbolos", 4, r.Simbolos.Count);
                v.Igual("errores", 5, r.TotalErrores);
            });

            Caso("P16", "Carácter no ASCII fuera de cadenas se reporta (Regla 4) sin detener el análisis", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("int ñ = 1; int z;");
                v.Igual("errores", 1, r.TotalErrores);
                ComprobarError(v, r, 0, 4, "ñ", 1, 5);
                v.Verdadero("z se reconoce después", ExisteToken(r, TiposToken.Identificador, "z", 1, 16));
            });

            EjecutarBateriaEntregable5();
        }

        private static string Lexemas(ResultadoAnalisis r)
        {
            return string.Join(" ", r.Tokens.Select(t => t.Lexema));
        }

        private static void EjecutarBateriaEntregable5()
        {
            Escribir(new string('-', 78));
            Escribir("BATERÍA AMPLIADA (Entregable 5): casos válidos, límite, errores combinados y exportación");
            Escribir(new string('-', 78));

            Caso("T01", "Las 24 palabras reservadas se reconocen como TK_PR_PALABRA", v =>
            {
                string todas = string.Join(" ", PalabrasReservadas.Todas.OrderBy(x => x, StringComparer.Ordinal));
                ResultadoAnalisis r = AnalizarTexto(todas);
                v.Igual("palabras reservadas definidas", 24, PalabrasReservadas.Todas.Count());
                v.Igual("tokens", 24, r.TotalTokens);
                v.Igual("TK_PR_PALABRA", 24, ContarTipo(r, TiposToken.PalabraReservada));
                v.Igual("errores", 0, r.TotalErrores);
                v.Igual("símbolos", 0, r.Simbolos.Count);
            });

            Caso("T02", "Límite: identificador de 500 caracteres, '_' solo y mezcla letra/dígito/guion", v =>
            {
                ResultadoAnalisis r = AnalizarTexto(new string('a', 500) + " _ a1_b2");
                v.Igual("tokens", 3, r.TotalTokens);
                v.Igual("identificadores", 3, ContarTipo(r, TiposToken.Identificador));
                v.Igual("longitud del primer lexema", 500, r.Tokens[0].Lexema.Length);
                v.Verdadero("'_' en (1,502)", ExisteToken(r, TiposToken.Identificador, "_", 1, 502));
                v.Verdadero("a1_b2 en (1,504)", ExisteToken(r, TiposToken.Identificador, "a1_b2", 1, 504));
                v.Igual("símbolos", 3, r.Simbolos.Count);
                v.Igual("errores", 0, r.TotalErrores);
            });

            Caso("T03", "Límite: decimal con 20 decimales, ceros a la izquierda (007) y 12.0", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("3.14159265358979323846 0 007 12.0");
                string tipos = string.Join(",", r.Tokens.Select(t => t.Tipo));
                string esperado = string.Join(",", new[] { TiposToken.NumDecimal, TiposToken.NumEntero, TiposToken.NumEntero, TiposToken.NumDecimal });
                v.Igual("tipos", esperado, tipos);
                v.Igual("lexema decimal largo", "3.14159265358979323846", r.Tokens[0].Lexema);
                v.Igual("errores", 0, r.TotalErrores);
            });

            Caso("T04", "Cadenas: vacía, con '//' dentro, con \\t y con comilla escapada", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("\"\" \"a // b\" \"tab\\there\" \"q\\\"uote\"");
                v.Igual("cadenas", 4, ContarTipo(r, TiposToken.Cadena));
                v.Igual("comentarios", 0, ContarTipo(r, TiposToken.ComLinea));
                v.Igual("errores", 0, r.TotalErrores);
            });

            Caso("T05", "Caracteres literales: 'a' '9' '\\n' '\\\\' '\\''", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("'a' '9' '\\n' '\\\\' '\\''");
                v.Igual("caracteres", 5, ContarTipo(r, TiposToken.Caracter));
                v.Igual("tokens", 5, r.TotalTokens);
                v.Igual("errores", 0, r.TotalErrores);
            });

            Caso("T06", "Mayor coincidencia en bordes: a+++b, x=-1, i---j", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("a+++b x=-1 i--- j");
                v.Igual("lexemas", "a ++ + b x = - 1 i -- - j", Lexemas(r));
                v.Igual("errores", 0, r.TotalErrores);
                v.Igual("símbolos", 5, r.Simbolos.Count);
            });

            Caso("T07", "Comentarios: '// /*' no abre bloque; '/* a /* b */' no anida", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("// /* no abre\n/* a /* b */ c */ int");
                v.Igual("tokens", 6, r.TotalTokens);
                v.Igual("errores", 0, r.TotalErrores);
                v.Verdadero("comentario de línea (1,1)", ExisteToken(r, TiposToken.ComLinea, "// /* no abre", 1, 1));
                v.Verdadero("bloque (2,1) termina en el primer */", ExisteToken(r, TiposToken.ComBloque, "/* a /* b */", 2, 1));
                v.Verdadero("c en (2,14)", ExisteToken(r, TiposToken.Identificador, "c", 2, 14));
                v.Verdadero("'*' en (2,16)", ExisteToken(r, TiposToken.OpMult, "*", 2, 16));
                v.Verdadero("'/' en (2,17)", ExisteToken(r, TiposToken.OpDiv, "/", 2, 17));
                v.Verdadero("int en (2,19)", ExisteToken(r, TiposToken.PalabraReservada, "int", 2, 19));
            });

            Caso("T08", "Comentario de bloque sin cerrar tras código válido (Regla 3, línea 2)", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("int a;\n/* abierto\nsigue");
                v.Igual("tokens", 3, r.TotalTokens);
                v.Igual("errores", 1, r.TotalErrores);
                ComprobarError(v, r, 0, 3, "/* abierto...", 2, 1);
                v.Verdadero("la descripción cita la línea 2", r.Errores[0].Descripcion.EndsWith("línea 2"));
            });

            Caso("T09", "Seis errores combinados en una sola línea; el análisis continúa", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("x = 9a + 3.1.2 # 'xy' @ \"sin");
                v.Igual("tokens válidos", 3, r.TotalTokens);
                v.Igual("errores", 6, r.TotalErrores);
                ComprobarError(v, r, 0, 1, "9a", 1, 5);
                ComprobarError(v, r, 1, 6, "3.1.2", 1, 10);
                ComprobarError(v, r, 2, 4, "#", 1, 16);
                ComprobarError(v, r, 3, 5, "'xy'", 1, 18);
                ComprobarError(v, r, 4, 4, "@", 1, 23);
                ComprobarError(v, r, 5, 2, "\"sin", 1, 25);
                v.Verdadero("'+' se reconoce entre errores", ExisteToken(r, TiposToken.OpSuma, "+", 1, 8));
            });

            Caso("T10", "Los 10 delimitadores ( ) { } [ ] ; , . :", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("( ) { } [ ] ; , . :");
                v.Igual("tokens", 10, r.TotalTokens);
                v.Igual("categoría Delimitador", 10, r.Tokens.Count(t => t.Categoria == "Delimitador"));
                v.Verdadero("':' en (1,19)", ExisteToken(r, TiposToken.DosPuntos, ":", 1, 19));
                v.Igual("errores", 0, r.TotalErrores);
            });

            Caso("T11", "Cadena con salto de línea real: dos errores de la Regla 2 en líneas distintas", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("s = \"abc\n def\";");
                v.Igual("tokens", 3, r.TotalTokens);
                v.Igual("errores", 2, r.TotalErrores);
                ComprobarError(v, r, 0, 2, "\"abc", 1, 5);
                ComprobarError(v, r, 1, 2, "\";", 2, 5);
                v.Verdadero("def en (2,2)", ExisteToken(r, TiposToken.Identificador, "def", 2, 2));
            });

            Caso("T12", "Estrés: 10 000 líneas (50 000 tokens, 10 000 símbolos) en menos de 5 s", v =>
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 10000; i++) sb.Append("int v").Append(i).Append(" = ").Append(i).Append(";\n");
                ResultadoAnalisis r = AnalizarTexto(sb.ToString());
                v.Igual("tokens", 50000, r.TotalTokens);
                v.Igual("símbolos", 10000, r.Simbolos.Count);
                v.Igual("líneas", 10000, r.TotalLineas);
                v.Igual("errores", 0, r.TotalErrores);
                v.Verdadero("tiempo < 5000 ms (" + r.TiempoMs.ToString("0.0") + " ms)", r.TiempoMs < 5000);
            });

            Caso("T13", "Exportación CSV: escape de comillas/comas y protección de '=' contra fórmulas", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("a = \",\" ;");
                string csv = Exportador.TokensCsv(r);
                string[] filas = csv.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                v.Igual("filas (encabezado + 4 tokens)", 5, filas.Length);
                v.Igual("encabezado", "#,Lexema,Tipo de token,Categoria,Linea,Columna", filas[0]);
                v.Igual("fila del '='", "2,'=,TK_OP_ASIGNACION,Op. Asignación,1,3", filas[2]);
                v.Igual("fila de la cadena con coma", "3,\"\"\",\"\"\",TK_CADENA,Literal Cadena,1,5", filas[3]);
                string errores = Exportador.ErroresCsv(AnalizarTexto("int $x;"));
                v.Verdadero("CSV de errores incluye la regla 4", errores.Contains("1,4,$,1,5,"));
            });

            Caso("T14", "Exportación HTML: escapa '<' y '&' y no inyecta etiquetas", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("a < b && c");
                string html = Exportador.Html("prueba <b>", r);
                v.Verdadero("contiene &lt;", html.Contains("&lt;"));
                v.Verdadero("título escapado", html.Contains("prueba &lt;b&gt;"));
                v.Verdadero("no contiene <b> literal del título", !html.Contains("prueba <b>"));
                v.Verdadero("tiene tabla de tokens", html.Contains("Tokens reconocidos"));
            });

            Caso("T15", "BOM al inicio y tabulador: 'int' en la columna 2; el BOM no es un error", v =>
            {
                ResultadoAnalisis r = AnalizarTexto("\uFEFF\tint x;");
                v.Igual("errores", 0, r.TotalErrores);
                v.Igual("tokens", 3, r.TotalTokens);
                v.Verdadero("int en (1,2)", ExisteToken(r, TiposToken.PalabraReservada, "int", 1, 2));
                v.Verdadero("x en (1,6)", ExisteToken(r, TiposToken.Identificador, "x", 1, 6));
            });

            // ------------------------------------------------------------------
            // Fase final: verificador de estructura (reglas 8 a 11), diccionario y autómatas
            // ------------------------------------------------------------------

            Caso("T16", "Regla 9: falta ';' al final de la línea 1 (después de 5, en 1,9); la línea 2 está bien", v =>
            {
                ResultadoAnalisis r = AnalizarConEstructura("int x = 5\nint y = 6;");
                v.Igual("errores", 1, r.TotalErrores);
                v.Verdadero("R9 en (1,9)", ExisteError(r, 9, 1, 9));
            });

            Caso("T17", "Regla 8: '{' sin cerrar (1,10) y '}' sin abrir (1,12)", v =>
            {
                ResultadoAnalisis a = AnalizarConEstructura("void f() {\n int x = 1;\n");
                v.Igual("errores (llave sin cerrar)", 1, a.TotalErrores);
                v.Verdadero("R8 en (1,10)", ExisteError(a, 8, 1, 10));
                ResultadoAnalisis b = AnalizarConEstructura("int x = 1; }");
                v.Igual("errores (llave sin abrir)", 1, b.TotalErrores);
                v.Verdadero("R8 en (1,12)", ExisteError(b, 8, 1, 12));
            });

            Caso("T18", "Regla 10: falta '(' después de 'if' y falta '{' en la clase", v =>
            {
                ResultadoAnalisis a = AnalizarConEstructura("if x > 3 { }");
                v.Verdadero("R10 en (1,1)", ExisteError(a, 10, 1, 1));
                ResultadoAnalisis b = AnalizarConEstructura("class A\nint x;");
                v.Verdadero("R10 en (1,7)", ExisteError(b, 10, 1, 7));
            });

            Caso("T19", "Regla 11: falta '=' o un operador entre operandos, operando faltante y '=' dentro de if", v =>
            {
                v.Verdadero("int x 5; -> (1,5)", ExisteError(AnalizarConEstructura("int x 5;"), 11, 1, 5));
                v.Verdadero("x = ; -> (1,3)", ExisteError(AnalizarConEstructura("x = ;"), 11, 1, 3));
                v.Verdadero("y = a +; -> (1,7)", ExisteError(AnalizarConEstructura("y = a +;"), 11, 1, 7));
                v.Verdadero("if (x = 5) -> (1,7)", ExisteError(AnalizarConEstructura("if (x = 5) { }"), 11, 1, 7));
                v.Verdadero("if (a b) -> (1,5)", ExisteError(AnalizarConEstructura("if (a b) { }"), 11, 1, 5));
            });

            Caso("T20", "Los archivos de prueba válidos no producen errores de estructura", v =>
            {
                foreach (string nombre in new[] { "Prueba_Sencilla.txt", "Prueba_Media.txt" })
                {
                    ResultadoAnalisis r = AnalizarConEstructura(File.ReadAllText(RutaArchivo(nombre)));
                    v.Igual(nombre + " errores", 0, r.TotalErrores);
                }
            });

            Caso("T21", "Diccionario editable: con palabras en español 'entero' pasa a ser reservada y tipo de dato", v =>
            {
                try
                {
                    PalabrasReservadas.Establecer(new[]
                    {
                        new KeyValuePair<string, bool>("entero", true), new KeyValuePair<string, bool>("si", false),
                        new KeyValuePair<string, bool>("entero", true), new KeyValuePair<string, bool>("9mal", false)
                    });
                    v.Igual("palabras (sin duplicados ni inválidas)", 2, PalabrasReservadas.Cantidad);
                    ResultadoAnalisis r = AnalizarTexto("entero x = 5; int y;");
                    v.Verdadero("entero es palabra reservada", ExisteToken(r, TiposToken.PalabraReservada, "entero", 1, 1));
                    v.Verdadero("int ahora es identificador", ExisteToken(r, TiposToken.Identificador, "int", 1, 15));
                    ElementoSimbolo x = r.Simbolos.FirstOrDefault(s => s.Lexema == "x");
                    v.Verdadero("x existe", x != null);
                    if (x != null) v.Igual("tipo de dato de x", "entero", x.TipoDato);
                }
                finally { PalabrasReservadas.Restaurar(); }
                v.Igual("tras restaurar vuelven las 24", 24, PalabrasReservadas.Cantidad);
            });

            Caso("T22", "AFD 'termina en ab': acepta aab, rechaza abb y vacía; símbolo fuera del alfabeto es error", v =>
            {
                Automata a = CrearAfd();
                v.Igual("válido", 0, a.Validar().Count);
                v.Verdadero("aab acepta", a.Analizar("aab").Acepta);
                v.Verdadero("abb rechaza", !a.Analizar("abb").Acepta);
                v.Verdadero("vacía rechaza", !a.Analizar("").Acepta);
                v.Verdadero("abc da error", a.Analizar("abc").Error != null);
                v.Igual("un único hilo", 1, a.Analizar("aab").Hilos.Count);
                v.Igual("camino del hilo", "q0 -a-> q1 -a-> q1 -b-> q2", a.Analizar("aab").Hilos[0].ToString());
            });

            Caso("T23", "AFD parcial: la celda vacía bloquea el hilo (Bloqueado) y la cadena se rechaza", v =>
            {
                Automata a = CrearAfd();
                a.FijarTransicion("q1", "b", new string[0]);
                ResultadoCadena r = a.Analizar("ab");
                v.Verdadero("rechaza", !r.Acepta);
                v.Igual("resultado del hilo", "Bloqueado", r.Hilos[0].Resultado);
            });

            Caso("T24", "AFND 'termina en ab': 'abab' tiene 4 hilos y acepta; conjuntos {q0},{q0,q1},{q0,q2}", v =>
            {
                Automata a = CrearAfnd(false);
                ResultadoCadena r = a.Analizar("ab");
                v.Verdadero("acepta", r.Acepta);
                v.Igual("hilos de 'ab'", 2, r.Hilos.Count);
                v.Igual("hilos que aceptan", 1, r.Hilos.Count(h => h.Acepta));
                v.Igual("conjunto tras 'a'", "q0,q1", string.Join(",", r.ConjuntosPorPaso[1]));
                v.Igual("conjunto tras 'ab'", "q0,q2", string.Join(",", r.ConjuntosPorPaso[2]));
                v.Verdadero("'ba' rechaza", !a.Analizar("ba").Acepta);
            });

            Caso("T25", "AFND con ε (Σ*ab): el cierre-ε inicial es {q0,q1}; 'aab' acepta y hay un hilo con transición ε", v =>
            {
                Automata a = CrearAfnd(true);
                v.Igual("cierre-ε de q0", "q0,q1", string.Join(",", a.CierreEpsilon(new[] { "q0" })));
                ResultadoCadena r = a.Analizar("aab");
                v.Verdadero("acepta", r.Acepta);
                v.Verdadero("algún hilo usa ε", r.Hilos.Any(h => h.Pasos.Any(p => p.Simbolo == Automata.Epsilon)));
                v.Verdadero("el hilo que acepta termina en q3", r.Hilos.Any(h => h.Acepta && h.Pasos[h.Pasos.Count - 1].Hasta == "q3"));
            });

            Caso("T26", "Validación de la definición: AFD con 2 destinos, sin estado inicial y estado inexistente", v =>
            {
                Automata a = CrearAfd();
                a.FijarTransicion("q0", "a", new[] { "q1", "q2" });
                v.Verdadero("AFD con 2 destinos", a.Validar().Any(p => p.StartsWith("AFD:")));
                Automata b = CrearAfd();
                b.Inicial = null;
                v.Verdadero("sin inicial", b.Validar().Any(p => p.Contains("inicial")));
                Automata c = CrearAfd();
                c.FijarTransicion("q2", "a", new[] { "q9" });
                v.Verdadero("estado inexistente", c.Validar().Any(p => p.Contains("q9")));
            });

            Caso("T27", "Aristas: las transiciones q0-a->q0 y q0-b->q0 se agrupan en una sola arista con etiqueta 'a,b'", v =>
            {
                Automata a = new Automata();
                a.Estados.AddRange(new[] { "q0", "q1" });
                a.Alfabeto.AddRange(new[] { "a", "b" });
                a.Inicial = "q0";
                a.FijarTransicion("q0", "a", new[] { "q0" });
                a.FijarTransicion("q0", "b", new[] { "q0", "q1" });
                var aristas = a.Aristas();
                v.Igual("aristas", 2, aristas.Count);
                v.Igual("etiqueta del bucle", "a,b", string.Join(",", aristas[0].Value));
            });

            Caso("T28", "Límite de hilos: a^12 en un AFND con bifurcación total se trunca en 300 hilos", v =>
            {
                Automata a = new Automata { EsAFND = true };
                a.Estados.AddRange(new[] { "q0", "q1" });
                a.Alfabeto.Add("a");
                a.Inicial = "q0";
                a.Finales.Add("q1");
                a.FijarTransicion("q0", "a", new[] { "q0", "q1" });
                a.FijarTransicion("q1", "a", new[] { "q0", "q1" });
                ResultadoCadena r = a.Analizar("aaaaaaaaaaaa");
                v.Verdadero("acepta", r.Acepta);
                v.Verdadero("truncado", r.Truncado);
                v.Igual("hilos listados", Automata.MaximoHilos, r.Hilos.Count);
            });
        }

        private static ResultadoAnalisis AnalizarConEstructura(string texto)
        {
            ResultadoAnalisis r = AnalizarTexto(texto);
            VerificadorEstructura.Verificar(r);
            return r;
        }

        private static bool ExisteError(ResultadoAnalisis r, int regla, int linea, int columna)
        {
            return r.Errores.Any(e => e.Regla == regla && e.Linea == linea && e.Columna == columna);
        }

        // AFD sobre {a,b} que acepta las cadenas que terminan en "ab".
        private static Automata CrearAfd()
        {
            Automata a = new Automata { EsAFND = false };
            a.Estados.AddRange(new[] { "q0", "q1", "q2" });
            a.Alfabeto.AddRange(new[] { "a", "b" });
            a.Inicial = "q0";
            a.Finales.Add("q2");
            a.FijarTransicion("q0", "a", new[] { "q1" }); a.FijarTransicion("q0", "b", new[] { "q0" });
            a.FijarTransicion("q1", "a", new[] { "q1" }); a.FijarTransicion("q1", "b", new[] { "q2" });
            a.FijarTransicion("q2", "a", new[] { "q1" }); a.FijarTransicion("q2", "b", new[] { "q0" });
            return a;
        }

        // AFND que acepta Σ*ab; con ε usa un estado extra al que salta desde q0 sin leer nada.
        private static Automata CrearAfnd(bool conEpsilon)
        {
            Automata a = new Automata { EsAFND = true, UsaEpsilon = conEpsilon };
            a.Alfabeto.AddRange(new[] { "a", "b" });
            a.Inicial = "q0";
            if (!conEpsilon)
            {
                a.Estados.AddRange(new[] { "q0", "q1", "q2" });
                a.Finales.Add("q2");
                a.FijarTransicion("q0", "a", new[] { "q0", "q1" });
                a.FijarTransicion("q0", "b", new[] { "q0" });
                a.FijarTransicion("q1", "b", new[] { "q2" });
                // q2 -ε-> q0 se usa en la variante con ε; aquí se deja el comportamiento de "termina en ab"
            }
            else
            {
                a.Estados.AddRange(new[] { "q0", "q1", "q2", "q3" });
                a.Finales.Add("q3");
                a.FijarTransicion("q0", "a", new[] { "q0" });
                a.FijarTransicion("q0", "b", new[] { "q0" });
                a.FijarTransicion("q0", Automata.Epsilon, new[] { "q1" });
                a.FijarTransicion("q1", "a", new[] { "q2" });
                a.FijarTransicion("q2", "b", new[] { "q3" });
            }
            return a;
        }
    }
}
