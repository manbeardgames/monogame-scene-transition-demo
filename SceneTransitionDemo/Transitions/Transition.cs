using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SceneTransitionDemo
{
    /// <summary>
    ///     Base class for all transition instances.
    /// </summary>
    public abstract class Transition : IDisposable
    {
        public enum TransitionKind
        {
            In,
            Out
        }

        protected bool _disposed;   //  Indicates if this instance has been disposed of.
        protected Game1 _game;   //  Cached reference to our game instance.

        /// <summary>
        ///     Gets a value indicating if this transition is currently transitioning.
        /// </summary>
        public bool IsTransitioning { get; private set; }

        /// <summary>
        ///     Gets a value indicating the type of transition this is
        /// </summary>
        public TransitionKind Kind { get; private set; }

        /// <summary>
        ///     Gets the total amount of time required for this transition to complete.
        /// </summary>
        public TimeSpan TransitionTime { get; private set; }

        /// <summary>
        ///     Gets the total amount of time remaining for the transition to complete.
        /// </summary>
        public TimeSpan TransitionTimeRemaining { get; private set; }

        /// <summary>
        ///     Gets a cached reference to the RenderTarget2D instance used by the Scene
        ///     this transition is transitioning.
        /// </summary>
        public RenderTarget2D SourceTexture { get; private set; }

        /// <summary>
        ///     Gets the RenderTarget2D instance this transition renders to.
        /// </summary>
        public RenderTarget2D RenderTarget { get; private set; }

        /// <summary>
        ///     Event triggered when the transition has fully completed.
        /// </summary>
        public event EventHandler TransitionCompleted;

        /// <summary>
        ///     Creates a new Transition instance.
        /// </summary>
        /// <param name="game">
        ///     A reference to our Game instance.
        /// </param>
        /// <param name="transitionTime">
        ///     The total amount of time the transition will take.
        /// </param>
        /// <param name="kind">
        ///     The type of transition.
        /// </param>
        public Transition(Game1 game, TimeSpan transitionTime, TransitionKind kind)
        {
            _game = game;
            TransitionTimeRemaining = TransitionTime = transitionTime;
            Kind = kind;
            CreateRenderTarget();
        }

        /// <summary>
        ///     Starts the transition.
        /// </summary>
        /// <param name="sourceTexture">
        ///     A reference to the RenderTarget2D instance of the scene being transitioned.
        /// </param>
        public virtual void Start(RenderTarget2D sourceTexture)
        {
            SourceTexture = sourceTexture;
            IsTransitioning = true;
        }

        /// <summary>
        ///     Updates this Transition.
        /// </summary>
        /// <param name="gameTime">
        ///     A snapshot of the timing values provided by the MonoGame Framework.
        /// </param>
        public virtual void Update(GameTime gameTime)
        {
            TransitionTimeRemaining -= gameTime.ElapsedGameTime;

            if (TransitionTimeRemaining <= TimeSpan.Zero)
            {
                IsTransitioning = false;

                if (TransitionCompleted != null)
                {
                    TransitionCompleted(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///     Draws the Transition to its render target.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance used for rendering.
        /// </param>
        /// <param name="clearColor">
        ///     The color value to use when clearing the backbuffer.
        /// </param>
        public void Draw(SpriteBatch spriteBatch, Color clearColor)
        {
            BeginRender(spriteBatch, clearColor);
            Render(spriteBatch);
            EndRender(spriteBatch);
        }

        /// <summary>
        ///     Prepares this transition for rendering.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance used for rendering.
        /// </param>
        /// <param name="clearColor">
        ///     The color value to use when clearing the backbuffer.
        /// </param>
        private void BeginRender(SpriteBatch spriteBatch, Color clearColor)
        {
            //  Prepare the graphics device.
            _game.GraphicsDevice.SetRenderTarget(RenderTarget);
            _game.GraphicsDevice.Viewport = new Viewport(RenderTarget.Bounds);
            _game.GraphicsDevice.Clear(clearColor);

            //  Begin the sprite batch.
            spriteBatch.Begin(blendState: BlendState.AlphaBlend,
                                samplerState: SamplerState.PointClamp);
        }

        /// <summary>
        ///     Renders this transition.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance used for rendering.
        /// </param>
        protected virtual void Render(SpriteBatch spriteBatch) { }

        /// <summary>
        ///     Ends the rendering for this transition.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance used for rendering.
        /// </param>
        private void EndRender(SpriteBatch spriteBatch)
        {
            //  End the sprite batch.
            spriteBatch.End();

            _game.GraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        ///     When the graphics device is created, no contents are in VRAM, so we need
        ///     to ensure that the RenderTarget is created.
        /// </summary>
        public void HandleGraphicsCreated()
        {
            CreateRenderTarget();
        }

        /// <summary>
        ///     When the graphics device is reset, all contents of VRAM are discarded. When
        ///     this happens, we need to create things like RenderTarget instances.
        /// </summary>
        public void HandleGraphicsReset()
        {
            CreateRenderTarget();
        }

        /// <summary>
        ///     Creates a new RenderTarget instance for this Transition.
        /// </summary>
        private void CreateRenderTarget()
        {
            int width = _game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int height = _game.GraphicsDevice.PresentationParameters.BackBufferHeight;

            //  If the RenderTarget instance has already been created previously but has yet
            //  to be disposed of properly, dispose of the instance before setting a new one.
            if (RenderTarget != null && !RenderTarget.IsDisposed)
            {
                RenderTarget.Dispose();
            }

            RenderTarget = new RenderTarget2D(_game.GraphicsDevice, width, height);
        }

        /// <summary>
        ///     Handles the disposing of resources used by this instance.
        /// </summary>
        /// <remarks>
        ///     For more information on using Dispose and the IDisposable interface
        ///     https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Handles the disposing of resources used by this instance.
        /// </summary>
        /// <param name="isDisposing">
        ///     A value indicating if resources should be disposed.
        /// </param>
        /// <remarks>
        ///     For more information on using Dispose and the IDisposable interface
        ///     https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        /// </remarks>
        protected virtual void Dispose(bool isDisposing)
        {
            if (_disposed)
            {
                return;
            }

            if (isDisposing)
            {
                if (RenderTarget != null)
                {
                    RenderTarget.Dispose();
                    RenderTarget = null;
                }
            }

            _disposed = true;
        }
    }
}
