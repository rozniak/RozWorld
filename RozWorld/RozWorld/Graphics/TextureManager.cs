//
// RozWorld.Graphics.TextureManager -- RozWorld Texture Manager
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

using System;
using System.Collections.Generic;
using System.IO;

using OpenGL;

using RozWorld.IO;

namespace RozWorld.Graphics
{
    public class TextureManager
    {
        private Dictionary<string, Texture> LoadedTextures = new Dictionary<string, Texture>();
        

        /// <summary>
        /// Loads a fresh batch of textures from the loaded game content definitions.
        /// </summary>
        /// <returns>Whether the game succesfully loaded all textures and required textures.</returns>
        public bool LoadTextures()
        {
            // If there are textures already loaded, make sure they are disposed.
            DumpAllTextures();

            foreach (var dictionaryItem in RozWorld.GameContent.Textures)
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
                return false;
            }

            return true;
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
    }
}
