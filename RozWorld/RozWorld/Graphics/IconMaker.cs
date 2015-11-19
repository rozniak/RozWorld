using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace RozWorld.Graphics
{
    public static class IconMaker
    {
        /**
         * This code is a pick 'n' mix of some stuff found from various answers on StackOverflow.
         * 
         * Yes, setting the window icon REALLY needs to be this stupid.
         */

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int message, int wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        static extern IntPtr GetFocus();

        private const int WM_SETICON = 0x80;
        private const int ICON_SMALL = 0;
        private const int ICON_BIG = 1;


        public static void SetRozWorldIcon()
        {
            if (File.Exists(Environment.CurrentDirectory + "\\roz.ico"))
            {
                Process gameProcess = Process.GetCurrentProcess();
                Icon gameIcon = new Icon(Environment.CurrentDirectory + "\\roz.ico");

                // Set the 'BIG' and 'SMALL' icons or something
                SendMessage(gameProcess.MainWindowHandle, WM_SETICON, ICON_BIG, gameIcon.Handle);
                SendMessage(gameProcess.MainWindowHandle, WM_SETICON, ICON_SMALL, gameIcon.Handle);
            }
        }


        public static bool HasFocus()
        {
            Process gameProcess = Process.GetCurrentProcess();
            return gameProcess.MainWindowHandle.ToInt32() == GetFocus().ToInt32();
        }
    }
}
