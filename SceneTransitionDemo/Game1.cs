using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SceneTransitionDemo
{
    public class Game1 : Game
    {
        //  Manages the presentation of graphics
        private GraphicsDeviceManager _graphics;

        //  Used for 2D rendering to the screen.
        private SpriteBatch _spriteBatch;

        //  The current scene that is active.
        private Scene _activeScene;

        //  The next scene to switch to.
        private Scene _nextScene;

        //   The Transition Out instance.
        private Transition _transitionOut;

        //  The Transition In instance.
        private Transition _transitionIn;

        //  The current transition being used.
        private Transition _currentTransition;

        /// <summary>
        ///     Gets the state of keyboard input during the previous frame.
        /// </summary>
        public KeyboardState PrevKeyboardState { get; private set; }

        /// <summary>
        ///     Gets the state of keyboard input during the current frame.
        /// </summary>
        public KeyboardState CurKeyboardState { get; private set; }

        /// <summary>
        ///     Creates a new Game1 instance.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        ///     Initializes the game.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            //  Handle the created and reset events.
            _graphics.DeviceCreated += GraphicsDeviceCreated;
            _graphics.DeviceReset += GraphicsDeviceReset;

            //  Load the GreenCircleScene as our first scene.
            ChangeScene(new GreenCircleScene(this));
        }

        /// <summary>
        ///     Loads the content for our game.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //  Load the global spritefont.
            Content.Load<SpriteFont>("font");
        }

        /// <summary>
        ///     Updates our game.
        /// </summary>
        /// <param name="gameTime">
        ///     A snapshot of the timing values.
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            PrevKeyboardState = CurKeyboardState;
            CurKeyboardState = Keyboard.GetState();

            //  If there is a current transition happening, then we need to update
            //  that transition. Otherwise, if there is no current transition, but there
            //  is a next scene to switch to, switch to that scene instead.
            if (_currentTransition != null && _currentTransition.IsTransitioning)
            {
                _currentTransition.Update(gameTime);
            }
            else if (_currentTransition == null && _nextScene != null)
            {
                TransitionScene();
            }

            //  If there is an active scene, update it.
            if (_activeScene != null)
            {
                _activeScene.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        ///     Draws our game.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            //  The color to use when clearing the backbuffer.
            Color clearColor = Color.CornflowerBlue;

            if (_activeScene != null)
            {
                //  Render the current scene.
                _activeScene.BeforeDraw(_spriteBatch, clearColor);
                _activeScene.Draw(_spriteBatch);
                _activeScene.AfterDraw(_spriteBatch);

                //  If there is a current transition happening, render the transition.
                if (_currentTransition != null && _currentTransition.IsTransitioning)
                {
                    _currentTransition.Draw(_spriteBatch, Color.Black);
                }

                //  Prepare the graphics device for the final render.
                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin();

                //  If we are transitioning, then we render the transition effect; otherwise, we'll render
                //  the current scene.
                if (_currentTransition != null && _currentTransition.IsTransitioning)
                {
                    _spriteBatch.Draw(texture: _currentTransition.RenderTarget,
                                        destinationRectangle: _currentTransition.RenderTarget.Bounds,
                                        sourceRectangle: _currentTransition.RenderTarget.Bounds,
                                        color: Color.White);
                }
                else if (_activeScene != null)
                {
                    _spriteBatch.Draw(texture: _activeScene.RenderTarget,
                                        destinationRectangle: _activeScene.RenderTarget.Bounds,
                                        sourceRectangle: _activeScene.RenderTarget.Bounds,
                                        color: Color.White);
                }

                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        ///     Sets the next scene to switch to.
        /// </summary>
        /// <param name="next">
        ///     The Scene instance to switch to.
        /// </param>
        public void ChangeScene(Scene next)
        {
            //  Only set the _nextScene value if it is not the
            //  same instance as the _activeScene.
            if (_activeScene != next)
            {
                _nextScene = next;
            }
        }

        /// <summary>
        ///     Changes the current scene to the screen provided using the transition
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
                if (_activeScene != to)
                {
                    _nextScene = to;
                    _transitionOut = tOut;
                    _transitionIn = tIn;

                    //  Subscribe to the TransitionCompleted events for each
                    _transitionOut.TransitionCompleted += TransitionOutCompleted;
                    _transitionIn.TransitionCompleted += TransitionInCompleted;

                    //  Set the curren transition to the out transition first.
                    _currentTransition = _transitionOut;

                    //  Start the current transition.
                    _currentTransition.Start(_activeScene.RenderTarget);
                }
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
            TransitionScene();

            //  Set the current transition to the in transition and start it.
            _currentTransition = _transitionIn;
            _currentTransition.Start(_activeScene.RenderTarget);
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

        /// <summary>
        ///     Handles transitioning gracefully from one scene to
        ///     the next.
        /// </summary>
        private void TransitionScene()
        {
            if (_activeScene != null)
            {
                _activeScene.UnloadContent();
            }

            //  Perform a garbage collection to ensure memory is cleared
            GC.Collect();

            //  Set the active scene.
            _activeScene = _nextScene;

            //  Null the next scene value
            _nextScene = null;

            //  If the active scene isn't null, initialize it.
            //  Remember, the Initialize method also calls the LoadContent method
            if (_activeScene != null)
            {
                _activeScene.Initialize();
            }
        }

        /// <summary>
        ///     Called when the graphics device is created.
        /// </summary>
        protected virtual void GraphicsDeviceCreated(object sender, EventArgs e)
        {
            if (_activeScene != null)
            {
                _activeScene.HandleGraphicsCreated();
            }

            if (_transitionOut != null)
            {
                _transitionOut.HandleGraphicsCreated();
            }

            if (_transitionIn != null)
            {
                _transitionIn.HandleGraphicsCreated();
            }
        }

        /// <summary>
        ///     Called when the graphics device is reset.
        /// </summary>
        protected virtual void GraphicsDeviceReset(object sender, EventArgs e)
        {
            if (_activeScene != null)
            {
                _activeScene.HandleGraphicsReset();
            }

            if (_transitionOut != null)
            {
                _transitionOut.HandleGraphicsReset();
            }

            if (_transitionIn != null)
            {
                _transitionIn.HandleGraphicsReset();
            }
        }
    }
}
