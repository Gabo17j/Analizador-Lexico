namespace Analizador_Lexico.Core
{
    public class Token
    {
        public string Tipo { get; }
        public string Lexema { get; }
        public int Linea { get; }
        public int Columna { get; }

        public string Categoria => TiposToken.Categoria(Tipo);

        public Token(string tipo, string lexema, int linea, int columna)
        {
            Tipo = tipo;
            Lexema = lexema;
            Linea = linea;
            Columna = columna;
        }

        public override string ToString()
        {
            return Tipo + " '" + Lexema + "' (" + Linea + "," + Columna + ")";
        }
    }
}
