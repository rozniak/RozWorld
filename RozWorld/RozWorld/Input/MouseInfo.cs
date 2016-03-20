using OpenGL;
using System;

namespace RozWorld.Input
{
    /// <summary>
    /// Represents mouse information within a given frame update.
    /// </summary>
    internal class MouseInfo : ICloneable
    {
        /// <summary>
        /// States the position of the mouse cursor on screen.
        /// </summary>
        public Vector2 Position = Vector2.Zero;

        /// <summary>
        /// The state of the left mouse button.
        /// </summary>
        public MouseButtonState ButtonLeft = new MouseButtonState();

        /// <summary>
        /// The state of the middle mouse button.
        /// </summary>
        public MouseButtonState ButtonMiddle = new MouseButtonState();

        /// <summary>
        /// The state of the right mouse button.
        /// </summary>
        public MouseButtonState ButtonRight = new MouseButtonState();

        /// <summary>
        /// States the amount that has been scrolled.
        /// </summary>
        public double ScrollAmount = 0;


        /// <summary>
        /// Creates an exact copy of this MouseInfo.
        /// </summary>
        /// <returns>An exact copy of this MouseInfo casted as an object.</returns>
        public object Clone()
        {
            var clone = new MouseInfo();
            clone.Position = Position;
            clone.ButtonLeft = (MouseButtonState)ButtonLeft.Clone();
            clone.ButtonMiddle = (MouseButtonState)ButtonMiddle.Clone();
            clone.ButtonRight = (MouseButtonState)ButtonRight.Clone();
            clone.ScrollAmount = ScrollAmount;
            return clone;
        }
    }
}
