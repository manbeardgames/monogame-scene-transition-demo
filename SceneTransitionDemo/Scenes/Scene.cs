/* ----------------------------------------------------------------------------
    This is free and unencumbered software released into the public domain.

    Anyone is free to copy, modify, publish, use, compile, sell, or
    distribute this software, either in source code form or as a compiled
    binary, for any purpose, commercial or non-commercial, and by any
    means.

    In jurisdictions that recognize copyright laws, the author or authors
    of this software dedicate any and all copyright interest in the
    software to the public domain.We make this dedication for the benefit
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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SceneTransitionDemo
{
    public abstract class Scene
    {
        //  A cached reference to our Game instance.
        protected Game1 _game;

        //  Used to load scene specific content
        protected ContentManager _content;

        /// <summary>
        ///     Gets the RenderTarget this Scene draws to.
        /// </summary>
        public RenderTarget2D RenderTarget { get; protected set; }

        /// <summary>
        ///     Creates a new Scene instance.
        /// </summary>
        /// <param name="game">
        ///     A reference to our Game1 instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if the value supplied for <paramref name="game"/> 
        ///     is null
        /// </exception>
        public Scene(Game1 game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game), "Game cannot be null!");
            }

            _game = game;

            //  Generate the render target
            GenerateRenderTarget();
        }

        /// <summary>
        ///     Initializes the Scene
        /// </summary>
        /// <remarks>
        ///     This is called only once, immediately after the scene becomes
        ///     the active scene, and before the first Update is called on 
        ///     the scene
        /// </remarks>
        public virtual void Initialize()
        {
            _content = new ContentManager(_game.Services);
            _content.RootDirectory = _game.Content.RootDirectory;
            LoadContent();
        }

        /// <summary>
        ///     Loads the content specific to the Scene.
        /// </summary>
        /// <remarks>
        ///     This is called internally by the Initialize() method.
        /// </remarks>
        public virtual void LoadContent() { }

        /// <summary>
        ///     Unloads any content that has been loaded by the scene.
        /// </summary>
        /// <remarks>
        ///     This will be called after the game switches to a new
        ///     scene.
        /// </remarks>
        public virtual void UnloadContent()
        {
            _content.Unload();
            _content = null;

            //  Dispose of the render target if it is not already disposed.
            if (RenderTarget != null && !RenderTarget.IsDisposed)
            {
                RenderTarget.Dispose();
                RenderTarget = null;
            }
        }

        /// <summary>
        ///     Updates the Scene.
        /// </summary>
        /// <param name="gameTime">
        ///     A snapshot of the frame specific timing values.
        /// </param>
        public virtual void Update(GameTime gameTime) { }


        /// <summary>
        ///     Handles preparing the Scene to draw.
        /// </summary>
        /// <remarks>
        ///     This is called just before the main Draw method.
        /// </remarks>
        /// <param name="spriteBatch"></param>
        public virtual void BeforeDraw(SpriteBatch spriteBatch, Color clearColor)
        {
            //  Tell the graphics device to use this scene's render target.
            _game.GraphicsDevice.SetRenderTarget(RenderTarget);

            //  Clear the backbuffer
            _game.GraphicsDevice.Clear(clearColor);

            //  Begin the spritebatch
            spriteBatch.Begin();
        }

        /// <summary>
        ///     Draws the Scene to the screen.
        /// </summary>
        /// <remarks>
        ///     This is called immediately after BeforeDraw.
        /// </remarks>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance used for rendering.
        /// </param>
        public virtual void Draw(SpriteBatch spriteBatch) { }

        /// <summary>
        ///     Handles ending any drawing the scene is performing.
        /// </summary>
        /// <remarks>
        ///     This is called immediately after Draw.
        /// </remarks>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance used for rendering.
        /// </param>
        public virtual void AfterDraw(SpriteBatch spriteBatch)
        {
            //  End the spritebatch
            spriteBatch.End();

            //  Tell the graphics device to stop using the render target
            _game.GraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        ///     Generates a RenderTarget2D instance for our Scene.
        /// </summary>
        public virtual void GenerateRenderTarget()
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
        ///     Handles creating the contents of VRAM for the scene when the GraphicsDevice
        ///     is created.
        /// </summary>
        public virtual void HandleGraphicsCreated()
        {
            GenerateRenderTarget();
        }

        /// <summary>
        ///     Handles recreating contents of VRAM for the scene when the GraphicsDevice
        ///     is reset.
        /// </summary>
        public virtual void HandleGraphicsReset()
        {
            GenerateRenderTarget();
        }
    }
}
