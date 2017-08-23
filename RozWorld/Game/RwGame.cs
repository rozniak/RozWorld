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

using Oddmatics.RozWorld.API.Generic;
using Oddmatics.RozWorld.API.Generic.Event;
using System;

namespace Oddmatics.RozWorld.Client.Game
{
    /// <summary>
    /// Represents the locally running RozWorld game instance.
    /// </summary>
    internal class RwGame : IRwGame
    {
        /// <summary>
        /// Occurs when the logic update portion of the main game loop is reached.
        /// </summary>
        public event GameUpdateEventHandler Updated;


        /// <summary>
        /// Initializes a new instance of the RwGame class.
        /// </summary>
        public RwGame()
        {
            // TODO: Initialize here
        }


        /// <summary>
        /// Invokes a logic update.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update</param>
        public void InvokeUpdate(TimeSpan deltaTime)
        {
            // TODO: Anything else that needs updating here - user input etc.

            Updated?.Invoke(this, new GameUpdateEventArgs(deltaTime));
        }

        /// <summary>
        /// Stops the running game instance.
        /// </summary>
        public void Stop()
        {
            // TODO: Handle other game close events here
        }
    }
}
