/**
 * RozWorld.BasicObject.BasicItem -- RozWorld Item Base Class
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
    public abstract class BasicItem
    {
        /// <summary>
        /// Gets the status of whether this item is allowed to be renamed.
        /// </summary>
        public abstract bool CanRename { get; }


        /// <summary>
        /// Gets or sets the player-given name of this item.
        /// </summary>
        public string GivenName { get { return this._GivenName; } set { if (CanRename) this._GivenName = value; } }
        private string _GivenName;


        /// <summary>
        /// Gets or sets the ID of this item.
        /// </summary>
        public int ID { get { return this._ID; } set { if (this._ID == 0) this._ID = value; } }
        private int _ID;


        /// <summary>
        /// Gets the name of this item to be displayed on interfaces as its original name.
        /// </summary>
        public abstract string InterfaceName { get; }


        /// <summary>
        /// Gets the internal name of this item, to be used when getting this item.
        /// </summary>
        public abstract string InternalName { get; }


        /// <summary>
        /// Gets the maximum quantity of this item, if it exceeds the server's maximum, the server's maximum will be used.
        /// </summary>
        public abstract byte MaxQuantity { get; }


        /// <summary>
        /// Gets the type of this item.
        /// </summary>
        public abstract ItemType Type { get; }


        #region Weapon Related Properties

        /// <summary>
        /// Gets the amount of power this item inflicts as a weapon.
        /// </summary>
        public abstract ushort Power { get; }

        #endregion


        #region Bucket Related Properties

        #endregion


        #region Floor and Wall Related Properties

        /// <summary>
        /// Ges the internal name of the floor or wall that this item constructs.
        /// </summary>
        public abstract string BuildingName { get; }

        #endregion


        #region Material Related Properties

        #endregion


        /// <summary>
        /// Sends extra data to this item so that it can be handled.
        /// </summary>
        /// <param name="data">The extra data.</param>
        /// <returns>Whether or not the extra data was accepted and handled successfully or not.</returns>
        public abstract bool HandleExtraData(byte[] data);
    }
}
