//
// RozWorld.Graphics.UI.Control.ControlSkeleton -- RozWorld UI Control Skeleton
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

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

        protected FloatPoint _Position;
        public FloatPoint Position
        {
            get
            {
                return this._Position;
            }

            set
            {
                this._Position = value;
                UpdateDrawInstruction("position");
            }
        }

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
                UpdateDrawInstruction("visible");
            }
        }

        protected GameWindow ParentWindow;

        // Keep track of whether the mouse was pressed inside the control, or whether it was already pressed and simply moved inside the control.
        protected bool MouseDownOnControl;

        // Keep track of whether the mouse has already entered the control, to prevent firing OnMouseEnter too many times.
        protected bool MouseEntered;


        public ControlSkeleton()
        {
            this._Visible = true;
            DrawInstructions = new List<DrawInstruction>();
        }        


        /// <summary>
        /// Base method to load all texture references used by the control.
        /// </summary>
        public abstract void LoadReferences();


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
