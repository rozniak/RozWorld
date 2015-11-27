/**
 * RozWorld.Graphics.UI.DrawInstruction -- RozWorld Drawing Instruction
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using OpenGL;

using RozWorld.IO;

using System.Drawing;


namespace RozWorld.Graphics.UI
{
    public class DrawInstruction
    {
        public Vector2[] DrawPoints = new Vector2[4];
        public Vector2[] BlitPoints = new Vector2[4];
        
        public readonly Texture TextureReference;
        public readonly string InstructionKey;
        public Vector4 TintColour;

        private readonly GameWindow ParentWindow;


        public DrawInstruction(Texture texture, Vector2 position, GameWindow parentWindow, string instructionKey = "")
        {
            ParentWindow = parentWindow;

            if (texture == null)
            {
                texture = ParentWindow.TextureManagement.GetTexture("Missing");
            }
            else
            {
                TextureReference = texture;
            }

            Size textureSize = texture.Size;

            SetupGLDrawCoordinates(position, textureSize);
            SetupGLBlitCoordinates(new Vector2(0, 0), new Vector2(textureSize.Width, textureSize.Height));

            InstructionKey = instructionKey;

            TintColour = VectorColour.NoTint;
        }

        public DrawInstruction(Texture texture, Size dimensions, Vector2 position, GameWindow parentWindow, string instructionKey = "")
        {
            Size finalDrawSize = dimensions;

            ParentWindow = parentWindow;

            if (texture == null)
            {
                texture = ParentWindow.TextureManagement.GetTexture("Missing");
            }
            else
            {
                TextureReference = texture;
            }

            Size textureSize = texture.Size;

            if (finalDrawSize.Height <= 0 || finalDrawSize.Width <= 0)
            {
                finalDrawSize = textureSize;
            }

            SetupGLDrawCoordinates(position, finalDrawSize);
            SetupGLBlitCoordinates(new Vector2(0, 0), new Vector2(finalDrawSize.Width, finalDrawSize.Height));

            InstructionKey = instructionKey;

            TintColour = VectorColour.NoTint;
        }

        public DrawInstruction(Texture texture, Vector2 blitFrom, Vector2 blitTo, Size dimensions, Vector2 position, GameWindow parentWindow, Vector4 tintColour, string instructionKey = "")
        {
            Size finalDrawSize = dimensions;
            Vector2 finalBlitFrom = blitFrom;
            Vector2 finalBlitTo = blitTo;

            ParentWindow = parentWindow;

            if (texture == null)
            {
                TextureReference = ParentWindow.TextureManagement.GetTexture("Missing");
            }
            else
            {
                TextureReference = texture;
            }

            Size textureSize = TextureReference.Size;

            if (finalDrawSize.Height <= 0 || finalDrawSize.Width <= 0)
            {
                finalDrawSize = textureSize;
            }

            if (finalBlitFrom.x < 0 || finalBlitFrom.x > textureSize.Width || finalBlitFrom.y < 0 || finalBlitFrom.y > textureSize.Height)
            {
                finalBlitFrom = new Vector2(0, 0);
            }

            if (finalBlitTo.x <= 0 || finalBlitTo.x > textureSize.Width || finalBlitTo.y <= 0 || finalBlitTo.y > textureSize.Height)
            {
                finalBlitTo = new Vector2(0, 0);
            }

            SetupGLDrawCoordinates(position, finalDrawSize);
            SetupGLBlitCoordinates(finalBlitFrom, finalBlitTo);

            InstructionKey = instructionKey;

            TintColour = tintColour;
        }


        /// <summary>
        /// Sets up the vector points for drawing the texture in GL.
        /// </summary>
        /// <param name="origin">The top left point of where the texture should be drawn.</param>
        /// <param name="drawDimensions">The size of the drawn texture.</param>
        private void SetupGLDrawCoordinates(Vector2 origin, Size drawDimensions)
        {
            // Set up drawing scale, the drawing vectors work as below:
            // 
            // [1] <---- [0]
            //  |
            //  v
            // [2] ----> [3]

            for (int i = 0; i <= 3; i++)
            {
                DrawPoints[i] = new Vector2(origin.x, origin.y);
            }

            DrawPoints[0].x += drawDimensions.Width;
            DrawPoints[2].y += drawDimensions.Height;
            DrawPoints[3].x += drawDimensions.Width;
            DrawPoints[3].y += drawDimensions.Height;


            // Set up final floats for GL to use as vector points:

            for (int i = 0; i <= 3; i++)
            {
                DrawPoints[i].x = ((DrawPoints[i].x / ParentWindow.WindowScale.Width) * 2) - 1.0f;
                DrawPoints[i].y = (((DrawPoints[i].y / ParentWindow.WindowScale.Height) * 2) - 1.0f) * -1.0f;
            }
        }


        /// <summary>
        /// Sets up the vector points for blitting the texture in GL.
        /// </summary>
        /// <param name="blitOrigin">The top left point of where to start blitting the texture.</param>
        /// <param name="blitEnd">The bottom right point of where to end blitting the texture.</param>
        private void SetupGLBlitCoordinates(Vector2 blitOrigin, Vector2 blitEnd)
        {
            // Set up blitting rectangle, the blitting vectors work as below:
            //
            // [1] <---- [0]
            //  |
            //  v
            // [2] ----> [3]

            Size textureSize = TextureReference.Size;

            for (int i = 0; i <= 3; i++)
            {
                BlitPoints[i] = new Vector2(blitEnd.x, blitEnd.y);
            }

            BlitPoints[1].x = blitOrigin.x;
            BlitPoints[2].x = blitOrigin.x;
            BlitPoints[2].y = blitOrigin.y;
            BlitPoints[3].y = blitOrigin.y;

            for (int i = 0; i <= 3; i++)
            {
                BlitPoints[i].x = BlitPoints[i].x / (float)textureSize.Width;
                BlitPoints[i].y = BlitPoints[i].y / (float)textureSize.Height;
            }
        }

        
        /// <summary>
        /// Creates blitting coordinates for a character in a specified font.
        /// </summary>
        /// <param name="font">The font that will be blitted.</param>
        /// <param name="character">The character to blit from the font.</param>
        /// <returns>The blitting coordinates if they were created successfully, null otherwise.</returns>
        public static Vector2[] CreateBlitCoordsForFont(FontType font, char character)
        {
            switch (font)
            {
                case FontType.SmallText:
                    byte charCode = RozEncoding.GetByteFromChar(character);

                    if (charCode == 0)
                    {
                        return new Vector2[] { new Vector2(0, 0), new Vector2(0, 0) };
                    }
                    else if (charCode >= 1 && charCode <= 6) // 0 - 5
                    {
                        float offsetAmount = (charCode - 1) * 11;   // Get the offset by taking the character code and multiplying by 11 (width of characters)
                        return new Vector2[] { new Vector2(572 + offsetAmount, 11), new Vector2(583 + offsetAmount, 22) };
                    }
                    else if (charCode >= 7 && charCode <= 10) // 6 - 9
                    {
                        float offsetAmount = (charCode - 7) * 11;
                        return new Vector2[] { new Vector2(offsetAmount, 0), new Vector2(11 + offsetAmount, 11) };
                    }
                    else if (charCode >= 11 && charCode <= 62) // A - z
                    {
                        float offsetAmount = (charCode - 11) * 11;
                        return new Vector2[] { new Vector2(offsetAmount, 11), new Vector2(11 + offsetAmount, 22) };
                    }
                    else if (charCode >= 63 && charCode <= 92) // ! - |
                    {
                        float offsetAmount = (charCode - 63) * 11;
                        return new Vector2[] { new Vector2(44 + offsetAmount, 0), new Vector2(55 + offsetAmount, 11) };
                    }
                    else if (charCode == 253) // *
                    {
                        return new Vector2[] { new Vector2(373, 0), new Vector2(385, 11) };
                    }
                    else if (charCode == 254) // €
                    {
                        return new Vector2[] { new Vector2(395, 0), new Vector2(407, 11) };
                    }
                    else if (charCode == 255) // ^
                    {
                        return new Vector2[] { new Vector2(384, 0), new Vector2(396, 11) };
                    }
                    
                    return null;

                default:
                    // No font match, blitting cannot be done.

                    return null;
            }
        }
    }
}
