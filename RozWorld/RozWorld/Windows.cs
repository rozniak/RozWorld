/**
 * RozWorld.Windows -- Ugly Windows Specific Things
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace RozWorld.Graphics
{
    public static class Windows
    {
        /**
         * This is all Windows specific stuff. Put simply, it's disgusting.
         */

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int message, int wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        static extern IntPtr GetFocus();

        private const int WM_SETICON = 0x80;
        private const int ICON_SMALL = 0;
        private const int ICON_BIG = 1;


        /// <summary>
        /// Set the RozWorld icon to the main game window.
        /// </summary>
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


        /// <summary>
        /// Gets whether RozWorld's main game window has focus or not.
        /// </summary>
        /// <returns>Whether RozWorld's main game window has focus or not.</returns>
        public static bool GameHasFocus()
        {
            Process gameProcess = Process.GetCurrentProcess();
            return gameProcess.MainWindowHandle.ToInt32() == GetFocus().ToInt32();
        }
    }
}
