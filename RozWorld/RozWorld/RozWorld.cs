/**
 * RozWorld.RozWorld -- RozWorld Game Globals
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.COMFY;
using RozWorld.Graphics;
using RozWorld.Network;

namespace RozWorld
{
    public static class RozWorld
    {
        public const bool SHOW_VERSION_STRING = true;
        public const string VERSION_STRING = "RozWorld private status build Jun 04 2015 ";

        public static RozClient GameClient;
        public static RozServer GameServer;
        public static GameWindow GameWindow;
        public static ComfyContent GameContent;

        private static Status _GameStatus;
        public static Status GameStatus
        {
            get
            {
                return _GameStatus;
            }

            set
            {
                // Call to client plugins for status change

                _GameStatus = value;
            }
        }


        /// <summary>
        /// Makes calls to plugins to inform of a status change.
        /// </summary>
        private static void StatusChange()
        {
            // insert calls here.
        }
    }
}
