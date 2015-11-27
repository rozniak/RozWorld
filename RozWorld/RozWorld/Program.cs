/**
 * RozWorld.Program -- RozWorld Main Program
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.COMFY;
using RozWorld.Graphics;
using RozWorld.Graphics.UI;
using RozWorld.IO;
using RozWorld.Network;

using System;
using System.IO;
using System.Windows.Forms;


namespace RozWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start initialising game content and environment
            RozWorld.GameStatus = Status.StartingUp;

            Files.SetupGameDirectories();
            RozWorld.Content = ComfyInterpreter.Load();
            RozWorld.Settings.Load();

            // Start the game, events will stem from this window
            RozWorld.MainWindow = new GameWindow();
        }
    }
}
