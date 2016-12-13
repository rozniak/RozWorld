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
using System;

namespace Oddmatics.RozWorld.Client
{
    public class RwClient :IRwClient
    {
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

            // TODO: More of this of course

            return true;
        }
    }
}
