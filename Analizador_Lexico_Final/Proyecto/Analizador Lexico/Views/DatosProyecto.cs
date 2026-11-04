namespace Analizador_Lexico.Views
{
    /// <summary>
    /// Datos que se muestran en la cabecera del módulo de autómatas (los pide la rúbrica del video:
    /// nombre del proyecto, estudiante, carné y fecha de creación). Modifique estos valores si
    /// otro integrante graba su propio video.
    /// </summary>
    internal static class DatosProyecto
    {
        public const string Proyecto = "Analizador Léxico — Autómatas AFD y AFND";
        public const string Estudiante = "Elvis Steve Xoná Nolasco";
        public const string Carne = "202345156";
        public const string FechaCreacion = "03/11/2026";

        public static string Resumen
        {
            get
            {
                return "Proyecto: " + Proyecto + "     |     Estudiante: " + Estudiante
                     + "     |     Carné: " + Carne + "     |     Fecha de creación: " + FechaCreacion;
            }
        }
    }
}
