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

using OpenGL;

namespace RozWorld.Graphics.UI.InGame
{
    public class SettingsMenu : ControlSystem
    {
        public SettingsMenu(GameWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
            this.DialogKey = 5;
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

            // Return button
            Button returnMenu = new Button(this.ParentWindow);

            returnMenu.Text = "Return...";
            returnMenu.Width = 200;
            returnMenu.Position = new Vector2(0, 500);
            returnMenu.Anchor = AnchorType.TopCentre;
            returnMenu.DialogKey = this.DialogKey;
            returnMenu.OnMouseDown += new SenderEventHandler(Button_OnMouseDown);
            returnMenu.OnMouseEnter += new SenderEventHandler(Button_OnMouseEnter);
            returnMenu.OnMouseLeave += new SenderEventHandler(Button_OnMouseLeave);
            returnMenu.OnMouseUp += new SenderEventHandler(returnMenu_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("ReturnSTMenuButton", returnMenu);

            SetupSubscribers();
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
            this.MouseSubscribers = new ControlSkeleton[] {
                ParentWindow.GameInterface.Controls["ReturnSTMenuButton"]
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
            ParentWindow.GameInterface.Controls["ReturnSTMenuButton"].UpdatePosition();
        }
    }
}
