using System;
using System.Collections.Generic;
using System.Linq;

namespace Analizador_Lexico.Core
{
    public static class PalabrasReservadas
    {
        private static readonly string[] predeterminadas =
        {
            "if", "else", "while", "for", "do", "switch", "case", "default",
            "break", "continue", "return",
            "int", "float", "double", "char", "bool", "string", "void",
            "class", "static", "public", "private", "true", "false"
        };

        private static readonly string[] tiposPredeterminados =
            { "int", "float", "double", "char", "bool", "string" };

        private static List<string> orden = new List<string>();
        private static HashSet<string> reservadas = new HashSet<string>(StringComparer.Ordinal);
        private static HashSet<string> tiposDeDato = new HashSet<string>(StringComparer.Ordinal);

        static PalabrasReservadas()
        {
            Restaurar();
        }

        public static IEnumerable<string> Todas => orden;
        public static int Cantidad => orden.Count;

        public static bool EsReservada(string lexema)
        {
            return reservadas.Contains(lexema);
        }

        public static bool EsTipoDeDato(string lexema)
        {
            return tiposDeDato.Contains(lexema);
        }

        public static IEnumerable<string> PredeterminadasLista => predeterminadas;

        public static List<KeyValuePair<string, bool>> PredeterminadasConTipo()
        {
            return predeterminadas.Select(p => new KeyValuePair<string, bool>(p, tiposPredeterminados.Contains(p))).ToList();
        }

        public static bool EsIdentificadorValido(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            char c0 = s[0];
            if (!((c0 >= 'a' && c0 <= 'z') || (c0 >= 'A' && c0 <= 'Z') || c0 == '_')) return false;
            foreach (char c in s)
                if (!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_'))
                    return false;
            return true;
        }

        public static void Restaurar()
        {
            Establecer(predeterminadas.Select(p => new KeyValuePair<string, bool>(p, tiposPredeterminados.Contains(p))));
        }
        public static void Establecer(IEnumerable<KeyValuePair<string, bool>> palabras)
        {
            List<string> nuevoOrden = new List<string>();
            HashSet<string> nuevasReservadas = new HashSet<string>(StringComparer.Ordinal);
            HashSet<string> nuevosTipos = new HashSet<string>(StringComparer.Ordinal);
            foreach (KeyValuePair<string, bool> kv in palabras)
            {
                string p = (kv.Key ?? "").Trim();
                if (!EsIdentificadorValido(p) || !nuevasReservadas.Add(p)) continue;
                nuevoOrden.Add(p);
                if (kv.Value) nuevosTipos.Add(p);
            }
            orden = nuevoOrden;
            reservadas = nuevasReservadas;
            tiposDeDato = nuevosTipos;
        }

        public static List<KeyValuePair<string, bool>> Copiar()
        {
            return orden.Select(p => new KeyValuePair<string, bool>(p, tiposDeDato.Contains(p))).ToList();
        }
    }
}
