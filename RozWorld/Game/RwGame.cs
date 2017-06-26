/**
 * Oddmatics.RozWorld.Client.RwGame -- RozWorld Client Game Instance
 *
 * This source-code is part of the client program for the RozWorld project by Rory Fewell (rozniak) of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System.Timers;

namespace Oddmatics.RozWorld.Client.Game
{
    /// <summary>
    /// Represents the locally running RozWorld game instance.
    /// </summary>
    internal class RwGame
    {
        /// <summary>
        /// The internal game tickrate.
        /// </summary>
        private Timer TickRate { get; set; }


        /// <summary>
        /// Initializes a new instance of the RwGame class.
        /// </summary>
        public RwGame()
        {
            TickRate = new Timer(1000 / 150); // 150FPS tickrate
            TickRate.Elapsed += TickRate_Elapsed;
            TickRate.Enabled = true;
            TickRate.Start();
            // TODO: Initialize here
        }


        /// <summary>
        /// Stops the running game instance.
        /// </summary>
        public void Stop()
        {
            TickRate.Stop();
            
            // TODO: Handle other game close events here
        }


        /// <summary>
        /// [Event] Client tickrate timer elapsed.
        /// </summary>
        private void TickRate_Elapsed(object sender, ElapsedEventArgs e)
        {
            // TODO: Handle engine events here
        }
    }
}
