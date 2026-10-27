using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Analizador_Lexico.Views
{
    internal static class Nativos
    {
        public const int WM_SETREDRAW = 0x000B;
        public const int EM_LINESCROLL = 0x00B6;
        public const int EM_GETFIRSTVISIBLELINE = 0x00CE;
        public const int EM_GETSCROLLPOS = 0x0400 + 221;
        public const int EM_SETSCROLLPOS = 0x0400 + 222;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref Point lParam);
    }
}
