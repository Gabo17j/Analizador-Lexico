namespace Analizador_Lexico.Core
{
    public static class TiposToken
    {
        public const string PalabraReservada = "TK_PR_PALABRA";
        public const string Identificador = "TK_IDENTIFICADOR";
        public const string NumEntero = "TK_NUM_ENTERO";
        public const string NumDecimal = "TK_NUM_DECIMAL";
        public const string Cadena = "TK_CADENA";
        public const string Caracter = "TK_CARACTER";

        public const string OpSuma = "TK_OP_SUMA";
        public const string OpResta = "TK_OP_RESTA";
        public const string OpMult = "TK_OP_MULT";
        public const string OpDiv = "TK_OP_DIV";
        public const string OpModulo = "TK_OP_MODULO";
        public const string OpInc = "TK_OP_INC";
        public const string OpDec = "TK_OP_DEC";

        public const string OpIgualdad = "TK_OP_IGUALDAD";
        public const string OpDiferente = "TK_OP_DIFERENTE";
        public const string OpMenor = "TK_OP_MENOR";
        public const string OpMayor = "TK_OP_MAYOR";
        public const string OpMenorIgual = "TK_OP_MENOR_IGUAL";
        public const string OpMayorIgual = "TK_OP_MAYOR_IGUAL";

        public const string OpAnd = "TK_OP_AND";
        public const string OpOr = "TK_OP_OR";
        public const string OpNot = "TK_OP_NOT";

        public const string OpAsignacion = "TK_OP_ASIGNACION";
        public const string OpAsigSuma = "TK_OP_ASIG_SUMA";
        public const string OpAsigResta = "TK_OP_ASIG_RESTA";
        public const string OpAsigMult = "TK_OP_ASIG_MULT";
        public const string OpAsigDiv = "TK_OP_ASIG_DIV";

        public const string ParIzq = "TK_PAR_IZQ";
        public const string ParDer = "TK_PAR_DER";
        public const string CorcheteIzq = "TK_CORCHETE_IZQ";
        public const string CorcheteDer = "TK_CORCHETE_DER";
        public const string LlaveIzq = "TK_LLAVE_IZQ";
        public const string LlaveDer = "TK_LLAVE_DER";
        public const string PuntoComa = "TK_PUNTO_COMA";
        public const string Coma = "TK_COMA";
        public const string Punto = "TK_PUNTO";
        public const string DosPuntos = "TK_DOS_PUNTOS";

        public const string ComLinea = "TK_COM_LINEA";
        public const string ComBloque = "TK_COM_BLOQUE";

        public static string Categoria(string tipo)
        {
            switch (tipo)
            {
                case PalabraReservada: return "Palabra Reservada";
                case Identificador: return "Identificador";
                case NumEntero: return "Número Entero";
                case NumDecimal: return "Número Decimal";
                case Cadena: return "Literal Cadena";
                case Caracter: return "Literal Carácter";
                case OpSuma:
                case OpResta:
                case OpMult:
                case OpDiv:
                case OpModulo:
                case OpInc:
                case OpDec:
                    return "Op. Aritmético";
                case OpIgualdad:
                case OpDiferente:
                case OpMenor:
                case OpMayor:
                case OpMenorIgual:
                case OpMayorIgual:
                    return "Op. Relacional";
                case OpAnd:
                case OpOr:
                case OpNot:
                    return "Op. Lógico";
                case OpAsignacion:
                case OpAsigSuma:
                case OpAsigResta:
                case OpAsigMult:
                case OpAsigDiv:
                    return "Op. Asignación";
                case ParIzq:
                case ParDer:
                case CorcheteIzq:
                case CorcheteDer:
                case LlaveIzq:
                case LlaveDer:
                case PuntoComa:
                case Coma:
                case Punto:
                case DosPuntos:
                    return "Delimitador";
                case ComLinea:
                case ComBloque:
                    return "Comentario";
                default: return "Desconocido";
            }
        }
    }
}
