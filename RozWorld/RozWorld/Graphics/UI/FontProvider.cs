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
        private static Dictionary<string, Bitmap> Fonts = new Dictionary<string, Bitmap>();

        public static bool Loaded
        {
            get
            {
                return RozWorld.InterfaceGeometry != null &&
                    RozWorld.Textures != null &&
                    Fonts.Count > 0;
            }
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
                // The final bitmap size height will be = singleFontSize.Height * colours.Count
                Size singleFontSize = Size.Empty;

                bool formatCodeActive = false;

                List<char> colours = new List<char>();
                colours.Add('0'); // Black

                int maxCharHeight = 0; // Track how tall characters could get
                int lowCharHeight = 0; // Track how low characters could get
                int totalCharHeight = 0; // The total height
                int lines = 1; // How many lines there are
                int maxLineWidth = 0;
                int thisLineWidth = 0;

                string fontName = GetFontInternalName(fontType);

                FontInfo fontInfo = RozWorld.InterfaceGeometry.GetFont(fontName);

                // Get string details
                foreach (char character in text)
                {
                    // If & is already encountered, this character should be a code
                    if (formatCodeActive)
                    {
                        char charTest = character.ToLower();

                        if (charTest == 'n') // Newline code
                        {
                            lines++;
                            thisLineWidth = 0;
                        }
                        else if (charTest.IsHexChar() && !colours.Contains(charTest)) // Colour code
                        {
                            colours.Add(charTest);
                        }

                        formatCodeActive = false;
                        continue;
                    }

                    // If & is encountered, the next character should be treated as a code
                    if (character == '&')
                    {
                        formatCodeActive = true;
                        continue;
                    }

                    // If a space is encountered, add the spacing width of the font
                    if (character == ' ')
                    {
                        thisLineWidth += fontInfo.SpacingWidth;
                        maxLineWidth = maxLineWidth.CompareHighest(thisLineWidth);
                        continue;
                    }

                    // Treat everything else as a standard character
                    CharacterInfo charInfo = fontInfo.GetCharacter(character);

                    if (charInfo != null) // Make sure this character is present in the font
                    {
                        Rectangle charRect = fontInfo.GetCharacter(character).GetBlitRectangle();
                        int totalHeight = charRect.Height + charInfo.YOffset;
                        maxCharHeight = maxCharHeight.CompareHighest(totalHeight);
                        lowCharHeight = lowCharHeight.CompareLowest(charInfo.YOffset);

                        thisLineWidth += charRect.Width + charInfo.After + charInfo.Before;
                        maxLineWidth = maxLineWidth.CompareHighest(thisLineWidth);
                    }
                }

                // Sort out the total sizes
                totalCharHeight = maxCharHeight + Math.Abs(lowCharHeight);
                singleFontSize = new Size(maxLineWidth, totalCharHeight * lines);

                int drawY = maxCharHeight;
                int drawX = 0;

                /**
                 * This value is because each colour is offsetted by the height of a single string
                 * So this value should equal the height of a single string texture multiplied by the colour's
                 * index inside of the colours list.
                 */
                int colourHeightModifier = 0;

                formatCodeActive = false;

                using (Image stringTexture = new Bitmap(singleFontSize.Width, singleFontSize.Height * colours.Count))
                using (System.Drawing.Graphics GFX = System.Drawing.Graphics.FromImage(stringTexture))
                {
                    foreach (char character in text)
                    {
                        // If & is already encountered, this character should be a code
                        if (formatCodeActive)
                        {
                            char charTest = character.ToLower();

                            if (charTest == 'n') // Newline code
                            {
                                drawX = 0;
                                drawY += fontInfo.LineHeight;
                            }
                            else if (charTest.IsHexChar())
                            {
                                colourHeightModifier = singleFontSize.Height * colours.IndexOf(charTest);
                            }

                            formatCodeActive = false;
                            continue;
                        }

                        // If & is encountered, the next character should be treated as a code
                        if (character == '&')
                        {
                            formatCodeActive = true;
                            continue;
                        }

                        // If a space is encountered, add the spacing width of the font
                        if (character == ' ')
                        {
                            drawX += fontInfo.SpacingWidth;
                            continue;
                        }

                        // Treat everything else as a standard character
                        CharacterInfo charInfo = fontInfo.GetCharacter(character);

                        if (charInfo != null) // Make sure this character is present in the font
                        {
                            Rectangle charRect = fontInfo.GetCharacter(character).GetBlitRectangle();

                            // The compare highests here are used to stop the X and Y values from becoming lower than 0
                            GFX.DrawImage(Fonts[fontName],
                                (drawX + charInfo.Before + charInfo.After).CompareHighest(0),
                                (drawY - charRect.Height - charInfo.YOffset).CompareHighest(0) + colourHeightModifier,
                                charRect, GraphicsUnit.Pixel);
                            drawX += (charRect.Width + charInfo.Before + charInfo.After).CompareHighest(0);
                        }
                    }

                    stringTexture.Save("U:\\Files\\test" + DateTime.Now.ToShortTimeString().Replace(':', '.') + ".png");
                }
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
        public static bool Load()
        {
            DisposeFonts();

            string[] fontsToLoad = new string[] { "ChatFont", "SmallFont", "MediumFont", "HugeFont" };

            foreach (string font in fontsToLoad)
            {
                string fontSource = RozWorld.Textures.GetFontSource(font);

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
    }
}
