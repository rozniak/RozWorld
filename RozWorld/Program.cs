/**
 * Oddmatics.RozWorld.Client.Program -- RozWorld Desktop Version Entry-Point
 *
 * This source-code is part of the client program for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.API.Generic;
using System;

namespace Oddmatics.RozWorld.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RwClient();

            client.Logger = new GameLogger();
            RwCore.Client = client;

            if (client.Run())
                client.Logger.Out("Game ran successfully.", LogLevel.Info);
            else
                client.Logger.Out("Game failed to run successfully! Review the log for any errors.",
                    LogLevel.Info);

            Console.ReadKey(true); // Temporary
        }
    }

    internal sealed class GameLogger : ILogger
    {
        public void Out(string message, LogLevel level, bool colours = true)
        {
            Console.WriteLine("[" + DateTime.Now.ToString() + "] " + level + ":  " + message);
        }
    }
}
