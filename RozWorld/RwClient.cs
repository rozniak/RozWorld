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
using System.Timers;

namespace Oddmatics.RozWorld.Client
{
    /// <summary>
    /// Represents the RozWorld client.
    /// </summary>
    public class RwClient : IRwClient
    {
        /// <summary>
        /// Gets the nice name of the client.
        /// </summary>
        public string ClientName { get { return "Vanilla RozWorld Client"; } }

        /// <summary>
        /// Gets the version string of the client.
        /// </summary>
        public string ClientVersion { get { return "0.01"; } }

        /// <summary>
        /// Gets the window title used within the client.
        /// </summary>
        public string ClientWindowTitle { get { return "RozWorld"; } }

        /// <summary>
        /// Gets the display resolution of each individual display.
        /// </summary>
        public Dictionary<byte, Size> DisplayResolutions { get; private set; }

        /// <summary>
        /// Gets the input handler of the client..
        /// </summary>
        public IInputHandler Input { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets the interface handler of the client.
        /// </summary>
        public IInterfaceHandler Interface { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets the logger of the client.
        /// </summary>
        public ILogger Logger
        {
            get { return _Logger; } set { if (_Logger == null) _Logger = value; }
        }
        private ILogger _Logger;

        /// <summary>
        /// Gets the version of RozWorld that this client targets.
        /// </summary>
        public string RozWorldVersion { get { return "0.01"; } }


        /// <summary>
        /// The active Renderer object.
        /// </summary>
        private Renderer ActiveRenderer;

        /// <summary>
        /// The full name of the chosen renderer.
        /// </summary>
        private string ChosenRenderer;

        /// <summary>
        /// The client tickrate timer.
        /// </summary>
        private Timer ClientUpdateTimer;
        
        /// <summary>
        /// The value that represents whether the client has been started.
        /// </summary>
        private bool HasStarted;

        /// <summary>
        /// The renderers detected and available to the client.
        /// </summary>
        private Dictionary<string, Type> Renderers;

        /// <summary>
        /// The value that represents whether the client should close.
        /// </summary>
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

                    case "renderer":
                        // TODO: Possibly add more verif here in future
                        ChosenRenderer = item.Value;

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

            FileSystem.MakeDirectory(RwClientParameters.RendererPath);

            // Load configs
            Logger.Out("Setting configs...", LogLevel.Info);

            if (!File.Exists(RwClientParameters.ConfigurationPath))
                MakeDefaultConfigs(RwClientParameters.ConfigurationPath);

            DisplayResolutions = new Dictionary<byte, Size>();

            // Load defaults first then load the file on disk
            LoadConfigs(Properties.Resources.DefaultConfigs.Split('\n'));
            LoadConfigs(FileSystem.GetTextFile(RwClientParameters.ConfigurationPath));

            // Load renderers
            Logger.Out("Loading renderers...", LogLevel.Info);

            Renderers = new Dictionary<string, Type>();
            string lastRenderer = string.Empty;
            var availableRenderers = new List<string>(); // For use when we try to launch

            foreach (string file in Directory.GetFiles(RwClientParameters.RendererPath))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    Type[] detectedObjects = assembly.GetTypes();

                    foreach (var detectedObject in detectedObjects)
                    {
                        if (detectedObject.BaseType == typeof(Renderer))
                        {
                            availableRenderers.Add(detectedObject.FullName);
                            Renderers.Add(detectedObject.FullName, detectedObject);
                            lastRenderer = detectedObject.FullName;
                        }
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

            if (Renderers.Count == 0)
            {
                Logger.Out("No renderers were loaded! Cannot continue.", LogLevel.Fatal);
                return false;
            }


            // Attempt to launch the renderer
            bool successfulLaunch = false;

            // Use renderer from configs
            if (Renderers.ContainsKey(ChosenRenderer))
                ActiveRenderer = (Renderer)Activator.CreateInstance(Renderers[ChosenRenderer]);
            else
                ActiveRenderer = (Renderer)Activator.CreateInstance(Renderers[lastRenderer]);

            while (!successfulLaunch && availableRenderers.Count > 0)
            {
                if (!ActiveRenderer.Initialise()) // If renderer fails to start
                {
                    availableRenderers.Remove(ActiveRenderer.GetType().FullName);

                    if (availableRenderers.Count > 0)
                        ActiveRenderer = (Renderer)Activator.CreateInstance(Renderers[availableRenderers[0]]);
                }
                else
                    successfulLaunch = true;
            }

            if (successfulLaunch)
            {
                // Load the rest and then start/run the game
                ShouldClose = false;

                ActiveRenderer.Closed += new EventHandler(ActiveRenderer_Closed);
                ActiveRenderer.Start();

                ClientUpdateTimer = new Timer(1000 / 150); // 150FPS tickrate
                ClientUpdateTimer.Elapsed += new ElapsedEventHandler(ClientUpdateTimer_Elapsed);
                ClientUpdateTimer.Enabled = true;
                ClientUpdateTimer.Start();


                // Wait until the game should close or is manually
                while (!ShouldClose) { };

                return true;
            }

            return false; // Failed to launch renderer
        }


        /// <summary>
        /// [Event] Active renderer closed.
        /// </summary>
        private void ActiveRenderer_Closed(object sender, EventArgs e)
        {
            // TODO: Handle closing safely! (eg. send disconnect packets/close world nicely if on local)
            ClientUpdateTimer.Stop();
            ShouldClose = true;
        }

        /// <summary>
        /// [Event] Client update timer elapsed.
        /// </summary>
        private void ClientUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // TODO: Handle engine events here
        }
    }
}
