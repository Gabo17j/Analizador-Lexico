using System;
using System.Windows.Forms;
using Analizador_Lexico.Views;

namespace Analizador_Lexico
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
                MessageBox.Show("Ocurrió un error inesperado, pero la aplicación sigue en ejecución:\n\n" + e.Exception.Message,
                    "Error inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Run(new FrmPrincipal());
        }
    }
}
