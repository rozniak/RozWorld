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

using RozWorld.COMFY;

using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;


namespace RozWorld.IO
{
    public static class Files
    {
        public static readonly string ComfyDirectory = Environment.CurrentDirectory + @"\comfy";

        public static readonly string LanguagesDirectory = Environment.CurrentDirectory + @"\lang";
        public static readonly string LanguagesFile = Environment.CurrentDirectory + @"\link\langs.ini";

        public static readonly string LinksDirectory = Environment.CurrentDirectory + @"\link";
        public static readonly string FontsFile = Environment.CurrentDirectory + @"\link\fonts.ini";

        public static readonly string ModsDirectory = Environment.CurrentDirectory + @"\mods";
        public static readonly string SoundsDirectory = Environment.CurrentDirectory + @"\sounds";
        public static readonly string TexturesDirectory = Environment.CurrentDirectory + @"\tex";


        private static string _TexturePackSubFolder;
        public static string TexturePackSubFolder
        {
            get { return _TexturePackSubFolder; }
            set
            {
                if (Directory.Exists(TexturesDirectory + "\\" + value))
                    _TexturePackSubFolder = value;
            }
        }


        public static string LiveTextureDirectory
        {
            get { return TexturesDirectory + "\\" + TexturePackSubFolder; }
        }


        /// <summary>
        /// Gets a text file with the specified filename from the disk.
        /// </summary>
        /// <param name="fileName">The filename of the file to get.</param>
        /// <returns>The file contents if it exists.</returns>
        public static IList<string> GetTextFile(string fileName)
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

                return fileContents.AsReadOnly();
            }
            
            return null;
        }


        /// <summary>
        /// Gets a binary file with the specified filename from the disk.
        /// </summary>
        /// <param name="fileName">The filename of the file to get.</param>
        /// <returns>The file contents if it exists.</returns>
        public static IList<byte> GetBinaryFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllBytes(fileName);
            }

            return null;
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
        /// Reads an INI file into a dictionary for easy reading
        /// </summary>
        /// <param name="filePath">The file path of the INI file to read.</param>
        /// <returns>A dictionary containing the variable names as keys, alongside their values.</returns>
        public static Dictionary<string, string> ReadINIToDictionary(string filePath)
        {
            IList<string> iniFile = GetTextFile(filePath);

            if (iniFile != null)
            {
                var finalDictionary = new Dictionary<string, string>();

                foreach (string line in iniFile)
                {
                    if (!line.StartsWith("#")) // Ignore comments
                    {
                        string[] resultingSplit = SplitFirstInstance(":", line);

                        if (resultingSplit[0] != "" && resultingSplit[1] != "") // Check that this line is a valid property
                        {
                            if (finalDictionary.ContainsKey(resultingSplit[0]))
                            {
                                finalDictionary[resultingSplit[0]] = resultingSplit[1];
                            }
                            else
                            {
                                finalDictionary.Add(resultingSplit[0], resultingSplit[1]);
                            }
                        }
                    }
                }

                return finalDictionary;
            }

            return null;
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
            finalReplaced = finalReplaced.Replace("%link%", LinksDirectory);
            finalReplaced = finalReplaced.Replace("%mods%", ModsDirectory);
            return finalReplaced;
        }
        
        
        /// <summary>
        /// Checks if all necessary game directories exist, if they don't, they will be created.
        /// </summary>
        public static void SetupGameDirectories()
        {
            // COMFY Directory
            if (!Directory.Exists(Files.ComfyDirectory))
                Directory.CreateDirectory(Files.ComfyDirectory);

            // Languages Directory
            if (!Directory.Exists(Files.LanguagesDirectory))
                Directory.CreateDirectory(Files.LanguagesDirectory);

            // Sounds Directory
            if (!Directory.Exists(Files.SoundsDirectory))
                Directory.CreateDirectory(Files.SoundsDirectory);

            // Textures Directory
            if (!Directory.Exists(Files.TexturesDirectory))
                Directory.CreateDirectory(Files.TexturesDirectory);

            // Links Directory
            if (!Directory.Exists(Files.LinksDirectory))
                Directory.CreateDirectory(Files.LinksDirectory);

            // Mods Directory
            if (!Directory.Exists(Files.ModsDirectory))
                Directory.CreateDirectory(Files.ModsDirectory);
        }
    }
}
