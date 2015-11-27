/**
 * RozWorld.BasicObject.BasicPet -- RozWorld Pet Base Class
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
    public abstract class BasicPet
    {
        /// <summary>
        /// Gets the status of whether this pet is allowed to be renamed.
        /// </summary>
        public abstract bool CanRename { get; }


        /// <summary>
        /// Gets or sets the player-given name of this pet.
        /// </summary>
        public string GivenName { get { return this._GivenName; } set { if (CanRename) this._GivenName = value; } }
        private string _GivenName;


        /// <summary>
        /// Gets or sets the ID of this pet.
        /// </summary>
        public ushort ID { get { return this._ID; } set { if (this._ID == 0) this._ID = value; } }
        private ushort _ID;


        /// <summary>
        /// Gets the name of this pet to be displayed on interfaces as its original name.
        /// </summary>
        public abstract string InterfaceName { get; }


        /// <summary>
        /// Gets the internal name of this pet, to be used when getting this pet.
        /// </summary>
        public abstract string InternalName { get; }
    }
}
