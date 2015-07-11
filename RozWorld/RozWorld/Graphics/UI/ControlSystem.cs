/**
 * RozWorld.Graphics.UI.ControlSystem -- RozWorld UI Objects and Dialog Control System
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace RozWorld.Graphics.UI
{
    public abstract class ControlSystem
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

        protected ControlSkeleton[] MouseSubscribers = new ControlSkeleton[] { };
        protected ControlSkeleton[] KeyboardSubscribers = new ControlSkeleton[] { };


        /**
         * Methods for starting and closing the control system.
         */
        public abstract void Start();
        public abstract void Close();


        /// <summary>
        /// Base method for setting up mouse and keyboard subscribers.
        /// </summary>
        public virtual void SetupSubscribers() { }


        /// <summary>
        /// Triggers mouse checks on each of the subscribed controls.
        /// </summary>
        public void TriggerMouse() 
        {
            foreach (ControlSkeleton control in MouseSubscribers)
            {
                control.CheckMouse();
            }
        }


        /// <summary>
        /// Triggers keyboard checks on each of the subscribed controls.
        /// </summary>
        /// <param name="down">Whether the event state is a key down or up.</param>
        /// <param name="key">The key changing state.</param>
        public void TriggerKeyboard(bool down, byte key) 
        {
            foreach (ControlSkeleton control in KeyboardSubscribers)
            {
                control.CheckKeyboard(down, key);
            }
        }
    }
}
