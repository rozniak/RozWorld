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

using OpenGL;

using System;

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
        public ushort ID { get { return this._ID; } set { if (this._ID == 0) this._ID = value; } }
        private ushort _ID;


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


        #region Weapon Related

        /// <summary>
        /// Gets the amount of power this item inflicts as a weapon.
        /// </summary>
        public abstract ushort Power { get; }

        #endregion


        #region Bucket Related

        /// <summary>
        /// Gets whether this item is a full bucket or not.
        /// </summary>
        public bool IsFull { get { return BucketContents.Item1 != 0; } }


        /// <summary>
        /// Gets or sets this item's contents as a bucket.
        /// </summary>
        public Tuple<ushort, byte[]> BucketContents { get; protected set; }


        /// <summary>
        /// Attempts to fill this item's bucket contents.
        /// </summary>
        /// <param name="contents">The ID of the contents.</param>
        /// <param name="data">Any extra data to suppliment the contents.</param>
        /// <returns>Whether this item was successfully filled with bucket contents or not.</returns>
        public virtual bool FillBucket(ushort contents, byte[] data = null)
        {
            if (Type == ItemType.Bucket && !IsFull)
            {
                BucketContents = new Tuple<ushort, byte[]>(contents, data);
                return true;
            }

            return false;
        }


        /// <summary>
        /// Attempts to use this item as a bucket at a location in the world.
        /// </summary>
        /// <param name="worldName">The name of the world the bucket is used in.</param>
        /// <param name="location">The location that the bucket is used in.</param>
        /// <returns>The contents of the bucket to use, null if unusable for some reason.</returns>
        public virtual Tuple<ushort, byte[]> UseBucket(string worldName, Vector3 location) { /* TO BE IMPLEMENTED */ return null; }

        #endregion


        #region Floor and Wall Related

        /// <summary>
        /// Ges the internal name of the floor or wall that this item constructs.
        /// </summary>
        public abstract string BuildingName { get; }

        #endregion


        #region Material Related

        #endregion


        /// <summary>
        /// Sends extra data to this item so that it can be handled.
        /// </summary>
        /// <param name="data">The extra data.</param>
        /// <returns>Whether or not the extra data was accepted and handled successfully or not.</returns>
        public abstract bool HandleExtraData(byte[] data);
    }
}
