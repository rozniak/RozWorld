/**
 * RozWorld.Graphics.UI.Geometry.CharacterInfo -- RozWorld UI Character Information
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Drawing;


namespace RozWorld.Graphics.UI.Geometry
{
    public class CharacterInfo
    {
        public Point BlitOrigin = new Point(0, 0);
        public Point BlitDestination = new Point(0, 0);
        public sbyte Before = 0;
        public sbyte After = 0;
        public sbyte YOffset = 0;


        /// <summary>
        /// Returns the bounding rectangle of the blitting coordinates in this character's info.
        /// </summary>
        /// <returns>A Rectangle that represents the bounds of the blitting coordinates in this character's info.</returns>
        public Rectangle GetBlitRectangle()
        {
            return new Rectangle(BlitOrigin.X,
                BlitDestination.Y,
                Math.Abs(BlitDestination.X - BlitOrigin.X),
                Math.Abs(BlitOrigin.Y - BlitDestination.Y));
        }
    }
}
