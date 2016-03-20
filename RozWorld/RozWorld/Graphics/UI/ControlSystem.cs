/**
 * RozWorld.Graphics.UI.ControlSystem -- RozWorld UI Objects and Dialog Control System
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace RozWorld.Graphics.UI
{
    internal abstract class ControlSystem
    {
        protected int _DialogKey;
        public int DialogKey
        {
            get
            {
                return this._DialogKey;
            }

            set
            {
                if (this._DialogKey == 0)
                {
                    this._DialogKey = value;
                }
            }
        }

        protected GameWindow ParentWindow;

        /**
         * Methods for starting and closing the control system.
         */
        public abstract void Start();
        public abstract void Close();


        /// <summary>
        /// Base method for calling all control position updating routines.
        /// </summary>
        public virtual void UpdateControlPositions() { }
    }
}
