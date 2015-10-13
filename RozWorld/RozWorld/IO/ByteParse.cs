/**
 * RozWorld.IO.ByteParse -- Byte Based Data Parsing Functions
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System.Collections.Generic;

namespace RozWorld.IO
{
    public static class ByteParse
    {
        /**
         * I KNOW THIS ALL RETURNS DEFAULT VALUES, JUST GIVE ME A SEC AND IT'LL BE TOP-NOTCH.
         * 
         * Maybe not top-notch, but it'll work, I think.
         */

        /// <summary>
        /// Reads the next byte into a boolean value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next byte in the data as a boolean value.</returns>
        public static bool NextBool(IList<byte> data, ref int currentIndex)
        {
            return true;
        }


        /// <summary>
        /// Reads the next 4 bytes into a signed 32-bit integer value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 4 bytes in the data as a signed 32-bit integer value.</returns>
        public static int NextInt(IList<byte> data, ref int currentIndex)
        {
            return 0;
        }


        /// <summary>
        /// Reads the next 4 bytes into an unsigned 32-bit integer value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 4 bytes in the data as an unsigned 32-bit integer value.</returns>
        public static uint NextUInt(IList<byte> data, ref int currentIndex)
        {
            return 0;
        }


        /// <summary>
        /// Reads the next 2 bytes into a signed 16-bit integer value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 2 bytes in the data as a signed 16-bit integer value.</returns>
        public static short NextShort(IList<byte> data, ref int currentIndex)
        {
            return 0;
        }


        /// <summary>
        /// Reads the next 2 bytes into an unsigned 16-bit integer value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 2 bytes in the data as an unsigned 16-bit integer value.</returns>
        public static ushort NextUShort(IList<byte> data, ref int currentIndex)
        {
            return 0;
        }


        /// <summary>
        /// Reads the next 8 bytes into a signed 64-bit integer value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 8 bytes in the data as a signed 64-bit integer value.</returns>
        public static long NextLong(IList<byte> data, ref int currentIndex)
        {
            return 0;
        }


        /// <summary>
        /// Reads the next 8 bytes into an unsigned 64-bit integer value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 8 bytes in the data as an unsigned 64-bit integer value.</returns>
        public static ulong NextULong(IList<byte> data, ref int currentIndex)
        {
            return 0;
        }


        /// <summary>
        /// Reads the next set of bytes into a string.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next set of bytes in the data as a string, terminated by a null character or end of data.</returns>
        public static string NextString(IList<byte> data, ref int currentIndex)
        {
            return string.Empty;
        }


        /// <summary>
        /// Reads the next 2 bytes into a Unicode character value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 2 bytes in the data as a character value.</returns>
        public static char NextChar(IList<byte> data, ref int currentIndex)
        {
            return ' ';
        }
    }
}
