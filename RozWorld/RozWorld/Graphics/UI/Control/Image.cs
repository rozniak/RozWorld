//
// RozWorld.Graphics.UI.Control.Image -- RozWorld UI Image Control
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

using System.Drawing;

using OpenGL;

namespace RozWorld.Graphics.UI.Control
{
    public class Image : ControlSkeleton
    {
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
        
        private string _TextureName;
        public string TextureName
        {
            get
            {
                return this._TextureName;
            }

            set
            {
                if (RozWorld.GameContent.Textures.ContainsKey(value))
                {
                    this._TextureName = value;
                }
                else
                {
                    this._TextureName = "Missing";
                }

                this.ImageTexture = ParentWindow.TextureManagement.GetTexture(this._TextureName);

                this.BlitFrom = new FloatPoint(0, 0);
                this.BlitTo = new FloatPoint(ImageTexture.Size.Width, ParentWindow.TextureManagement.GetTexture(this._TextureName).Size.Height);
                this.Dimensions = new Size(ImageTexture.Size.Width, ParentWindow.TextureManagement.GetTexture(this._TextureName).Size.Height);
                UpdateDrawInstruction("texture");
            }
        }

        private Size _Dimensions;
        public Size Dimensions
        {
            get
            {
                return this._Dimensions;
            }

            set
            {
                if (value.Width > 0 && value.Height > 0)
                {
                    this._Dimensions = value;
                    UpdateDrawInstruction("texture");
                }
            }
        }

        private FloatPoint _BlitFrom;
        public FloatPoint BlitFrom
        {
            get
            {
                return this._BlitFrom;
            }

            set
            {
                if (value.X >= 0 && value.Y >= 0 && value.X <= ParentWindow.TextureManagement.GetTexture(TextureName).Size.Width && value.Y <= ParentWindow.TextureManagement.GetTexture(TextureName).Size.Height)
                {
                    this._BlitFrom = value;
                    UpdateDrawInstruction("texture");
                }
            }
        }

        private FloatPoint _BlitTo;
        public FloatPoint BlitTo
        {
            get
            {
                return this._BlitTo;
            }

            set
            {
                if (value.X >= 0 && value.Y >= 0 && value.X <= ParentWindow.TextureManagement.GetTexture(TextureName).Size.Width && value.Y <= ParentWindow.TextureManagement.GetTexture(TextureName).Size.Height)
                {
                    this._BlitTo = value;
                    UpdateDrawInstruction("texture");
                }
            }
        }

        // Texture reference for Image control
        private Texture ImageTexture;

        // Events for the control
        public event KeyEventHandler OnKeyDown;
        public event KeyEventHandler OnKeyUp;
        public event SenderEventHandler OnMouseDown;
        public event SenderEventHandler OnMouseEnter;
        public event SenderEventHandler OnMouseLeave;
        public event SenderEventHandler OnMouseUp;


        public Image(GameWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
            LoadReferences();

            this.TextureName = "Missing";
            this._TintColour = VectorColour.NoTint;            
            this.Position = new FloatPoint(0, 0);            
            this.Dimensions = ParentWindow.TextureManagement.GetTexture(TextureName).Size;
            this.BlitFrom = new FloatPoint(0, 0);
            this.BlitTo = new FloatPoint(ParentWindow.TextureManagement.GetTexture(TextureName).Size.Width, ParentWindow.TextureManagement.GetTexture(this._TextureName).Size.Height);
            this.ZIndex = 1;
        }


        /// <summary>
        /// Implementation of the base mouse checking method.
        /// </summary>
        public override void CheckMouse()
        {
            if (ParentWindow.MouseX >= this.Position.X &&
                ParentWindow.MouseX < this.Position.X + Dimensions.Width &&
                ParentWindow.MouseY >= this.Position.Y &&
                ParentWindow.MouseY < this.Position.Y + Dimensions.Height &&
                !MouseEntered)
            {
                MouseEntered = true;

                if (OnMouseEnter != null)
                {
                    OnMouseEnter(this);
                }
            }
            else if (!(ParentWindow.MouseX >= this.Position.X &&
                ParentWindow.MouseX < this.Position.X + Dimensions.Width &&
                ParentWindow.MouseY >= this.Position.Y &&
                ParentWindow.MouseY < this.Position.Y + Dimensions.Height) &&
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


        /// <summary>
        /// Implementation of the base keyboard checking method.
        /// </summary>
        /// <param name="down">Whether the event state is a key down or up.</param>
        /// <param name="key">The key changing state.</param>
        public override void CheckKeyboard(bool down, byte key)
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


        /// <summary>
        /// Implementation of the base texture reference loading method.
        /// </summary>
        public override void LoadReferences()
        {
            this.ImageTexture = ParentWindow.TextureManagement.GetTexture("Missing");
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
                case "texture":
                    DrawInstructions.Clear();

                    if (Dimensions != null)
                    {
                        DrawInstructions.Add(new DrawInstruction(
                            ImageTexture,
                            BlitFrom,
                            BlitTo,
                            Dimensions,
                            Position,
                            ParentWindow,
                            TintColour));
                    }                    

                    break;

                case "visible":
                    DrawInstructions.Clear();

                    if (Visible)
                    {
                        UpdateDrawInstruction("texture");
                    }

                    break;

                case "colour":
                    foreach (DrawInstruction instruction in DrawInstructions)
                    {
                        instruction.TintColour = TintColour;
                    }

                    break;
            }
        }
    }
}
