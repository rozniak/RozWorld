/**
 * RozWorld.Graphics.UI.Geometry.FontInfo -- RozWorld UI Font Information
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System.Collections.Generic;

namespace RozWorld.Graphics.UI.Geometry
{
    public class FontInfo
    {
        private Dictionary<char, CharacterInfo> Characters = new Dictionary<char, CharacterInfo>();

        public byte SpacingWidth = 0;
        public byte LineHeight = 0;


        /// <summary>
        /// Adds a new character into this font's character list.
        /// </summary>
        /// <param name="newChar">The character to add.</param>
        /// <param name="charInfo">The character information of the character to add.</param>
        /// <returns>Whether the character was added, otherwise it is already present.</returns>
        public bool AddNewCharacter(char newChar, CharacterInfo charInfo)
        {
            if (!Characters.ContainsKey(newChar))
            {
                Characters.Add(newChar, charInfo);
                return true;
            }

            return false;
        }


        /// <summary>
        /// Gets the character information of the specified character key.
        /// </summary>
        /// <param name="key">The character to get the info of.</param>
        /// <returns>The character's info if it is present, null otherwise.</returns>
        public CharacterInfo GetCharacter(char key)
        {
            if (Characters.ContainsKey(key)) return Characters[key];
            return null;
        }


        /// <summary>
        /// Gets a read-only list of all the characters present in this font's information.
        /// </summary>
        /// <returns>A read-only list containing all the characters inside this font's information.</returns>
        public IList<char> GetListCharacters()
        {
            var characters = new List<char>();

            foreach (char character in Characters.Keys)
            {
                characters.Add(character);
            }

            return characters.AsReadOnly();
        }


        /// <summary>
        /// Removes a character from this font's character list.
        /// </summary>
        /// <param name="newChar">The character to remove.</param>
        /// <returns>Whether the character was remove, otherwise it didn't exist in the first place.</returns>
        public bool RemoveCharacter(char removeChar)
        {
            if (Characters.ContainsKey(removeChar))
            {
                Characters.Remove(removeChar);
                return true;
            }

            return false;
        }
    }
}
