/**
 * RozWorld.Graphics.UI.InGame.SettingsMenu -- RozWorld Settings Menu Control System
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.Graphics.UI.Control;
using RozWorld.Graphics.UI.InGame.Generic;

using OpenGL;

namespace RozWorld.Graphics.UI.InGame
{
    public class SettingsMenu : ControlSystem
    {
        public SettingsMenu(GameWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
            this.DialogKey = 6;
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

            // Settings title label
            Label screenTitle = new Label(this.ParentWindow);

            screenTitle.Text = "Settings (unfinished)";
            screenTitle.ForeColour = VectorColour.OpaqueWhite;
            screenTitle.ZIndex = 1;
            screenTitle.Position = new Vector2(-100, 148);
            screenTitle.Anchor = AnchorType.TopCentre;
            screenTitle.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("ScreenTitle", screenTitle);

            // Video settings button
            Button videoSettings = new Button(this.ParentWindow);

            videoSettings.Text = "Video Settings...";
            videoSettings.Width = 200;
            videoSettings.Position = new Vector2(-110, 180);
            videoSettings.Anchor = AnchorType.TopCentre;
            videoSettings.DialogKey = this.DialogKey;
            videoSettings.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            videoSettings.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            videoSettings.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            videoSettings.OnMouseUp += new SenderEventHandler(videoSettings_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("VideoSettingsButton", videoSettings);

            // Return button
            Button returnMenu = new Button(this.ParentWindow);

            returnMenu.Text = "Return...";
            returnMenu.Width = 200;
            returnMenu.Position = new Vector2(0, 500);
            returnMenu.Anchor = AnchorType.TopCentre;
            returnMenu.DialogKey = this.DialogKey;
            returnMenu.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            returnMenu.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            returnMenu.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            returnMenu.OnMouseUp += new SenderEventHandler(returnMenu_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("ReturnSTMenuButton", returnMenu);

            SetupSubscribers();
        }


        /// <summary>
        /// Implementation of the base subscriber setup method.
        /// </summary>
        public override void SetupSubscribers()
        {
            this.MouseSubscribers = new ControlSkeleton[] {
                ParentWindow.GameInterface.Controls["ReturnSTMenuButton"],
                ParentWindow.GameInterface.Controls["VideoSettingsButton"]
            };
        }


        /// <summary>
        /// Implementation of the base control system closing method.
        /// </summary>
        public override void Close()
        {
            ParentWindow.GameInterface.KillFromDialogKey(this.DialogKey);
            ParentWindow.GameInterface.ControlSystems.Remove("SettingsMenu");

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
            ParentWindow.GameInterface.Controls["VideoSettingsButton"].UpdatePosition();
            ParentWindow.GameInterface.Controls["ReturnSTMenuButton"].UpdatePosition();
        }


        /// <summary>
        /// [Event] "Video Settings..." button clicked.
        /// </summary>
        void videoSettings_OnMouseUp(object sender)
        {
            ParentWindow.GameInterface.ControlSystems.Add("VideoSettingsMenu", new VideoSettingsMenu(this.ParentWindow));
            ParentWindow.GameInterface.ControlSystems["VideoSettingsMenu"].Start();
        }


        /// <summary>
        /// [Event] "Return..." button clicked.
        /// </summary>
        /// <param name="sender"></param>
        void returnMenu_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;
            this.Close();
        }
    }
}
