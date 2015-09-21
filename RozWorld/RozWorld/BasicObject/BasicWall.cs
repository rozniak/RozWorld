/**
 * RozWorld.BasicObject.BasicWall -- RozWorld Tile Wall Base Class
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
    public abstract class BasicWall
    {
        /// <summary>
        /// Gets the internal name of this wall, to be used when getting this wall.
        /// </summary>
        public abstract string InternalName { get; }


    }
}
