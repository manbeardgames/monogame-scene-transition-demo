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

namespace SceneTransitionDemo.Scenes
{
    /// <summary>
    ///     A Scene used to render vertical strips to the screen.
    /// </summary>
    public class VerticleStripScene : Scene
    {
        //  A 1x1 pixel texture used for rendering the strips.
        private Texture2D _pixel;

        /// <summary>
        ///     Gets the width, in pixels, of each column.
        /// </summary>
        public int ColumnWidth { get; private set; }

        /// <summary>
        ///     Gets the total number of columns.
        /// </summary>
        public int TotalColumns { get; private set; }

        /// <summary>
        ///     Creates a new VerticalStripScene instance.
        /// </summary>
        /// <param name="game">
        ///     A reference to the Game instance.
        /// </param>
        /// <param name="columnWidth">
        ///     The width, in pixels, of each column.
        /// </param>
        public VerticleStripScene(GameBase game, int columnWidth) : base(game)
        {
            ColumnWidth = columnWidth;
            TotalColumns = _game.Resolution.X / ColumnWidth;

            _pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[] { Color.White });
        }

        /// <summary>
        ///     Draws the Scene to its render target.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance used for rendering.
        /// </param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, ColumnWidth, _game.Resolution.Y);


            for (int column = 0; column < TotalColumns; column++)
            {
                rect.X = column * ColumnWidth;

                if (column % 2 == 0)
                {
                    spriteBatch.Draw(_pixel, rect, Color.White);
                }
                else
                {

                    spriteBatch.Draw(_pixel, rect, Color.Gray);
                }
            }

            base.Draw(spriteBatch);
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
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_pixel != null && !_pixel.IsDisposed)
                {
                    _pixel.Dispose();
                    _pixel = null;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
