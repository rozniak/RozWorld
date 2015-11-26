/**
 * RozWorld.Graphics.TextureManager -- RozWorld Texture Manager
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using OpenGL;

using RozWorld.IO;

using System;
using System.Collections.Generic;
using System.IO;
using RozWorld.Graphics.UI;


namespace RozWorld.Graphics
{
    public class TextureManager
    {
        private Dictionary<string, Texture> LoadedTextures = new Dictionary<string, Texture>();
        private Dictionary<string, string> FontSources = new Dictionary<string, string>();


        /// <summary>
        /// Disposes all loaded texture objects, if any.
        /// </summary>
        public void DumpAllTextures()
        {
            foreach (Texture texture in LoadedTextures.Values)
            {
                texture.Dispose();
            }

            LoadedTextures.Clear();
        }


        /// <summary>
        /// Gets a texture with the specified name from the loaded texture content.
        /// </summary>
        /// <param name="textureName">The name of the texture to get.</param>
        /// <returns>The texture from the loaded texture content.</returns>
        public Texture GetTexture(string textureName)
        {
            if (LoadedTextures.ContainsKey(textureName))
            {
                return LoadedTextures[textureName];
            }

            return null;
        }


        /// <summary>
        /// Loads the game's font sources from the font link file.
        /// </summary>
        public void LoadFontSources()
        {
            // Clear any old font sources
            FontSources.Clear();

            if (File.Exists(Files.FontsFile))
            {
                var fontFile = Files.ReadINIToDictionary(Files.FontsFile);

                foreach (var fontSource in fontFile)
                {
                    if (File.Exists(Files.LiveTextureDirectory + "\\gui\\fonts\\" + fontSource.Value))
                        FontSources.Add(fontSource.Key, fontSource.Value);
                }

                if (FontSources.Count > 0)
                    return;
            }

            UIHandler.CriticalError(Error.BROKEN_FONT_LINK_FILE);
        }


        /// <summary>
        /// Loads a fresh batch of textures from the loaded game content definitions.
        /// </summary>
        /// <returns>Whether the game succesfully loaded all textures and required textures.</returns>
        public void LoadTextures()
        {
            // If there are textures already loaded, make sure they are disposed.
            DumpAllTextures();

            foreach (var dictionaryItem in RozWorld.Content.Textures)
            {
                string textureLocation = Files.ReplaceSpecialDirectories(dictionaryItem.Value);

                if (!File.Exists(textureLocation))
                {
                    continue;
                }

                // If a texture of the name already exists then dispose the old texture first.
                if (LoadedTextures.ContainsKey(dictionaryItem.Key))
                {
                    LoadedTextures[dictionaryItem.Key].Dispose();
                }

                LoadedTextures[dictionaryItem.Key] = new Texture(textureLocation);
            }

            // Check if the missing texture placeholder is loaded.
            if (!LoadedTextures.ContainsKey("Missing") || LoadedTextures["Missing"].Size.Width != 32 || LoadedTextures["Missing"].Size.Height != 32)
            {
                UIHandler.CriticalError(Error.MISSING_CRITICAL_FILES, "No default texture provided (\"Missing\"), or default texture has invalid dimensions (must be 32x32).");
            }
        }
    }
}
