/**
 * RozWorld.ExtensionMethods -- Project Extension Methods
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System.Text.RegularExpressions;

namespace RozWorld
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Check whether this character is equal to another, regardless of case.
        /// </summary>
        /// <param name="comparison">The character to compare to.</param>
        /// <returns>Whether the characters are equal, regardless of case.</returns>
        public static bool EqualsIgnoreCase(this char subject, char comparison)
        {
            return subject.ToString().ToLower() == comparison.ToString().ToLower();
        }


        /// <summary>
        /// Check whether this string is equal to another, regardless of case.
        /// </summary>
        /// <param name="comparison">The string to compare to.</param>
        /// <returns>Whether the strings are equal, regardless of case.</returns>
        public static bool EqualsIgnoreCase(this string subject, string comparison)
        {
            return subject.ToLower() == comparison.ToLower();
        }


        /// <summary>
        /// Checks whether this character is a valid hexadecimal digit.
        /// </summary>
        /// <returns>Whether this character is a hexadecimal digit or not.</returns>
        public static bool IsHexChar(this char subject)
        {
            Regex hexRule = new Regex("^[a-fA-F0-9_]*$");
            return hexRule.Match(subject.ToString()).Success;
        }


        /// <summary>
        /// If the value of this integer is below the limit, it will be bumped to the limit.
        /// </summary>
        /// <param name="limit">The limit value to bump towards.</param>
        //public static void BumpLowerThan(this int subject, int limit)
        //{
        //    if (subject < limit)
        //        subject = limit;
        //}


        /// <summary>
        /// Compares the subject to a value to see which is the lowest.
        /// </summary>
        /// <param name="comparison">The value to test against.</param>
        /// <returns>The lowest of the two values.</returns>
        public static int CompareLowest(this int subject, int comparison)
        {
            if (comparison < subject)
                return comparison;
            return subject;
        }


        /// <summary>
        /// Compares the subject to a value to see which is the highest.
        /// </summary>
        /// <param name="comparsion">The value to test against.</param>
        /// <returns>The highest of the two values.</returns>
        public static int CompareHighest(this int subject, int comparison)
        {
            if (comparison > subject)
                return comparison;
            return subject;
        }


        /// <summary>
        /// Returns a copy of this character coverted to lowercase.
        /// </summary>
        /// <returns>A copy of the character in lowercase.</returns>
        public static char ToLower(this char subject)
        {
            return subject.ToString().ToLower()[0];
        }


        /// <summary>
        /// Returns a copy of this character converted to uppercase.
        /// </summary>
        /// <returns>A copy of the character in uppercase</returns>
        public static char ToUpper(this char subject)
        {
            return subject.ToString().ToUpper()[0];
        }
    }
}
