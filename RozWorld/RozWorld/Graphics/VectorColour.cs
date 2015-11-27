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

        // Specialist Tints (Good for specific things or controls)
        public static readonly Vector4 ButtonHoverTint = new Vector4(1.0, 1.0, 0.0, 0.3);
        public static readonly Vector4 ButtonDownTint = new Vector4(1.0, 1.0, 0.0, 0.4);
    }
}
