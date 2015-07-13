/**
 * RozWorld.Graphics.GUI.InGame.NewWorldMenu -- RozWorld New World Menu Control System
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.Graphics.UI;
using RozWorld.Graphics.UI.Control;

using OpenGL;

namespace RozWorld.Graphics.UI.InGame
{
    public class NewWorldMenu : ControlSystem
    {
        public NewWorldMenu(GameWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
            this.DialogKey = 4;
        }


        /// <summary>
        /// Implementation of the base control system starting method.
        /// </summary>
        public override void Start()
        {
            // Hide the singleplayer menu controls
            ParentWindow.GameInterface.Controls["NewWorldButton"].Visible = false;
            ((Button)ParentWindow.GameInterface.Controls["NewWorldButton"]).TintColour = VectorColour.NoTint;
            ParentWindow.GameInterface.Controls["ReturnSPMenuButton"].Visible = false;
            ((Button)ParentWindow.GameInterface.Controls["ReturnSPMenuButton"]).TintColour = VectorColour.NoTint;

            // Change title of menu
            ((Label)ParentWindow.GameInterface.Controls["ScreenTitle"]).Text = "New World";

            // World name label
            Label tagWorldName = new Label(this.ParentWindow);

            tagWorldName.Text = "World Name:";
            tagWorldName.ForeColour = VectorColour.OpaqueWhite;
            tagWorldName.ZIndex = 1;
            tagWorldName.Position = new Vector2(-100, 180);
            tagWorldName.Anchor = AnchorType.TopCentre;
            tagWorldName.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("TagWorldName", tagWorldName);

            // World name text box
            TextBox worldName = new TextBox(this.ParentWindow);

            worldName.Width = 200;
            worldName.ForeColour = VectorColour.OpaqueBlack;
            worldName.ZIndex = 1;
            worldName.Position = new Vector2(0, 200);
            worldName.Anchor = AnchorType.TopCentre;
            worldName.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("WorldNameBox", worldName);

            // Play button
            Button playWorld = new Button(this.ParentWindow);

            playWorld.Text = "Play!";
            playWorld.Width = 200;
            playWorld.Position = new Vector2(0, 288);
            playWorld.Anchor = AnchorType.TopCentre;
            playWorld.DialogKey = this.DialogKey;
            playWorld.OnMouseDown += new SenderEventHandler(Button_OnMouseDown);
            playWorld.OnMouseEnter += new SenderEventHandler(Button_OnMouseEnter);
            playWorld.OnMouseLeave += new SenderEventHandler(Button_OnMouseLeave);
            playWorld.OnMouseUp += new SenderEventHandler(playWorld_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("PlayWorldButton", playWorld);

            // Return button
            Button returnMenu = new Button(this.ParentWindow);

            returnMenu.Text = "Return...";
            returnMenu.Width = 200;
            returnMenu.Position = new Vector2(0, 328);
            returnMenu.Anchor = AnchorType.TopCentre;
            returnMenu.DialogKey = this.DialogKey;
            returnMenu.OnMouseDown += new SenderEventHandler(Button_OnMouseDown);
            returnMenu.OnMouseEnter += new SenderEventHandler(Button_OnMouseEnter);
            returnMenu.OnMouseLeave += new SenderEventHandler(Button_OnMouseLeave);
            returnMenu.OnMouseUp += new SenderEventHandler(returnMenu_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("ReturnNWMenuButton", returnMenu);

            SetupSubscribers();
        }


        /// <summary>
        /// [Event] "Return..." button clicked.
        /// </summary>
        void returnMenu_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;
            this.Close();
        }


        /// <summary>
        /// [Event] "Play!" button clicked.
        /// </summary>
        void playWorld_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;

            // TODO: Start the game with a new world here
        }


        /// <summary>
        /// [Event] Generic button mouse leave.
        /// </summary>
        void Button_OnMouseLeave(object sender)
        {
            ((Button)sender).TintColour = VectorColour.NoTint;
        }


        /// <summary>
        /// [Event] Generic button mouse enter.
        /// </summary>
        void Button_OnMouseEnter(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;
        }


        /// <summary>
        /// [Event] Generic button mouse down.
        /// </summary>
        void Button_OnMouseDown(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonDownTint;
        }


        /// <summary>
        /// Implementation of the base subscriber setup method.
        /// </summary>
        public override void SetupSubscribers()
        {
            MouseSubscribers = new ControlSkeleton[] {
                ParentWindow.GameInterface.Controls["PlayWorldButton"],
                ParentWindow.GameInterface.Controls["ReturnNWMenuButton"]
            };
        }


        /// <summary>
        /// Implementation of the base control system closing method.
        /// </summary>
        public override void Close()
        {
            ParentWindow.GameInterface.KillFromDialogKey(this.DialogKey);

            // Change title of menu
            ((Label)ParentWindow.GameInterface.Controls["ScreenTitle"]).Text = "Play Game";

            // Show the singleplayer menu controls
            ParentWindow.GameInterface.Controls["NewWorldButton"].Visible = true;
            ParentWindow.GameInterface.Controls["ReturnSPMenuButton"].Visible = true;

            ParentWindow.GameInterface.ControlSystems.Remove("NewWorldMenu");
        }


        /// <summary>
        /// Implementation of the base control position updater method.
        /// </summary>
        public override void UpdateControlPositions()
        {
            ParentWindow.GameInterface.Controls["Title"].UpdatePosition();
            ParentWindow.GameInterface.Controls["ScreenTitle"].UpdatePosition();
            ParentWindow.GameInterface.Controls["PlayWorldButton"].UpdatePosition();
            ParentWindow.GameInterface.Controls["ReturnNWMenuButton"].UpdatePosition();
            ParentWindow.GameInterface.Controls["WorldNameBox"].UpdatePosition();
            ParentWindow.GameInterface.Controls["TagWorldName"].UpdatePosition();
        }
    }
}
