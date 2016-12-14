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

        #endregion


        public string ClientName { get { return "Vanilla RozWorld Client"; } }
        public string ClientVersion { get { return "0.01"; } }
        public IInputHandler Input { get { throw new System.NotImplementedException(); } }
        public IInterfaceHandler Interface { get { throw new System.NotImplementedException(); } }
        private ILogger _Logger;
        public ILogger Logger
        {
            get { return _Logger; } set { if (_Logger == null) _Logger = value; }
        }
        public string RozWorldVersion { get { return "0.01"; } }


        private bool HasStarted;
        private List<Type> Renderers;
        private Renderer ActiveRenderer;


        /// <summary>
        /// Runs this client instance.
        /// </summary>
        public bool Run()
        {
            // A logger must be set and this should be set as the current client in RwCore
            if (RwCore.Client != this)
                throw new InvalidOperationException("RwClient.Start: RwCore.Client must reference this client instance before calling Start().");

            if (Logger == null)
                throw new InvalidOperationException("RwClient.Start: An ILogger instance must be attached before calling Start().");

            if (HasStarted)
                throw new InvalidOperationException("RwClient.Start: Client is already started.");

            Logger.Out("RozWorld client starting...", LogLevel.Info);
            Logger.Out("Initialising directories...", LogLevel.Info);

            FileSystem.MakeDirectory(DIRECTORY_RENDERERS);

            // TODO: LOAD CONFIGURATION FILE HERE!!!

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
                        if (detectedObject.IsAssignableFrom(typeof(Renderer)))
                            Renderers.Add(detectedObject);
                    }
                }
                catch (ReflectionTypeLoadException reflectionEx)
                {
                    Logger.Out("An error occurred trying to enumerate the types inside of the library \"" +
                        Path.GetFileName(file) + "\", renderers from this library cannot be loaded. It may" +
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

            ActiveRenderer = (Renderer)Activator.CreateInstance(Renderers[0]); // This will be replaced - dw for now

            ActiveRenderer.Initialise();

            // Run game here!

            return true;
        }
    }
}
