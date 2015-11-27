/**
 * RozWorld.Graphics.UI.Strings.LanguageSystem -- RozWorld Language System
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
    public class LanguageSystem
    {
        private Dictionary<string, Language> AvailableLanguages = new Dictionary<string,Language>();

        private string _SelectedLanguage;
        private string SelectedLanguage
        {
            get { return this._SelectedLanguage; }
            set
            {
                if(!string.IsNullOrEmpty(this._SelectedLanguage))
                    AvailableLanguages[this._SelectedLanguage].Unload();

                AvailableLanguages[value].Load();
                this._SelectedLanguage = value;
            }
        }


        /// <summary>
        /// Gets a language string from the currently selected language with the given string ID.
        /// </summary>
        /// <param name="stringID">The string's ID.</param>
        /// <returns>The string of the specified ID if it exists, an empty string otherwise.</returns>
        public string GetString(string stringID)
        {
            if (SelectedLanguage != "" && AvailableLanguages.ContainsKey(SelectedLanguage))
                return AvailableLanguages[SelectedLanguage].GetString(stringID);

            return "Undefined";
        }


        /// <summary>
        /// Load or reload the languages for RozWorld.
        /// </summary>
        /// <param name="initialLanguage">The name of the language to try and select once loading is complete.</param>
        public void Load(string initialLanguage = "")
        {
            // If value is null, set it to an empty string
            initialLanguage = string.IsNullOrEmpty(initialLanguage) ?
                "" :
                initialLanguage;

            if (File.Exists(Files.LanguagesFile))
            {
                // languageFiles.Keys = The *nice-names* of the languages eg. British English
                // languageFiles.Values = The filenames of the languages eg. en_brit.ini
                var languageFiles = Files.ReadINIToDictionary(Files.LanguagesFile);

                foreach (var languageFile in languageFiles)
                {
                    if (File.Exists(Files.LanguagesDirectory + "\\" + languageFile.Value))
                        AvailableLanguages.Add(languageFile.Key, new Language(languageFile.Value));
                }

                if (AvailableLanguages.ContainsKey(initialLanguage))
                    SelectedLanguage = initialLanguage;
                else if (AvailableLanguages.Count > 0)
                {
                    foreach (var firstKey in AvailableLanguages.Keys)
                    {
                        SelectedLanguage = firstKey;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// Try to select a language from the loaded languages.
        /// </summary>
        /// <param name="languageName">The nice-name of the language to select.</param>
        /// <returns>Whether the language was successfully selected or not.</returns>
        public bool TrySelect(string languageName)
        {
            if (AvailableLanguages.ContainsKey(languageName))
            {
                SelectedLanguage = languageName;
                return true;
            }

            return false;
        }
    }
}
