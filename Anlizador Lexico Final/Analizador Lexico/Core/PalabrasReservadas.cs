using System;
using System.Collections.Generic;

namespace Analizador_Lexico.Core
{
    public static class PalabrasReservadas
    {
        private static readonly HashSet<string> reservadas = new HashSet<string>(
            new[]
            {
                "if", "else", "while", "for", "do", "switch", "case", "default",
                "break", "continue", "return",
                "int", "float", "double", "char", "bool", "string", "void",
                "class", "static", "public", "private", "true", "false"
            },
            StringComparer.Ordinal);

        private static readonly HashSet<string> tiposDeDato = new HashSet<string>(
            new[] { "int", "float", "double", "char", "bool", "string" },
            StringComparer.Ordinal);

        public static IEnumerable<string> Todas => reservadas;

        public static bool EsReservada(string lexema)
        {
            return reservadas.Contains(lexema);
        }

        public static bool EsTipoDeDato(string lexema)
        {
            return tiposDeDato.Contains(lexema);
        }
    }
}
