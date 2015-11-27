/**
 * RozWorld.BasicObject.BasicFloor - RozWorld Tile Floor Base Class
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
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
        /// Gets whether a bucket is usable on this floor.
        /// </summary>
        public abstract bool CanBucket { get; }


        /// <summary>
        /// Gets whether this floor can have walls built upon it.
        /// </summary>
        public abstract bool CanBuildUpon { get; }


        /// <summary>
        /// Gets whether this floor can be dug.
        /// </summary>
        public abstract bool CanDig { get; }


        /// <summary>
        /// Gets whether this floor can have items dropped onto it.
        /// </summary>
        public abstract bool CanDropUpon { get; }


        /// <summary>
        /// Gets the amount of damage this floor does per second, if it is a hazard.
        /// </summary>
        public abstract ushort DamagePerSecond { get; }


        /// <summary>
        /// Gets the type of hazard this floor is.
        /// </summary>
        public abstract HazardType Hazard { get; }


        /// <summary>
        /// Gets or sets the ID of this floor.
        /// </summary>
        public ushort ID { get { return this._ID; } set { if (this._ID == 0) this._ID = value; } }
        private ushort _ID;


        /// <summary>
        /// Gets the internal name of this floor, to be used when getting this floor.
        /// </summary>
        public abstract string InternalName { get; }


        /// <summary>
        /// Gets whether this floor is liquid or not.
        /// </summary>
        public abstract bool IsLiquid { get; }

        
        /// <summary>
        /// Gets whether this floor is a warp or not.
        /// </summary>
        public bool IsWarp { get { return !Warp.Equals(null); } }


        /// <summary>
        /// Gets the current warp assigned to this floor.
        /// </summary>
        public WorldPoint Warp { get; protected set; }
    }
}
