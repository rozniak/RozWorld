/**
 * RozWorld.Graphics.UI.FontProvider -- RozWorld UI Font and Text Provider
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using OpenGL;

using RozWorld.Graphics.UI.Geometry;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace RozWorld.Graphics.UI
{
    public static class FontProvider
    {
        private static TextureManager GameTextureManager;
        private static GUIOMETRY GameGUIOMETRY;

        public static Bitmap ChatFont;
        public static Bitmap SmallFont;
        public static Bitmap MediumFont;
        public static Bitmap HugeFont;


        public static Texture BuildText(FontType fontType, string text, out List<DrawInstruction> drawInstructions)
        {
            // TODO: Actually make this work
            drawInstructions = new List<DrawInstruction>();
            return null;
        }


        /// <summary>
        /// Loads texture references for fonts ready to begin text work.
        /// </summary>
        /// <returns>Whether the texture references were successfully loaded or not.</returns>
        public static bool Load(TextureManager gameTextureManager, GUIOMETRY gameGUIOMETRY)
        {
            GameTextureManager = gameTextureManager;
            GameGUIOMETRY = gameGUIOMETRY;

            return true; // just to allow building for now
        }


        public static uint MeasureString(FontType fontType, string text)
        {
            return 0; // TODO: Make this measure strings whatever
        }
    }
}
