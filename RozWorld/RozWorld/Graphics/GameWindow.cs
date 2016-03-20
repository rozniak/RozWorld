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
using RozWorld.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;


namespace RozWorld.Graphics
{
    /// <summary>
    /// Represents the game window of RozWorld.
    /// </summary>
    internal class GameWindow
    {
        /// <summary>
        /// The window title of GameWindow instances.
        /// </summary>
        public const string WINDOW_TITLE = "RozWorld";


        /// <summary>
        /// Gets the size this GameWindow.
        /// </summary>
        public Size WindowScale { get; private set; }

        /// <summary>
        /// Gets whether this GameWindow is currently in focus.
        /// </summary>
        public bool HasFocus { get; private set; }

        /// <summary>
        /// Gets the GLFW pointer for this GameWindow.
        /// </summary>
        public GlfwWindowPtr GlfwPtr { get; private set; }

        /// <summary>
        /// Gets the Win32 window handle for this GameWindow.
        /// </summary>
        public IntPtr HwndPtr { get; private set; }

        /// <summary>
        /// Gets the user interface management subsystem for this GameWindow.
        /// </summary>
        public UIHandler GameInterface { get; private set; }

        /// <summary>
        /// Gets the keyboard input subsystem for this GameWindow.
        /// </summary>
        public Keyboard Keyboard { get; private set; }

        /// <summary>
        /// Gets the mouse input subsystem for this GameWindow.
        /// </summary>
        public Mouse Mouse { get; private set; }


        /// <summary>
        /// The ShaderProgram used to render the game.
        /// </summary>
        private ShaderProgram GLProgram;

        /// <summary>
        /// The amount of controls in the last draw, when this changes between draws, SortControlZIndexes() should be called.
        /// </summary>
        private int LastControlAmount;


        public GameWindow()
        {
            // Set up settings
            WindowScale = RozWorld.Settings.WindowResolution;
            Files.TexturePackSubFolder = RozWorld.Settings.TexturePackDirectory;

            Glfw.SetErrorCallback(OnError);

            // Create GLFW window...
            Glfw.WindowHint(WindowHint.ContextVersionMajor, 3);
            Glfw.WindowHint(WindowHint.ContextVersionMinor, 2);
            Glfw.WindowHint(WindowHint.OpenGLForwardCompat, 1);
            Glfw.WindowHint(WindowHint.OpenGLProfile, (int)OpenGLProfile.Core);


            GlfwPtr = Glfw.CreateWindow(WindowScale.Width, WindowScale.Height,
                WINDOW_TITLE, GlfwMonitorPtr.Null, GlfwWindowPtr.Null);

            // Allocate the GL context
            Glfw.MakeContextCurrent(GlfwPtr);

            // Retrieve the Win32 window handle for this window
            HwndPtr = Process.GetCurrentProcess().MainWindowHandle;

            Windows.SetRozWorldIcon(); // Set the icon

            // Initialise input control subsystems
            Keyboard = new Keyboard(this);
            Mouse = new Mouse();

            // Set up GLFW functions...
            Glfw.SetWindowCloseCallback(GlfwPtr, OnClose);
            Glfw.SetMouseButtonCallback(GlfwPtr, OnMouseChanged);
            Glfw.SetCursorPosCallback(GlfwPtr, OnMouseMove);
            Glfw.SetWindowRefreshCallback(GlfwPtr, OnDisplay);
            Glfw.SetWindowSizeCallback(GlfwPtr, OnReshape);
            Glfw.SetScrollCallback(GlfwPtr, OnScroll);

            // Set up (shadow) blending...
            Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            Gl.Enable(EnableCap.Blend);

            // Starting the GL window...
            try
            {
                GLProgram = new ShaderProgram(Shaders.VertexShader, Shaders.FragmentShader);
            }
            catch (EntryPointNotFoundException ex) // Either shaders unsupported or no GL context set
            {
                UIHandler.CriticalError(Error.SHADERS_UNSUPPORTED);
            }

            GLProgram.Use();

            uint vao = Gl.GenVertexArray();
            Gl.BindVertexArray(vao);

            // Load texture content...
            GameInterface = new UIHandler();
            RozWorld.LoadResources();

            GameInterface.ControlSystems.Add("Splash", new Splash(this));
            GameInterface.ControlSystems["Splash"].Start();

            // Set the clear colour
            Gl.ClearColor(VectorColour.OpaqueWhite.x,
                VectorColour.OpaqueWhite.y,
                VectorColour.OpaqueWhite.z,
                VectorColour.OpaqueWhite.w);


            // THIS IS TEST CODE ONLY!
            List<DrawInstruction> test;
            //FontProvider.BuildString(FontType.HugeFont, "RIG sucks", out test, StringFormatting.Both);
            // // // // // // // // //

            Glfw.SetTime(0); // Initial delta-time of 0 (or close to it)

            while (!Glfw.WindowShouldClose(GlfwPtr))
            {
                double deltaTime = Glfw.GetTime();
                Glfw.SetTime(0);

                Glfw.PollEvents();

                Update(deltaTime);
                Draw(deltaTime);

                Glfw.SwapBuffers(GlfwPtr);
            }
        }


        /// <summary>
        /// Routine called to perform the game updates (input etc.)
        /// </summary>
        /// <param name="deltaTime">The time difference since the last game loop completed.</param>
        private void Update(double deltaTime)
        {
            // TODO: Call systems updates here
        }


        /// <summary>
        /// Routine called when it is time to redraw the GL window and game screen.
        /// </summary>
        /// <param name="deltaTime">The time difference since the last game loop completed.</param>
        private void Draw(double deltaTime)
        {
            // Debugging purposes
            int instructionsDrawn = 0;

            // Check if amount of controls has changed, if so, call z-index sorting
            if (GameInterface.Controls.Count != LastControlAmount)
            {
                LastControlAmount = GameInterface.Controls.Count;
                GameInterface.SortControlZIndexes();
            }

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

                        if (!Glfw.WindowShouldClose(GlfwPtr))
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
            switch (code)
            {
                case GlfwError.VersionUnavailable:
                    MessageBox.Show("Required OpenGL version (3.2) is unsupported by your graphics card, updating your video adapter drivers may help. If you're on Intel integrated, you're most likely out of luck entirely, sorry!",
                        "RozWorld: GLFW Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                default:
                    MessageBox.Show("RozWorld encountered an error with GLFW (OpenGL context/window system), the error encountered was: " + code.ToString() + ".",
                        "RozWorld: GLFW Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            Environment.Exit(1);
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
            Draw(0);

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

            // UpdateControlPositions() here
            foreach (ControlSystem system in GameInterface.ControlSystems.Values)
            {
                system.UpdateControlPositions();
            }
        }


        /// <summary>
        /// Routine called when a mouse button changes state.
        /// </summary>
        private void OnMouseChanged(GlfwWindowPtr wnd, MouseButton btn, KeyAction action)
        {
            var mouseButton = Mouse.EnumerateMouseButtons(btn, true);

            if (action == KeyAction.Press)
                mouseButton.Pressed = true;
            else
                mouseButton.Pressed = false;
        }


        /// <summary>
        /// Routine called when the mouse moves.
        /// </summary>
        private void OnMouseMove(GlfwWindowPtr wnd, double x, double y)
        {
            Mouse.ActiveMouseStates.Position = new Vector2(x, y);
        }


        /// <summary>
        /// Routine called when scrolling with the mouse.
        /// </summary>
        private void OnScroll(GlfwWindowPtr wnd, double xoffset, double yoffset)
        {
            Mouse.ActiveMouseStates.ScrollAmount = yoffset;
        }
    }
}
