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
using Oddmatics.RozWorld.API.Client.Window;
using Oddmatics.RozWorld.API.Generic;
using Oddmatics.Util.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        /// Gets the display resolutions of screens that have been configured.
        /// </summary>
        public Dictionary<byte, RwSize> DisplayResolutions
        {
            get { return Configuration.DisplayResolutions;  }
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
        public string RozWorldVersion { get { return "0.01"; } }
        

        /// <summary>
        /// The active window manager.
        /// </summary>
        private IWindowManager ActiveWindowManager;

        /// <summary>
        /// The client configuration.
        /// </summary>
        private RwClientConfiguration Configuration { get; set; }
        
        /// <summary>
        /// The value that represents whether the client has been started.
        /// </summary>
        private bool HasStarted { get; set; }

        /// <summary>
        /// The window managers that have been loaded.
        /// </summary>
        private Dictionary<string, Type> LoadedWindowManagers { get; set; }

        /// <summary>
        /// The value that represents whether the client should close.
        /// </summary>
        private bool ShouldClose { get; set; }
        

        /// <summary>
        /// Runs this client instance.
        /// </summary>
        public bool Run()
        {
            if (Logger == null)
                throw new InvalidOperationException("RwClient.Start: An ILogger instance must be attached before calling Start().");

            if (HasStarted)
                throw new InvalidOperationException("RwClient.Start: Client is already started.");

            HasStarted = true;

            Logger.Out("RozWorld client starting...", LogLevel.Info);
            
            InitializeDirectories();
            LoadConfigs();
            LoadWindowManagers();
            

            if (SelectWindowManager(Configuration.ChosenRenderer))
            {
                // Load the rest and then start/run the game
                ShouldClose = false;

                // Initiate stopwatch to track elapsed game time
                var gameTime = new Stopwatch();
                TimeSpan elapsedTime;

                //
                // TODO: Spawn client game state here?
                //

                // Wait until the game should close
                while (!ShouldClose)
                {
                    elapsedTime = gameTime.Elapsed;
                    gameTime.Restart();
                    
                    InputUpdate inputs = ActiveWindowManager.GetInputEvents();

                    //
                    // Handle client game here
                    //

                    ActiveWindowManager.RenderFrame();
                }

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
        /// Loads available window managers from libraries on disk.
        /// </summary>
        private void LoadWindowManagers()
        {
            // Do not allow renderers to be reloaded!
            //
            if (LoadedWindowManagers != null)
                throw new InvalidOperationException("RwClient.LoadWindowManagers: Cannot load renderers after they have already been loaded.");

            // Load renderers
            //
            Logger.Out("Loading renderers...", LogLevel.Info);

            LoadedWindowManagers = new Dictionary<string, Type>();

            // First we should look for subfolders in the renderer path...
            //
            const string rendererIdFileName = "renderer-id.json";

            foreach (string folder in Directory.GetDirectories(RwClientParameters.RendererPath))
            {
                string libraryPath = Path.Combine(RwClientParameters.RendererPath, folder);
                string rendererIdPath = Path.Combine(libraryPath, rendererIdFileName);
                RendererInfo rendererInfo;

                // ...then try to parse the "renderer-id.json" file in them if there is one
                //
                if (!File.Exists(rendererIdPath))
                {
                    Logger.Out(
                        String.Format(
                            "I found {0} in the renderers folder was found, but it didn't contain a " +
                            "renderer-id.json file in it, so I don't know if it's a renderer or not. You " +
                            "should inspect this.",
                            folder
                            ),
                        LogLevel.Error
                        );

                    continue;
                }

                try
                {
                    rendererInfo = JsonConvert.DeserializeObject<RendererInfo>(File.ReadAllText(rendererIdPath));
                }
                catch (JsonException jsonEx)
                {
                    Logger.Out(
                        String.Format(
                            "Invalid JSON when reading renderer info from {0}.",
                            folder
                            ),
                        LogLevel.Error
                        );
                    continue;
                }

                // Check if the target DLL has been specified (could be blank in the JSON, we don't know)
                //
                if (String.IsNullOrWhiteSpace(rendererInfo.TargetDll))
                {
                    Logger.Out(
                        String.Format(
                            "I can't load the renderer in {0} because there isn't a target DLL specified " +
                            "in its associated JSON file.",
                            folder
                            ),
                        LogLevel.Error
                        );

                    continue;
                }

                // Since it's not blank, let's have a go at loading it
                //
                string targetDllPath = Path.Combine(libraryPath, rendererInfo.TargetDll);

                if (!File.Exists(targetDllPath))
                {
                    Logger.Out(
                        String.Format(
                            "I can't load the renderer in {0}, it should be {1} but it either doesn't " +
                            "exist or the account I'm running under doesn't have access to it.",
                            folder,
                            targetDllPath
                            ),
                        LogLevel.Error
                        );

                    continue;
                }

                try
                {
                    Assembly assembly = Assembly.LoadFrom(targetDllPath);
                    Type[] assemblyTypes = assembly.GetTypes();

                    foreach (Type assemblyType in assemblyTypes)
                    {
                        if (assemblyType.GetInterfaces().Contains(typeof(IWindowManager)))
                            LoadedWindowManagers.Add(assemblyType.FullName, assemblyType);
                    }
                }
                catch (ReflectionTypeLoadException reflectionEx)
                {
                    Logger.Out(
                        String.Format(
                            "Failed to enumerate types inside of the library {0} (that resides in folder {1}), " +
                            "so no renderers shall be loaded from it.\n\nException: {2}\n\nStack: {3}",
                            targetDllPath,
                            folder,
                            reflectionEx.Message,
                            reflectionEx.StackTrace
                            ),
                        LogLevel.Error
                        );
                }
                catch (Exception ex)
                {
                    Logger.Out(
                        String.Format(
                            "Some kind of exception was thrown whilst I was trying to load {0} from {1}.\n\n" +
                            "Exception: {2}\n\nStack: {3}",
                            targetDllPath,
                            folder,
                            ex.Message,
                            ex.StackTrace
                            ),
                        LogLevel.Error
                        );
                }
            }

            if (LoadedWindowManagers.Count == 0)
                Logger.Out("No renderers were loaded! Cannot continue.", LogLevel.Fatal);
        }

        /// <summary>
        /// Attempts to select the specified window manager.
        /// </summary>
        /// <param name="windowMgrAssemblyName">The assembly name of the window manager to load.</param>
        /// <returns>True if a window manager was successfully loaded.</returns>
        private bool SelectWindowManager(string windowMgrAssemblyName)
        {
            // Configuration.ChosenRenderer
            if (ActiveWindowManager != null)
            {
                ActiveWindowManager.Closed -= ActiveRenderer_Closed;
                ActiveWindowManager.Stop();
            }

            var availableRenderers = new List<Type>(LoadedWindowManagers.Values);

            // Find the renderer object first
            if (LoadedWindowManagers.ContainsKey(windowMgrAssemblyName))
                ActiveWindowManager = (IWindowManager)Activator.CreateInstance(LoadedWindowManagers[windowMgrAssemblyName]);
            else
                Logger.Out("Unknown renderer '" + Configuration.ChosenRenderer + "'.", LogLevel.Error);

            while (availableRenderers.Count > 0)
            {
                if (ActiveWindowManager == null || !ActiveWindowManager.Start(this)) // If renderer fails to start
                {
                    if (ActiveWindowManager != null)
                        availableRenderers.Remove(ActiveWindowManager.GetType());

                    if (availableRenderers.Count > 0)
                        ActiveWindowManager = (IWindowManager)Activator.CreateInstance(availableRenderers[0]);
                }
                else
                {
                    ActiveWindowManager.Closed += new EventHandler(ActiveRenderer_Closed);
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// [Event] Active renderer closed.
        /// </summary>
        private void ActiveRenderer_Closed(object sender, EventArgs e)
        {
            // TODO: Check if instance is running first
            ShouldClose = true;
        }
    }
}
