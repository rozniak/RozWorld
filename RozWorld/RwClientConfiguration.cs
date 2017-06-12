﻿/**
 * Oddmatics.RozWorld.Client.RwClientConfiguration -- RozWorld Client Configuration
 *
 * This source-code is part of the client program for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Newtonsoft.Json;
using Oddmatics.RozWorld.API.Generic;
using System;
using System.Collections.Generic;

namespace Oddmatics.RozWorld.Client
{
    /// <summary>
    /// Represents the RozWorld client configurations.
    /// </summary>
    internal class RwClientConfiguration
    {
        /// <summary>
        /// Gets or sets the display resolutions for each game window
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<byte, Size> DisplayResolutions { get; set; }

        /// <summary>
        /// Gets or sets the full name of the chosen renderer class.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ChosenRenderer { get; set; }


        /// <summary>
        /// Initialises a new instance of the RwClientConfiguration class.
        /// </summary>
        public RwClientConfiguration()
        {
            DisplayResolutions = new Dictionary<byte, Size>();
            DisplayResolutions.Add(0, new Size(1366, 768)); // Default size of first window

            ChosenRenderer = String.Empty;
        }
    }
}
