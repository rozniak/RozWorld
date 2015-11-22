using RozWorld.IO;

using System.Collections.Generic;
using System.IO;


namespace RozWorld.Graphics.UI.Strings
{
    public class Language
    {
        private Dictionary<string, string> AvailableStrings;
        private string Source;
        private bool Loaded;


        public Language(string source)
        {
            AvailableStrings = new Dictionary<string, string>();
            Source = source;
            Loaded = false;
        }


        /// <summary>
        /// Gets a string with the associated string ID from the loaded dictionary.
        /// </summary>
        /// <param name="stringID">The string's ID.</param>
        /// <returns>The string of the specified ID if it exists, an empty string otherwise.</returns>
        public string GetString(string stringID)
        {
            if (AvailableStrings.ContainsKey(stringID))
                return AvailableStrings[stringID];

            return "Undefined";
        }


        /// <summary>
        /// Loads this language with all the strings from the source it was given.
        /// </summary>
        public void Load()
        {
            Unload();

            if (File.Exists(Files.LanguagesDirectory + "\\" + Source))
            {
                AvailableStrings = Files.ReadINIToDictionary(Source);
                Loaded = true;
            }
        }


        /// <summary>
        /// Clears the loaded strings.
        /// </summary>
        public void Unload()
        {
            AvailableStrings.Clear();
            Loaded = false;
        }
    }
}
