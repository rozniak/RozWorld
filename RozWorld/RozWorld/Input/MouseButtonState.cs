using OpenGL;
using System;

namespace RozWorld.Input
{
    /// <summary>
    /// Represents an individual mouse button's state.
    /// </summary>
    internal class MouseButtonState : ICloneable
    {
        /// <summary>
        /// States whether this mouse button is currently pressed or not.
        /// </summary>
        public bool Pressed = false;

        /// <summary>
        /// States where this mouse button was pressed down at.
        /// </summary>
        public Vector2 ClickOrigin = Vector2.Zero;

        /// <summary>
        /// States where this mouse button was lifted at.
        /// </summary>
        public Vector2 ClickDestination = Vector2.Zero;


        /// <summary>
        /// Creates an exact copy of this MouseButtonState.
        /// </summary>
        /// <returns>An exact copy of this MouseButtonState casted as an object.</returns>
        public object Clone()
        {
            var clone = new MouseButtonState();
            clone.Pressed = Pressed;
            clone.ClickOrigin = ClickOrigin;
            clone.ClickDestination = ClickDestination;
            return clone;
        }
    }
}
