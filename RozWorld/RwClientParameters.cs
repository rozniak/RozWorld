﻿/**
 * Oddmatics.RozWorld.Client.RwClientParameters -- RozWorld Client Parameters
 *
 * This source-code is part of the client program for the RozWorld project by Rory Fewell (rozniak) of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;

namespace Oddmatics.RozWorld.Client
{
    /// <summary>
    /// Specifies constants defining RozWorld client related parameters.
    /// </summary>
    internal class RwClientParameters
    {
        /// <summary>
        /// The configuration file path.
        /// </summary>
        public static readonly string ConfigurationPath = Environment.CurrentDirectory + @"\rwclient-config.json";

        /// <summary>
        /// The 'renderers' directory path.
        /// </summary>
        public static readonly string RendererPath = Environment.CurrentDirectory + @"\renderers";
    }
}
