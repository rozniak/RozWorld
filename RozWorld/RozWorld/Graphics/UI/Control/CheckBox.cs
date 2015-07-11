/**
 * RozWorld.Graphics.UI.Control.CheckBox -- RozWorld UI Checkbox Control
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using OpenGL;
using System.Drawing;
namespace RozWorld.Graphics.UI.Control
{
    public class CheckBox : ControlSkeleton
    {
        public bool _Checked;
        public bool Checked
        {
            get
            {
                return this._Checked;
            }

            set
            {
                this._Checked = value;
                UpdateDrawInstruction("tick");
            }
        }

        private bool _TransparentBody;
        public bool TransparentBody
        {
            get
            {
                return this._TransparentBody;
            }

            set
            {
                this._TransparentBody = value;
                UpdateDrawInstruction("body");
            }
        }

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

        /**
         * Texture references for this control.
         */
        private Texture CheckBoxBorderCornerRight;
        private Texture CheckBoxBorderCornerLeft;
        private Texture CheckBoxBorderTop;
        private Texture CheckBoxBorderSide;
        private Texture CheckBoxBody;
        private Texture CheckBoxTick;


        public CheckBox(GameWindow parentWindow, string name)
        {
            this.ParentWindow = parentWindow;
            LoadReferences();

            this._ForeColour = VectorColour.OpaqueBlack;
            this._TintColour = VectorColour.NoTint;
            this.Position = new Vector2(0, 0);
            this.Checked = false;
        }


        /// <summary>
        /// Implementation of the base texture reference loading method.
        /// </summary>
        public override void LoadReferences()
        {
            CheckBoxBorderCornerRight = ParentWindow.TextureManagement.GetTexture("CheckBoxBorderCornerRight");
            CheckBoxBorderCornerLeft = ParentWindow.TextureManagement.GetTexture("CheckBoxBorderCornerLeft");
            CheckBoxBorderSide = ParentWindow.TextureManagement.GetTexture("CheckBoxBorderSide");
            CheckBoxBorderTop = ParentWindow.TextureManagement.GetTexture("CheckBoxBorderTop");
            CheckBoxBody = ParentWindow.TextureManagement.GetTexture("CheckBoxBody");
            CheckBoxTick = ParentWindow.TextureManagement.GetTexture("CheckBoxTick");
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
                    int xCheckBox = (int)Position.x;
                    int yCheckBox = (int)Position.y;

                    DrawInstructions.Clear();

                    // Draw the CheckBox Border

                    // Top Left Corner //
                    DrawInstructions.Add(new DrawInstruction(
                        CheckBoxBorderCornerRight,
                        new Vector2(3, 3),
                        new Vector2(0, 0),
                        new Size(3, 3),
                        new Vector2(xCheckBox, yCheckBox),
                        ParentWindow,
                        TintColour,
                        "checkbox"));

                    // Top Bar //
                    DrawInstructions.Add(new DrawInstruction(
                        CheckBoxBorderTop,
                        new Vector2(0, 0),
                        new Vector2(1, 3),
                        new Size(22, 3),
                        new Vector2(xCheckBox + 3, yCheckBox),
                        ParentWindow,
                        TintColour,
                        "checkbox"));

                    // Top Right Corner //
                    DrawInstructions.Add(new DrawInstruction(
                        CheckBoxBorderCornerLeft,
                        new Vector2(3, 3),
                        new Vector2(0, 0),
                        new Size(3, 3),
                        new Vector2(xCheckBox + 25, yCheckBox),
                        ParentWindow,
                        TintColour,
                        "checkbox"));

                    // Left Side //
                    DrawInstructions.Add(new DrawInstruction(
                        CheckBoxBorderSide,
                        new Vector2(3, 1),
                        new Vector2(0, 0),
                        new Size(3, 22),
                        new Vector2(xCheckBox, yCheckBox + 3),
                        ParentWindow,
                        TintColour,
                        "checkbox"));

                    // Bottom Left Corner //
                    DrawInstructions.Add(new DrawInstruction(
                        CheckBoxBorderCornerLeft,
                        new Vector2(0, 0),
                        new Vector2(3, 3),
                        new Size(3, 3),
                        new Vector2(xCheckBox, yCheckBox + 25),
                        ParentWindow,
                        TintColour,
                        "checkbox"));

                    // Bottom Bar //
                    DrawInstructions.Add(new DrawInstruction(
                        CheckBoxBorderTop,
                        new Vector2(1, 3),
                        new Vector2(0, 0),
                        new Size(22, 3),
                        new Vector2(xCheckBox + 3, yCheckBox + 25),
                        ParentWindow,
                        TintColour,
                        "checkbox"));

                    // Bottom Right Corner //
                    DrawInstructions.Add(new DrawInstruction(
                        CheckBoxBorderCornerRight,
                        new Vector2(0, 0),
                        new Vector2(3, 3),
                        new Size(3, 3),
                        new Vector2(xCheckBox + 25, yCheckBox + 25),
                        ParentWindow,
                        TintColour,
                        "checkbox"));

                    // Right Side //
                    DrawInstructions.Add(new DrawInstruction(
                        CheckBoxBorderSide,
                        new Vector2(0, 0),
                        new Vector2(3, 1),
                        new Size(3, 22),
                        new Vector2(xCheckBox + 25, yCheckBox + 3),
                        ParentWindow,
                        TintColour,
                        "checkbox"));

                    DrawCheckBoxBody();

                    if (Checked)
                    {
                        DrawCheckBoxTick();
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
                        if (instruction.InstructionKey == "checkbox" || instruction.InstructionKey == "body")
                        {
                            instruction.TintColour = TintColour;
                        }
                        else if (instruction.InstructionKey == "tick")
                        {
                            instruction.TintColour = ForeColour;
                        }
                    }

                    break;

                case "body":
                    ClearInstructionsFromKey("body");

                    if (!TransparentBody)
                    {
                        DrawCheckBoxBody();
                    }

                    break;

                case "tick":
                    ClearInstructionsFromKey("tick");

                    if (Checked)
                    {
                        DrawCheckBoxTick();
                    }

                    break;
            }
        }


        /// <summary>
        /// Routine to create the drawing instruction for the checkbox body.
        /// </summary>
        private void DrawCheckBoxBody()
        {
            DrawInstructions.Add(new DrawInstruction(
                    CheckBoxBody,
                    new Vector2(0, 0),
                    new Vector2(22, 22),
                    new Size(22, 22),
                    new Vector2((int)Position.x + 3, (int)Position.y + 3),
                    ParentWindow,
                    TintColour,
                    "body"));
        }


        /// <summary>
        /// Routine to create the drawing instruction for the checkbox tick.
        /// </summary>
        private void DrawCheckBoxTick()
        {
            DrawInstructions.Add(new DrawInstruction(
                    CheckBoxTick,
                    new Vector2(0, 0),
                    new Vector2(22, 22),
                    new Size(22, 22),
                    new Vector2((int)Position.x + 3, (int)Position.y + 3),
                    ParentWindow,
                    ForeColour,
                    "tick"));
        }
    }
}
