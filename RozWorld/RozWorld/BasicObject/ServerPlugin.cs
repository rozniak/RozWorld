/**
 * RozWorld.BasicObject.ServerPlugin -- RozWorld Server Plugin Base Class
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace RozWorld.BasicObject
{
    public abstract class ServerPlugin
    {
        /// <summary>
        /// Gets the internal name of this plugin, to be used when getting or listing this plugin.
        /// </summary>
        public abstract string InternalName { get; }


        /// <summary>
        /// Gets the author of this plugin.
        /// </summary>
        public abstract string PluginAuthor { get; }


        /// <summary>
        /// Gets the download URL of this plugin, to relay to players.
        /// </summary>
        public abstract string URL { get; }


        /// <summary>
        /// Gets the version of this plugin.
        /// </summary>
        public abstract double Version { get; }


        /// <summary>
        /// Base method called upon initialising plugins on the server.
        /// </summary>
        public virtual void Start() { }


        /// <summary>
        /// Base method called upon ending plugins on the server.
        /// </summary>
        public virtual void Stop() { }
    }
}
