/**
 * RozWorld.Graphics.UI.InGame.MultiplayerMenu -- RozWorld Multiplayer Menu Control System
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
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
    public class MultiplayerMenu : ControlSystem
    {
        public MultiplayerMenu(GameWindow parentWindow)
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

            // Multiplayer title label
            Label screenTitle = new Label(this.ParentWindow);

            screenTitle.Text = "Multiplayer";
            screenTitle.ForeColour = VectorColour.OpaqueWhite;
            screenTitle.ZIndex = 1;
            screenTitle.Position = new Vector2(218, 148);
            screenTitle.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("ScreenTitle", screenTitle);

            // Connect button
            Button connectButton = new Button(this.ParentWindow);

            connectButton.Text = "Connect";
            connectButton.Width = 100;
            connectButton.Position = new Vector2(218, 288);
            connectButton.DialogKey = this.DialogKey;
            connectButton.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            connectButton.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            connectButton.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            connectButton.OnMouseUp += new SenderEventHandler(connectButton_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("ConnectButton", connectButton);

            // Specify (direct connect) button
            Button specifyButton = new Button(this.ParentWindow);

            specifyButton.Text = "Specify";
            specifyButton.Width = 100;
            specifyButton.Position = new Vector2(218, 328);
            specifyButton.DialogKey = this.DialogKey;
            specifyButton.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            specifyButton.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            specifyButton.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);

            ParentWindow.GameInterface.Controls.Add("SpecifyButton", specifyButton);

            SetupSubscribers();            
        }


        /// <summary>
        /// Implementation of the base subscriber setup method.
        /// </summary>
        public override void SetupSubscribers()
        {
            MouseSubscribers = new ControlSkeleton[] {
                ParentWindow.GameInterface.Controls["ConnectButton"],
                ParentWindow.GameInterface.Controls["SpecifyButton"]
            };
        } 


        /// <summary>
        /// Implementation of the base control system closing method.
        /// </summary>
        public override void Close()
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Implementation of the base control position updater method.
        /// </summary>
        public override void UpdateControlPositions()
        {
            
        }


        /// <summary>
        /// [Event] "Connect" button clicked.
        /// </summary>
        private void connectButton_OnMouseUp(object sender)
        {
            // TODO: connect code here
        }
    }
}
