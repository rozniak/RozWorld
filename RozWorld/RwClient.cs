/**
 * Oddmatics.RozWorld.Client.RwClient -- RozWorld Client Implementation
 *
 * This source-code is part of the client program for the RozWorld project by Rory Fewell (rozniak) of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Newtonsoft.Json;
using Oddmatics.RozWorld.API.Client;
using Oddmatics.RozWorld.API.Client.Graphics;
using Oddmatics.RozWorld.API.Client.Input;
using Oddmatics.RozWorld.API.Client.Interface;
using Oddmatics.RozWorld.API.Generic;
using Oddmatics.RozWorld.Client.Game;
using Oddmatics.Util.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
        public string ClientName
        {
            get { return "Vanilla RozWorld Client"; }
        }

        /// <summary>
        /// Gets the version string of the client.
        /// </summary>
        public string ClientVersion
        {
            get { return "0.01"; }
        }

        /// <summary>
        /// Gets the window title used within the client.
        /// </summary>
        public string ClientWindowTitle
        {
            get { return "RozWorld"; }
        }

        /// <summary>
        /// Gets the display resolutions of screens that have been configured.
        /// </summary>
        public Dictionary<byte, RwSize> DisplayResolutions
        {
            get { return Configuration.DisplayResolutions;  }
        }

        /// <summary>
        /// Gets the input handler of the client..
        /// </summary>
        public IInputHandler Input
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the interface handler of the client.
        /// </summary>
        public IInterfaceHandler Interface
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the logger of the client.
        /// </summary>
        public ILogger Logger
        {
            get { return _Logger; }
            set
            {
                if (_Logger == null)
                    _Logger = value;
            }
        }
        private ILogger _Logger;

        /// <summary>
        /// Gets the version of RozWorld that this client targets.
        /// </summary>
        public string RozWorldVersion
        {
            get { return "0.01"; }
        }

        /// <summary>
        /// Gets the root directory that relative texture paths stem from.
        /// </summary>
        public string TexturesRoot
        {
            get { return Environment.CurrentDirectory + @"\textures\" + Configuration.TexturePack + @"\"; }
        }
        

        /// <summary>
        /// The active Renderer object.
        /// </summary>
        private Renderer ActiveRenderer;

        /// <summary>
        /// The client configuration.
        /// </summary>
        private RwClientConfiguration Configuration { get; set; }

        /// <summary>
        /// The RozWorld game instance.
        /// </summary>
        private RwGame Game { get; set; }
        
        /// <summary>
        /// The value that represents whether the client has been started.
        /// </summary>
        private bool HasStarted { get; set; }

        /// <summary>
        /// The renderers detected and available to the client.
        /// </summary>
        private Dictionary<string, Type> Renderers { get; set; }

        /// <summary>
        /// The value that represents whether the client should close.
        /// </summary>
        private bool ShouldClose { get; set; }
        

        /// <summary>
        /// Runs this client instance.
        /// </summary>
        public bool Run()
        {
            if (RwCore.InstanceType != RwInstanceType.ClientOnly && RwCore.InstanceType != RwInstanceType.Both)
                throw new InvalidOperationException("RwClient.Start: This RozWorld instance is not a client.");

            if (RwCore.Client != this)
                throw new InvalidOperationException("RwClient.Start: RwCore.Client must reference this client instance before calling Start().");

            if (Logger == null)
                throw new InvalidOperationException("RwClient.Start: An ILogger instance must be attached before calling Start().");

            if (HasStarted)
                throw new InvalidOperationException("RwClient.Start: Client is already started.");

            HasStarted = true;

            Logger.Out("RozWorld client starting...", LogLevel.Info);

            InitializeDirectories();
            LoadConfigs();
            LoadRenderers();
            

            if (SelectRenderer(Configuration.ChosenRenderer))
            {
                // Initialize game instance
                Game = new RwGame();
                
                
                // Load the rest and then start/run the game
                ShouldClose = false;

                ActiveRenderer.Closed += new EventHandler(ActiveRenderer_Closed);
                ActiveRenderer.Start();

                // Wait until the game should close or is manually
                while (!ShouldClose) { };

                // Write configs to disk
                File.WriteAllText(
                    RwClientParameters.ConfigurationPath,
                    JsonConvert.SerializeObject(Configuration)
                    );

                return true;
            }

            HasStarted = false;

            return false; // Failed to launch renderer
        }


        /// <summary>
        /// Ensures that any required directories are created.
        /// </summary>
        private void InitializeDirectories()
        {
            Logger.Out("Initialising directories...", LogLevel.Info);

            FileSystem.MakeDirectory(RwClientParameters.RendererPath);
        }

        /// <summary>
        /// Loads the configurations from the file on disk.
        /// </summary>
        private void LoadConfigs()
        {
            // Load configs
            Logger.Out("Setting configs...", LogLevel.Info);

            if (!File.Exists(RwClientParameters.ConfigurationPath))
            {
                File.WriteAllText(
                    RwClientParameters.ConfigurationPath,
                    Properties.Resources.DefaultConfigs
                    );
            }

            // Load defaults first then load the file on disk
            Configuration = new RwClientConfiguration();

            if (File.Exists(RwClientParameters.ConfigurationPath))
            {
                try
                {
                    JsonConvert.PopulateObject(
                        File.ReadAllText(RwClientParameters.ConfigurationPath),
                        Configuration
                        );
                }
                catch (JsonReaderException jsonEx)
                {
                    Logger.Out("Bad configuration JSON, defaults will be used instead.", LogLevel.Warning);
                    Logger.Out("JsonReaderException: " + jsonEx.Message, LogLevel.Error);
                }
            }
        }

        /// <summary>
        /// Loads available renderers from libraries on disk.
        /// </summary>
        private void LoadRenderers()
        {
            // Do not allow renderers to be reloaded!
            if (Renderers != null)
                throw new InvalidOperationException("RwClient.LoadRenderers: Cannot load renderers after they have already been loaded.");

            // Load renderers
            Logger.Out("Loading renderers...", LogLevel.Info);

            Renderers = new Dictionary<string, Type>();

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
                            Renderers.Add(detectedObject.FullName, detectedObject);
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
                throw new InvalidOperationException("RwClient.LoadRenderers: No renderers were loaded, cannot continue in this state.");
            }
        }

        /// <summary>
        /// Attempts to select the specified renderer.
        /// </summary>
        /// <param name="rendererAssemblyName">The assembly name of the renderer to load.</param>
        /// <returns>True if a renderer was successfully loaded.</returns>
        private bool SelectRenderer(string rendererAssemblyName)
        {
            // Configuration.ChosenRenderer
            if (ActiveRenderer != null)
            {
                ActiveRenderer.Closed -= ActiveRenderer_Closed;
                ActiveRenderer.Stop();
            }

            var availableRenderers = new List<Type>(Renderers.Values);

            // Find the renderer object first
            if (Renderers.ContainsKey(rendererAssemblyName))
                ActiveRenderer = (Renderer)Activator.CreateInstance(Renderers[rendererAssemblyName]);
            else
                Logger.Out("Unknown renderer '" + Configuration.ChosenRenderer + "'.", LogLevel.Error);

            while (availableRenderers.Count > 0)
            {
                if (!ActiveRenderer.Initialise()) // If renderer fails to start
                {
                    availableRenderers.Remove(ActiveRenderer.GetType());

                    if (availableRenderers.Count > 0)
                        ActiveRenderer = (Renderer)Activator.CreateInstance(availableRenderers[0]);
                }
                else
                    return true;
            }

            return false;
        }


        /// <summary>
        /// [Event] Active renderer closed.
        /// </summary>
        private void ActiveRenderer_Closed(object sender, EventArgs e)
        {
            // TODO: Check if instance is running first
            Game.Stop();
            ShouldClose = true;
        }
    }
}
