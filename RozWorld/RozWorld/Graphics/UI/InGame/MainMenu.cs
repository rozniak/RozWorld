//
// RozWorld.Graphics.UI.InGame.MainMenu -- RozWorld Main Menu Control System
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

using RozWorld.Graphics.UI;
using RozWorld.Graphics.UI.Control;

using System;

using OpenGL;

namespace RozWorld.Graphics.UI.InGame
{
    public class MainMenu : ControlSystem
    {
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
            // RozWorld logo
            Image title = new Image(this.ParentWindow);

            title.TextureName = "Title";
            title.ZIndex = 1;
            title.Position = new Vector2(106, 12);
            title.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("Title", title);

            // Background
            Image backDrop = new Image(this.ParentWindow);

            backDrop.TextureName = "BackDrop";
            backDrop.ZIndex = 0;
            backDrop.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("BackDrop", backDrop);

            // Play game button
            Button playGame = new Button(this.ParentWindow);

            playGame.Text = "Play Game";
            playGame.Width = 200;
            playGame.Position = new Vector2(218, 148);
            playGame.DialogKey = this.DialogKey;
            playGame.OnMouseDown += new SenderEventHandler(Button_OnMouseDown);
            playGame.OnMouseEnter += new SenderEventHandler(Button_OnMouseEnter);
            playGame.OnMouseLeave += new SenderEventHandler(Button_OnMouseLeave);
            playGame.OnMouseUp += new SenderEventHandler(playGame_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("PlayGameButton", playGame);

            // Multiplayer button
            Button multiplayer = new Button(this.ParentWindow);

            multiplayer.Text = "Multiplayer";
            multiplayer.Width = 200;
            multiplayer.Position = new Vector2(218, 188);
            multiplayer.DialogKey = this.DialogKey;
            multiplayer.OnMouseDown += new SenderEventHandler(Button_OnMouseDown);
            multiplayer.OnMouseEnter += new SenderEventHandler(Button_OnMouseEnter);
            multiplayer.OnMouseLeave += new SenderEventHandler(Button_OnMouseLeave);
            multiplayer.OnMouseUp += new SenderEventHandler(multiplayer_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("MultiplayerButton", multiplayer);

            // Settings button
            Button settings = new Button(this.ParentWindow);

            settings.Text = "Settings";
            settings.Width = 200;
            settings.Position = new Vector2(218, 228);
            settings.DialogKey = this.DialogKey;
            settings.OnMouseDown += new SenderEventHandler(Button_OnMouseDown);
            settings.OnMouseEnter += new SenderEventHandler(Button_OnMouseEnter);
            settings.OnMouseLeave += new SenderEventHandler(Button_OnMouseLeave);
            settings.OnMouseUp += new SenderEventHandler(settings_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("SettingsButton", settings);

            // Exit game button
            Button exitGame = new Button(this.ParentWindow);

            exitGame.Text = "Exit Game";
            exitGame.Width = 200;
            exitGame.Position = new Vector2(218, 268);
            exitGame.DialogKey = this.DialogKey;
            exitGame.OnMouseDown += new SenderEventHandler(Button_OnMouseDown);
            exitGame.OnMouseEnter += new SenderEventHandler(Button_OnMouseEnter);
            exitGame.OnMouseLeave += new SenderEventHandler(Button_OnMouseLeave);
            exitGame.OnMouseUp += new SenderEventHandler(exitGame_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("ExitGameButton", exitGame);

            SetupSubscribers();
        }


        /// <summary>
        /// [Event] "Settings" button clicked.
        /// </summary>
        void settings_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;
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


        /// <summary>
        /// [Event] Generic button mouse down.
        /// </summary>
        /// <param name="sender"></param>
        void Button_OnMouseDown(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonDownTint;
        }


        /// <summary>
        /// [Event] Generic button mouse enter.
        /// </summary>
        /// <param name="sender"></param>
        void Button_OnMouseEnter(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;
        }


        /// <summary>
        /// [Event] Generic button mouse leave.
        /// </summary>
        /// <param name="sender"></param>
        void Button_OnMouseLeave(object sender)
        {
            ((Button)sender).TintColour = VectorColour.NoTint;
        }


        /// <summary>
        /// Implementation of the base subscriber setup method.
        /// </summary>
        public override void SetupSubscribers()
        {
            MouseSubscribers = new ControlSkeleton[] {
                ParentWindow.GameInterface.Controls["PlayGameButton"],
                ParentWindow.GameInterface.Controls["MultiplayerButton"],
                ParentWindow.GameInterface.Controls["SettingsButton"],
                ParentWindow.GameInterface.Controls["ExitGameButton"]
            };
        }


        /// <summary>
        /// Implementation of the base control system closing method.
        /// </summary>
        public override void Close()
        {
            ParentWindow.GameInterface.KillFromDialogKey(this.DialogKey);
        }
    }
}