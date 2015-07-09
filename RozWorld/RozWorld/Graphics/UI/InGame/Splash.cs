//
// RozWorld.Graphics.UI.InGame.Splash -- RozWorld Splash Screen Control System
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

using RozWorld.Graphics.UI;
using RozWorld.Graphics.UI.Control;

using System.Timers;

using OpenGL;

namespace RozWorld.Graphics.UI.InGame
{
    public class Splash : ControlSystem
    {
        Timer SplashTimer;

        public Splash(GameWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
            this.DialogKey = 1;
        }


        /// <summary>
        /// Implementation of the base control system starting method.
        /// </summary>
        public override void Start()
        {
            // Version string in upper left corner
            if (RozWorld.SHOW_VERSION_STRING)
            {
                // Version string
                Label versionString = new Label(this.ParentWindow);

                versionString.Text = RozWorld.VERSION_STRING;
                versionString.Position = new Vector2(10, 10);
                versionString.ForeColour = VectorColour.OpaqueGrey;
                versionString.ZIndex = 9000;

                ParentWindow.GameInterface.Controls.Add("VersionString", versionString);

                // FPS counter
                Label fpsCounter = new Label(this.ParentWindow);

                fpsCounter.Text = "FPS: 0";
                fpsCounter.Position = new Vector2(10, 26);
                fpsCounter.ForeColour = VectorColour.OpaqueGrey;
                fpsCounter.ZIndex = 9000;

                ParentWindow.GameInterface.Controls.Add("FPSCounter", fpsCounter);
            }
            
            // Splash screen image
            Image splashScreen = new Image(this.ParentWindow);

            splashScreen.Dimensions = new System.Drawing.Size(640, 480);
            splashScreen.DialogKey = this.DialogKey;
            splashScreen.TextureName = "OddmaticsSplash";
            splashScreen.ZIndex = 1;

            ParentWindow.GameInterface.Controls.Add("SplashScreen", splashScreen);

            // Timer for the splash screen
            SplashTimer = new Timer(2000);  // Time until the splash disappears

            SplashTimer.Elapsed += new ElapsedEventHandler(SplashTimer_Elapsed);
            SplashTimer.Start();

            SetupSubscribers();
        }


        /// <summary>
        /// [Event] Splash Screen transition timer elapsed.
        /// </summary>
        void SplashTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Start the main menu
            ParentWindow.GameInterface.ControlSystems.Add("MainMenu", new MainMenu(this.ParentWindow));
            ParentWindow.GameInterface.ControlSystems["MainMenu"].Start();

            // Kill the splash screen
            this.Close();
        }


        /// <summary>
        /// Implementation of the base control system closing method.
        /// </summary>
        public override void Close()
        {
            ParentWindow.GameInterface.KillFromDialogKey(this.DialogKey);

            SplashTimer.Stop();
            SplashTimer.Dispose();

            ParentWindow.GameInterface.ControlSystems.Remove("Splash");
        }
    }
}
