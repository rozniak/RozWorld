/**
 * RozWorld.Program -- RozWorld Main Program
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.IO;
using System.Windows.Forms;

using RozWorld.COMFY;
using RozWorld.Graphics;
using RozWorld.Graphics.UI;
using RozWorld.IO;
using RozWorld.Network;

namespace RozWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start initialising game content and environment
            RozWorld.GameStatus = Status.StartingUp;
            
            Initialise();
        }


        private static void Initialise()
        {
            Files.SetupGameDirectories();
            RozWorld.GameContent = ComfyInterpreter.Load();

            // Graphic initialisation here
            RozWorld.GameWindow = new GameWindow();

            // Check when the game window/textures have successfully loaded or not
            do
            {
                // Wait
            } while (RozWorld.GameStatus == Status.StartingUp);

            //Console.ReadKey(true);
        }
    }
}
