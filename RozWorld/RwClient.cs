/**
 * Oddmatics.RozWorld.Client.RwClient -- RozWorld Client Implementation
 *
 * This source-code is part of the client program for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.API.Client;
using Oddmatics.RozWorld.API.Client.Input;
using Oddmatics.RozWorld.API.Client.Interface;
using Oddmatics.RozWorld.API.Generic;
using Oddmatics.Util.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Oddmatics.RozWorld.Client
{
    public class RwClient : IRwClient
    {
        #region Path Constants

        /// <summary>
        /// The renderers directory.
        /// </summary>
        public static string DIRECTORY_RENDERERS = Directory.GetCurrentDirectory() + @"\renderers";

        /// <summary>
        /// The config file for client variables.
        /// </summary>
        public static string FILE_CONFIG = Directory.GetCurrentDirectory() + @"\client.cfg";

        #endregion


        public string ClientName { get { return "Vanilla RozWorld Client"; } }
        public string ClientVersion { get { return "0.01"; } }
        public Dictionary<byte, Size> DisplayResolutions { get; private set; }
        public IInputHandler Input { get { throw new System.NotImplementedException(); } }
        public IInterfaceHandler Interface { get { throw new System.NotImplementedException(); } }
        private ILogger _Logger;
        public ILogger Logger
        {
            get { return _Logger; } set { if (_Logger == null) _Logger = value; }
        }
        public string RozWorldVersion { get { return "0.01"; } }


        private Renderer ActiveRenderer;
        private bool HasStarted;
        private List<Type> Renderers;
        private bool ShouldClose;


        /// <summary>
        /// Loads a client configuration from a file on disk.
        /// </summary>
        /// <param name="file">The configuration file to load from.</param>
        private void LoadConfigs(IList<string> file)
        {
            var configs = FileSystem.ReadINIToDictionary(file);

            foreach (var item in configs)
            {
                switch (item.Key.ToLower())
                {
                    case "display-resolution":
                        string[] splitSettings = item.Value.Split(',');
                        byte displayNumber;
                        Size displayResolution;

                        // Attempt to parse split value
                        if (splitSettings.Length != 3)
                        {
                            Logger.Out("Invalid display-resolution setting encountered: Not enough values.",
                                LogLevel.Error);
                            break;
                        }

                        if (!byte.TryParse(splitSettings[0], out displayNumber) ||
                            !int.TryParse(splitSettings[1], out displayResolution.Width) ||
                            !int.TryParse(splitSettings[2], out displayResolution.Height))
                        {
                            Logger.Out("Invalid display-resolution setting encountered: Wrong type in values.",
                                LogLevel.Error);
                            break;
                        }

                        // Now do the actual setting
                        if (!DisplayResolutions.ContainsKey(displayNumber))
                            DisplayResolutions.Add(displayNumber, displayResolution);
                        else
                            DisplayResolutions[displayNumber] = displayResolution;

                        break;

                    default:
                        Logger.Out("Unknown setting: \"" + item.Key + "\".", LogLevel.Error);
                        break;
                }
            }
        }

        /// <summary>
        /// Writes the default client configuration file to the disk.
        /// </summary>
        /// <param name="targetFile">The target filename to write to.</param>
        private void MakeDefaultConfigs(string targetFile)
        {
            FileSystem.PutTextFile(targetFile, new string[] { Properties.Resources.DefaultConfigs });
        }

        /// <summary>
        /// Runs this client instance.
        /// </summary>
        public bool Run()
        {
            // A logger must be set and this should be set as the current client in RwCore
            if (RwCore.InstanceType != RwInstanceType.ClientOnly && RwCore.InstanceType != RwInstanceType.Both)
                throw new InvalidOperationException("RwClient.Start: This RozWorld instance is not a client.");

            if (RwCore.Client != this)
                throw new InvalidOperationException("RwClient.Start: RwCore.Client must reference this client instance before calling Start().");

            if (Logger == null)
                throw new InvalidOperationException("RwClient.Start: An ILogger instance must be attached before calling Start().");

            if (HasStarted)
                throw new InvalidOperationException("RwClient.Start: Client is already started.");

            Logger.Out("RozWorld client starting...", LogLevel.Info);
            Logger.Out("Initialising directories...", LogLevel.Info);

            FileSystem.MakeDirectory(DIRECTORY_RENDERERS);

            // Load configs
            Logger.Out("Setting configs...", LogLevel.Info);

            if (!File.Exists(FILE_CONFIG))
                MakeDefaultConfigs(FILE_CONFIG);

            DisplayResolutions = new Dictionary<byte, Size>();

            // Load defaults first then load the file on disk
            LoadConfigs(Properties.Resources.DefaultConfigs.Split('\n'));
            LoadConfigs(FileSystem.GetTextFile(FILE_CONFIG));

            // Load renderers
            Logger.Out("Loading renderers...", LogLevel.Info);

            Renderers = new List<Type>();

            foreach (string file in Directory.GetFiles(DIRECTORY_RENDERERS))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    Type[] detectedObjects = assembly.GetTypes();

                    foreach (var detectedObject in detectedObjects)
                    {
                        if (detectedObject.BaseType == typeof(Renderer))
                            Renderers.Add(detectedObject);
                    }
                }
                catch (ReflectionTypeLoadException reflectionEx)
                {
                    Logger.Out("An error occurred trying to enumerate the types inside of the library \"" +
                        Path.GetFileName(file) + "\", renderers from this library cannot be loaded. It may " +
                        "have been built with a different version of the RozWorld API.", LogLevel.Error);
                }
                catch (Exception ex)
                {
                    Logger.Out("An error occurred trying to load library \"" + Path.GetFileName(file) +
                        "\", renderers from this library cannot be loaded. The exception that occurred " +
                        "reported the following:\n" + ex.Message + "\nStack:\n" + ex.StackTrace, LogLevel.Error);
                }
            }

            // TODO: Replace this bit so that renderer choice will be loaded from configs
            if (Renderers.Count == 0)
            {
                Logger.Out("No renderers were loaded! Cannot continue.", LogLevel.Fatal);
                return false;
            }

            ActiveRenderer = (Renderer)Activator.CreateInstance(Renderers[0]);
            ActiveRenderer.Initialise();
            //////////////////////////////////////////////////////////////////////////////

            // Load the rest and then start/run the game
            ShouldClose = false;
            ActiveRenderer.Start();


            // Wait until the game should close or is manually
            while (!ShouldClose) { };

            return true;
        }
    }
}
