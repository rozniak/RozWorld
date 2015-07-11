/**
 * RozWorld.Graphics.UI.Control.TextBox -- RozWorld UI Textbox Control
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System.Drawing;
using OpenGL;
using System;

namespace RozWorld.Graphics.UI.Control
{
    public class TextBox : ControlSkeleton
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


        private int _Width;
        public int Width
        {
            get
            {
                return this._Width;
            }

            set
            {
                this._Width = (int)Math.Round((double)value / 11) * 11;
                UpdateDrawInstruction("control");
            }
        }

        /**
         * Texture references for this control.
         */
        private Texture TextBoxBorderCornerRight;
        private Texture TextBoxBorderCornerLeft;
        private Texture TextBoxBorderSide;
        private Texture TextBoxBorderTop;
        private Texture TextBoxBody;
        private Texture SmallFont;


        public TextBox(GameWindow parentWindow)
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
        public override void LoadReferences()
        {
            TextBoxBorderCornerRight = ParentWindow.TextureManagement.GetTexture("TextBoxBorderCornerRight");
            TextBoxBorderCornerLeft = ParentWindow.TextureManagement.GetTexture("TextBoxBorderCornerLeft");
            TextBoxBorderSide = ParentWindow.TextureManagement.GetTexture("TextBoxBorderSide");
            TextBoxBorderTop = ParentWindow.TextureManagement.GetTexture("TextBoxBorderTop");
            TextBoxBody = ParentWindow.TextureManagement.GetTexture("TextBoxBody");
            SmallFont = ParentWindow.TextureManagement.GetTexture("SmallFont");
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
                    int yString = (int)Position.y + 8;
                    int xString = (int)Position.x + 6;
                    int xTextBox = (int)Position.x;
                    int yTextBox = (int)Position.y;
                    int textOffset = 0;

                    DrawInstructions.Clear();

                    // Draw the TextBox Border

                    // Top Left Corner //
                    DrawInstructions.Add(new DrawInstruction(
                        TextBoxBorderCornerRight,
                        new Vector2(3, 3),
                        new Vector2(0, 0),
                        new Size(3, 3),
                        new Vector2(xTextBox, yTextBox),
                        ParentWindow,
                        TintColour,
                        "textbox"));

                    // Top Bar //
                    DrawInstructions.Add(new DrawInstruction(
                        TextBoxBorderTop,
                        new Vector2(0, 0),
                        new Vector2(1, 3),
                        new Size(Width, 3),
                        new Vector2(xTextBox + 3, yTextBox),
                        ParentWindow,
                        TintColour,
                        "textbox"));

                    // Top Right Corner //
                    DrawInstructions.Add(new DrawInstruction(
                        TextBoxBorderCornerLeft,
                        new Vector2(3, 3),
                        new Vector2(0, 0),
                        new Size(3, 3),
                        new Vector2(xTextBox + Width + 3, yTextBox),
                        ParentWindow,
                        TintColour,
                        "textbox"));

                    // Left Side //
                    DrawInstructions.Add(new DrawInstruction(
                        TextBoxBorderSide,
                        new Vector2(3, 1),
                        new Vector2(0, 0),
                        new Size(3, 22),
                        new Vector2(xTextBox, yTextBox + 3),
                        ParentWindow,
                        TintColour,
                        "textbox"));

                    // Bottom Left Corner //
                    DrawInstructions.Add(new DrawInstruction(
                        TextBoxBorderCornerLeft,
                        new Vector2(0, 0),
                        new Vector2(3, 3),
                        new Size(3, 3),
                        new Vector2(xTextBox, yTextBox + 25),
                        ParentWindow,
                        TintColour,
                        "textbox"));

                    // Bottom Bar //
                    DrawInstructions.Add(new DrawInstruction(
                        TextBoxBorderTop,
                        new Vector2(1, 3),
                        new Vector2(0, 0),
                        new Size(Width, 3),
                        new Vector2(xTextBox + 3, yTextBox + 25),
                        ParentWindow,
                        TintColour,
                        "textbox"));

                    // Bottom Right Corner //
                    DrawInstructions.Add(new DrawInstruction(
                        TextBoxBorderCornerRight,
                        new Vector2(0, 0),
                        new Vector2(3, 3),
                        new Size(3, 3),
                        new Vector2(xTextBox + Width + 3, yTextBox + 25),
                        ParentWindow,
                        TintColour,
                        "textbox"));

                    // Right Side //
                    DrawInstructions.Add(new DrawInstruction(
                        TextBoxBorderSide,
                        new Vector2(0, 0),
                        new Vector2(3, 1),
                        new Size(3, 22),
                        new Vector2(xTextBox + Width + 3, yTextBox + 3),
                        ParentWindow,
                        TintColour,
                        "textbox"));

                    // Draw the TextBox Body
                    for (int offset = 0; offset < Width; offset += 11)
                    {
                        DrawInstructions.Add(new DrawInstruction(
                            TextBoxBody,
                            new Vector2(0, 0),
                            new Vector2(11, 22),
                            new Size(11, 22),
                            new Vector2(xTextBox + offset + 3, yTextBox + 3),
                            ParentWindow,
                            TintColour,
                            "textbox"));
                    }

                    // Draw TextBox Text

                    if (Text != null)
                    {
                        foreach (char c in Text)
                        {
                            Vector2[] position = DrawInstruction.CreateBlitCoordsForFont(FontType.SmallText, c);

                            if (position != null)
                            {
                                if (c == 'g' || c == 'y')
                                {
                                    DrawInstructions.Add(new DrawInstruction(
                                        SmallFont,
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
                                        SmallFont,
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
                                        SmallFont,
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
                        if (instruction.InstructionKey == "textbox")
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
