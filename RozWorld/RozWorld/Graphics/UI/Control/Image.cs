/**
 * RozWorld.Graphics.UI.Control.Image -- RozWorld UI Image Control
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
                if (RozWorld.Textures.TextureExists(value))
                {
                    this._TextureName = value;
                }
                else
                {
                    this._TextureName = "Missing";
                }

                this.ImageTexture = RozWorld.Textures.GetTexture(this._TextureName);

                this.BlitFrom = new Vector2(0, 0);
                this.BlitTo = new Vector2(ImageTexture.Size.Width, RozWorld.Textures.GetTexture(this._TextureName).Size.Height);
                this.Dimensions = new Size(ImageTexture.Size.Width, RozWorld.Textures.GetTexture(this._TextureName).Size.Height);
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
                    this.Width = value.Width;
                    this.Height = value.Height;
                    UpdateDrawInstruction("texture");
                }
            }
        }

        public ImageSizeMode SizeMode;

        /**
         * Blitting information, more information on blitting in DrawInstruction.cs.
         */
        private Vector2 _BlitFrom;
        public Vector2 BlitFrom
        {
            get
            {
                return this._BlitFrom;
            }

            set
            {
                if (value.x >= 0 && value.y >= 0 &&
                    value.x <= RozWorld.Textures.GetTexture(TextureName).Size.Width &&
                    value.y <= RozWorld.Textures.GetTexture(TextureName).Size.Height)
                {
                    this._BlitFrom = value;
                    UpdateDrawInstruction("texture");
                }
            }
        }

        private Vector2 _BlitTo;
        public Vector2 BlitTo
        {
            get
            {
                return this._BlitTo;
            }

            set
            {
                if (value.x >= 0 && value.y >= 0 &&
                    value.x <= RozWorld.Textures.GetTexture(TextureName).Size.Width &&
                    value.y <= RozWorld.Textures.GetTexture(TextureName).Size.Height)
                {
                    this._BlitTo = value;
                    UpdateDrawInstruction("texture");
                }
            }
        }

        /**
         * Texture reference for this control.
         */
        private Texture ImageTexture;

        /**
         * Events for this control.
         */
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
            this.Position = new Vector2(0, 0);
            this.Dimensions = RozWorld.Textures.GetTexture(TextureName).Size;
            this.BlitFrom = new Vector2(0, 0);
            this.BlitTo = new Vector2(RozWorld.Textures.GetTexture(TextureName).Size.Width, RozWorld.Textures.GetTexture(this._TextureName).Size.Height);
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
                ParentWindow.MouseX < this.Position.x + Dimensions.Width &&
                ParentWindow.MouseY >= this.Position.y &&
                ParentWindow.MouseY < this.Position.y + Dimensions.Height &&
                !MouseEntered)
                {
                    MouseEntered = true;

                    if (OnMouseEnter != null)
                    {
                        OnMouseEnter(this);
                    }
                }
                else if (!(ParentWindow.MouseX >= this.Position.x &&
                    ParentWindow.MouseX < this.Position.x + Dimensions.Width &&
                    ParentWindow.MouseY >= this.Position.y &&
                    ParentWindow.MouseY < this.Position.y + Dimensions.Height) &&
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
        protected override void LoadReferences()
        {
            this.ImageTexture = RozWorld.Textures.GetTexture("Missing");
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

                    if (Visible)
                    {
                        switch (SizeMode)
                        {
                            default:
                            case ImageSizeMode.Default:
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

                            case ImageSizeMode.Tile:
                                if (Dimensions != null)
                                {
                                    // Get how many times we will draw the texture across the two dimensions
                                    int tilesX = (int)Math.Ceiling((double)Dimensions.Width / ImageTexture.Size.Width);
                                    int tilesY = (int)Math.Ceiling((double)Dimensions.Height / ImageTexture.Size.Height);

                                    // See what the dimensions are for textures that go offscreen (clip them)
                                    int clipWidth = 0;
                                    int clipHeight = 0;

                                    // See where we are at on the screen
                                    int offsetX = 0;
                                    int offsetY = 0;

                                    // The y-blitting must be inverted to clip off the bottom rather than the top of the texture
                                    int blitY = 0;

                                    for (int y = 1; y <= tilesY; y++)
                                    {
                                        for (int x = 1; x <= tilesX; x++)
                                        {
                                            if (y == tilesY && x == tilesX)
                                            {
                                                clipWidth = Dimensions.Width - offsetX;
                                                clipHeight = Dimensions.Height - offsetY;
                                                blitY = ImageTexture.Size.Height - clipHeight;

                                                DrawInstructions.Add(new DrawInstruction(
                                                    ImageTexture,
                                                    new Vector2(0, blitY),
                                                    new Vector2(clipWidth, ImageTexture.Size.Height),
                                                    new Size(clipWidth, clipHeight),
                                                    new Vector2(offsetX, offsetY),
                                                    ParentWindow,
                                                    TintColour));
                                            }
                                            else if (y == tilesY)
                                            {
                                                clipHeight = Dimensions.Height - offsetY;
                                                blitY = ImageTexture.Size.Height - clipHeight;

                                                DrawInstructions.Add(new DrawInstruction(
                                                    ImageTexture,
                                                    new Vector2(0, blitY),
                                                    new Vector2(ImageTexture.Size.Width, ImageTexture.Size.Height),
                                                    new Size(ImageTexture.Size.Width, clipHeight),
                                                    new Vector2(offsetX, offsetY),
                                                    ParentWindow,
                                                    TintColour));
                                            }
                                            else if (x == tilesX)
                                            {
                                                clipWidth = Dimensions.Width - offsetX;

                                                DrawInstructions.Add(new DrawInstruction(
                                                    ImageTexture,
                                                    new Vector2(0, 0),
                                                    new Vector2(clipWidth, ImageTexture.Size.Height),
                                                    new Size(clipWidth, ImageTexture.Size.Height),
                                                    new Vector2(offsetX, offsetY),
                                                    ParentWindow,
                                                    TintColour));
                                            }
                                            else
                                            {
                                                DrawInstructions.Add(new DrawInstruction(
                                                    ImageTexture,
                                                    BlitFrom,
                                                    BlitTo,
                                                    new Size(ImageTexture.Size.Width, ImageTexture.Size.Height),
                                                    new Vector2(offsetX, offsetY),
                                                    ParentWindow,
                                                    TintColour));
                                            }

                                            offsetX += ImageTexture.Size.Width;
                                        }

                                        offsetX = 0;
                                        offsetY += ImageTexture.Size.Height;
                                    }
                                }

                                break;
                        }
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
