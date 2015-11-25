/**
 * RozWorld.Graphics.UI.Control.ZetaLabel -- RozWorld UI Label Control Updated
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using OpenGL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RozWorld.Graphics.UI.Control
{
    public class ZetaLabel : ControlSkeleton
    {
        private Vector4 _DefaultColour;
        public Vector4 DefaultColour
        {
            get { return this._DefaultColour; }
            set
            {
                this._DefaultColour = value;
                UpdateDrawInstruction("colour");
            }
        }

        private string _Text;
        public string Text
        {
            get { return this._Text; }
            set
            {
                this._Text = value;
                UpdateDrawInstruction("text");
            }
        }


        private FontType _Font;
        public FontType Font
        {
            get { return this._Font; }
            set
            {
                this._Font = value;
                UpdateDrawInstruction("text");
            }
        }


        public event KeyEventHandler OnKeyDown;
        public event KeyEventHandler OnKeyUp;
        public event SenderEventHandler OnMouseDown;
        public event SenderEventHandler OnMouseEnter;
        public event SenderEventHandler OnMouseLeave;
        public event SenderEventHandler OnMouseUp;


        public ZetaLabel(GameWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
            this._DefaultColour = VectorColour.OpaqueWhite;
            this.Position = new Vector2(0, 0);
            this._Font = FontType.ChatFont;
            this._Text = "";
            this.ZIndex = 1;
        }


        /// <summary>
        /// Implementation of the base draw instruction updating method.
        /// </summary>
        /// <param name="updateKey">The update key representing the desired update.</param>
        public override void UpdateDrawInstruction(string updateKey)
        {
            // TODO: Sort this out with FontProvider
            switch (updateKey)
            {
                case "position":
                    break;

                case "text":
                    break;

                case "visible":
                    break;

                case "colour":
                    break;
            }
        }
    }
}
