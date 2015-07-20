/**
 * RozWorld.Graphics.UI.InGame.VideoSettingsMenu -- RozWorld Video Settings Menu Control System
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
    public class VideoSettingsMenu : ControlSystem
    {
        public VideoSettingsMenu(GameWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
            this.DialogKey = 7;
        }


        /// <summary>
        /// Implementation of the base control system starting method.
        /// </summary>
        public override void Start()
        {
            // Hide the settings menu controls
            ParentWindow.GameInterface.Controls["VideoSettingsButton"].Visible = false;
            ((Button)ParentWindow.GameInterface.Controls["VideoSettingsButton"]).TintColour = VectorColour.NoTint;
            ParentWindow.GameInterface.Controls["ReturnSTMenuButton"].Visible = false;
            ((Button)ParentWindow.GameInterface.Controls["ReturnSTMenuButton"]).TintColour = VectorColour.NoTint;

            // Change title of menu
            ((Label)ParentWindow.GameInterface.Controls["ScreenTitle"]).Text = "Video Settings";
        
            // Window resolution label
            Label resolutionLabel = new Label(this.ParentWindow);

            resolutionLabel.Text = "Preferred Resolution:";
            resolutionLabel.Position = new Vector2(-315, 188);
            resolutionLabel.Anchor = AnchorType.TopCentre;
            resolutionLabel.ForeColour = VectorColour.OpaqueWhite;
            resolutionLabel.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("ResolutionLabel", resolutionLabel);

            // Window resolution button
            Button resolutionButton = new Button(this.ParentWindow);

            resolutionButton.Text = "800x600";
            resolutionButton.Width = 100;
            resolutionButton.Position = new Vector2(-50, 180);
            resolutionButton.Anchor = AnchorType.TopCentre;
            resolutionButton.DialogKey = this.DialogKey;
            resolutionButton.OnMouseDown += new SenderEventHandler(Button_OnMouseDown);
            resolutionButton.OnMouseEnter += new SenderEventHandler(Button_OnMouseEnter);
            resolutionButton.OnMouseLeave += new SenderEventHandler(Button_OnMouseLeave);
            resolutionButton.OnMouseUp += new SenderEventHandler(resolutionButton_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("ResolutionButton", resolutionButton);

            // Minimum size is preferred label
            Label minimumSizeLabel = new Label(this.ParentWindow);

            minimumSizeLabel.Text = "Preferred Res is Minimum:";
            minimumSizeLabel.Position = new Vector2(-355, 228);
            minimumSizeLabel.Anchor = AnchorType.TopCentre;
            minimumSizeLabel.ForeColour = VectorColour.OpaqueWhite;
            minimumSizeLabel.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("MinimumSizeLabel", minimumSizeLabel);

            // Minimum size is preferred button
            Button minimumSizeButton = new Button(this.ParentWindow);

            minimumSizeButton.Text = "TRUE";
            minimumSizeButton.Width = 100;
            minimumSizeButton.Position = new Vector2(-50, 220);
            minimumSizeButton.Anchor = AnchorType.TopCentre;
            minimumSizeButton.DialogKey = this.DialogKey;
            minimumSizeButton.OnMouseDown += new SenderEventHandler(Button_OnMouseDown);
            minimumSizeButton.OnMouseEnter += new SenderEventHandler(Button_OnMouseEnter);
            minimumSizeButton.OnMouseLeave += new SenderEventHandler(Button_OnMouseLeave);
            minimumSizeButton.OnMouseUp += new SenderEventHandler(minimumSizeButton_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("MinimumSizeButton", minimumSizeButton);

            // Return button
            Button returnButton = new Button(this.ParentWindow);

            returnButton.Text = "Return...";
            returnButton.Width = 200;
            returnButton.Position = new Vector2(0, 500);
            returnButton.Anchor = AnchorType.TopCentre;
            returnButton.DialogKey = this.DialogKey;
            returnButton.OnMouseDown += new SenderEventHandler(Button_OnMouseDown);
            returnButton.OnMouseEnter += new SenderEventHandler(Button_OnMouseEnter);
            returnButton.OnMouseLeave += new SenderEventHandler(Button_OnMouseLeave);
            returnButton.OnMouseUp += new SenderEventHandler(returnButton_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("ReturnVSMenuButton", returnButton);

            SetupSubscribers();
        }


        /// <summary>
        /// Implementation of the base subscriber setup method.
        /// </summary>
        public override void SetupSubscribers()
        {
            this.MouseSubscribers = new ControlSkeleton[] {
                ParentWindow.GameInterface.Controls["ReturnVSMenuButton"],
                ParentWindow.GameInterface.Controls["ResolutionLabel"],
                ParentWindow.GameInterface.Controls["ResolutionButton"],
                ParentWindow.GameInterface.Controls["MinimumSizeLabel"],
                ParentWindow.GameInterface.Controls["MinimumSizeButton"]
            };
        }


        /// <summary>
        /// Implementation of the base control system closing method.
        /// </summary>
        public override void Close()
        {
            ParentWindow.GameInterface.KillFromDialogKey(this.DialogKey);
            ParentWindow.GameInterface.ControlSystems.Remove("VideoSettingsMenu");

            // Change title of menu
            ((Label)ParentWindow.GameInterface.Controls["ScreenTitle"]).Text = "Settings (unfinished)";

            // Show the settings menu controls
            ParentWindow.GameInterface.Controls["VideoSettingsButton"].Visible = true;
            ParentWindow.GameInterface.Controls["ReturnSTMenuButton"].Visible = true;
        }


        /// <summary>
        /// Implementation of the base control position updater method.
        /// </summary>
        public override void UpdateControlPositions()
        {
            ParentWindow.GameInterface.Controls["ReturnVSMenuButton"].UpdatePosition();
            ParentWindow.GameInterface.Controls["ResolutionLabel"].UpdatePosition();
            ParentWindow.GameInterface.Controls["ResolutionButton"].UpdatePosition();
            ParentWindow.GameInterface.Controls["MinimumSizeLabel"].UpdatePosition();
            ParentWindow.GameInterface.Controls["MinimumSizeButton"].UpdatePosition();
        }


        /// <summary>
        /// [Event] Minimum size is preferred option clicked.
        /// </summary>
        /// <param name="sender"></param>
        void minimumSizeButton_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;
        }


        /// <summary>
        /// [Event] Resolution option clicked.
        /// </summary>
        void resolutionButton_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;
        }


        /// <summary>
        /// [Event] "Return..." button clicked.
        /// </summary>
        void returnButton_OnMouseUp(object sender)
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
        /// [Event] Generic button mouse down.
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
    }
}
