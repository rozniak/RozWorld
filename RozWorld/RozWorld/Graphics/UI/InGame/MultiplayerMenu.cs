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

using RozWorld.Graphics.UI;
using RozWorld.Graphics.UI.Control;
using RozWorld.Graphics.UI.InGame.Generic;

using OpenGL;

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
            ParentWindow.GameInterface.Controls["MultiplayerButton"].Visible = false;
            ParentWindow.GameInterface.Controls["SettingsButton"].Visible = false;
            ParentWindow.GameInterface.Controls["ExitGameButton"].Visible = false;

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
            connectButton.Width = 200;
            connectButton.Position = new Vector2(218, 288);
            connectButton.DialogKey = this.DialogKey;
            connectButton.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            connectButton.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            connectButton.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            connectButton.OnMouseUp += new SenderEventHandler(connectButton_OnMouseUp);

            // Specify (direct connect) button
            Button specifyButton = new Button(this.ParentWindow);

            specifyButton.Text = "Specify";
            specifyButton.Width = 200;
            specifyButton.Position = new Vector2(218, 328);
            specifyButton.DialogKey = this.DialogKey;
            specifyButton.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            specifyButton.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            specifyButton.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);

            SetupSubscribers();            
        }


        /// <summary>
        /// Implementation of the base subscriber setup method.
        /// </summary>
        public override void SetupSubscribers()
        {
            base.SetupSubscribers();
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
            // TODO: Finish this screen, sort of.
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
