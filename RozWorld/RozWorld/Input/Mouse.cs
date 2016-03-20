using OpenGL;
using Pencil.Gaming;
using System.Drawing;

namespace RozWorld.Input
{
    /// <summary>
    /// Represents a susbsystem for managing mouse input.
    /// </summary>
    internal class Mouse
    {
        /// <summary>
        /// The currently live mouse states.
        /// </summary>
        public MouseInfo ActiveMouseStates = new MouseInfo();

        /// <summary>
        /// The mouse states of the last update.
        /// </summary>
        private MouseInfo LastMouseStates;

        /// <summary>
        /// The key states of the most current update.
        /// </summary>
        private MouseInfo CurrentMouseStates;


        /// <summary>
        /// Checks whether a mouse button was clicked within a given region.
        /// </summary>
        /// <param name="button">The mouse button to check (MOUSE1, MOUSE2 or MOUSE3).</param>
        /// <param name="region">The region to check that the click occurred in.</param>
        /// <returns>Whether or not a click was registered in the region specified.</returns>
        public bool Clicked(byte button, Rectangle region)
        {
            // Get the button state to work on
            MouseButtonState buttonState = EnumerateMouseButtons(button, false);

            if (buttonState == null)
                return false;

            // I know using Vector2i.Zero isn't the best, but I can't be arsed to sort this out
            // It works, that's about all I can say (hopefully)
            if (!buttonState.Pressed && buttonState.ClickOrigin != Vector2.Zero &&
                buttonState.ClickDestination != Vector2.Zero)
            {
                // Now check if the click occurred fully in the region given
                if (region.Contains(buttonState.ClickOrigin) &&
                    region.Contains(buttonState.ClickDestination))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Checks whether a mouse button is currently down.
        /// </summary>
        /// <param name="button">The mouse button to check (MOUSE1, MOUSE2 or MOUSE3).</param>
        /// <returns>Whether or not the mouse button is currently down.</returns>
        public bool IsDown(byte button)
        {
            if (CurrentMouseStates == null)
                return false;

            return EnumerateMouseButtons(button, false).Pressed;
        }


        /// <summary>
        /// Selects a MouseButtonState based on a button index.
        /// </summary>
        /// <param name="button">The mouse button to check (MOUSE1, MOUSE2 or MOUSE3).</param>
        /// <param name="fromActiveStates">When set to true, enumeration will use active key states, otherwise it will use the current key states.</param>
        /// <returns>The resulting enumerated mouse button, if the button index was valid.</returns>
        public MouseButtonState EnumerateMouseButtons(byte button, bool fromActiveStates)
        {
            MouseInfo mouseInfo = fromActiveStates ?
                ActiveMouseStates :
                CurrentMouseStates;

            if (mouseInfo == null)
                return null;

            switch (button)
            {
                case 1: return mouseInfo.ButtonLeft;
                case 2: return mouseInfo.ButtonRight;
                case 3: return mouseInfo.ButtonMiddle;
                default: return null;
            }
        }

        /// <summary>
        /// Selects a MouseButtonState based on a MouseButton enum.
        /// </summary>
        /// <param name="button">The MouseButton to enumerate by.</param>
        /// <param name="fromActiveStates">When set to true, enumeration will use active key states, otherwise it will use the current key states.</param>
        /// <returns>The resulting enumerated mouse button, if the button index was valid.</returns>
        public MouseButtonState EnumerateMouseButtons(MouseButton button, bool fromActiveStates)
        {
            MouseInfo mouseInfo = fromActiveStates ?
                ActiveMouseStates :
                CurrentMouseStates;

            if (mouseInfo == null)
                return null;

            switch (button)
            {
                case MouseButton.LeftButton: return mouseInfo.ButtonLeft;
                case MouseButton.RightButton: return mouseInfo.ButtonRight;
                case MouseButton.MiddleButton: return mouseInfo.ButtonMiddle;
                default: return null;
            }
        }


        /// <summary>
        /// Gets how much the mouse has scrolled since the latest update.
        /// </summary>
        /// <returns>The amount that has been scrolled since the lastest update.</returns>
        public sbyte ScrolledBy()
        {
            if (CurrentMouseStates == null)
                return 0;
            else
                return (sbyte)CurrentMouseStates.ScrollAmount;
        }


        /// <summary>
        /// Updates the mouse states monitored by this subsystem.
        /// </summary>
        public void Update()
        {
            LastMouseStates = CurrentMouseStates;
            CurrentMouseStates = ActiveMouseStates;

            MouseButtonState[] mouseButtons = new MouseButtonState[] { ActiveMouseStates.ButtonLeft,
                ActiveMouseStates.ButtonMiddle, ActiveMouseStates.ButtonRight };

            foreach (MouseButtonState mouseButtonState in mouseButtons)
            {
                if (!mouseButtonState.Pressed && mouseButtonState.ClickOrigin != Vector2.Zero &&
                    mouseButtonState.ClickDestination != Vector2.Zero)
                {
                    mouseButtonState.ClickOrigin = Vector2.Zero;
                    mouseButtonState.ClickDestination = Vector2.Zero;
                }
            }

            ActiveMouseStates.ScrollAmount = 0;
        }


        /// <summary>
        /// Gets the location of the mouse since the latest update.
        /// </summary>
        /// <returns>The location of the mouse since the lastest update.</returns>
        public Vector2 Where()
        {
            if (CurrentMouseStates == null)
                return Vector2.Zero;
            else
                return CurrentMouseStates.Position;
        }
    }
}
