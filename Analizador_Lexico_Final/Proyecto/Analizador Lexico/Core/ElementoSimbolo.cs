using System.Collections.Generic;

namespace Analizador_Lexico.Core
{
    public class ElementoSimbolo
    {
        public const string SinEspecificar = "Sin Especificar";

        public int Id { get; }
        public string Lexema { get; }
        public string TipoToken { get; }
        public string TipoDato { get; set; }
        public int LineaPrimera { get; }
        public int ColumnaPrimera { get; }
        public int Frecuencia { get; set; }

        public List<int> Referencias { get; } = new List<int>();

        public ElementoSimbolo(int id, string lexema, string tipoDato, int linea, int columna)
        {
            Id = id;
            Lexema = lexema;
            TipoToken = TiposToken.Identificador;
            TipoDato = tipoDato ?? SinEspecificar;
            LineaPrimera = linea;
            ColumnaPrimera = columna;
            Frecuencia = 1;
        }
    }
}
