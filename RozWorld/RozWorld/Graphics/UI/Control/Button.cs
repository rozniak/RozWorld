/**
 * RozWorld.Graphics.UI.Control.Button -- RozWorld UI Button Control
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using OpenGL;

using System;
using System.Drawing;


namespace RozWorld.Graphics.UI.Control
{
    public class Button : ControlSkeleton
    {
        private Vector4 _ForeColour;
        public Vector4 ForeColour
        {
            get
            {
                return this._ForeColour;
            }

            set
            {
                this._ForeColour = value;
                UpdateDrawInstruction("colour");
            }
        }

        private Vector4 _TintColour;
        public Vector4 TintColour
        {
            get
            {
                return this._TintColour;
            }

            set
            {
                this._TintColour = value;
                UpdateDrawInstruction("colour");
            }
        }

        private string _Text;
        public string Text
        {
            get
            {
                return this._Text;
            }

            set
            {
                this._Text = value;
                UpdateDrawInstruction("control");
            }
        }

        private FontType _Font;
        public FontType Font
        {
            get
            {
                return this._Font;
            }

            set
            {
                this._Font = value;

                if (this._Font == FontType.SmallText)
                {
                    SpriteFont = RozWorld.Textures.GetTexture("SmallFont");
                }

                UpdateDrawInstruction("control");
            }
        }

        public override float Width
        {
            get
            {
                return this._Width;
            }
            set
            {
                this._Width = (int)Math.Round((double)value / 11) * 11;
                UpdatePosition();
            }
        }

        /**
         * Texture references for this control.
         */
        private Texture ButtonBorderCornerRight;
        private Texture ButtonBorderCornerLeft;
        private Texture ButtonBorderTop;
        private Texture ButtonBorderSide;
        private Texture ButtonBody;
        private Texture SpriteFont;


        public Button(GameWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
            LoadReferences();

            this._ForeColour = VectorColour.OpaqueWhite;
            this._TintColour = VectorColour.NoTint;
            this._Text = "";
            this.Position = new Vector2(0, 0);
            this.ZIndex = 1;
        }


        /// <summary>
        /// Implementation of the base texture reference loading method.
        /// </summary>
        protected override void LoadReferences()
        {
            this.ButtonBorderCornerRight = RozWorld.Textures.GetTexture("ButtonBorderCornerRight");
            this.ButtonBorderCornerLeft = RozWorld.Textures.GetTexture("ButtonBorderCornerLeft");
            this.ButtonBorderTop = RozWorld.Textures.GetTexture("ButtonBorderTop");
            this.ButtonBorderSide = RozWorld.Textures.GetTexture("ButtonBorderSide");
            this.ButtonBody = RozWorld.Textures.GetTexture("ButtonBody");
            this.SpriteFont = RozWorld.Textures.GetTexture("SmallFont");
        }


        /// <summary>
        /// Implementation of the base draw instruction updating method.
        /// </summary>
        /// <param name="updateKey">The update key representing the desired update.</param>
        public override void UpdateDrawInstruction(string updateKey)
        {
            switch (updateKey)
            {
                case "position":
                case "control":
                    int stringWidth = 10 * Text.Length;
                    int xString = ((int)Position.x + ((int)Width / 2)) - (stringWidth / 2);
                    int yString = (int)Position.y + 8;
                    int xButton = (int)Position.x;
                    int yButton = (int)Position.y;
                    int textOffset = 0;

                    DrawInstructions.Clear();

                    if (Visible)
                    {
                        // Draw the Button Border

                        // Top Left Corner //
                        DrawInstructions.Add(new DrawInstruction(
                            ButtonBorderCornerRight,
                            new Vector2(3, 3),
                            new Vector2(0, 0),
                            new Size(3, 3),
                            new Vector2(xButton, yButton),
                            ParentWindow,
                            TintColour,
                            "button"));

                        // Top Bar //
                        DrawInstructions.Add(new DrawInstruction(
                            ButtonBorderTop,
                            new Vector2(0, 0),
                            new Vector2(1, 3),
                            new Size((int)Width, 3),
                            new Vector2(xButton + 3, yButton),
                            ParentWindow,
                            TintColour,
                            "button"));

                        // Top Right Corner //
                        DrawInstructions.Add(new DrawInstruction(
                            ButtonBorderCornerLeft,
                            new Vector2(3, 3),
                            new Vector2(0, 0),
                            new Size(3, 3),
                            new Vector2(xButton + Width + 3, yButton),
                            ParentWindow,
                            TintColour,
                            "button"));

                        // Left Side //
                        DrawInstructions.Add(new DrawInstruction(
                            ButtonBorderSide,
                            new Vector2(3, 1),
                            new Vector2(0, 0),
                            new Size(3, 22),
                            new Vector2(xButton, yButton + 3),
                            ParentWindow,
                            TintColour,
                            "button"));

                        // Bottom Left Corner //
                        DrawInstructions.Add(new DrawInstruction(
                            ButtonBorderCornerLeft,
                            new Vector2(0, 0),
                            new Vector2(3, 3),
                            new Size(3, 3),
                            new Vector2(xButton, yButton + 25),
                            ParentWindow,
                            TintColour,
                            "button"));

                        // Bottom Bar //
                        DrawInstructions.Add(new DrawInstruction(
                            ButtonBorderTop,
                            new Vector2(1, 3),
                            new Vector2(0, 0),
                            new Size((int)Width, 3),
                            new Vector2(xButton + 3, yButton + 25),
                            ParentWindow,
                            TintColour,
                            "button"));

                        // Button Right Corner //
                        DrawInstructions.Add(new DrawInstruction(
                            ButtonBorderCornerRight,
                            new Vector2(0, 0),
                            new Vector2(3, 3),
                            new Size(3, 3),
                            new Vector2(xButton + Width + 3, yButton + 25),
                            ParentWindow,
                            TintColour,
                            "button"));

                        // Right Side //
                        DrawInstructions.Add(new DrawInstruction(
                            ButtonBorderSide,
                            new Vector2(0, 0),
                            new Vector2(3, 1),
                            new Size(3, 22),
                            new Vector2(xButton + Width + 3, yButton + 3),
                            ParentWindow,
                            TintColour,
                            "button"));

                        // Draw the Button Body
                        for (int offset = 0; offset < Width; offset += 11)
                        {
                            DrawInstructions.Add(new DrawInstruction(
                                ButtonBody,
                                new Vector2(0, 0),
                                new Vector2(11, 22),
                                new Size(11, 22),
                                new Vector2(xButton + offset + 3, yButton + 3),
                                ParentWindow,
                                TintColour,
                                "button"));
                        }

                        // Draw Button Text

                        if (Text != null)
                        {
                            foreach (char c in Text)
                            {
                                Vector2[] position = DrawInstruction.CreateBlitCoordsForFont(Font, c);

                                if (position != null)
                                {
                                    if (c == 'g' || c == 'y')
                                    {
                                        DrawInstructions.Add(new DrawInstruction(
                                            SpriteFont,
                                            position[0],
                                            position[1],
                                            new Size(11, 11),
                                            new Vector2(xString + textOffset, yString + 3),
                                            ParentWindow,
                                            ForeColour,
                                            "text"));
                                    }
                                    else if (c == 'p' || c == 'q')
                                    {
                                        DrawInstructions.Add(new DrawInstruction(
                                            SpriteFont,
                                            position[0],
                                            position[1],
                                            new Size(11, 11),
                                            new Vector2(xString + textOffset, yString + 2),
                                            ParentWindow,
                                            ForeColour,
                                            "text"));
                                    }
                                    else
                                    {
                                        DrawInstructions.Add(new DrawInstruction(
                                            SpriteFont,
                                            position[0],
                                            position[1],
                                            new Size(11, 11),
                                            new Vector2(xString + textOffset, yString),
                                            ParentWindow,
                                            ForeColour,
                                            "text"));
                                    }

                                    textOffset += 10;
                                }
                            }
                        }
                    }

                    break;

                case "visible":
                    DrawInstructions.Clear();

                    if (Visible)
                    {
                        UpdateDrawInstruction("control");
                    }

                    break;

                case "colour":
                    foreach (DrawInstruction instruction in DrawInstructions)
                    {
                        if (instruction.InstructionKey == "button")
                        {
                            instruction.TintColour = TintColour;
                        }
                        else if (instruction.InstructionKey == "text")
                        {
                            instruction.TintColour = ForeColour;
                        }
                    }

                    break;
            }
        }
    }
}
