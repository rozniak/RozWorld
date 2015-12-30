/**
 * RozWorld.IO.StringFunction -- RozWorld String Functions
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RozWorld.IO
{
    public static class StringFunction
    {
        /// <summary>
        /// Replaces special directory identifiers with their corresponding paths.
        /// </summary>
        /// <param name="line">The string data to replace identifiers in.</param>
        /// <returns>The string with the identifiers replaced with the special directory paths.</returns>
        public static string ReplaceSpecialDirectories(string line)
        {
            string finalReplaced = line.Replace("%sounds%", Files.SoundsDirectory);
            finalReplaced = finalReplaced.Replace("%tex%", Files.TexturesDirectory);
            finalReplaced = finalReplaced.Replace("%lang%", Files.LanguagesDirectory);
            finalReplaced = finalReplaced.Replace("%link%", Files.LinksDirectory);
            finalReplaced = finalReplaced.Replace("%mods%", Files.ModsDirectory);
            return finalReplaced;
        }


        /// <summary>
        /// Attempts to split a string by a pattern on its first occurrence.
        /// </summary>
        /// <param name="pattern">The pattern to split the string by.</param>
        /// <param name="text">The string to split.</param>
        /// <returns>The split string at the first occurrence of the pattern, if pattern doesn't exit, returns two empty strings.</returns>
        public static string[] SplitFirstInstance(string pattern, string text)
        {
            string[] resultingSplit = new string[] { "", "" };

            if (text.Contains(pattern))
            {
                int splitIndex = text.IndexOf(pattern, 0, text.Length, StringComparison.CurrentCulture);

                resultingSplit[0] = text.Substring(0, splitIndex);
                resultingSplit[1] = text.Substring(splitIndex + pattern.Length, text.Length - (pattern.Length + splitIndex));
            }

            return resultingSplit;
        }


        /// <summary>
        /// Strips special characters out of a string and also strips characters relating to the specified strip type.
        /// </summary>
        /// <param name="text">The string to strip.</param>
        /// <param name="stripType">The strip type to follow.</param>
        /// <returns>The stripped string.</returns>
        public static string StripSpecialCharacters(string text, StripType stripType)
        {
            string strippedText = text.Replace("€", "");
            strippedText = strippedText.Replace("*", "");
            strippedText = strippedText.Replace("^", "");

            switch (stripType)
            {
                case StripType.SemiColons:
                    strippedText.Replace(";", "");

                    break;

                case StripType.WindowsSafe:
                    strippedText = StripWindowsSafe(strippedText);

                    break;

                case StripType.Both:
                    strippedText = strippedText.Replace(";", "");
                    strippedText = StripWindowsSafe(strippedText);

                    break;
            }

            return strippedText;
        }


        /// <summary>
        /// Strips unsafe characters out of a string to make it safe for Windows.
        /// </summary>
        /// <param name="text">The string to strip.</param>
        /// <returns>The stripped string.</returns>
        public static string StripWindowsSafe(string text)
        {
            string strippedText = text.Replace("\\", "");
            strippedText = strippedText.Replace("/", "");
            strippedText = strippedText.Replace(":", "");
            strippedText = strippedText.Replace("*", "");
            strippedText = strippedText.Replace("?", "");
            strippedText = strippedText.Replace("\"", "");
            strippedText = strippedText.Replace("<", "");
            strippedText = strippedText.Replace(">", "");
            strippedText = strippedText.Replace("|", "");
            return strippedText;
        }


        /// <summary>
        /// Strips the specified formatting specials out of a string.
        /// </summary>
        /// <param name="formatStripMode">The formatting specials to strip.</param>
        /// <returns>The stripped string.</returns>
        public static string StripStringFormatting(StringFormatting formatStripMode, string text)
        {
            string strippedText = text;

            if (formatStripMode == StringFormatting.Colours ||
                formatStripMode == StringFormatting.Both)
            {
                
            }

            if (formatStripMode == StringFormatting.LineBreaks ||
                formatStripMode == StringFormatting.Both)
            {
                
            }

            return string.Empty; // just for the build process atm
        }
    }
}
