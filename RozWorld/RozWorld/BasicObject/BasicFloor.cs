/**
 * RozWorld.BasicObject.BasicFloor - RozWorld Tile Floor Base Class
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
    public abstract class BasicFloor
    {
        /// <summary>
        /// Gets the internal name of this floor, to be used when getting this floor.
        /// </summary>
        public abstract string InternalName { get; }


        /// <summary>
        /// Gets whether this floor is liquid or not.
        /// </summary>
        public abstract bool IsLiquid { get; }


        /// <summary>
        /// Gets the type of hazard this floor is.
        /// </summary>
        public abstract HazardType Hazard { get; }


        /// <summary>
        /// Gets the amount of damage this floor does per second, if it is a hazard.
        /// </summary>
        public abstract ushort DamagePerSecond { get; }


        /// <summary>
        /// Gets whether this floor can have walls built upon it.
        /// </summary>
        public abstract bool CanBuildUpon { get; }


        /// <summary>
        /// Gets whether this floor can have items dropped onto it.
        /// </summary>
        public abstract bool CanDropUpon { get; }


        /// <summary>
        /// Gets whether this floor can be dug.
        /// </summary>
        public abstract bool CanDig { get; }


        /// <summary>
        /// Gets whether a bucket is usable on this floor.
        /// </summary>
        public abstract bool CanBucket { get; }
    }
}
