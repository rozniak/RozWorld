/**
 * RozWorld.Graphics.UI.Control.ZetaLabel -- RozWorld UI Label Control Updated
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


namespace RozWorld.Graphics.UI.Control
{
    public class ZetaLabel : ControlSkeleton
    {
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


        private List<DrawInstruction> _DrawInstructions;
        public override List<DrawInstruction> DrawInstructions
        {
            get { return _DrawInstructions; }
            protected set { _DrawInstructions = value; }
        }

        private Texture StringTexture;


        public event KeyEventHandler OnKeyDown;
        public event KeyEventHandler OnKeyUp;
        public event SenderEventHandler OnMouseDown;
        public event SenderEventHandler OnMouseEnter;
        public event SenderEventHandler OnMouseLeave;
        public event SenderEventHandler OnMouseUp;


        public ZetaLabel(GameWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
            this.Position = new Vector2(0, 0);
            this._Font = FontType.ChatFont;
            this._Text = "";
            this.ZIndex = 1;
        }


        /// <summary>
        /// Implementation of the base texture reference loading method.
        /// </summary>
        protected override void LoadReferences()
        {
            // For now, this does nothing
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
                    if (StringTexture != null)
                        StringTexture.Dispose();
                    StringTexture = FontProvider.BuildString(Font, Text, out _DrawInstructions, StringFormatting.Both);
                    break;

                case "visible":
                    break;

                case "colour":
                default:
                    break;
            }
        }
    }
}
