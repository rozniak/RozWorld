/**
 * RozWorld.Graphics.UI.InGame.MainMenu -- RozWorld Main Menu Control System
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

using System;
using System.Timers;


namespace RozWorld.Graphics.UI.InGame
{
    internal class MainMenu : ControlSystem
    {
        /**
         * Temporary backdrop animation code, will be moved to a separate control system
         * in future...
         */
        private Timer BackDropAnimator = new Timer(50);
        private float BackDropSpeed = 1f;
        private float BackDropTopSpeed = 5f;
        private Vector2[] BackDropTargets = new Vector2[] {
            new Vector2(0, -16),
            new Vector2(-16, -16),
            new Vector2(-16, 0),
            new Vector2(16, 32),
            new Vector2(32, 32),
            new Vector2(32, 16)
        };
        private int CurrentTarget = 0;


        public MainMenu(GameWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
            this.DialogKey = 2;
        }


        /// <summary>
        /// Implementation of the base control system starting method.
        /// </summary>
        public override void Start()
        {
            //    // Test ZetaLabel
            //    ZetaLabel testLabel = new ZetaLabel(this.ParentWindow);

            //    testLabel.Text = "Hello World!";
            //    testLabel.Font = FontType.ChatFont;
            //    testLabel.Position = new Vector2(64, 64);

            //    ParentWindow.GameInterface.Controls.Add("TestLabel", testLabel);

            // RozWorld logo
            Image title = new Image(this.ParentWindow);

            title.TextureName = "Title";
            title.ZIndex = 1;
            title.Position = new Vector2(0, 12);
            title.Anchor = AnchorType.TopCentre;
            title.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("Title", title);

            // Background
            Image backDrop = new Image(this.ParentWindow);

            backDrop.TextureName = "BackDrop";
            backDrop.ZIndex = 0;
            backDrop.DialogKey = this.DialogKey;
            backDrop.SizeMode = ImageSizeMode.Tile;
            backDrop.Dimensions = ParentWindow.WindowScale;

            ParentWindow.GameInterface.Controls.Add("BackDrop", backDrop);

            // Play game button
            Button playGame = new Button(this.ParentWindow);

            playGame.Text = "Play Game";
            playGame.Width = 200;
            playGame.Position = new Vector2(0, 178);
            playGame.Anchor = AnchorType.TopCentre;
            playGame.DialogKey = this.DialogKey;
            //playGame.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            //playGame.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            //playGame.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            //playGame.OnMouseUp += new SenderEventHandler(playGame_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("PlayGameButton", playGame);

            // Multiplayer button
            Button multiplayer = new Button(this.ParentWindow);

            multiplayer.Text = "Multiplayer";
            multiplayer.Width = 200;
            multiplayer.Position = new Vector2(0, 218);
            multiplayer.Anchor = AnchorType.TopCentre;
            multiplayer.DialogKey = this.DialogKey;
            //multiplayer.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            //multiplayer.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            //multiplayer.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            //multiplayer.OnMouseUp += new SenderEventHandler(multiplayer_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("MultiplayerButton", multiplayer);

            // Settings button
            Button settings = new Button(this.ParentWindow);

            settings.Text = "Settings";
            settings.Width = 200;
            settings.Position = new Vector2(0, 258);
            settings.Anchor = AnchorType.TopCentre;
            settings.DialogKey = this.DialogKey;
            //settings.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            //settings.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            //settings.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            //settings.OnMouseUp += new SenderEventHandler(settings_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("SettingsButton", settings);

            // Exit game button
            Button exitGame = new Button(this.ParentWindow);

            exitGame.Text = "Exit Game";
            exitGame.Width = 200;
            exitGame.Position = new Vector2(0, 298);
            exitGame.Anchor = AnchorType.TopCentre;
            exitGame.DialogKey = this.DialogKey;
            //exitGame.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            //exitGame.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            //exitGame.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            //exitGame.OnMouseUp += new SenderEventHandler(exitGame_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("ExitGameButton", exitGame);

            // Back drop animator timer
            BackDropAnimator.Elapsed += new ElapsedEventHandler(BackDropAnimator_Elapsed);
            BackDropAnimator.Start();
        }


        /// <summary>
        /// Implementation of the base control system closing method.
        /// </summary>
        public override void Close()
        {
            ParentWindow.GameInterface.KillFromDialogKey(this.DialogKey);
        }


        /// <summary>
        /// Implementation of the base control position updater method.
        /// </summary>
        public override void UpdateControlPositions()
        {
            ParentWindow.GameInterface.Controls["Title"].UpdatePosition();
            ParentWindow.GameInterface.Controls["PlayGameButton"].UpdatePosition();
            ParentWindow.GameInterface.Controls["MultiplayerButton"].UpdatePosition();
            ParentWindow.GameInterface.Controls["SettingsButton"].UpdatePosition();
            ParentWindow.GameInterface.Controls["ExitGameButton"].UpdatePosition();

            // Backdrop details
            ParentWindow.GameInterface.Controls["BackDrop"].UpdatePosition();
            ((Image)ParentWindow.GameInterface.Controls["BackDrop"]).Dimensions = new System.Drawing.Size(ParentWindow.WindowScale.Width + 16,
                ParentWindow.WindowScale.Height + 16);
        }


        /// <summary>
        /// [Event] Back drop animation timer elapsed.
        /// </summary>
        void BackDropAnimator_Elapsed(object sender, ElapsedEventArgs e)
        {

        }


        /// <summary>
        /// [Event] "Settings" button clicked.
        /// </summary>
        void settings_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;

            // Start the settings menu
            ParentWindow.GameInterface.ControlSystems.Add("SettingsMenu", new SettingsMenu(this.ParentWindow));
            ParentWindow.GameInterface.ControlSystems["SettingsMenu"].Start();
        }


        /// <summary>
        /// [Event] "Multiplayer" button clicked.
        /// </summary>
        void multiplayer_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;
        }


        /// <summary>
        /// [Event] "Play Game" button clicked.
        /// </summary>
        void playGame_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;

            // Start the singleplayer menu
            ParentWindow.GameInterface.ControlSystems.Add("SinglePlayerMenu", new SinglePlayerMenu(this.ParentWindow));
            ParentWindow.GameInterface.ControlSystems["SinglePlayerMenu"].Start();
        }


        /// <summary>
        /// [Event] "Exit Game" button clicked.
        /// </summary>
        void exitGame_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;
            Environment.Exit(1);
        }
    }
}