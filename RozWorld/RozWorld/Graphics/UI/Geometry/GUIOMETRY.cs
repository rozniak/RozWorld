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
            // Check if there is a GUIOMETRY.BIN file to load
            string guiometryLocation = Files.LinksDirectory + @"\guiometry.bin";

            if (System.IO.File.Exists(guiometryLocation))
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
                        } while (!endOfString || currentIndex <= guiometryFile.Count - 2);
                    }
                } while (!finishedMetadata || currentIndex <= guiometryFile.Count - 1);

                // Read the main GUIOMETRY file from here onwards...
            }

            return false;
        }
    }
}
