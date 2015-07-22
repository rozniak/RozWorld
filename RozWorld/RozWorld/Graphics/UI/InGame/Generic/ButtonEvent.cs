/**
 * RozWorld.Graphics.UI.InGame.Generic.ButtonEvent -- RozWorld Generic Button Events
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.Graphics.UI.Control;

namespace RozWorld.Graphics.UI.InGame.Generic
{
    public static class ButtonEvent
    {
        /// <summary>
        /// [Event] Generic button mouse leave.
        /// </summary>
        public static void OnMouseLeave(object sender)
        {
            ((Button)sender).TintColour = VectorColour.NoTint;
        }


        /// <summary>
        /// [Event] Generic button mouse enter.
        /// </summary>
        public static void OnMouseEnter(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;
        }


        /// <summary>
        /// [Event] Generic button mouse down.
        /// </summary>
        public static void OnMouseDown(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonDownTint;
        }
    }
}
