//
// RozWorld.Graphics.UI.Control.Button -- RozWorld UI Button Control
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

using System;
using System.Drawing;

using OpenGL;

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
                    SpriteFont = ParentWindow.TextureManagement.GetTexture("SmallFont");
                }

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

        // Texture references for Button control
        private Texture ButtonBorderCornerRight;
        private Texture ButtonBorderCornerLeft;
        private Texture ButtonBorderTop;
        private Texture ButtonBorderSide;
        private Texture ButtonBody;
        private Texture SpriteFont;

        // Events for the control
        public event KeyEventHandler OnKeyDown;
        public event KeyEventHandler OnKeyUp;
        public event SenderEventHandler OnMouseDown;
        public event SenderEventHandler OnMouseEnter;
        public event SenderEventHandler OnMouseLeave;
        public event SenderEventHandler OnMouseUp;
        

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
        /// Implementation of the base mouse checking method.
        /// </summary>
        public override void CheckMouse()
        {
            if (this.Visible)
            {
                if (ParentWindow.MouseX >= this.Position.x &&
                ParentWindow.MouseX < this.Position.x + this.Width + 6 &&
                ParentWindow.MouseY >= this.Position.y &&
                ParentWindow.MouseY < this.Position.y + 28 &&
                !this.MouseEntered)
                {
                    this.MouseEntered = true;

                    if (OnMouseEnter != null)
                    {
                        OnMouseEnter(this);
                    }
                }
                else if (!(ParentWindow.MouseX >= this.Position.x &&
                    ParentWindow.MouseX < this.Position.x + this.Width + 6 &&
                    ParentWindow.MouseY >= this.Position.y &&
                    ParentWindow.MouseY < this.Position.y + 28) &&
                    this.MouseEntered)
                {
                    this.MouseEntered = false;

                    if (OnMouseLeave != null)
                    {
                        OnMouseLeave(this);
                    }
                }

                if (((ParentWindow.CurrentMouseStates.Left && !ParentWindow.LastMouseStates.Left) ||
                    (ParentWindow.CurrentMouseStates.Middle && !ParentWindow.LastMouseStates.Middle) ||
                    (ParentWindow.CurrentMouseStates.Right && !ParentWindow.LastMouseStates.Right)) && this.MouseEntered)
                {
                    if (OnMouseDown != null)
                    {
                        OnMouseDown(this);
                    }
                }


                if (((!ParentWindow.CurrentMouseStates.Left && ParentWindow.LastMouseStates.Left) ||
                    (!ParentWindow.CurrentMouseStates.Middle && ParentWindow.LastMouseStates.Middle) ||
                    (!ParentWindow.CurrentMouseStates.Right && ParentWindow.LastMouseStates.Right)) && this.MouseEntered)
                {
                    if (OnMouseUp != null)
                    {
                        OnMouseUp(this);
                    }
                }
            }            
        }


        /// <summary>
        /// Implementation of the base keyboard checking method.
        /// </summary>
        /// <param name="down">Whether the event state is a key down or up.</param>
        /// <param name="key">The key changing state.</param>
        public override void CheckKeyboard(bool down, byte key)
        {
            if (this.Visible)
            {
                if (down && OnKeyDown != null)
                {
                    OnKeyDown(this, key);
                }
                else if (!down && OnKeyUp != null)
                {
                    OnKeyUp(this, key);
                }
            }            
        }


        /// <summary>
        /// Implementation of the base texture reference loading method.
        /// </summary>
        public override void LoadReferences()
        {
            this.ButtonBorderCornerRight = ParentWindow.TextureManagement.GetTexture("ButtonBorderCornerRight");
            this.ButtonBorderCornerLeft = ParentWindow.TextureManagement.GetTexture("ButtonBorderCornerLeft");
            this.ButtonBorderTop = ParentWindow.TextureManagement.GetTexture("ButtonBorderTop");
            this.ButtonBorderSide = ParentWindow.TextureManagement.GetTexture("ButtonBorderSide");
            this.ButtonBody = ParentWindow.TextureManagement.GetTexture("ButtonBody");
            this.SpriteFont = ParentWindow.TextureManagement.GetTexture("SmallFont");
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
                    int xString = ((int)Position.x + (Width / 2)) - (stringWidth / 2);
                    int yString = (int)Position.y + 8;
                    int xButton = (int)Position.x;
                    int yButton = (int)Position.y;
                    int textOffset = 0;

                    DrawInstructions.Clear();

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
                        new Size(Width, 3),
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
                        new Size(Width, 3),
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
