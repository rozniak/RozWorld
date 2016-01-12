using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RozWorld.Input.Keyboard
{
    public struct Keyboard
    {
        private IList<char> LastKeyStates;
        private IList<char> CurrentKeyStates;


        public Keyboard(IList<char> lastKeyStates, IList<char> currentKeyStates)
        {
            LastKeyStates = lastKeyStates;
            CurrentKeyStates = currentKeyStates;
        }


        /// <summary>
        /// Checks whether a key is in a pressed state.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Whether the key is pressed or not.</returns>
        public bool KeyDown(char key)
        {
            return false; // TODO: Implement this
        }


        /// <summary>
        /// Checks whether a key has had a single press within the last update.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Whether the key has had a single press within the last update.</returns>
        public bool KeyPress(char key)
        {
            return false; // TODO: Implement this
        }


        /// <summary>
        /// Checks whether a key is in an unpressed state.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Whether the key is unpressed or not.</returns>
        public bool KeyUp(char key)
        {
            return false; // TODO: Implement this
        }


        /// <summary>
        /// Gets a char from the latest key pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>The char of the latest key pressed.</returns>
        public char GetChar(char key)
        {
            return '\0'; // TODO: Implement this
        }
    }
}
