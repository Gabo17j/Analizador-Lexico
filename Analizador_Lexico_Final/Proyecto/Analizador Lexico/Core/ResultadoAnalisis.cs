using System.Collections.Generic;

namespace Analizador_Lexico.Core
{
    public class ResultadoAnalisis
    {
        public List<Token> Tokens { get; set; } = new List<Token>();
        public List<ElementoSimbolo> Simbolos { get; set; } = new List<ElementoSimbolo>();
        public List<ErrorLexico> Errores { get; set; } = new List<ErrorLexico>();

        public Dictionary<string, int> TokensPorCategoria { get; } = new Dictionary<string, int>();

        public int TotalLineas { get; set; }
        public double TiempoMs { get; set; }

        public int TotalTokens => Tokens.Count;
        public int TotalErrores => Errores.Count;
    }
}
