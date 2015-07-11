/**
 * RozWorld.Graphics.UI.InGame.SinglePlayerMenu -- RozWorld Singleplayer Menu Control System
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
            ParentWindow.GameInterface.Controls["MultiplayerButton"].Visible = false;
            ParentWindow.GameInterface.Controls["SettingsButton"].Visible = false;
            ParentWindow.GameInterface.Controls["ExitGameButton"].Visible = false;

            // Single player title label
            Label screenTitle = new Label(this.ParentWindow);

            screenTitle.Text = "Play Game";
            screenTitle.ForeColour = VectorColour.OpaqueWhite;
            screenTitle.ZIndex = 1;
            screenTitle.Position = new Vector2(218, 148);
            screenTitle.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("ScreenTitle", screenTitle);

            // New game button
            Button newWorld = new Button(this.ParentWindow);

            newWorld.Text = "New World...";
            newWorld.Width = 200;
            newWorld.Position = new Vector2(218, 288);
            newWorld.DialogKey = this.DialogKey;
            newWorld.OnMouseDown += new SenderEventHandler(Button_OnMouseDown);
            newWorld.OnMouseEnter += new SenderEventHandler(Button_OnMouseEnter);
            newWorld.OnMouseLeave += new SenderEventHandler(Button_OnMouseLeave);
            newWorld.OnMouseUp += new SenderEventHandler(newWorld_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("NewWorldButton", newWorld);

            // Return button
            Button returnMenu = new Button(this.ParentWindow);

            returnMenu.Text = "Return...";
            returnMenu.Width = 200;
            returnMenu.Position = new Vector2(218, 328);
            returnMenu.DialogKey = this.DialogKey;
            returnMenu.OnMouseDown += new SenderEventHandler(Button_OnMouseDown);
            returnMenu.OnMouseEnter += new SenderEventHandler(Button_OnMouseEnter);
            returnMenu.OnMouseLeave += new SenderEventHandler(Button_OnMouseLeave);
            returnMenu.OnMouseUp += new SenderEventHandler(returnMenu_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("ReturnSPMenuButton", returnMenu);

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
        /// [Event] "New World" button clicked.
        /// </summary>
        void newWorld_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;

            // Start the new world menu
            ParentWindow.GameInterface.ControlSystems.Add("NewWorldMenu", new NewWorldMenu(this.ParentWindow));
            ParentWindow.GameInterface.ControlSystems["NewWorldMenu"].Start();
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
    }
}
