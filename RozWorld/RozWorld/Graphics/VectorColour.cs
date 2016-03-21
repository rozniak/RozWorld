/**
 * RozWorld.Graphics.VectorColour -- RozWorld Colour Tinting Vectors
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using OpenGL;

using RozWorld_API.Chat;


namespace RozWorld.Graphics
{
    public static class VectorColour
    {
        // Default (No tint)
        public static readonly Vector4 NoTint = new Vector4(0.0, 0.0, 0.0, 0.0);

        // Opaque Tints (Good for fonts)
        public static readonly Vector4 OpaqueRed = new Vector4(1.0, 0.0, 0.0, 1.0);
        public static readonly Vector4 OpaqueGreen = new Vector4(0.0, 1.0, 0.0, 1.0);
        public static readonly Vector4 OpaqueBlue = new Vector4(0.0, 0.0, 1.0, 1.0);
        public static readonly Vector4 OpaqueYellow = new Vector4(1.0, 1.0, 0.0, 1.0);
        public static readonly Vector4 OpaqueCyan = new Vector4(0.0, 1.0, 1.0, 1.0);
        public static readonly Vector4 OpaqueMagenta = new Vector4(1.0, 0.0, 1.0, 1.0);
        public static readonly Vector4 OpaqueOrange = new Vector4(1.0, 0.41, 0.0, 1.0);
        public static readonly Vector4 OpaqueWhite = new Vector4(1.0, 1.0, 1.0, 1.0);
        public static readonly Vector4 OpaqueBlack = new Vector4(0.0, 0.0, 0.0, 1.0);
        public static readonly Vector4 OpaqueGrey = new Vector4(0.5, 0.5, 0.5, 1.0);

        // Translucent Tints (Good for sprites)
        public static readonly Vector4 TintRed = new Vector4(1.0, 0.0, 0.0, 0.6);
        public static readonly Vector4 TintGreen = new Vector4(0.0, 1.0, 0.0, 0.6);
        public static readonly Vector4 TintBlue = new Vector4(0.0, 0.0, 1.0, 0.6);
        public static readonly Vector4 TintYellow = new Vector4(1.0, 1.0, 0.0, 0.6);
        public static readonly Vector4 TintCyan = new Vector4(0.0, 1.0, 1.0, 0.6);
        public static readonly Vector4 TintMagenta = new Vector4(1.0, 0.0, 1.0, 0.6);
        public static readonly Vector4 TintOrange = new Vector4(1.0, 0.41, 0.0, 0.6);
        public static readonly Vector4 TintWhite = new Vector4(1.0, 1.0, 1.0, 0.6);
        public static readonly Vector4 TintBlack = new Vector4(0.0, 0.0, 0.0, 0.6);
        public static readonly Vector4 TintGrey = new Vector4(0.5, 0.5, 0.5, 0.6);

        // Tint Factors
        private static readonly Vector4 DarkFactor = new Vector4(0.75, 0.75, 0.75, 1.0);

        // Specialist Tints (Good for specific things or controls)
        public static readonly Vector4 ButtonHoverTint = new Vector4(1.0, 1.0, 0.0, 0.3);
        public static readonly Vector4 ButtonDownTint = new Vector4(1.0, 1.0, 0.0, 0.4);


        /// <summary>
        /// Retrieves a Vector4 representing an opaque colour from colour codes in the RozWorld API.
        /// </summary>
        /// <param name="colourCode">The character colour code.</param>
        /// <returns>The Vector4 of the corresponding colour code.</returns>
        public static Vector4 FromChar(char colourCode)
        {
            switch (colourCode)
            {
                case ColourCode.BLACK: return OpaqueBlack;
                case ColourCode.BLUE: return OpaqueBlue;
                case ColourCode.CYAN: return OpaqueCyan;
                case ColourCode.DARK_BLUE: return OpaqueBlue * DarkFactor;
                case ColourCode.DARK_GREY: return OpaqueGrey * DarkFactor;
                case ColourCode.DARK_RED: return OpaqueRed * DarkFactor;
                case ColourCode.GREEN: return OpaqueGreen * DarkFactor;
                case ColourCode.GREY: return OpaqueGrey;
                case ColourCode.LIME: return OpaqueGreen;
                case ColourCode.MAGENTA: return OpaqueMagenta;
                case ColourCode.ORANGE: return OpaqueOrange;
                case ColourCode.PURPLE: return OpaqueMagenta * DarkFactor;
                case ColourCode.RED: return OpaqueRed;
                case ColourCode.TEAL: return OpaqueCyan * DarkFactor;
                case ColourCode.WHITE: return OpaqueWhite;
                case ColourCode.YELLOW: return OpaqueYellow;

                default: return NoTint;
            }
        }
    }
}
