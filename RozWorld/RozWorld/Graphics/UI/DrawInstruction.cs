//
// RozWorld.Graphics.UI.DrawInstruction -- RozWorld Drawing Instruction
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

using System.Drawing;

using RozWorld.IO;
using OpenGL;

namespace RozWorld.Graphics.UI
{
    public class DrawInstruction
    {
        public FloatPoint[] DrawPoints = new FloatPoint[4];
        public FloatPoint[] BlitPoints = new FloatPoint[4];
        
        public readonly Texture TextureReference;
        public readonly string InstructionKey;
        public Vector4 TintColour;

        private readonly GameWindow ParentWindow;


        public DrawInstruction(Texture texture, FloatPoint position, GameWindow parentWindow, string instructionKey = "")
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
            SetupGLBlitCoordinates(new FloatPoint(0, 0), new FloatPoint(textureSize.Width, textureSize.Height));

            InstructionKey = instructionKey;

            TintColour = VectorColour.NoTint;
        }
        
        public DrawInstruction(Texture texture, Size dimensions, FloatPoint position, GameWindow parentWindow, string instructionKey = "")
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
            SetupGLBlitCoordinates(new FloatPoint(0, 0), new FloatPoint(finalDrawSize.Width, finalDrawSize.Height));

            InstructionKey = instructionKey;

            TintColour = VectorColour.NoTint;
        }
        
        public DrawInstruction(Texture texture, FloatPoint blitFrom, FloatPoint blitTo, Size dimensions, FloatPoint position, GameWindow parentWindow, Vector4 tintColour, string instructionKey = "")
        {
            Size finalDrawSize = dimensions;
            FloatPoint finalBlitFrom = blitFrom;
            FloatPoint finalBlitTo = blitTo;

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

            if (finalBlitFrom.X < 0 || finalBlitFrom.X > textureSize.Width || finalBlitFrom.Y < 0 || finalBlitFrom.Y > textureSize.Height)
            {
                finalBlitFrom = new FloatPoint(0, 0);
            }

            if (finalBlitTo.X <= 0 || finalBlitTo.X > textureSize.Width || finalBlitTo.Y <= 0 || finalBlitTo.Y > textureSize.Height)
            {
                finalBlitTo = new FloatPoint(0, 0);
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
        private void SetupGLDrawCoordinates(FloatPoint origin, Size drawDimensions)
        {
            // Set up drawing scale, the drawing vectors work as below:
            // 
            // [1] <---- [0]
            //  |
            //  v
            // [2] ----> [3]

            for (int i = 0; i <= 3; i++)
            {
                DrawPoints[i] = new FloatPoint(origin.X, origin.Y);
            }

            DrawPoints[0].X += drawDimensions.Width;
            DrawPoints[2].Y += drawDimensions.Height;
            DrawPoints[3].X += drawDimensions.Width;
            DrawPoints[3].Y += drawDimensions.Height;


            // Set up final floats for GL to use as vector points:

            for (int i = 0; i <= 3; i++)
            {
                DrawPoints[i].X = ((DrawPoints[i].X / ParentWindow.ResolutionScale[0]) * 2) - 1.0f;
                DrawPoints[i].Y = (((DrawPoints[i].Y / ParentWindow.ResolutionScale[1]) * 2) - 1.0f) * -1.0f;
            }
        }


        /// <summary>
        /// Sets up the vector points for blitting the texture in GL.
        /// </summary>
        /// <param name="blitOrigin">The top left point of where to start blitting the texture.</param>
        /// <param name="blitEnd">The bottom right point of where to end blitting the texture.</param>
        private void SetupGLBlitCoordinates(FloatPoint blitOrigin, FloatPoint blitEnd)
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
                BlitPoints[i] = new FloatPoint(blitEnd.X, blitEnd.Y);
            }

            BlitPoints[1].X = blitOrigin.X;
            BlitPoints[2].X = blitOrigin.X;
            BlitPoints[2].Y = blitOrigin.Y;
            BlitPoints[3].Y = blitOrigin.Y;

            for (int i = 0; i <= 3; i++)
            {
                BlitPoints[i].X = BlitPoints[i].X / (float)textureSize.Width;
                BlitPoints[i].Y = BlitPoints[i].Y / (float)textureSize.Height;
            }
        }

        
        /// <summary>
        /// Creates blitting coordinates for a character in a specified font.
        /// </summary>
        /// <param name="font">The font that will be blitted.</param>
        /// <param name="character">The character to blit from the font.</param>
        /// <returns>The blitting coordinates if they were created successfully, null otherwise.</returns>
        public static FloatPoint[] CreateBlitCoordsForFont(FontType font, char character)
        {
            switch (font)
            {
                case FontType.SmallText:
                    byte charCode = RozEncoding.GetByteFromChar(character);

                    if (charCode == 0)
                    {
                        return new FloatPoint[] { new FloatPoint(0, 0), new FloatPoint(0, 0) };
                    }
                    else if (charCode >= 1 && charCode <= 6) // 0 - 5
                    {
                        float offsetAmount = (charCode - 1) * 11;   // Get the offset by taking the character code and multiplying by 11 (width of characters)
                        return new FloatPoint[] { new FloatPoint(572 + offsetAmount, 11), new FloatPoint(583 + offsetAmount, 22) };
                    }
                    else if (charCode >= 7 && charCode <= 10) // 6 - 9
                    {
                        float offsetAmount = (charCode - 7) * 11;
                        return new FloatPoint[] { new FloatPoint(offsetAmount, 0), new FloatPoint(11 + offsetAmount, 11) };
                    }
                    else if (charCode >= 11 && charCode <= 62) // A - z
                    {
                        float offsetAmount = (charCode - 11) * 11;
                        return new FloatPoint[] { new FloatPoint(offsetAmount, 11), new FloatPoint(11 + offsetAmount, 22) };
                    }
                    else if (charCode >= 63 && charCode <= 92) // ! - |
                    {
                        float offsetAmount = (charCode - 63) * 11;
                        return new FloatPoint[] { new FloatPoint(44 + offsetAmount, 0), new FloatPoint(55 + offsetAmount, 11) };
                    }
                    else if (charCode == 253) // *
                    {
                        return new FloatPoint[] { new FloatPoint(373, 0), new FloatPoint(385, 11) };
                    }
                    else if (charCode == 254) // €
                    {
                        return new FloatPoint[] { new FloatPoint(395, 0), new FloatPoint(407, 11) };
                    }
                    else if (charCode == 255) // ^
                    {
                        return new FloatPoint[] { new FloatPoint(384, 0), new FloatPoint(396, 11) };
                    }
                    
                    return null;

                default:
                    // No font match, blitting cannot be done.

                    return null;
            }
        }
    }
}
