/* ----------------------------------------------------------------------------
    This is free and unencumbered software released into the public domain.

    Anyone is free to copy, modify, publish, use, compile, sell, or
    distribute this software, either in source code form or as a compiled
    binary, for any purpose, commercial or non-commercial, and by any
    means.

    In jurisdictions that recognize copyright laws, the author or authors
    of this software dedicate any and all copyright interest in the
    software to the public domain. We make this dedication for the benefit
    of the public at large and to the detriment of our heirs and
    successors. We intend this dedication to be an overt act of
    relinquishment in perpetuity of all present and future rights to this
    software under copyright law.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
    IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
    OTHER DEALINGS IN THE SOFTWARE.

    For more information, please refer to <https://unlicense.org>
---------------------------------------------------------------------------- */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SceneTransitionDemo.Scenes;
using SceneTransitionDemo.Transitions;
using System;

namespace SceneTransitionDemo
{
    public class GameBase : Game
    {
        // ------------------------------------------------
        //  Graphics
        // ------------------------------------------------
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        /// <summary>
        ///     Gets the color to use when clearing the backbuffer.
        /// </summary>
        public Color ClearColor { get; protected set; }

        /// <summary>
        ///     Gets or Sets the resolution (widht x height) of our game.
        /// </summary>
        public Point Resolution { get; private set; }

        // ------------------------------------------------
        //  Scene management
        // ------------------------------------------------
        private int _sceneIndex;                //  Scene to switch to
        private Transition _transitionOut;      //  Transition Out instnace.
        private Transition _transitionIn;       //  Transition In instance.
        private Transition _currentTransition;  //  Current transition being used.

        /// <summary>
        ///     Gets the current Scene.
        /// </summary>
        public Scene CurrentScene { get; protected set; }

        /// <summary>
        ///     Gets the next Scene to switch to.
        /// </summary>
        public Scene NextScene { get; protected set; }

        // ------------------------------------------------
        //  Assets
        // ------------------------------------------------
        private SpriteFont _font;   //  Font used to render text.
        private Texture2D _pixel;   //  1x1 white pixel texture.

        // ------------------------------------------------
        //  Input
        // ------------------------------------------------
        private KeyboardState _prevKeyboardState;   //  Previous frame keyboard state.
        private KeyboardState _curKeyboardState;    //  Current frame keyboard state.

        /// <summary>
        ///     Creates a new instance of our Game.
        /// </summary>
        public GameBase()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //  We need to subscribe to these two events so they can be handled down
            //  through to the scene.
            _graphics.DeviceCreated += GraphicsCreated;
            _graphics.DeviceReset += GraphicsReset;

            //  The width and height, in pixels, of our resolution.
            Resolution = new Point(1280, 704);

            //  Clear color to clear the backbuffer to.
            ClearColor = Color.Black;

            //  Start on scene index 0
            _sceneIndex = 0;
        }

        /// <summary>
        ///     Initializes our game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            //  Due to a current bug in MonoGame 3.8, the graphics resolution and applying
            //  the changes have to be set after the base.Initialize() here insetad of in
            //  the constructor like in 3.7
            //
            //  https://github.com/MonoGame/MonoGame/issues/7373
            //
            //  issue will be fixed in MonoGame 3.8.1 release
            _graphics.PreferredBackBufferWidth = Resolution.X;
            _graphics.PreferredBackBufferHeight = Resolution.Y;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
        }

        /// <summary>
        ///     Loads the content for our game.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //  Load the font we'll use for text.
            _font = Content.Load<SpriteFont>(@"font");

            //  Create a 1x1 pixel texture to use for rendering later.s
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[] { Color.White });

            //  Set our initiale scene
            CurrentScene = new GridScene(this, 32, 32);
        }

        /// <summary>
        ///     Udpates our game.
        /// </summary>
        /// <param name="gameTime">
        ///     Snapshot of timing values provided by the MonoGame frameowkr.
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            //  Update the keyboard states.
            _prevKeyboardState = _curKeyboardState;
            _curKeyboardState = Keyboard.GetState();

            //  If the user pressed space, swhcih scenes
            if (_curKeyboardState.IsKeyDown(Keys.Space) && _prevKeyboardState.IsKeyUp(Keys.Space))
            {
                _sceneIndex++;
                if (_sceneIndex > 2)
                {
                    _sceneIndex = 0;
                }

                if (_sceneIndex == 0)
                {
                    ChangeScene(new GridScene(this, 32, 32),
                                new EvenOddTransition(this, 32, TimeSpan.FromSeconds(1), TransitionKind.Out),
                                new EvenOddTransition(this, 32, TimeSpan.FromSeconds(1), TransitionKind.In));
                }
                else if (_sceneIndex == 1)
                {
                    ChangeScene(new VerticleStripScene(this, 32),
                                new EvenOddTransition(this, 32, TimeSpan.FromSeconds(1), TransitionKind.Out),
                                new EvenOddTransition(this, 32, TimeSpan.FromSeconds(1), TransitionKind.In));
                }
                else if (_sceneIndex == 2)
                {
                    ChangeScene(new HorizontalStripScene(this, 32),
                                new EvenOddTransition(this, 32, TimeSpan.FromSeconds(1), TransitionKind.Out),
                                new EvenOddTransition(this, 32, TimeSpan.FromSeconds(1), TransitionKind.In));
                }
            }

            //  Update the current scene.
            if (CurrentScene != null)
            {
                CurrentScene.Update(gameTime);
            }

            //  Update the current scene transition.
            if (_currentTransition != null && _currentTransition.IsTransitioning)
            {
                _currentTransition.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        ///     Renders our game to the screen.
        /// </summary>
        /// <param name="gameTime">
        ///     Snapshot of timing values provided by the MonoGame framework.
        /// </param>
        protected override void Draw(GameTime gameTime)
        {
            if (CurrentScene != null)
            {
                //  Render the current scene.
                CurrentScene.BeforeDraw(_spriteBatch);
                CurrentScene.Draw(_spriteBatch);
                CurrentScene.AfterDraw(_spriteBatch);

                //  If there is a current transition happening, render the transition.
                if (_currentTransition != null && _currentTransition.IsTransitioning)
                {
                    _currentTransition.Draw(_spriteBatch);
                }

                //  Prepare the graphics device for the final render.
                GraphicsDevice.Viewport = new Viewport(0, 0, Resolution.X, Resolution.Y);
                GraphicsDevice.Clear(ClearColor);

                _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);

                //  If we are transitioning, then we render the transition effect; otherwise, we'll render
                //  the current scene.
                if (_currentTransition != null && _currentTransition.IsTransitioning)
                {
                    _spriteBatch.Draw(texture: _currentTransition.RenderTarget,
                                      destinationRectangle: _currentTransition.RenderTarget.Bounds,
                                      sourceRectangle: _currentTransition.RenderTarget.Bounds,
                                      color: Color.White);
                }
                else if (CurrentScene != null)
                {
                    _spriteBatch.Draw(texture: CurrentScene.RenderTarget,
                                      destinationRectangle: CurrentScene.RenderTarget.Bounds,
                                      sourceRectangle: CurrentScene.RenderTarget.Bounds,
                                      color: Color.White);
                }

                //  Draws the "Press Space To Switch Scenes" textbox to the screen.
                DrawHelpText();
                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        ///     Renders a small textbox in the bottom left corner of the screne to inform
        ///     the user to press the space bar to switch the scene.
        /// </summary>
        private void DrawHelpText()
        {
            string text = "Press Space To Change Scenes";
            Vector2 size = _font.MeasureString(text);

            Rectangle bgRect = new Rectangle
            {
                X = 5,
                Y = Resolution.Y - 5 - (int)size.Y - 10,
                Width = (int)size.X + 10,
                Height = (int)size.Y + 10
            };

            _spriteBatch.Draw(_pixel, bgRect, Color.Black);

            bgRect.X++;
            bgRect.Y++;
            bgRect.Height -= 2;
            bgRect.Width -= 2;

            _spriteBatch.Draw(_pixel, bgRect, Color.White);

            Vector2 pos = new Vector2(10, Resolution.Y - size.Y - 5);

            _spriteBatch.DrawString(_font, text, pos, Color.Black);
        }

        /// <summary>
        ///     Called when the graphics device is created.  The current scene and 
        ///     and any transitions that are happening will need to recreate their
        ///     RenderTarget2D instances.
        /// </summary>
        protected virtual void GraphicsCreated(object sender, EventArgs e)
        {
            if (CurrentScene != null)
            {
                CurrentScene.GraphicsCreated();
            }

            if (_transitionOut != null)
            {
                _transitionOut.GraphicsCreated();
            }

            if (_transitionIn != null)
            {
                _transitionIn.GraphicsCreated();
            }
        }

        /// <summary>
        ///     Called the the graphics device is reset.  Whent his happens, all contents
        ///     of VRAM are cleard, so the current scene and any transitions that are 
        ///     happening will need to recreat their RenderTarget2D instances.
        /// </summary>
        protected virtual void GraphicsReset(object sender, EventArgs e)
        {
            if (CurrentScene != null)
            {
                CurrentScene.GraphicsReset();
            }

            if (_transitionOut != null)
            {
                _transitionOut.GraphicsReset();
            }

            if (_transitionIn != null)
            {
                _transitionIn.GraphicsReset();
            }
        }

        /// <summary>
        ///     Changes the current scene to the scene provided. No transition
        ///     is applied.
        /// </summary>
        /// <param name="to">
        ///     The Scene to change to.
        /// </param>
        public void ChangeScene(Scene to)
        {
            if (CurrentScene != null)
            {
                CurrentScene.Dispose();
            }

            GC.Collect();

            CurrentScene = to;
        }

        /// <summary>
        ///     Changes the current scene to the sceen provided using the transition
        ///     in and out instance provided.
        /// </summary>
        /// <param name="to">
        ///     The scene to change to.
        /// ></param>
        /// <param name="tOut">
        ///     The transition to use when transitioning out.
        /// </param>
        /// <param name="tIn">
        ///     The transition to use when transitioning in.
        /// </param>
        public void ChangeScene(Scene to, Transition tOut, Transition tIn)
        {
            if (_currentTransition == null || !_currentTransition.IsTransitioning)
            {
                NextScene = to;
                _transitionOut = tOut;
                _transitionIn = tIn;

                //  Subscribe to the TransitionCompleted events for each
                _transitionOut.TransitionCompleted += TransitionOutCompleted;
                _transitionIn.TransitionCompleted += TransitionInCompleted;

                //  Set the curren transition to the out transition first.
                _currentTransition = _transitionOut;

                //  Start the current transition.
                _currentTransition.Start(CurrentScene.RenderTarget);
            }
        }

        /// <summary>
        ///     Called when the transition out being used is completed.
        /// </summary>
        private void TransitionOutCompleted(object sender, EventArgs e)
        {
            //  Unsubscribe from the event so we don't leave any references.
            _transitionOut.TransitionCompleted -= TransitionOutCompleted;

            //  Dispose of the instance.
            _transitionOut.Dispose();
            _transitionOut = null;

            //  Change the scene.
            ChangeScene(NextScene);

            //  Set the current transition to the in transition and start it.
            _currentTransition = _transitionIn;
            _currentTransition.Start(CurrentScene.RenderTarget);
        }

        /// <summary>
        ///     Called when the transition in being used is completed.
        /// </summary>
        private void TransitionInCompleted(object sender, EventArgs e)
        {
            //  Unsubscribe from the event so we don't leave any references.
            _transitionIn.TransitionCompleted -= TransitionInCompleted;

            //  Dispose of the instance.
            _transitionIn.Dispose();
            _transitionIn = null;
            _currentTransition = null;
        }
    }
}
