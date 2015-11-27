/**
 * RozWorld.Graphics.UI.Geometry.GUIOMETRY -- RozWorld UI Geometry
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.IO;

using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace RozWorld.Graphics.UI.Geometry
{
    public class GUIOMETRY
    {
        private Dictionary<string, FontInfo> Fonts = new Dictionary<string, FontInfo>();
        private Dictionary<string, ElementInfo> Elements = new Dictionary<string, ElementInfo>();

        public bool ButtonCentredText { get; private set; }
        public sbyte ButtonOffsetTop { get; private set; }
        public sbyte ButtonOffsetLeft { get; private set; }

        public bool TextCentredText { get; private set; }
        public sbyte TextOffsetTop { get; private set; }
        public sbyte TextOffsetLeft { get; private set; }


        public GUIOMETRY()
        {
            if (Fonts.Count == 0 || Elements.Count == 0)
                BuildKeys();
        }


        /// <summary>
        /// Initialises or rebuilds all font and element keys in their respective Dictionary collections.
        /// </summary>
        private void BuildKeys()
        {
            // Clear old keys
            Fonts.Clear();
            Elements.Clear();

            // Add font keys
            Fonts.Add("ChatFont", new FontInfo());
            Fonts.Add("SmallFont", new FontInfo());
            Fonts.Add("MediumFont", new FontInfo());
            Fonts.Add("HugeFont", new FontInfo());

            // Add element keys

            // Button ElementInfos
            Elements.Add("ButtonBody", new ElementInfo());
            Elements.Add("ButtonTop", new ElementInfo());
            Elements.Add("ButtonSide", new ElementInfo());
            Elements.Add("ButtonEdgeSE", new ElementInfo());
            Elements.Add("ButtonEdgeSW", new ElementInfo());

            // TextBox ElementInfos
            Elements.Add("TextBody", new ElementInfo());
            Elements.Add("TextTop", new ElementInfo());
            Elements.Add("TextSide", new ElementInfo());
            Elements.Add("TextEdgeSE", new ElementInfo());
            Elements.Add("TextEdgeSW", new ElementInfo());

            // CheckBox ElementInfos
            Elements.Add("CheckBody", new ElementInfo());
            Elements.Add("CheckTop", new ElementInfo());
            Elements.Add("CheckSide", new ElementInfo());
            Elements.Add("CheckEdgeSE", new ElementInfo());
            Elements.Add("CheckEdgeSW", new ElementInfo());
            Elements.Add("CheckTick", new ElementInfo());
        }


        /// <summary>
        /// Gets an ElementInfo from this GUIOMETRY's element collection of the specified name.
        /// </summary>
        /// <param name="name">The name of the ElementInfo to get.</param>
        /// <returns>The ElementInfo of the specified name if it exists, null otherwise.</returns>
        public ElementInfo GetElement(string name)
        {
            if (Elements.ContainsKey(name))
                return Elements[name];

            return null;
        }


        /// <summary>
        /// Gets a FontInfo from this GUIOMETRY's font collection of the specified name.
        /// </summary>
        /// <param name="name">The name of the FontInfo to get.</param>
        /// <returns>The FontInfo of the specified name if it exists, null otherwise.</returns>
        public FontInfo GetFont(string name)
        {
            if (Fonts.ContainsKey(name))
                return Fonts[name];

            return null;
        }


        /// <summary>
        /// Attempts to load or reload GUIOMETRY data from the game's guiometry.bin file.
        /// </summary>
        public void Load()
        {
            // Check if there is a GUIOMETRY.BIN file to load
            string guiometryLocation = Files.LinksDirectory + @"\guiometry.bin";

            if (File.Exists(guiometryLocation))
            {
                // Clear old data from the collections
                BuildKeys();

                // Read the GUIOMETRY file...
                IList<byte> guiometryFile = Files.GetBinaryFile(guiometryLocation);
                int currentIndex = 0; // The current index pointer

                // Get the version before doing anything
                byte version = guiometryFile[currentIndex++];

                // Set this to true to move onto the next section
                bool nextSection = false;

                // Skip the metadata
                while (!nextSection && currentIndex <= guiometryFile.Count - 1)
                {
                    byte textureID = ByteParse.NextByte(guiometryFile, ref currentIndex);

                    // Check if the metadata is finished
                    if (textureID == 0)
                        nextSection = true;
                    else
                        ByteParse.NextString(guiometryFile, ref currentIndex); // Read the next string to nothing
                }

                
                // Actual reading of the GUIOMETRY data starts here, version by version!
                // Versions split into #region blocks just because they're so nice (and I'm so nice too!)

                if (version == 1)
                {
                    #region Version 1

                    // This should be the fonts to look for (eg. ChatFont, SmallFont etc.) in the order they are
                    // supposed to be according to the format
                    string[] fonts = new string[] { "ChatFont", "SmallFont", "MediumFont", "HugeFont" };

                    // Keep track of the current character being read (out of the number in the font)
                    short currentChar = 0; // Make sure to reset this to 0 after reading a font or you will be shot

                    // However many characters are in the font being read
                    short charsInFont;


                    // This should be the elements to look for (eg. Button, Text etc.) in the order they are
                    // supposed to be according to the format
                    string[] elements = new string[] { "Button", "Text", "Check" };

                    // This should be the 'parts' of the elements to look for (eg. Body, Top, Side etc.) in the
                    // order they are supposed to be according to the format
                    string[] elementParts;


                    #region Font Data

                    foreach (string font in fonts)
                    {
                        charsInFont = ByteParse.NextShort(guiometryFile, ref currentIndex);

                        while (currentChar++ < charsInFont)
                        {
                            char character = ByteParse.NextChar(guiometryFile, ref currentIndex);

                            Fonts[font].AddNewCharacter(character,
                                NextCharacter(guiometryFile, ref currentIndex));
                        }

                        Fonts[font].SpacingWidth = ByteParse.NextByte(guiometryFile, ref currentIndex);
                        Fonts[font].LineHeight = ByteParse.NextByte(guiometryFile, ref currentIndex);

                        currentChar = 0;
                    }

                    #endregion


                    #region Element Data

                    foreach (string element in elements)
                    {
                        // Set up the parts to look for...
                        if (element == "Button" || element == "Text")
                            elementParts = new string[] { "Top", "Side", "EdgeSE", "EdgeSW" };
                        else // element == "CheckBox"
                            elementParts = new string[] { "Top", "Side", "EdgeSE", "EdgeSW", "Tick" };

                        foreach (string part in elementParts)
                        {
                            if (part == "Side" || part == "EdgeSE" || part == "EdgeSW" || part == "Tick")
                                Elements[element + part].XOffset = ByteParse.NextSByte(guiometryFile, ref currentIndex);

                            if (part == "Top" || part == "EdgeSE" || part == "EdgeSW" || part == "Tick")
                                Elements[element + part].YOffset = ByteParse.NextSByte(guiometryFile, ref currentIndex);
                        }

                        // Read the element details too
                        if (element == "Button")
                        {
                            ButtonCentredText = ByteParse.NextBool(guiometryFile, ref currentIndex);
                            ButtonOffsetTop = ByteParse.NextSByte(guiometryFile, ref currentIndex);
                            ButtonOffsetLeft = ByteParse.NextSByte(guiometryFile, ref currentIndex);
                        }
                        else if (element == "Text")
                        {
                            TextCentredText = ByteParse.NextBool(guiometryFile, ref currentIndex);
                            TextOffsetTop = ByteParse.NextSByte(guiometryFile, ref currentIndex);
                            TextOffsetLeft = ByteParse.NextSByte(guiometryFile, ref currentIndex);
                        }
                    }

                    #endregion


                    #endregion

                    return; // Successfully loaded!
                }
            }

            // Unable to load for whatever reason
            UIHandler.CriticalError(Error.BROKEN_GUIOMETRY_FILE);
        }


        /// <summary>
        /// (For internal use) Attempts to read the next character from a GUIOMETRY file into a CharacterInfo object.
        /// </summary>
        /// <param name="data">The GUIOMETRY data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>A new CharacterInfo object containing as much of the character info that could be read.</returns>
        private CharacterInfo NextCharacter(IList<byte> data, ref int currentIndex)
        {
            CharacterInfo charInfo = new CharacterInfo(); // Build the character up

            short blitOriginX = ByteParse.NextShort(data, ref currentIndex);
            short blitOriginY = ByteParse.NextShort(data, ref currentIndex);
            charInfo.BlitOrigin = new Point(blitOriginX, blitOriginY);

            short blitDestinationX = ByteParse.NextShort(data, ref currentIndex);
            short blitDestinationY = ByteParse.NextShort(data, ref currentIndex);
            charInfo.BlitDestination = new Point(blitDestinationX, blitDestinationY);

            charInfo.Before = ByteParse.NextSByte(data, ref currentIndex);
            charInfo.After = ByteParse.NextSByte(data, ref currentIndex);
            charInfo.YOffset = ByteParse.NextSByte(data, ref currentIndex);

            return charInfo;
        }
    }
}
