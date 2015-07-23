/**
 * RozWorld.GameSettings -- RozWorld Game Settings Information
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System.Drawing;

namespace RozWorld
{
    public struct GameSettings
    {
        /**
         * The current settings file being used by RozWorld
         */
        public string SettingsFile
        {
            get;
            private set;
        }

        /**
         * The size of the window at startup.
         * 
         * (Not accounting for the aero offset)
         */
        public Size WindowResolution
        {
            get;
            private set;
        }

        /**
         * The currently selected texture pack will have its subdirectory stored
         * here.
         */
        public string TexturePackDirectory
        {
            get;
            private set;
        }

        /**
         * Aero likes to screw up the *true* size of the window, this offset option
         * when enabled should fix the window size and hit detection issues.
         * 
         * (turn off when using a maximised game window)
         */
        public bool AeroOffsets
        {
            get;
            private set;
        }

        /**
         * By default the game disallows the minimum window size to be less than
         * 800x600, this setting makes it so that the minimum window size is the
         * chosen preferred startup size.
         * 
         * (If WindowResolution is 1366x768, the game window will resize itself
         * if the user attempts to shrink it past 1366x768)
         */
        public bool MinimumSizeIsPreferred
        {
            get;
            private set;
        }

        
        /// <summary>
        /// Load or reload the settings for RozWorld, specifying a file to load if necessary.
        /// </summary>
        public void Load(string settingsPath = "")
        {

        }


        /// <summary>
        /// Save the settings for RozWorld, specifying a file to save to if necessary.
        /// </summary>
        /// <param name="settingsPath"></param>
        public void Save(string settingsPath = "")
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowResolution"></param>
        /// <param name="aeroOffsets"></param>
        /// <param name="minimumSizeIsPreferred"></param>
        public void UpdateVideoSettings(Size windowResolution, bool aeroOffsets, bool minimumSizeIsPreferred)
        {
            if (windowResolution.Width >= 800 && windowResolution.Height >= 600)
            {
                WindowResolution = windowResolution;
            }

            AeroOffsets = aeroOffsets;
            MinimumSizeIsPreferred = minimumSizeIsPreferred;

            Save();
        }
    }
}
