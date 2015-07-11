﻿/**
 * RozWorld.Graphics.GameWindow -- RozWorld Game Window
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

using OpenGL;
using Tao.FreeGlut;

using RozWorld.Graphics.UI;
using RozWorld.Graphics.UI.InGame;

namespace RozWorld.Graphics
{
    public class GameWindow
    {
        public const string WINDOW_TITLE = "RozWorld ' OR '1'='1";

        // Resolution relevant stuff //
        public int[] WindowScale
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
         * FPS relevant stuff.
         */
        private Stopwatch Timer;

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
            WindowScale = new int[] { 640, 480 };

            Glut.glutInit();

            // Create GL window...
            Glut.glutInitWindowSize(WindowScale[0], WindowScale[1]);
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

            if (!TextureManagement.LoadTextures())
            {
                RozWorld.GameStatus = Status.FatalError;
            }
            else
            {
                RozWorld.GameStatus = Status.Splash;
            }

            Timer = Stopwatch.StartNew();
            LowestFPS = double.MaxValue;

            GameInterface.ControlSystems.Add("Splash", new Splash(this));

            GameInterface.ControlSystems["Splash"].Start();

            //FloatPoint[] fp = DrawInstruction.CreateBlitCoordsForFont(FontType.SmallText, 'r'); test that the code works.
            
            Glut.glutMainLoop();
        }


        /// <summary>
        /// Routine called when it is time to redraw the GL window and game screen.
        /// </summary>
        private void Draw()
        {
            // FPS check system:
            Timer.Stop();

            FPS = Math.Round(1000 / (double)Timer.ElapsedMilliseconds);

            if (FPS > HighestFPS && FPS < 90) // Ignore extreme values
            {
                HighestFPS = FPS;
            }
            else if (FPS < LowestFPS && FPS > 20)
            {
                LowestFPS = FPS;
            }

            Timer.Restart();

            try
            {
                ((UI.Control.Label)GameInterface.Controls["FPSCounter"]).Text = "FPS: " + FPS.ToString() + "; Highest: " + HighestFPS.ToString() + "; Lowest: " + LowestFPS;
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

            Gl.Viewport(0, 0, WindowScale[0], WindowScale[1]);
            Gl.Clear(ClearBufferMask.ColorBufferBit);
            Gl.UseProgram(GLProgram);

            foreach (StringIntPair key in GameInterface.ControlZMap)
            {
                try
                {
                    foreach (DrawInstruction instruction in GameInterface.Controls[key.StringValue].DrawInstructions)
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
            WindowScale[0] = width;
            WindowScale[1] = height;

            // Make sure the screen updates, so you don't get the solitaire effect
            OnDisplay();
            Draw();

            GLProgram.Use();
        }

        private void OnDisplay()
        {
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
            catch { } // Most likely transitioning from control systems
        }


        /// <summary>
        /// Routine called when the mouse moves.
        /// </summary>
        private void OnMouseMove(int x, int y)
        {
            MouseX = x;
            MouseY = y;

            LastSystemAmount = GameInterface.ControlSystems.Count;

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
            catch { } // Most likely transitioning from control systems
        }
    }
}
