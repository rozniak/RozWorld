/**
 * RozWorld.IO.Files -- RozWorld File/File System Management
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using RozWorld.COMFY;

namespace RozWorld.IO
{
    public static class Files
    {        
        public static readonly string ComfyDirectory = Environment.CurrentDirectory + "\\comfy";
        public static readonly string LanguagesDirectory = Environment.CurrentDirectory + "\\lang";
        public static readonly string SoundsDirectory = Environment.CurrentDirectory + "\\sounds";
        public static readonly string TexturesDirectory = Environment.CurrentDirectory + "\\tex";

        /// <summary>
        /// Gets a text file with the specified filename from the disk.
        /// </summary>
        /// <param name="fileName">The filename of the file to get.</param>
        /// <returns>The file contents if it exists.</returns>
        public static string[] GetTextFile(string fileName)
        {
            List<string> fileContents = new List<string>();

            if (File.Exists(fileName))
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    do
                    {
                        fileContents.Add(r.ReadLine());
                    } while (r.Peek() > -1);
                }

                return fileContents.ToArray();
            }
            
            return new string[] { };
        }


        /// <summary>
        /// Gets a binary file with the specified filename from the disk.
        /// </summary>
        /// <param name="fileName">The filename of the file to get.</param>
        /// <returns>The file contents if it exists.</returns>
        public static byte[] GetBinaryFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllBytes(fileName);
            }

            return new byte[] { };
        }


        /// <summary>
        /// Writes a text file with the specified filename to the disk.
        /// </summary>
        /// <param name="fileName">The filename of the file to write.</param>
        /// <param name="contents">The contents of the file to write.</param>
        /// <returns>Whether the file was successfully written or not.</returns>
        public static bool PutTextFile(string fileName, string[] contents)
        {
            try
            {
                using (StreamWriter w = new StreamWriter(fileName))
                {
                    for (int i = 0; i <= contents.Length - 1; i++)
                    {
                        w.WriteLine(contents[i]);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Writes a binary file with the specified filename to the disk.
        /// </summary>
        /// <param name="fileName">The filename of the file to write.</param>
        /// <param name="contents">The contents of the file to write.</param>
        /// <returns>Whether the file was successfully written or not.</returns>
        public static bool PutBinaryFile(string fileName, byte[] contents)
        {
            try
            {
                using (FileStream w = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    w.Write(contents, 0, contents.Length);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Gets the MD5 hash of a file with the specified filename from the disk.
        /// </summary>
        /// <param name="fileName">The filename of the file to get the MD5 hash from.</param>
        /// <returns>The MD5 hash of the file, if it exists.</returns>
        public static string GetMD5Hash(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(fileName))
                    {
                        return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
                    }
                }
            }

            return "0";
        }


        /// <summary>
        /// Parses a string into separate data objects.
        /// </summary>
        /// <param name="line">The string data to parse.</param>
        /// <returns>The array of data objects retrieved from the string data.</returns>
        public static ParsedLineData[] ParseLine(string line)
        {
            List<ParsedLineData> parsedLineData = new List<ParsedLineData>();

            bool insideQuotes = false;
            bool finished = false;
            bool escaped = false;
            bool individualFinished = false;
            bool startedDefining = false;
            bool objectWhitespaceSeparated = true;
            bool errors = false;
            int index = 0;

            string definingData = "";

            // Strip all tabs, because they are evil.
            line = line.Replace("\t", "    ");

            if (line.Length > 0)
            {
                do
                {
                    switch (line[index])
                    {
                        //                  //
                        // Handling spaces. //
                        //                  //
                        case ' ':
                            if (startedDefining)
                            {
                                if (insideQuotes)
                                {
                                    definingData += " ";
                                }
                                else
                                {
                                    individualFinished = true;
                                    objectWhitespaceSeparated = true;
                                }
                            }
                            else if (!objectWhitespaceSeparated)
                            {
                                objectWhitespaceSeparated = true;
                            }

                            break;

                        //                         //
                        // Handling hashes/pounds. //
                        //                         //
                        case '#':
                            if (startedDefining)
                            {
                                if (insideQuotes)
                                {
                                    definingData += "#";
                                }
                                else
                                {
                                    individualFinished = true;
                                    finished = true;
                                }
                            }
                            else
                            {
                                finished = true;
                            }

                            break;

                        //                  //
                        // Handling quotes. //
                        //                  //
                        case '"':
                            if (startedDefining)
                            {
                                if (insideQuotes)
                                {
                                    if (escaped)
                                    {
                                        definingData += "\"";
                                        escaped = false;
                                    }
                                    else
                                    {
                                        definingData += "\"";
                                        insideQuotes = false;
                                        individualFinished = true;
                                        objectWhitespaceSeparated = false;
                                    }
                                }
                                else
                                {
                                    errors = true;
                                    individualFinished = true;
                                    objectWhitespaceSeparated = false;
                                }
                            }
                            else
                            {
                                if (objectWhitespaceSeparated)
                                {
                                    startedDefining = true;
                                    definingData += "\"";
                                    insideQuotes = true;
                                }
                            }

                            break;

                        case '\\':
                            //                       //
                            // Handling backslashes. //
                            //                       //
                            if (insideQuotes)
                            {
                                if (escaped)
                                {
                                    definingData += "\\";
                                    escaped = false;
                                }
                                else
                                {
                                    escaped = true;
                                }
                            }
                            else
                            {
                                errors = true;
                                individualFinished = true;
                                objectWhitespaceSeparated = false;
                            }

                            break;

                        default:
                            //                           //
                            // Handling everything else. //
                            //                           //
                            if (startedDefining)
                            {
                                definingData += line[index];
                            }
                            else
                            {
                                if (objectWhitespaceSeparated)
                                {
                                    startedDefining = true;
                                    definingData += line[index];
                                }
                            }

                            break;
                    }

                    if (index == line.Length - 1 && startedDefining && !individualFinished)
                    {
                        individualFinished = true;
                    }

                    if (individualFinished)
                    {
                        startedDefining = false;

                        ParsedLineData parsedDataItem = new ParsedLineData();

                        parsedDataItem.Data = definingData;

                        if (!errors)
                        {
                            if (definingData.Length >= 2 && definingData[0] == '"' && definingData[definingData.Length - 1] == '"')
                            {
                                parsedDataItem.Data = definingData.Substring(1, definingData.Length - 2);
                                parsedDataItem.Type = ParsedObjectType.String;
                            }
                            else
                            {
                                int resultingParsedInt = 0;
                                bool successfulParse = int.TryParse(definingData, out resultingParsedInt);

                                if (successfulParse)
                                {
                                    parsedDataItem.Type = ParsedObjectType.Integer;
                                    parsedDataItem.Data = resultingParsedInt;
                                }
                                else
                                {
                                    parsedDataItem.Type = ParsedObjectType.Word;
                                }
                            }
                        }
                        else
                        {
                            parsedDataItem.Type = ParsedObjectType.Invalid;
                            errors = false;
                        }

                        parsedLineData.Add(parsedDataItem);

                        definingData = "";
                        individualFinished = false;
                    }

                    index++;
                } while (!finished && index <= line.Length - 1);
            }

            return parsedLineData.ToArray();
        }


        /// <summary>
        /// Replaces special directory identifiers with their corresponding paths.
        /// </summary>
        /// <param name="line">The string data to replace identifiers in.</param>
        /// <returns>The string with the identifiers replaced with the special directory paths.</returns>
        public static string ReplaceSpecialDirectories(string line)
        {
            string finalReplaced = line.Replace("%sounds%", SoundsDirectory);
            finalReplaced = finalReplaced.Replace("%tex%", TexturesDirectory);
            finalReplaced = finalReplaced.Replace("%lang%", LanguagesDirectory);
            finalReplaced = finalReplaced.Replace("%comfy%", ComfyDirectory);
            return finalReplaced;
        }
        
        
        /// <summary>
        /// Checks if all necessary game directories exist, if they don't, they will be created.
        /// </summary>
        public static void SetupGameDirectories()
        {
            if (!Directory.Exists(Files.ComfyDirectory))
            {
                Directory.CreateDirectory(Files.ComfyDirectory);
            }

            if (!Directory.Exists(Files.LanguagesDirectory))
            {
                Directory.CreateDirectory(Files.LanguagesDirectory);
            }

            if (!Directory.Exists(Files.SoundsDirectory))
            {
                Directory.CreateDirectory(Files.SoundsDirectory);
            }

            if (!Directory.Exists(Files.TexturesDirectory))
            {
                Directory.CreateDirectory(Files.TexturesDirectory);
            }
        }
    }
}
