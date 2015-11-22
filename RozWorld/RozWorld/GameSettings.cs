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

using RozWorld.Graphics.UI;
using RozWorld.IO;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


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
         * The INI file for the game settings, in dictionary form.
         */
        private Dictionary<string, string> SettingsINI;

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
            if (settingsPath == "")
            {
                settingsPath = Environment.CurrentDirectory + @"\game.ini";

                if (!File.Exists(settingsPath))
                {
                    GenerateDefaultSettings();
                }
            }

            try
            {
                Dictionary<string, string> settingsDictionary = Files.ReadINIToDictionary(settingsPath);

                int windowWidth = Convert.ToInt32(settingsDictionary["WindowWidth"]);
                int windowHeight = Convert.ToInt32(settingsDictionary["WindowHeight"]);

                WindowResolution = new Size(windowWidth, windowHeight);

                TexturePackDirectory = settingsDictionary["TextureDirectory"];

                AeroOffsets = Convert.ToBoolean(settingsDictionary["AeroOffsets"]);
                MinimumSizeIsPreferred = Convert.ToBoolean(settingsDictionary["MinimumSizeIsPreferred"]);
            }
            catch (KeyNotFoundException ex)
            {
                UIHandler.CriticalError(Error.MISSING_INI_DICTIONARY_KEY, "Failed to fully load the game settings; a default settings file has been generated in the game directory, you may replace the current broken one, or use it as a reference to fix it.");
            }
            catch
            {
                UIHandler.CriticalError(Error.UNKNOWN_ERROR, "Failed to fully load the game settings; a default settings file has been generated in the game directory, you may replace the current broken one, or use it as a reference to fix it.");
            }
        }


        /// <summary>
        /// Save the settings for RozWorld, specifying a file to save to if necessary.
        /// </summary>
        /// <param name="settingsPath">Specify the path to save the settings to.</param>
        public void Save(string settingsPath = "")
        {
            string[] settingsINIFile = new string[] {
                "# RozWorld game settings",
                "# -",
                "# [VideoSettings]",
                "WindowWidth:" + WindowResolution.Width.ToString(),
                "WindowHeight:" + WindowResolution.Height.ToString(),
                "TextureDirectory:" + TexturePackDirectory,
                "AeroOffsets:" + AeroOffsets.ToString(),
                "MinimumSizeIsPreferred:" + MinimumSizeIsPreferred.ToString(),
                "# -",
                "# [Language]"
            };

            if (settingsPath == "")
            {
                Files.PutTextFile(Environment.CurrentDirectory + @"\game.ini", settingsINIFile);
            }
            else
            {
                Files.PutTextFile(settingsPath, settingsINIFile);
            }
        }


        /// <summary>
        /// Generates a default settings file in the local game directory.
        /// </summary>
        /// <param name="asMainSettings">Specify whether this should generate the main settings file for RozWorld.</param>
        public void GenerateDefaultSettings(bool asMainSettings = false)
        {
            // This is the game's default INI file, this will be generated on first run/and load errors
            string[] defaultINIFile = new string[] {
                "# RozWorld default configuration file",
                "# -",
                "# [VideoSettings]",
                "WindowWidth:800",
                "WindowHeight:600",
                "TextureDirectory:roz",
                "AeroOffsets:false",
                "MinimumSizeIsPreferred:true"
            };

            if (asMainSettings)
            {
                Files.PutTextFile(Environment.CurrentDirectory + @"\game.ini", defaultINIFile);
            }
            else
            {
                Files.PutTextFile(Environment.CurrentDirectory + @"\default-configuration.ini", defaultINIFile);
            }
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
