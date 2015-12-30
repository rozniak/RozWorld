/**
 * RozWorld.RozWorld -- RozWorld Game Globals
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.Graphics;
using RozWorld.Graphics.UI;
using RozWorld.Graphics.UI.Geometry;
using RozWorld.Graphics.UI.Strings;
using RozWorld.Network;
using RozWorld.IO;


namespace RozWorld
{
    public static class RozWorld
    {
        public const bool SHOW_VERSION_STRING = true;
        public const string VERSION_STRING = "RozWorld Progress Build";

        public static RozClient Client;
        public static RozServer Server;
        public static GameWindow MainWindow;
        public static GameSettings Settings;

        public static TextureManager Textures;
        public static GUIOMETRY InterfaceGeometry;
        public static LanguageSystem Languages;
        private static bool LoadedResources;

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
        /// Loads RozWorld's resources if they haven't already been loaded.
        /// </summary>
        public static void LoadResources()
        {
            if (!LoadedResources)
            {
                // Set up the texture manager
                Textures = new TextureManager();
                Files.TexturePackSubFolder = Settings.TexturePackDirectory;
                Textures.LoadFontSources();
                Textures.LoadTextures();

                // Set up other resources
                InterfaceGeometry = new GUIOMETRY();
                InterfaceGeometry.Load();
                Languages = new LanguageSystem();
                Languages.Load(RozWorld.Settings.LanguageSource);
                FontProvider.Load(RozWorld.Textures, RozWorld.InterfaceGeometry);

                LoadedResources = true;
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
