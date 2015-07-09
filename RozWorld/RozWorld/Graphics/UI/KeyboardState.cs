//
// RozWorld.Graphics.UI.KeyboardState -- RozWorld Keyboard State Tracker
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

using System.Collections.Generic;

namespace RozWorld.Graphics.UI
{
    public struct KeyboardState
    {
        private List<byte> ActiveKeys;
        

        /// <summary>
        /// Sets the key to active within the keyboard state.
        /// </summary>
        /// <param name="key">The key to activate.</param>
        public void KeyDown(byte key)
        {
            if (ActiveKeys == null) { ActiveKeys = new List<byte>(); };

            if (!ActiveKeys.Contains(key))
            {
                ActiveKeys.Add(key);
            }
        }


        /// <summary>
        /// Sets the key to inactive within the keyboard state.
        /// </summary>
        /// <param name="key">The key to deactivate.</param>
        public void KeyUp(byte key)
        {
            if (ActiveKeys == null) { ActiveKeys = new List<byte>(); };

            if (ActiveKeys.Contains(key))
            {
                ActiveKeys.Remove(key);
            }
        }


        /// <summary>
        /// Gets whether the specified key is active or not.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Whether the key is active or not.</returns>
        public bool Get(byte key)
        {
            if (ActiveKeys == null) { ActiveKeys = new List<byte>(); };

            return ActiveKeys.Contains(key);
        }
    }
}
