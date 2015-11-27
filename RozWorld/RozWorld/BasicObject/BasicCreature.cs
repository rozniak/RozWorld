/**
 * RozWorld.BasicObject.BasicCreature -- RozWorld Creature Base Class
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
    public abstract class BasicCreature
    {
        /// <summary>
        /// Gets the status of whether this item is allowed to be renamed.
        /// </summary>
        public abstract bool CanRename { get; }


        /// <summary>
        /// Gets or sets the player-given name of this creature.
        /// </summary>
        public string GivenName { get { return this._GivenName; } set { if (CanRename) this._GivenName = value; } }
        private string _GivenName;


        /// <summary>
        /// Gets whether this creature has an attack target.
        /// </summary>
        public virtual bool HasTarget { get { return TargetName != ""; } }


        /// <summary>
        /// Gets or sets the health of this creature.
        /// </summary>
        public virtual int Health { get; set; }


        /// <summary>
        /// Gets the name of this creature to be displayed on interfaces as its original name.
        /// </summary>
        public abstract string InterfaceName { get; }


        /// <summary>
        /// Gets the internal name of this creature, to be used when getting this creature.
        /// </summary>
        public abstract string InternalName { get; }


        /// <summary>
        /// Gets whether this creature is currently alive.
        /// </summary>
        public virtual bool IsAlive { get { return Health > 0; } }


        /// <summary>
        /// Gets or sets the nature of this creature.
        /// </summary>
        public abstract NatureType Nature { get; protected set; }


        /// <summary>
        /// Gets or sets the attack target of this creature.
        /// </summary>
        public virtual string TargetName { get; protected set; }


        public virtual bool Kill(string message = "")
        {
            if (IsAlive)
            {
                Health = 0;

                // TODO: Finish this
            }

            return true; // Temporary
        }
    }
}
