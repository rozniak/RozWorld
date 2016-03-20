/**
 * RozWorld.Graphics.UI.InGame.SinglePlayerMenu -- RozWorld Singleplayer Menu Control System
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using OpenGL;

using RozWorld.Graphics.UI;
using RozWorld.Graphics.UI.Control;
using RozWorld.Graphics.UI.InGame.Generic;


namespace RozWorld.Graphics.UI.InGame
{
    public class SinglePlayerMenu : ControlSystem
    {
        public SinglePlayerMenu(GameWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
            this.DialogKey = 3;
        }


        /// <summary>
        /// Implementation of the base control system starting method.
        /// </summary>
        public override void Start()
        {
            // Hide the main menu controls
            ParentWindow.GameInterface.Controls["PlayGameButton"].Visible = false;
            ((Button)ParentWindow.GameInterface.Controls["PlayGameButton"]).TintColour = VectorColour.NoTint;
            ParentWindow.GameInterface.Controls["MultiplayerButton"].Visible = false;
            ((Button)ParentWindow.GameInterface.Controls["MultiplayerButton"]).TintColour = VectorColour.NoTint;
            ParentWindow.GameInterface.Controls["SettingsButton"].Visible = false;
            ((Button)ParentWindow.GameInterface.Controls["SettingsButton"]).TintColour = VectorColour.NoTint;
            ParentWindow.GameInterface.Controls["ExitGameButton"].Visible = false;
            ((Button)ParentWindow.GameInterface.Controls["ExitGameButton"]).TintColour = VectorColour.NoTint;

            // Single player title label
            Label screenTitle = new Label(this.ParentWindow);

            screenTitle.Text = "Play Game";
            screenTitle.ForeColour = VectorColour.OpaqueWhite;
            screenTitle.ZIndex = 1;
            screenTitle.Position = new Vector2(-100, 148);
            screenTitle.Anchor = AnchorType.TopCentre;
            screenTitle.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("ScreenTitle", screenTitle);

            // New world button
            Button newWorld = new Button(this.ParentWindow);

            newWorld.Text = "New World...";
            newWorld.Width = 200;
            newWorld.Position = new Vector2(0, 288);
            newWorld.Anchor = AnchorType.TopCentre;
            newWorld.DialogKey = this.DialogKey;
            newWorld.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            newWorld.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            newWorld.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            newWorld.OnMouseUp += new SenderEventHandler(newWorld_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("NewWorldButton", newWorld);

            // Return button
            Button returnMenu = new Button(this.ParentWindow);

            returnMenu.Text = "Return...";
            returnMenu.Width = 200;
            returnMenu.Position = new Vector2(0, 328);
            returnMenu.Anchor = AnchorType.TopCentre;
            returnMenu.DialogKey = this.DialogKey;
            returnMenu.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            returnMenu.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            returnMenu.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            returnMenu.OnMouseUp += new SenderEventHandler(returnMenu_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("ReturnSPMenuButton", returnMenu);

            SetupSubscribers();
        }


        /// <summary>
        /// Implementation of the base subscriber setup method.
        /// </summary>
        public override void SetupSubscribers()
        {
            this.MouseSubscribers = new ControlSkeleton[] {
                ParentWindow.GameInterface.Controls["NewWorldButton"],
                ParentWindow.GameInterface.Controls["ReturnSPMenuButton"]
            };
        }


        /// <summary>
        /// Implementation of the base control system closing method.
        /// </summary>
        public override void Close()
        {
            ParentWindow.GameInterface.KillFromDialogKey(this.DialogKey);
            ParentWindow.GameInterface.ControlSystems.Remove("SinglePlayerMenu");

            // Show the main menu controls
            ParentWindow.GameInterface.Controls["PlayGameButton"].Visible = true;
            ParentWindow.GameInterface.Controls["MultiplayerButton"].Visible = true;
            ParentWindow.GameInterface.Controls["SettingsButton"].Visible = true;
            ParentWindow.GameInterface.Controls["ExitGameButton"].Visible = true;
        }


        /// <summary>
        /// Implementation of the base control position updater method.
        /// </summary>
        public override void UpdateControlPositions()
        {
            ParentWindow.GameInterface.Controls["Title"].UpdatePosition();
            ParentWindow.GameInterface.Controls["ScreenTitle"].UpdatePosition();
            ParentWindow.GameInterface.Controls["NewWorldButton"].UpdatePosition();
            ParentWindow.GameInterface.Controls["ReturnSPMenuButton"].UpdatePosition();
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
        /// [Event] "New World" button clicked.
        /// </summary>
        void newWorld_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;

            // Start the new world menu
            ParentWindow.GameInterface.ControlSystems.Add("NewWorldMenu", new NewWorldMenu(this.ParentWindow));
            ParentWindow.GameInterface.ControlSystems["NewWorldMenu"].Start();
        }
    }
}
