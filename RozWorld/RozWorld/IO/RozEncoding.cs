/**
 * RozWorld.IO.RozEncoding -- RozWorld Text Encoding
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Text;


namespace RozWorld.IO
{
    public static class RozEncoding
    {
        /// <summary>
        /// Gets the conversion of RozEncoded bytes to a string.
        /// </summary>
        /// <param name="data">The RozEncoded bytes to convert.</param>
        /// <returns>The resulting conversion.</returns>
        public static string GetString(byte[] data)
        {
            string resultingString = "";

            for (int i = 0; i <= data.Length; i++)
            {
                resultingString += GetCharFromByte(data[i]);
            }

            return resultingString;
        }


        /// <summary>
        /// Gets the conversion of a string to RozEncoded bytes.
        /// </summary>
        /// <param name="data">The string to convert.</param>
        /// <returns>The resulting conversion.</returns>
        public static byte[] GetBytes(string data)
        {
            byte[] resultingByteArray = new byte[data.Length];

            for (int i = 0; i <= data.Length; i++)
            {
                resultingByteArray[i] = GetByteFromChar(data[i]);
            }

            return resultingByteArray;
        }


        /// <summary>
        /// Gets the conversion of an individual character to a RozEncoded byte.
        /// </summary>
        /// <param name="data">The character to convert.</param>
        /// <returns>The resulting conversion.</returns>
        public static byte GetByteFromChar(char data)
        {
            byte charByte = Encoding.ASCII.GetBytes(new char[] { data })[0];

            if (charByte >= 48 && charByte <= 57)  // 0 - 9
            {
                return (byte)(charByte - 47);
            }
            else if (charByte >= 65 && charByte <= 90)  // A - Z
            {
                return (byte)(charByte - 54);
            }
            else if (charByte >= 97 && charByte <= 122)  // a - z
            {
                return (byte)(charByte - 60);
            }
            else
            {
                switch (data)
                {
                    case ' ': return 0;
                    case '!': return 63;
                    case '~': return 64;
                    case '#': return 65;
                    case '£': return 66;
                    case '$': return 67;
                    case '=': return 68;
                    case ':': return 69;
                    case '(': return 70;
                    case ')': return 71;
                    case '/': return 72;
                    case '\\': return 73;
                    case ';': return 74;
                    case '-': return 75;
                    case '+': return 76;
                    case '"': return 77;
                    case '?': return 78;
                    case '@': return 79;
                    case '\'': return 80;
                    case '>': return 81;
                    case '<': return 82;
                    case '.': return 83;
                    case ',': return 84;
                    case '_': return 85;
                    case '[': return 86;
                    case ']': return 87;
                    case '{': return 88;
                    case '}': return 89;
                    case '&': return 90;
                    case '%': return 91;
                    case '|': return 92;
                    case '*': return 253;
                    case '^': return 255;
                    default: return 254;
                }
            }
        }


        /// <summary>
        /// Gets the conversion of an individual RozEncoded byte.
        /// </summary>
        /// <param name="data">The RozEncoded byte to convert.</param>
        /// <returns>The resulting conversion.</returns>
        public static char GetCharFromByte(byte data)
        {
            if (data == 0)
            {
                return ' ';
            }
            else if (data >= 1 && data <= 10)
            {
                return Encoding.ASCII.GetString(new byte[] { (byte)(data + 47) })[0];
            }
            else if (data >= 11 && data <= 36)
            {
                return Encoding.ASCII.GetString(new byte[] { (byte)(data + 54) })[0];
            }
            else if (data >= 37 && data <= 62)
            {
                return Encoding.ASCII.GetString(new byte[] { (byte)(data + 60) })[0];
            }
            else
            {
                switch (data)
                {
                    case 63: return '!';
                    case 64: return '~';
                    case 65: return '#';
                    case 66: return '£';
                    case 67: return '$';
                    case 68: return '=';
                    case 69: return ':';
                    case 70: return '(';
                    case 71: return ')';
                    case 72: return '/';
                    case 73: return '\\';
                    case 74: return ';';
                    case 75: return '-';
                    case 76: return '+';
                    case 77: return '"';
                    case 78: return '?';
                    case 79: return '@';
                    case 80: return '\'';
                    case 81: return '>';
                    case 82: return '<';
                    case 83: return '.';
                    case 84: return ',';
                    case 85: return '_';
                    case 86: return '[';
                    case 87: return ']';
                    case 88: return '{';
                    case 89: return '}';
                    case 90: return '&';
                    case 91: return '%';
                    case 92: return '|';
                    case 253: return '*';
                    case 255: return '^';
                    default: return '€';
                }
            }
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
        private static string StripWindowsSafe(string text)
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
    }
}