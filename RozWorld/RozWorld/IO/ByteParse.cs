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
using System.Text;

namespace RozWorld.IO
{
    public static class ByteParse
    {
        /// <summary>
        /// Reads the next byte into a boolean value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next byte in the data as a boolean value.</returns>
        public static bool NextBool(IList<byte> data, ref int currentIndex)
        {
            if (data[currentIndex++] == 1) return true;

            return false; // Assume that it's 0, because if it isn't, then that's just silly
        }


        /// <summary>
        /// Reads the next 2 bytes into a Unicode character value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 2 bytes in the data as a character value.</returns>
        public static char NextChar(IList<byte> data, ref int currentIndex)
        {
            char conversion = '\0';

            if (data.Count - 1 >= currentIndex + 1)
            {
                conversion = UnicodeEncoding.Unicode.GetString(new byte[] { data[currentIndex], data[currentIndex + 1] })[0];
                currentIndex += 2;
            }

            return conversion;
        }


        /// <summary>
        /// Reads the next 4 bytes into a signed 32-bit integer value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 4 bytes in the data as a signed 32-bit integer value.</returns>
        public static int NextInt(IList<byte> data, ref int currentIndex)
        {
            int conversion = 0;

            if (data.Count - 1 >= currentIndex + 3)
            {
                for (int i = 0; i <= 3; i++)
                {
                    conversion += (int)(data[currentIndex + i] << (24 - (8 * i)));
                }

                currentIndex += 4;
            }

            return conversion;
        }


        /// <summary>
        /// Reads the next 8 bytes into a signed 64-bit integer value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 8 bytes in the data as a signed 64-bit integer value.</returns>
        public static long NextLong(IList<byte> data, ref int currentIndex)
        {
            long conversion = 0;

            if (data.Count - 1 >= currentIndex + 7)
            {
                for (int i = 0; i <= 7; i++)
                {
                    conversion += (long)(data[currentIndex + i] << (56 - (8 * i)));
                }

                currentIndex += 8;
            }

            return conversion;
        }


        /// <summary>
        /// Reads the next 2 bytes into a signed 16-bit integer value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 2 bytes in the data as a signed 16-bit integer value.</returns>
        public static short NextShort(IList<byte> data, ref int currentIndex)
        {
            short conversion = 0;

            if (data.Count - 1 >= currentIndex + 1)
            {
                conversion = (short)((data[currentIndex] << 8) + data[currentIndex + 2]);
                currentIndex += 2;
            }

            return conversion;
        }


        /// <summary>
        /// Reads the next set of bytes into a string.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <param name="includeNullCharacter">Whether to include the terminating null character in the converted string or not.</param>
        /// <returns>Returns the next set of bytes in the data as a string, terminated by a null character or end of data.</returns>
        public static string NextString(IList<byte> data, ref int currentIndex, bool includeNullCharacter = false)
        {
            string conversion = string.Empty;
            bool endOfString = false; // Set this to true when a null character is discovered

            do
            {
                char nextChar = UnicodeEncoding.Unicode.GetString(new byte[] { data[currentIndex], data[currentIndex + 1] })[0];
                
                if (nextChar == '\0')
                {
                    if (includeNullCharacter)
                        conversion += nextChar;

                    endOfString = true;
                }
                else
                {
                    conversion += nextChar;
                }

                currentIndex += 2;
            } while (currentIndex < data.Count - 2 || endOfString);

            return conversion;
        }


        /// <summary>
        /// Reads the next 4 bytes into an unsigned 32-bit integer value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 4 bytes in the data as an unsigned 32-bit integer value.</returns>
        public static uint NextUInt(IList<byte> data, ref int currentIndex)
        {
            uint conversion = 0;

            if (data.Count - 1 >= currentIndex + 3)
            {
                for (int i = 0; i <= 3; i++)
                {
                    conversion += (uint)(data[currentIndex + i] << (24 - (8 * i)));
                }

                currentIndex += 4;
            }

            return conversion;
        }


        /// <summary>
        /// Reads the next 8 bytes into an unsigned 64-bit integer value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 8 bytes in the data as an unsigned 64-bit integer value.</returns>
        public static ulong NextULong(IList<byte> data, ref int currentIndex)
        {
            ulong conversion = 0;

            if (data.Count - 1 >= currentIndex + 7)
            {
                for (int i = 0; i <= 7; i++)
                {
                    conversion += (ulong)(data[currentIndex + i] << (56 - (8 * i)));
                }

                currentIndex += 8;
            }

            return conversion;
        }
        

        /// <summary>
        /// Reads the next 2 bytes into an unsigned 16-bit integer value.
        /// </summary>
        /// <param name="data">The byte data to read from.</param>
        /// <param name="currentIndex">The current index pointer.</param>
        /// <returns>Returns the next 2 bytes in the data as an unsigned 16-bit integer value.</returns>
        public static ushort NextUShort(IList<byte> data, ref int currentIndex)
        {
            ushort conversion = 0;

            if (data.Count - 1 >= currentIndex + 1)
            {
                conversion = (ushort)((data[currentIndex] << 8) + data[currentIndex + 2]);
                currentIndex += 2;
            }

            return conversion;
        }
    }
}
