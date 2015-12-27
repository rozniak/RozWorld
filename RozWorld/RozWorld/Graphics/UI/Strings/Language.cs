/**
 * RozWorld.Graphics.UI.Strings.Language -- RozWorld Language Class
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

            if (Directory.Exists(Files.LanguagesDirectory + "\\" + Source))
            {
                foreach (string languageFile in Directory.GetFiles(Files.LanguagesDirectory + "\\" + Source))
                {
                    if (languageFile.EndsWith(".ini"))
                    {
                        // Add all the strings from the file to the dictionary
                        foreach (var item in Files.ReadINIToDictionary(languageFile))
                        {
                            AvailableStrings.Add(item.Key, item.Value);
                        }
                    }
                }

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
