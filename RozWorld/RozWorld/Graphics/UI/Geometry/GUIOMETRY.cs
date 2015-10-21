/**
 * RozWorld.Graphics.UI.Geometry.GUIOMETRY -- RozWorld UI Geometry
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RozWorld.IO;
using System.Drawing;
using System.IO;

namespace RozWorld.Graphics.UI.Geometry
{
    public class GUIOMETRY
    {
        private Dictionary<string, FontInfo> Fonts = new Dictionary<string, FontInfo>();
        private Dictionary<string, ElementInfo> Elements = new Dictionary<string, ElementInfo>();

        public bool CentredTextButton { get; private set; }
        public sbyte OffsetButtonTop { get; private set; }
        public sbyte OffsetButtonLeft { get; private set; }

        public bool CentredTextText { get; private set; }
        public sbyte OffsetTextTop { get; private set; }
        public sbyte OffsetTextLeft { get; private set; }


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
        /// <returns>Whether the GUIOMETRY data was successfully loaded or not.</returns>
        public bool Load()
        {
            // Note to anyone reading (if they're interested or if there's anyone reading to begin with)...
            // This function is probably pretty badly made, it might be 'cleaned up' later, or it might not.
            //
            // Right now it's just a 'get it working' job, if it *is* messy, feel free to call me an idiot
            // and tell me what's wrong (I'd prefer it if you skipped the name-calling, but in any case, help
            // is help).
            //
            // I could probably copy and paste this message for the entire RozWorld code, but this is one case
            // I feel like it might be most applicable to. I really have no idea.
            //
            // Cheers anyway. xoxoxo


            /**
             * THIS CODE IS UNTESTED, DO NOT USE IT PRACTICALLY YET!!!!
             * and if you do - it's your fault, not mine
             */


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

                // Skip past the fast section of bytes as it is metadata for the editor (textures)
                bool finishedMetadata = false;

                do
                {
                    if (guiometryFile[currentIndex++] == 0) // End of metadata
                    {
                        finishedMetadata = true;
                    }
                    else
                    {
                        // Time to skip through the texture source string...
                        bool endOfString = false;

                        // Strings *should* be in UTF16 format, so two bytes per char, the exit condition
                        // makes sure that there are at least two bytes left to read, or the string has
                        // terminated...

                        do
                        {
                            // Check if the next two bytes are null or not
                            if (guiometryFile[currentIndex] == 0 &&
                                guiometryFile[currentIndex + 1] == 0)
                            {
                                endOfString = true;
                            }

                            currentIndex += 2;
                        } while (!endOfString && currentIndex <= guiometryFile.Count - 2);
                    }
                } while (!finishedMetadata && currentIndex <= guiometryFile.Count - 1);

                
                // Actual reading of the GUIOMETRY data starts here, version by version!
                // Versions split into #region blocks just because they're so nice (and I'm so nice too!)

                if (version == 1)
                {
                    #region Version 1

                    // Keep track of the current character being read (out of the number in the font)
                    short currentChar = 1; // Make sure to reset this to 1 after reading a font or you will be shot

                    // However many characters are in the font being read
                    short charsInFont;


                    #region Chat Font Data

                    charsInFont = ByteParse.NextShort(guiometryFile, ref currentIndex);

                    do
                    {
                        char character = ByteParse.NextChar(guiometryFile, ref currentIndex);

                        Fonts["ChatFont"].AddNewCharacter(character, 
                            NextCharacter(guiometryFile, ref currentIndex));
                    } while (currentChar++ < charsInFont && currentIndex <= guiometryFile.Count - 1);

                    currentChar = 1;

                    #endregion


                    #region Small Font Data

                    charsInFont = ByteParse.NextShort(guiometryFile, ref currentIndex);

                    do
                    {
                        char character = ByteParse.NextChar(guiometryFile, ref currentIndex);

                        Fonts["SmallFont"].AddNewCharacter(character,
                            NextCharacter(guiometryFile, ref currentIndex));
                    } while (currentChar++ < charsInFont && currentIndex <= guiometryFile.Count - 1);

                    currentChar = 1;

                    #endregion


                    #region Medium Font Data

                    charsInFont = ByteParse.NextShort(guiometryFile, ref currentIndex);

                    do
                    {
                        char character = ByteParse.NextChar(guiometryFile, ref currentIndex);

                        Fonts["MediumFont"].AddNewCharacter(character,
                            NextCharacter(guiometryFile, ref currentIndex));
                    } while (currentChar++ < charsInFont && currentIndex <= guiometryFile.Count - 1);

                    currentChar = 1;

                    #endregion


                    #region Huge Font Data

                    charsInFont = ByteParse.NextShort(guiometryFile, ref currentIndex);

                    do
                    {
                        char character = ByteParse.NextChar(guiometryFile, ref currentIndex);

                        Fonts["HugeFont"].AddNewCharacter(character,
                            NextCharacter(guiometryFile, ref currentIndex));
                    } while (currentChar++ < charsInFont && currentIndex <= guiometryFile.Count - 1);

                    currentChar = 1;

                    #endregion

                    // TODO: Read element data from here and then test everything


                    #endregion
                }
            }

            return false;
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

            // Check if the file has 3 more bytes left at least
            if (currentIndex <= data.Count - 4)
            {
                charInfo.Before = (sbyte)data[currentIndex++];
                charInfo.After = (sbyte)data[currentIndex++];
                charInfo.YOffset = (sbyte)data[currentIndex++];
            }

            return charInfo;
        }
    }
}
