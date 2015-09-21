/**
 * RozWorld.BasicObject.BasicCreature -- RozWorld Creature Base Class
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
    public abstract class BasicCreature
    {
        /// <summary>
        /// Gets the internal name of this creature, to be used when getting this creature.
        /// </summary>
        public abstract string InternalName { get; }


        /// <summary>
        /// Gets the name of this creature to be displayed on interfaces as its original name.
        /// </summary>
        public abstract string InterfaceName { get; }


        /// <summary>
        /// Gets the status of whether this item is allowed to be renamed.
        /// </summary>
        public abstract bool CanRename { get; }
    }
}
