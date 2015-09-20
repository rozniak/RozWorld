/**
 * RozWorld.Entity.BasicItem -- RozWorld Item Base Class
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace RozWorld.Entity
{
    public abstract class BasicItem
    {
        // TODO: Implement base item class here (this will be inherited by other classes)


        /// <summary>
        /// The internal name of this item, to be used when getting this item.
        /// </summary>
        public abstract string InternalName { get; }


        /// <summary>
        /// The name of this item to be displayed on interfaces as its original name.
        /// </summary>
        public abstract string InterfaceName { get; }


        /**
         * Backing field for GivenName property.
         */
        private string _GivenName;

        /// <summary>
        /// The name of this item when renamed by the player, depends on whether the item can be renamed.
        /// </summary>
        public string GivenName
        {
            get
            {
                if (!string.IsNullOrEmpty(this._GivenName)) return this._GivenName;

                return this.InterfaceName;
            }

            set
            {
                if (this.CanRename) this._GivenName = value;
            }
        }


        /// <summary>
        /// The status of whether this item is allowed to be renamed.
        /// </summary>
        public abstract bool CanRename { get; }


        /// <summary>
        /// The type of this item.
        /// </summary>
        public abstract ItemType Type { get; }


        #region Weapon Related Properties

        #endregion

        #region Bucket Related Properties

        #endregion

        #region Floor Related Properties

        #endregion

        #region Wall Related Properties

        #endregion

        #region Material Related Properties

        #endregion
    }
}
