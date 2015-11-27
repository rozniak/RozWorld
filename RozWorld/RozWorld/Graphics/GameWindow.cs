/**
 * RozWorld.Graphics.GameWindow -- RozWorld Game Window
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
using RozWorld.Graphics.UI.InGame;
using RozWorld.IO;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Timers;

using Tao.FreeGlut;


namespace RozWorld.Graphics
{
    public class GameWindow
    {
        public const string WINDOW_TITLE = "RozWorld";

        /**
         * Resolution relevant stuff.
         */
        public Size WindowScale
        {
            get;
            private set;
        }

        /**
         * For setting the client window bounds, for example when the Windows Aero theme is active as
         * the point (0, 0) is no longer the top left pixel of the game screen, it is outside the
         * scene. This offset is to tell how far to shift the display and mouse detection.
         */
        public Vector2 WindowOffset
        {
            get;
            private set;
        }

        /**
         * Tell if the window has focus, this is for cooling down the rendering stuff when you're not
         * looking at the game.
         */
        private bool HasFocus;

        /**
         * GL relevant stuff.
         */
        private ShaderProgram GLProgram;

        public TextureManager TextureManagement
        {
            get;
            private set;
        }

        /**
         * RozWorld engine's GUI handler.
         */
        public UIHandler GameInterface
        {
            get;
            private set;
        }

        /**
         * Mouse relevant stuff.
         */
        public MouseState LastMouseStates
        {
            get;
            private set;
        }

        public MouseState CurrentMouseStates
        {
            get;
            private set;
        }

        public int MouseX
        {
            get;
            private set;
        }

        public int MouseY
        {
            get;
            private set;
        }

        private Timer MouseInputDelay;

        private bool CheckMouseInput;

        /**
         * Keyboard relevant stuff.
         */
        public KeyboardState LastKeyStates
        {
            get;
            private set;
        }

        public KeyboardState CurrentKeyStates
        {
            get;
            private set;
        }

        /**
         * The game input and general update ticker.
         */
        private Timer GameTime;

        /**
         * FPS relevant stuff.
         */
        private Stopwatch FPSTimer;

        public double FPS
        {
            get;
            private set;
        }

        public double LowestFPS
        {
            get;
            private set;
        }

        public double HighestFPS
        {
            get;
            private set;
        }

        /**
         * Control ZIndex watching (when this changes between draws, SortControlZIndexes() must be called)
         */
        private int LastControlAmount;

        /**
         * ControlSystems watching (when this changes between mouse/keyboard triggers, skip the rest of the trigger calls)
         */
        private int LastSystemAmount;


        public GameWindow()
        {
            // Set up settings
            WindowScale = RozWorld.Settings.WindowResolution;
            Files.TexturePackSubFolder = RozWorld.Settings.TexturePackDirectory;

            // Set up mouse input delay timer...
            MouseInputDelay = new Timer(1);
            MouseInputDelay.Elapsed += new ElapsedEventHandler(MouseInputDelay_Elapsed);
            CheckMouseInput = true;
            
            // Initialise GL stuff...
            Glut.glutInit();

            // Create GL window...
            Glut.glutInitWindowSize(WindowScale.Width, WindowScale.Height);
            Glut.glutCreateWindow(WINDOW_TITLE);

            // Set up GL functions...
            Glut.glutIdleFunc(Draw);
            Glut.glutCloseFunc(OnClose);
            Glut.glutKeyboardFunc(OnKeyDown);
            Glut.glutKeyboardUpFunc(OnKeyUp);
            Glut.glutMouseFunc(OnMouseChanged);
            Glut.glutPassiveMotionFunc(OnMouseMove);
            Glut.glutMotionFunc(OnMouseMove);
            Glut.glutDisplayFunc(OnDisplay);
            Glut.glutReshapeFunc(OnReshape);

            // Set up (shadow) blending...
            Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            Gl.Enable(EnableCap.Blend);

            // Starting the GL window...
            GLProgram = new ShaderProgram(Shaders.VertexShader, Shaders.FragmentShader);
            GLProgram.Use();

            // Load texture content...
            TextureManagement = new TextureManager();
            GameInterface = new UIHandler();

            TextureManagement.LoadFontSources();
            TextureManagement.LoadTextures();
            GameInterface.Geometry.Load();
            GameInterface.Language.Load(RozWorld.Settings.LanguageSource);
            FontProvider.Load(TextureManagement, GameInterface.Geometry);

            FPSTimer = Stopwatch.StartNew();
            LowestFPS = double.MaxValue;

            GameInterface.ControlSystems.Add("Splash", new Splash(this));

            GameInterface.ControlSystems["Splash"].Start();

            // Start the main game update timer
            GameTime = new Timer(16); // About 60Hz
            GameTime.Elapsed += new ElapsedEventHandler(Update);
            GameTime.Enabled = true;
            GameTime.Start();

            Gl.ClearColor(VectorColour.OpaqueWhite.x,
                VectorColour.OpaqueWhite.y,
                VectorColour.OpaqueWhite.z,
                VectorColour.OpaqueWhite.w);
            
            Glut.glutMainLoop();
        }


        /// <summary>
        /// Routine called to perform the game updates (input etc.)
        /// </summary>
        private void Update(object sender, ElapsedEventArgs e)
        {
            // Update the window focus status
            HasFocus = Windows.GameHasFocus();

            // Set window icon if it's gone for some reason
            if (!HasFocus)
                Windows.SetRozWorldIcon();
        }


        /// <summary>
        /// Routine called when it is time to redraw the GL window and game screen.
        /// </summary>
        private void Draw()
        {
            // Debugging purposes
            int instructionsDrawn = 0;

            // FPS check system:
            FPSTimer.Stop();
            FPS = Math.Round(1000 / (double)FPSTimer.ElapsedMilliseconds);
            FPSTimer.Restart();

            try
            {
                ((UI.Control.Label)GameInterface.Controls["FPSCounter"]).Text = "FPS: " + FPS.ToString();
            }
            catch { } // No FPS counter

            // Check if amount of controls has changed, if so, call z-index sorting
            if (GameInterface.Controls.Count != LastControlAmount)
            {
                LastControlAmount = GameInterface.Controls.Count;
                GameInterface.SortControlZIndexes();
            }

            // Update states (prevents ghosting of key/mouse)
            LastMouseStates = CurrentMouseStates;
            LastKeyStates = CurrentKeyStates;

            // Actual OpenGL drawing stuff starts here:
            Gl.Viewport(0, 0, WindowScale.Width, WindowScale.Height);
            Gl.Clear(ClearBufferMask.ColorBufferBit);
            Gl.UseProgram(GLProgram);

            foreach (Tuple<string, int> key in GameInterface.ControlZMap)
            {
                try
                {
                    foreach (DrawInstruction instruction in GameInterface.Controls[key.Item1].DrawInstructions)
                    {
                        VBO<Vector3> TextureDrawVectors = new VBO<Vector3>(new Vector3[] { new Vector3(instruction.DrawPoints[0].x, instruction.DrawPoints[0].y, 0), new Vector3(instruction.DrawPoints[1].x, instruction.DrawPoints[1].y, 0), new Vector3(instruction.DrawPoints[2].x, instruction.DrawPoints[2].y, 0), new Vector3(instruction.DrawPoints[3].x, instruction.DrawPoints[3].y, 0) });
                        VBO<int> TextureQuads = new VBO<int>(new int[] { 0, 1, 2, 3 }, BufferTarget.ElementArrayBuffer);

                        Gl.BindTexture(instruction.TextureReference);

                        VBO<Vector2> TextureBlitVectors = new VBO<Vector2>(new Vector2[] { new Vector2(instruction.BlitPoints[0].x, instruction.BlitPoints[0].y), new Vector2(instruction.BlitPoints[1].x, instruction.BlitPoints[1].y), new Vector2(instruction.BlitPoints[2].x, instruction.BlitPoints[2].y), new Vector2(instruction.BlitPoints[3].x, instruction.BlitPoints[3].y) });

                        Gl.Uniform4f(Gl.GetUniformLocation(GLProgram.ProgramID, "tint"), instruction.TintColour.x, instruction.TintColour.y, instruction.TintColour.z, instruction.TintColour.w);

                        Gl.BindBufferToShaderAttribute(TextureDrawVectors, GLProgram, "vertexPosition");
                        Gl.BindBufferToShaderAttribute(TextureBlitVectors, GLProgram, "vertexUV");

                        Gl.BindBuffer(TextureQuads);

                        Gl.DrawElements(BeginMode.TriangleFan, TextureQuads.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

                        TextureDrawVectors.Dispose();
                        TextureQuads.Dispose();
                        TextureBlitVectors.Dispose();

                        instructionsDrawn++;
                    }
                }
                catch (Exception ex)
                {
                    GameInterface.SortControlZIndexes(); // ControlZMap contains keys not present in the Controls dictionary.
                }
            }
            
            // Remember to flush!
            Gl.Flush();
        }


        /// <summary>
        /// Routine called when the GL window is closed.
        /// </summary>
        private void OnClose()
        {
            GLProgram.DisposeChildren = true;
            GLProgram.Dispose();
        }


        /// <summary>
        /// Routine called when the GL window is resized.
        /// </summary>
        /// <param name="width">New width of the window.</param>
        /// <param name="height">New height of the window.</param>
        private void OnReshape(int width, int height)
        {
            WindowScale = new Size(width, height);

            // Make sure the screen updates, so you don't get the solitaire effect
            OnDisplay();
            Draw();

            GLProgram.Use();
        }


        /// <summary>
        /// Routine called when the GL window is redisplayed.
        /// </summary>
        private void OnDisplay()
        {
            // Make sure the screen stays at least at the minimal resolution
            if (RozWorld.Settings.MinimumSizeIsPreferred)
            {
                if (WindowScale.Width < RozWorld.Settings.WindowResolution.Width ||
                    WindowScale.Height < RozWorld.Settings.WindowResolution.Height)
                {
                    Glut.glutReshapeWindow(RozWorld.Settings.WindowResolution.Width,
                        RozWorld.Settings.WindowResolution.Height);
                }
            }
            else
            {
                if (WindowScale.Width < 800 || WindowScale.Height < 600)
                {
                    Glut.glutReshapeWindow(800, 600);
                }
            }

            try
            {
                foreach (var item in GameInterface.ControlSystems)
                {
                    if (LastSystemAmount != GameInterface.ControlSystems.Count)
                    {
                        continue;
                    }

                    item.Value.UpdateControlPositions();
                }
            }
            catch (KeyNotFoundException ex)
            {
                UIHandler.CriticalError(Error.INVALID_GUI_DICTIONARY_KEY, "A reference to a non-existent key was made when updating control positions during OnDisplay() loop, check the code of any possibly active control systems' UpdateControlPositions() method.");
            }
            catch { } // Most likely transitioning from control systems

            // Update the version string location, if it is active
            if (RozWorld.SHOW_VERSION_STRING)
            {
                GameInterface.Controls["VersionString"].UpdatePosition();
            }
        }


        /// <summary>
        /// Routine called when a key is pressed.
        /// </summary>
        private void OnKeyDown(byte key, int x, int y)
        {
            LastKeyStates = CurrentKeyStates;
            CurrentKeyStates.KeyDown(key);

            LastSystemAmount = GameInterface.ControlSystems.Count;
            
            try
            {
                foreach (var item in GameInterface.ControlSystems)
                {
                    if (LastSystemAmount != GameInterface.ControlSystems.Count)
                    {
                        continue;
                    }

                    item.Value.TriggerKeyboard(true, key);
                }
            }
            catch { } // Most likely transitioning from control systems
        }


        /// <summary>
        /// Routine called when a key is lifted.
        /// </summary>
        private void OnKeyUp(byte key, int x, int y)
        {
            LastKeyStates = CurrentKeyStates;
            CurrentKeyStates.KeyUp(key);

            LastSystemAmount = GameInterface.ControlSystems.Count;

            try
            {
                foreach (var item in GameInterface.ControlSystems)
                {
                    if (LastSystemAmount != GameInterface.ControlSystems.Count)
                    {
                        continue;
                    }

                    item.Value.TriggerKeyboard(false, key);
                }
            }
            catch { } // Most likely transitioning from control systems
        }


        /// <summary>
        /// Routine called when a mouse button changes state.
        /// </summary>
        private void OnMouseChanged(int button, int state, int x, int y)
        {
            MouseState newMouseStates;
            newMouseStates.Left = button == Glut.GLUT_LEFT_BUTTON && state == Glut.GLUT_DOWN;
            newMouseStates.Middle = button == Glut.GLUT_MIDDLE_BUTTON && state == Glut.GLUT_DOWN;
            newMouseStates.Right = button == Glut.GLUT_RIGHT_BUTTON && state == Glut.GLUT_DOWN;

            LastMouseStates = CurrentMouseStates;
            CurrentMouseStates = newMouseStates;

            LastSystemAmount = GameInterface.ControlSystems.Count;

            // Make sure we actually want to activate any mouse events right now
            if (CheckMouseInput)
            {
                try
                {
                    foreach (var item in GameInterface.ControlSystems)
                    {
                        if (LastSystemAmount != GameInterface.ControlSystems.Count)
                        {
                            continue;
                        }

                        item.Value.TriggerMouse();
                    }
                }
                catch
                {
                    /**
                     * Most likely transitioning from control systems, start mouse
                     * delay to prevent it leaking to other controls.
                     */
                    DelayMouse();
                }
            }
        }


        /// <summary>
        /// Routine called when the mouse moves.
        /// </summary>
        private void OnMouseMove(int x, int y)
        {
            MouseX = x;
            MouseY = y;

            LastSystemAmount = GameInterface.ControlSystems.Count;

            if (CheckMouseInput)
            {
                try
                {
                    foreach (var item in GameInterface.ControlSystems)
                    {
                        if (LastSystemAmount != GameInterface.ControlSystems.Count)
                        {
                            continue;
                        }

                        item.Value.TriggerMouse();
                    }
                }
                catch
                {
                    /**
                     * Most likely transitioning from control systems, start mouse
                     * delay to prevent it leaking to other controls.
                     */
                    DelayMouse();
                }
            }
        }


        /// <summary>
        /// Delay the mouse input for 1ms to prevent ghosting.
        /// </summary>
        public void DelayMouse()
        {
            if (CheckMouseInput)
            {
                CheckMouseInput = false;
                MouseInputDelay.Start();
            }
        }


        /// <summary>
        /// [Event] Mouse input delay timer elapsed.
        /// </summary>
        void MouseInputDelay_Elapsed(object sender, ElapsedEventArgs e)
        {
            MouseInputDelay.Stop();
            CheckMouseInput = true;
        }
    }
}
