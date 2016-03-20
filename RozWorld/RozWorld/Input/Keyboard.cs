/**
 * RozWorld.Input.Keyboard -- RozWorld Keyboard Raw Input
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RawInput;

using RozWorld.Graphics;

using System;
using System.Collections.Generic;


namespace RozWorld.Input
{
    /// <summary>
    /// Represents a susbsystem for managing keyboard input.
    /// </summary>
    internal class Keyboard
    {
        /// <summary>
        /// The RawInput instance used to manage the keyboard(s).
        /// </summary>
        private RawInput.RawInput RawInput;

        /// <summary>
        /// The parent Engine instance of this Keyboard system.
        /// </summary>
        private GameWindow ParentWindow;

        /// <summary>
        /// The currently live key states.
        /// </summary>
        private List<string> ActiveKeyStates = new List<string>();

        /// <summary>
        /// The key states of the last update.
        /// </summary>
        private List<string> UpdateLastKeyStates = new List<string>();

        /// <summary>
        /// The key states of the most current update.
        /// </summary>
        private List<string> UpdateCurrentKeyStates = new List<string>();


        /// <summary>
        /// Initialises a new instance of the Keyboard class with a specified parent Engine.
        /// </summary>
        /// <param name="parentEngine">The parent Engine instance.</param>
        public Keyboard(GameWindow parentWindow)
        {
            if (parentWindow != null && parentWindow.HwndPtr != IntPtr.Zero)
            {
                ParentWindow = parentWindow;

                // Attach raw input to the window handle of the game client and only watch input when the window is in focus
                RawInput = new RawInput.RawInput(ParentWindow.HwndPtr, true);
                RawInput.AddMessageFilter();
                RawInput.KeyPressed += RawInput_KeyPressed;
            }
        }


        /// <summary>
        /// Updates the key states monitored by this subsystem.
        /// </summary>
        public void Update()
        {
            UpdateLastKeyStates = UpdateCurrentKeyStates;
            UpdateCurrentKeyStates = new List<string>(ActiveKeyStates);
        }


        /// <summary>
        /// Checks if the specified key has been pressed or not.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Whether the specified key has been pressed or not.</returns>
        public bool IsPressed(string key)
        {
            return UpdateLastKeyStates.Contains(key) &&
                    !UpdateCurrentKeyStates.Contains(key);
        }

        /// <summary>
        /// Checks if the specified key is pressed or not.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Whether the key is pressed or not.</returns>
        public bool IsDown(string key)
        {
            return UpdateCurrentKeyStates.Contains(key);
        }


        /// <summary>
        /// [RawInput Event | KeyPressed] Key pressed.
        /// </summary>
        private void RawInput_KeyPressed(object sender, RawInputEventArg e)
        {
            if (e.KeyPressEvent.KeyPressState == "MAKE")
                ActiveKeyStates.Add(e.KeyPressEvent.VKeyName);
            else // KeyPressState == "BREAK"
                ActiveKeyStates.Remove(e.KeyPressEvent.VKeyName);

        }
    }
}
