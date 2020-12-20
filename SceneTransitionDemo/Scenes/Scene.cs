using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SceneTransitionDemo
{
    /// <summary>
    ///     An abstrct represention of a Scene.
    /// </summary>
    public abstract class Scene : IDisposable
    {
        //  A value indicating if this instnace has been disposed.
        private bool _disposed;

        //  Cached reference to the Game instance.
        protected readonly GameBase _game;

        /// <summary>
        ///     Gets the RenderTarget2D that we'll draw the scene too.
        /// </summary>
        public RenderTarget2D RenderTarget { get; protected set; }

        /// <summary>
        ///     Creates a new Scene instance.
        /// </summary>
        /// <param name="GameBase">
        ///     The GameBase instance of our game.
        /// </param>
        public Scene(GameBase game)
        {
            _game = game;
            CreateRenderTarget();
        }

        /// <summary>
        ///     Updates this Scene.
        /// </summary>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        ///     Handles operations that need to be performed by the Scene before
        ///     the main Draw is called.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance our game is using for rendering.
        /// </param>
        public virtual void BeforeDraw(SpriteBatch spriteBatch)
        {
            _game.GraphicsDevice.SetRenderTarget(RenderTarget);
            _game.GraphicsDevice.Viewport = new Viewport(RenderTarget.Bounds);
            _game.GraphicsDevice.Clear(_game.ClearColor);

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                              blendState: BlendState.AlphaBlend,
                              samplerState: SamplerState.PointClamp);
        }

        /// <summary>
        ///     Draws the Scene.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance our game is using for rendering.
        /// </param>
        public virtual void Draw(SpriteBatch spriteBatch) { }

        /// <summary>
        ///     Handles operations that need to be perforemd by the Scene after
        ///     the main Draw is called.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance our game is using for rendering.
        /// </param>
        public virtual void AfterDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();

            _game.GraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        ///     When the graphics device is reset, all contents of VRAM are discared. When
        ///     this happens, we need to create things like RenderTarget instances.
        /// </summary>
        public virtual void GraphicsReset()
        {
            CreateRenderTarget();
        }

        /// <summary>
        ///     When the graphics device is created, no contents are in VRAM, so we need
        ///     to ensure that the RenderTarget is created.
        /// </summary>
        public virtual void GraphicsCreated()
        {
            CreateRenderTarget();
        }

        /// <summary>
        ///     Creates a new RenderTarget instance for this Scene.
        /// </summary>
        protected virtual void CreateRenderTarget()
        {

            int width = _game.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int height = _game.GraphicsDevice.PresentationParameters.BackBufferHeight;

            //  If the RenderTarget instance has already been created previously but has yet
            //  to be disposed of properly, dispose of the instnace before setting a new one.
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
        ///     For more informaiton on using Dispose and the IDisposable interface
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
        ///     For more informaiton on using Dispose and the IDisposable interface
        ///     https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        /// </remarks>
        protected virtual void Dispose(bool isDisposing)
        {
            //  If we have already disposed, return back
            if (_disposed)
            {
                return;
            }

            if (isDisposing)
            {
                //  Dispose of our rendertarget.
                if (RenderTarget != null && !RenderTarget.IsDisposed)
                {
                    RenderTarget.Dispose();
                    RenderTarget = null;
                }
            }

            _disposed = true;
        }

    }
}
