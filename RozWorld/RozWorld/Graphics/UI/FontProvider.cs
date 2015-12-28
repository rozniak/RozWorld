/**
 * RozWorld.Graphics.UI.FontProvider -- RozWorld UI Font and Text Provider
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using OpenGL;

using RozWorld.Graphics.UI.Geometry;
using RozWorld.IO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;


namespace RozWorld.Graphics.UI
{
    public static class FontProvider
    {
        private static TextureManager GameTextureManager;
        private static GUIOMETRY GameGUIOMETRY;

        private static Dictionary<string, Bitmap> Fonts = new Dictionary<string, Bitmap>();

        public static bool Loaded
        {
            get { return GameGUIOMETRY != null && GameTextureManager != null && Fonts.Count > 0; }
        }


        /// <summary>
        /// Construct the texture and draw instructions for a specified string.
        /// </summary>
        /// <param name="fontType">The type of font to use.</param>
        /// <param name="text">The text to make a texture of.</param>
        /// <param name="drawInstructions">The draw instructions to build.</param>
        /// <param name="formatStripMode">Whether to strip any formatting from the string.</param>
        /// <returns>The string in its new texture and updated draw instructions if all was successful.</returns>
        public static Texture BuildString(FontType fontType, string text, out List<DrawInstruction> drawInstructions,
            StringFormatting stringFormat)
        {
            drawInstructions = new List<DrawInstruction>();

            if (Loaded)
            {
                
            }

            return null;
        }


        /// <summary>
        /// Disposes all font bitmaps that are loaded.
        /// </summary>
        private static void DisposeFonts()
        {
            foreach (var font in Fonts)
                font.Value.Dispose(); // Bin all the fonts

            Fonts.Clear();
        }


        /// <summary>
        /// Get the internal name of the specified font type.
        /// </summary>
        /// <param name="fontType">The font type to get the name of.</param>
        /// <returns>The font's internal name if it is valid, an empty string otherwise.</returns>
        public static string GetFontInternalName(FontType fontType)
        {
            switch (fontType)
            {
                case FontType.ChatFont: return "ChatFont";
                case FontType.SmallFont: return "SmallFont";
                case FontType.MediumFont: return "MediumFont";
                case FontType.HugeFont: return "HugeFont";
                default: return string.Empty;
            }
        }


        /// <summary>
        /// Loads texture references for fonts ready to begin text work.
        /// </summary>
        /// <returns>Whether the texture references were successfully loaded or not.</returns>
        public static bool Load(TextureManager gameTextureManager, GUIOMETRY gameGUIOMETRY)
        {
            GameTextureManager = gameTextureManager;
            GameGUIOMETRY = gameGUIOMETRY;

            DisposeFonts();

            string[] fontsToLoad = new string[] { "ChatFont", "SmallFont", "MediumFont", "HugeFont" };

            foreach (string font in fontsToLoad)
            {
                string fontSource = gameTextureManager.GetFontSource(font);

                if (!string.IsNullOrEmpty(fontSource))
                    Fonts.Add(font, new Bitmap(Files.LiveTextureDirectory + fontSource));
            }

            // Make sure it loaded all the fonts
            return Fonts.Count == fontsToLoad.Length;
        }


        /// <summary>
        /// Measure the size of the area that contains the string in the specified font type.
        /// </summary>
        /// <param name="fontType">The type of font to use.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="formatStripMode">Whether to strip any formatting from the string.</param>
        /// <returns>The size of the text in the specified font if measuring was successful, an empty size otherwise.</returns>
        public static Size MeasureString(FontType fontType, string text, StringFormatting stringFormat)
        {
            // Implement this later

            return Size.Empty;
        }


        /// <summary>
        /// Gets the size of the bounding Bitmap that will contain the string image.
        /// </summary>
        /// <param name="fontType">The type of font to use.</param>
        /// <param name="text">The text to measure.</param>
        /// <param name="stripMode">Whether to strip any formatting from the string.</param>
        /// <returns>The size of the bitmap that will contain the string image if measuring was successful, an empty size otherwise.</returns>
        private static Size GetStringBitmapSize(FontType fontType, string text, StringFormatting stringFormat)
        {
            // The final bitmap size height will be = singleFontSize.Height * coloursPresent
            Size singleFontSize = Size.Empty;
            byte coloursPresent = 0;

            uint currentX = 0;
            uint highestX = 0;
            uint currentY = 0;

            FontInfo fontInfo = RozWorld.InterfaceGeometry.GetFont(GetFontInternalName(fontType));
            bool formatCodeActive = false;

            foreach (char character in text)
            {
                // Check whether to look for formatting in this string
                if (stringFormat != StringFormatting.None)
                {
                    // Check if this should trigger a format code
                    if (character == '&' && !formatCodeActive)
                    {
                        formatCodeActive = true;
                        continue;
                    }

                    if (formatCodeActive)
                    {
                        if ((stringFormat == StringFormatting.Colours ||
                            stringFormat == StringFormatting.Both) &&
                            character.IsHexChar()) // It's a colour code
                        {
                            coloursPresent++; // Increase colour count
                            continue;
                        }
                        else if ((stringFormat == StringFormatting.LineBreaks ||
                            stringFormat == StringFormatting.Both) &&
                            character.EqualsIgnoreCase('n')) // It's a newline code
                        {
                            currentY += fontInfo.LineHeight;
                            highestX = currentX;
                            currentX = 0;
                            continue;
                        }
                    }
                }

                // If the code continues to here, then calculate the character metrics

            }

            return Size.Empty;
        }
    }
}
