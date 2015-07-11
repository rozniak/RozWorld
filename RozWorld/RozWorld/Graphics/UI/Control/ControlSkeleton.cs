/**
 * RozWorld.Graphics.UI.Control.ControlSkeleton -- RozWorld UI Control Skeleton
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System.Drawing;
using System.Collections.Generic;
using OpenGL;

namespace RozWorld.Graphics.UI
{
    public abstract class ControlSkeleton
    {
        protected int _ZIndex;
        public int ZIndex
        {
            get
            {
                return this._ZIndex;
            }

            set
            {
                this._ZIndex = value;
                this.ParentWindow.GameInterface.SortControlZIndexes();
            }
        }

        /**
         * Control dimension stuff.
         */
        protected float _Width;
        public virtual float Width
        {
            get
            {
                return this._Width;
            }

            set
            {
                this._Width = value;
            }
        }

        protected float _Height;
        public virtual float Height
        {
            get
            {
                return this._Height;
            }

            set
            {
                this._Height = value;
            }
        }

        /**
         * Positioning/anchor relevant stuff.
         */
        protected Vector2 _Position;
        public Vector2 Position
        {
            get
            {
                return this._Position;
            }

            set
            {
                UpdatePosition(value);
                UpdateDrawInstruction("position");
            }
        }

        protected AnchorType _Anchor;
        public AnchorType Anchor
        {
            get
            {
                return this._Anchor;
            }

            set
            {
                this._Anchor = value;
                UpdateDrawInstruction("position");
            }
        }

        protected float _OffsetX;
        public float OffsetX
        {
            get
            {
                return this._OffsetX;
            }

            set
            {
                this._OffsetX = value;
                UpdatePosition(new Vector2(this.OffsetX, this.OffsetY));
            }
        }

        protected float _OffsetY;
        public float OffsetY
        {
            get
            {
                return this._OffsetY;
            }

            set
            {
                this._OffsetY = value;
                UpdatePosition(new Vector2(this.OffsetX, this.OffsetY));
            }
        }

        /**
         * Other details...
         */
        public List<DrawInstruction> DrawInstructions
        {
            get;
            private set;
        }

        protected int _DialogKey;
        public int DialogKey
        {
            get
            {
                return this._DialogKey;
            }

            set
            {
                if (this._DialogKey == 0)  // Once the dialog owner has been set, do not allow changes.
                {
                    this._DialogKey = value;
                }
            }
        }

        protected bool _Visible;
        public bool Visible
        {
            get
            {
                return this._Visible;
            }

            set
            {
                this._Visible = value;

                if (!value) // If this isn't visible, the control shouldn't accept input (unless told to).
                {
                    this.AcceptInputKeyboard = false;
                    this.AcceptInputMouse = false;
                }

                UpdateDrawInstruction("visible");
            }
        }

        /**
         * Keep track of whether mouse/keyboard input should be accepted by this control.
         */
        public bool AcceptInputMouse;
        public bool AcceptInputKeyboard;

        protected GameWindow ParentWindow;

        /**
         * Keep track of whether the mouse was pressed inside the control, or whether it was already pressed and simply moved inside the control.
         */
        protected bool MouseDownOnControl;

        /**
         * Keep track of whether the mouse has already entered the control, to prevent firing OnMouseEnter too many times.
         */
        protected bool MouseEntered;


        public ControlSkeleton()
        {
            this._Visible = true;
            DrawInstructions = new List<DrawInstruction>();
        }


        /// <summary>
        /// Update the position of this control based on border offsets and anchor status.
        /// </summary>
        /// <param name="offsetX">The x-offset of the control position.</param>
        /// <param name="offsetY">The y-offset of the control position.</param>
        protected void UpdatePosition(Vector2 offsets)
        {
            // Variables to store the final coordinates after manipulation
            float finalX;
            float finalY;

            switch (this.Anchor)
            {
                case AnchorType.None:
                    this._Position = offsets;
                    break;

                case AnchorType.Right:
                    finalX = this.ParentWindow.WindowScale[0] - offsets.x;
                    this._Position = new Vector2(finalX, offsets.y);
                    break;

                case AnchorType.Bottom:
                    finalY = this.ParentWindow.WindowScale[1] - offsets.y;
                    this._Position = new Vector2(offsets.x, finalY);
                    break;

                case AnchorType.BottomRight:
                    finalX = this.ParentWindow.WindowScale[0] - offsets.x;
                    finalY = this.ParentWindow.WindowScale[1] - offsets.y;
                    this._Position = new Vector2(finalX, finalY);
                    break;

                case AnchorType.TopCentre:
                    finalX = (this.ParentWindow.WindowScale[0] / 2) - (this.Width / 2) + offsets.x;
                    this._Position = new Vector2(finalX, offsets.y);
                    break;

                case AnchorType.BottomCentre:
                    finalX = (this.ParentWindow.WindowScale[0] / 2) - (this.Width / 2) + offsets.x;
                    finalY = this.ParentWindow.WindowScale[1] - offsets.y;
                    this._Position = new Vector2(finalX, finalY);
                    break;
            }
        }


        /// <summary>
        /// Base method to load all texture references used by the control.
        /// </summary>
        protected abstract void LoadReferences();


        /// <summary>
        /// Base method to update the control's draw instructions given the specified key.
        /// </summary>
        /// <param name="updateKey">The key to update the control's associated draw instructions with.</param>
        public virtual void UpdateDrawInstruction(string updateKey) { }


        /// <summary>
        /// Base method to check the status of the mouse in relation to the control, and fire off any events.
        /// </summary>
        public virtual void CheckMouse() { }


        /// <summary>
        /// Base method to check the status of the keyboard in relation to the control, and fire off any events.
        /// </summary>
        /// <param name="down">Whether the event state is a key down or up.</param>
        public virtual void CheckKeyboard(bool down, byte key) { }


        /// <summary>
        /// Clears all draw instructions from the control given the specified key.
        /// </summary>
        /// <param name="instructionKey">The key to clear all associated instructions with.</param>
        public void ClearInstructionsFromKey(string instructionKey)
        {
            foreach (DrawInstruction instruction in DrawInstructions)
            {
                if (instruction.InstructionKey == instructionKey)
                {
                    DrawInstructions.Remove(instruction);
                }
            }
        }
    }
}
