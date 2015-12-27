/**
 * RozWorld.Graphics.UI.Control.Label -- RozWorld UI Label Control
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using OpenGL;

using System.Collections.Generic;
using System.Drawing;


namespace RozWorld.Graphics.UI.Control
{
    public class Label : ControlSkeleton
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
                UpdateDrawInstruction("text");
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
                UpdateDrawInstruction("text");
            }
        }

        /**
         * Texture reference for this control.
         */
        private Texture SmallFont;

        /**
         * Events for this control.
         */
        public event KeyEventHandler OnKeyDown;
        public event KeyEventHandler OnKeyUp;
        public event SenderEventHandler OnMouseDown;
        public event SenderEventHandler OnMouseEnter;
        public event SenderEventHandler OnMouseLeave;
        public event SenderEventHandler OnMouseUp;


        public Label(GameWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
            LoadReferences();
            this._ForeColour = VectorColour.OpaqueWhite;
            this.Position = new Vector2(0, 0);
            this._Font = FontType.SmallText;
            this._Text = "";
            this.ZIndex = 1;
        }


        /// <summary>
        /// Implementation of the base mouse checking method.
        /// </summary>
        public override void CheckMouse()
        {
            if (AcceptInputMouse)
            {
                if (ParentWindow.MouseX >= this.Position.x &&
                ParentWindow.MouseX < this.Position.x + (Text.Length * 10) &&
                ParentWindow.MouseY >= this.Position.y &&
                ParentWindow.MouseY < this.Position.y + 11 &&
                !MouseEntered)
                {
                    MouseEntered = true;

                    if (OnMouseEnter != null)
                    {
                        OnMouseEnter(this);
                    }
                }
                else if (!(ParentWindow.MouseX >= this.Position.x &&
                    ParentWindow.MouseX < this.Position.x + (Text.Length * 10) &&
                    ParentWindow.MouseY >= this.Position.y &&
                    ParentWindow.MouseY < this.Position.y + 11) &&
                    MouseEntered)
                {
                    MouseEntered = false;

                    if (OnMouseLeave != null)
                    {
                        OnMouseLeave(this);
                    }
                }

                if (((ParentWindow.CurrentMouseStates.Left && !ParentWindow.LastMouseStates.Left) ||
                    (ParentWindow.CurrentMouseStates.Middle && !ParentWindow.LastMouseStates.Middle) ||
                    (ParentWindow.CurrentMouseStates.Right && !ParentWindow.LastMouseStates.Right)) && MouseEntered)
                {
                    if (OnMouseDown != null)
                    {
                        OnMouseDown(this);
                    }
                }


                if (((!ParentWindow.CurrentMouseStates.Left && ParentWindow.LastMouseStates.Left) ||
                    (!ParentWindow.CurrentMouseStates.Middle && ParentWindow.LastMouseStates.Middle) ||
                    (!ParentWindow.CurrentMouseStates.Right && ParentWindow.LastMouseStates.Right)) && MouseEntered)
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
            if (Visible)
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
        protected override void LoadReferences()
        {
            this.SmallFont = RozWorld.Textures.GetTexture("SmallFont");
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
                case "text":
                    int textOffset = 0;

                    if (Text != null)
                    {
                        DrawInstructions.Clear();

                        if (Visible)
                        {
                            foreach (char c in Text)
                            {
                                Vector2[] position = DrawInstruction.CreateBlitCoordsForFont(Font, c);

                                if (position != null)
                                {
                                    if (c == 'g' || c == 'y')
                                    {
                                        DrawInstructions.Add(new DrawInstruction(
                                            SmallFont,
                                            position[0],
                                            position[1],
                                            new Size(11, 11),
                                            new Vector2(Position.x + textOffset, Position.y + 3),
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
                                            new Vector2(Position.x + textOffset, Position.y + 2),
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
                                            new Vector2(Position.x + textOffset, Position.y),
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
                    if (Visible)
                    {
                        UpdateDrawInstruction("text");
                    }
                    else
                    {
                        DrawInstructions.Clear();
                    }

                    break;

                case "colour":
                    foreach (DrawInstruction instruction in DrawInstructions)
                    {
                        instruction.TintColour = ForeColour;
                    }

                    break;
            }
        }
    }
}
