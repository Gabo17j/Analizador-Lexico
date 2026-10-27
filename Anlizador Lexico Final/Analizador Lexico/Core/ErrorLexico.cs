namespace Analizador_Lexico.Core
{
    public class ErrorLexico
    {
        public string Lexema { get; }
        public string Descripcion { get; }
        public int Linea { get; }
        public int Columna { get; }
        public int Regla { get; }

        public ErrorLexico(string lexema, string descripcion, int linea, int columna, int regla)
        {
            Lexema = lexema;
            Descripcion = descripcion;
            Linea = linea;
            Columna = columna;
            Regla = regla;
        }

        public override string ToString()
        {
            return "[Regla " + Regla + "] (" + Linea + "," + Columna + ") '" + Lexema + "': " + Descripcion;
        }
    }
}
