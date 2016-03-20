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

using Pencil.Gaming;

using OpenGL;

using RozWorld.Graphics.UI;
using RozWorld.Graphics.UI.InGame;
using RozWorld.IO;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Timers;


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
        public UI.MouseState LastMouseStates
        {
            get;
            private set;
        }

        public UI.MouseState CurrentMouseStates
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
        public UI.KeyboardState LastKeyStates
        {
            get;
            private set;
        }

        public UI.KeyboardState CurrentKeyStates
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


        /**
         * Pointer for the GLFW window stuff
         */
        public GlfwWindowPtr GlfwPtr { get; private set; }


        public GameWindow()
        {
            // Set up settings
            WindowScale = RozWorld.Settings.WindowResolution;
            Files.TexturePackSubFolder = RozWorld.Settings.TexturePackDirectory;

            // Set up mouse input delay timer...
            MouseInputDelay = new Timer(1);
            MouseInputDelay.Elapsed += new ElapsedEventHandler(MouseInputDelay_Elapsed);
            CheckMouseInput = true;

            Glfw.SetErrorCallback(OnError);

            // Create GLFW window...
            Glfw.WindowHint(WindowHint.ContextVersionMajor, 3);
            Glfw.WindowHint(WindowHint.ContextVersionMinor, 2);
            Glfw.WindowHint(WindowHint.OpenGLForwardCompat, 1);
            Glfw.WindowHint(WindowHint.OpenGLProfile, (int)OpenGLProfile.Core);



            GlfwPtr = Glfw.CreateWindow(WindowScale.Width, WindowScale.Height,
                WINDOW_TITLE, GlfwMonitorPtr.Null, GlfwWindowPtr.Null);

            Glfw.MakeContextCurrent(GlfwPtr);

            // Set up GLFW functions...
            Glfw.SetWindowCloseCallback(GlfwPtr, OnClose);
            Glfw.SetKeyCallback(GlfwPtr, OnKey);
            Glfw.SetMouseButtonCallback(GlfwPtr, OnMouseChanged);
            Glfw.SetCursorPosCallback(GlfwPtr, OnMouseMove);
            Glfw.SetWindowRefreshCallback(GlfwPtr, OnDisplay);
            Glfw.SetWindowSizeCallback(GlfwPtr, OnReshape);

            // Set up (shadow) blending...
            Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            Gl.Enable(EnableCap.Blend);

            // Starting the GL window...
            try
            {
                GLProgram = new ShaderProgram(Shaders.VertexShader, Shaders.FragmentShader);
            }
            catch (EntryPointNotFoundException ex)
            {
                UIHandler.CriticalError(Error.SHADERS_UNSUPPORTED);
            }

            GLProgram.Use();

            uint vao = Gl.GenVertexArray();
            Gl.BindVertexArray(vao);

            // Load texture content...
            GameInterface = new UIHandler();
            RozWorld.LoadResources();

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


            // THIS IS TEST CODE ONLY!
            List<DrawInstruction> test;
            //FontProvider.BuildString(FontType.HugeFont, "RIG sucks", out test, StringFormatting.Both);
            // // // // // // // // //

            



            while (!Glfw.WindowShouldClose(GlfwPtr))
            {
                Glfw.PollEvents();

                Draw();

                Glfw.SwapBuffers(GlfwPtr);
            }


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
                        
                        Gl.ActiveTexture(TextureUnit.Texture0);
                        Gl.BindTexture(instruction.TextureReference);
                        Gl.Uniform1f(Gl.GetUniformLocation(GLProgram.ProgramID, "texture"), 0);

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
        private void OnClose(GlfwWindowPtr wnd)
        {
            GLProgram.DisposeChildren = true;
            GLProgram.Dispose();
        }


        /// <summary>
        /// Routine called when an error occurs in GLFW.
        /// </summary>
        private void OnError(GlfwError code, string desc)
        {
            string t = "";
        }


        /// <summary>
        /// Routine called when the GL window is resized.
        /// </summary>
        /// <param name="width">New width of the window.</param>
        /// <param name="height">New height of the window.</param>
        private void OnReshape(GlfwWindowPtr wnd, int width, int height)
        {
            WindowScale = new Size(width, height);

            // Make sure the screen updates, so you don't get the solitaire effect
            OnDisplay(GlfwPtr);
            Draw();

            GLProgram.Use();
        }


        /// <summary>
        /// Routine called when the GL window is redisplayed.
        /// </summary>
        private void OnDisplay(GlfwWindowPtr wnd)
        {
            // Make sure the screen stays at least at the minimal resolution
            if (RozWorld.Settings.MinimumSizeIsPreferred)
            {
                if (WindowScale.Width < RozWorld.Settings.WindowResolution.Width ||
                    WindowScale.Height < RozWorld.Settings.WindowResolution.Height)
                {
                    Glfw.SetWindowSize(GlfwPtr, RozWorld.Settings.WindowResolution.Width,
                        RozWorld.Settings.WindowResolution.Height);
                }
            }
            else
            {
                if (WindowScale.Width < 800 || WindowScale.Height < 600)
                {
                    Glfw.SetWindowSize(GlfwPtr, 800, 600);
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
        /// Routine called when a key's state changes.
        /// </summary>
        private void OnKey(GlfwWindowPtr wnd, Key key, int scanCode, KeyAction action, KeyModifiers mods)
        {
            // TODO: Code this
        }


        /// <summary>
        /// Routine called when a mouse button changes state.
        /// </summary>
        private void OnMouseChanged(GlfwWindowPtr wnd, MouseButton btn, KeyAction action)
        {
            // TODO: Code this
        }


        /// <summary>
        /// Routine called when the mouse moves.
        /// </summary>
        private void OnMouseMove(GlfwWindowPtr wnd, double x, double y)
        {
            // TODO: Code this
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
