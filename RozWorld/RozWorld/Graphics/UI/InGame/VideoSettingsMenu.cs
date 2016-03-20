/**
 * RozWorld.Graphics.UI.InGame.VideoSettingsMenu -- RozWorld Video Settings Menu Control System
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using OpenGL;

using RozWorld.Graphics.UI.Control;
using RozWorld.Graphics.UI.InGame.Generic;

using System.Drawing;


namespace RozWorld.Graphics.UI.InGame
{
    public class VideoSettingsMenu : ControlSystem
    {
        private Size WindowResolution;
        private Size[] StandardResolutions = new Size[]{
            new Size(800, 600),
            new Size(1024, 768),
            new Size(1152, 864),
            new Size(1280, 720),
            new Size(1280, 960),
            new Size(1366, 768),
            new Size(1440, 1080),
            new Size(1600, 900),
            new Size(1600, 1200),
            new Size(1920, 1080),
        };
        private int SelectedResolution = 0;

        private bool AeroOffsets;
        private bool MinimumSizeIsPreferred;

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

            // Load current settings
            WindowResolution = RozWorld.Settings.WindowResolution;
            AeroOffsets = RozWorld.Settings.AeroOffsets;
            MinimumSizeIsPreferred = RozWorld.Settings.MinimumSizeIsPreferred;

            // Set up the resolution button
            FindClosestResolution();

            // Window resolution label
            Label resolutionLabel = new Label(this.ParentWindow);

            resolutionLabel.Text = "Preferred resolution:";
            resolutionLabel.Position = new Vector2(-315, 188);
            resolutionLabel.Anchor = AnchorType.TopCentre;
            resolutionLabel.ForeColour = VectorColour.OpaqueWhite;
            resolutionLabel.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("ResolutionLabel", resolutionLabel);

            // Window resolution button
            Button resolutionButton = new Button(this.ParentWindow);

            resolutionButton.Text = WindowResolution.Width.ToString() + "x" + WindowResolution.Height.ToString();
            resolutionButton.Width = 100;
            resolutionButton.Position = new Vector2(-50, 180);
            resolutionButton.Anchor = AnchorType.TopCentre;
            resolutionButton.DialogKey = this.DialogKey;
            //resolutionButton.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            //resolutionButton.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            //resolutionButton.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            //resolutionButton.OnMouseUp += new SenderEventHandler(resolutionButton_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("ResolutionButton", resolutionButton);

            // Minimum size is preferred label
            Label minimumSizeLabel = new Label(this.ParentWindow);

            minimumSizeLabel.Text = "Preferred res is minimum:";
            minimumSizeLabel.Position = new Vector2(-355, 228);
            minimumSizeLabel.Anchor = AnchorType.TopCentre;
            minimumSizeLabel.ForeColour = VectorColour.OpaqueWhite;
            minimumSizeLabel.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("MinimumSizeLabel", minimumSizeLabel);

            // Minimum size is preferred button
            Button minimumSizeButton = new Button(this.ParentWindow);

            minimumSizeButton.Text = MinimumSizeIsPreferred.ToString().ToUpper();
            minimumSizeButton.Width = 100;
            minimumSizeButton.Position = new Vector2(-50, 220);
            minimumSizeButton.Anchor = AnchorType.TopCentre;
            minimumSizeButton.DialogKey = this.DialogKey;
            //minimumSizeButton.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            //minimumSizeButton.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            //minimumSizeButton.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            //minimumSizeButton.OnMouseUp += new SenderEventHandler(minimumSizeButton_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("MinimumSizeButton", minimumSizeButton);

            // Aero offsets label
            Label aeroOffsetsLabel = new Label(this.ParentWindow);

            aeroOffsetsLabel.Text = "Use aero window offsets:";
            aeroOffsetsLabel.Position = new Vector2(-345, 268);
            aeroOffsetsLabel.Anchor = AnchorType.TopCentre;
            aeroOffsetsLabel.ForeColour = VectorColour.OpaqueWhite;
            aeroOffsetsLabel.DialogKey = this.DialogKey;

            ParentWindow.GameInterface.Controls.Add("AeroOffsetsLabel", aeroOffsetsLabel);

            // Aero offsets button
            Button aeroOffsetsButton = new Button(this.ParentWindow);

            aeroOffsetsButton.Text = AeroOffsets.ToString().ToUpper();
            aeroOffsetsButton.Width = 100;
            aeroOffsetsButton.Position = new Vector2(-50, 260);
            aeroOffsetsButton.Anchor = AnchorType.TopCentre;
            aeroOffsetsButton.DialogKey = this.DialogKey;
            //aeroOffsetsButton.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            //aeroOffsetsButton.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            //aeroOffsetsButton.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            //aeroOffsetsButton.OnMouseUp += new SenderEventHandler(aeroOffsetsButton_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("AeroOffsetsButton", aeroOffsetsButton);

            // Texture pack screen button
            Button texturePackButton = new Button(this.ParentWindow);

            texturePackButton.Text = "Texture Pack...";
            texturePackButton.Width = 200;
            texturePackButton.Position = new Vector2(150, 180);
            texturePackButton.Anchor = AnchorType.TopCentre;
            texturePackButton.DialogKey = this.DialogKey;
            //texturePackButton.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            //texturePackButton.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            //texturePackButton.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            //texturePackButton.OnMouseUp += new SenderEventHandler(texturePackButton_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("TexturePackButton", texturePackButton);

            // Return button
            Button returnButton = new Button(this.ParentWindow);

            returnButton.Text = "Return...";
            returnButton.Width = 200;
            returnButton.Position = new Vector2(0, 500);
            returnButton.Anchor = AnchorType.TopCentre;
            returnButton.DialogKey = this.DialogKey;
            //returnButton.OnMouseDown += new SenderEventHandler(ButtonEvent.OnMouseDown);
            //returnButton.OnMouseEnter += new SenderEventHandler(ButtonEvent.OnMouseEnter);
            //returnButton.OnMouseLeave += new SenderEventHandler(ButtonEvent.OnMouseLeave);
            //returnButton.OnMouseUp += new SenderEventHandler(returnButton_OnMouseUp);

            ParentWindow.GameInterface.Controls.Add("ReturnVSMenuButton", returnButton);
        }


        /// <summary>
        /// Implementation of the base control system closing method.
        /// </summary>
        public override void Close()
        {
            // Save current settings
            RozWorld.Settings.UpdateVideoSettings(WindowResolution, AeroOffsets, MinimumSizeIsPreferred);

            // Kill this screen
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
            ParentWindow.GameInterface.Controls["AeroOffsetsLabel"].UpdatePosition();
            ParentWindow.GameInterface.Controls["AeroOffsetsButton"].UpdatePosition();
            ParentWindow.GameInterface.Controls["TexturePackButton"].UpdatePosition();
        }


        /// <summary>
        /// Finds the closest standard resolution to the currently active one so the button doesn't always reset to 800x600.
        /// </summary>
        private void FindClosestResolution()
        {
            bool matched = false;
            int i = 0;

            do
            {
                if (StandardResolutions[i].Width >= WindowResolution.Width)
                {
                    matched = true;
                }
                else
                {
                    i++;
                }
            } while (i <= StandardResolutions.Length - 1 && !matched);

            if (i > StandardResolutions.Length - 1)
            {
                i = StandardResolutions.Length - 1;
            }

            SelectedResolution = i;
        }


        /// <summary>
        /// "Texture Pack..." button clicked.
        /// </summary>
        void texturePackButton_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;
        }


        /// <summary>
        /// Use aero offsets option clicked.
        /// </summary>
        void aeroOffsetsButton_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;

            if (AeroOffsets)
            {
                AeroOffsets = false;
            }
            else
            {
                AeroOffsets = true;
            }

            ((Button)sender).Text = AeroOffsets.ToString().ToUpper();
        }


        /// <summary>
        /// [Event] Minimum size is preferred option clicked.
        /// </summary>
        /// <param name="sender"></param>
        void minimumSizeButton_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;

            if (MinimumSizeIsPreferred)
            {
                MinimumSizeIsPreferred = false;
            }
            else
            {
                MinimumSizeIsPreferred = true;
            }

            ((Button)sender).Text = MinimumSizeIsPreferred.ToString().ToUpper();
        }


        /// <summary>
        /// [Event] Resolution option clicked.
        /// </summary>
        void resolutionButton_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;

            if (SelectedResolution++ == StandardResolutions.Length - 1)
            {
                SelectedResolution = 0;
            }

            WindowResolution = StandardResolutions[SelectedResolution];

            ((Button)sender).Text = WindowResolution.Width + "x" + WindowResolution.Height;
        }


        /// <summary>
        /// [Event] "Return..." button clicked.
        /// </summary>
        void returnButton_OnMouseUp(object sender)
        {
            ((Button)sender).TintColour = VectorColour.ButtonHoverTint;
            this.Close();
        }
    }
}
