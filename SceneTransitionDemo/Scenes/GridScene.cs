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

namespace SceneTransitionDemo
{
    /// <summary>
    ///     A scene that renders a grid of alternating colored rectangles.
    /// </summary>
    public class GridScene : Scene
    {
        // A 1x1 texture used for rendering the grid.
        private Texture2D _pixel;

        /// <summary>
        ///     Gets the width (x) and height (y) size, in pixels of
        ///     each tile in the grid.
        /// </summary>
        public Point TileSize { get; private set; }

        /// <summary>
        ///     Gets the total number of columns in the grid.
        /// </summary>
        public int TotalColumns { get; private set; }

        /// <summary>
        ///     Gets the total number of rows in the grid.
        /// </summary>
        public int TotalRows { get; private set; }

        /// <summary>
        ///     Creates a new GridScene instance.
        /// </summary>
        /// <param name="game">
        ///     A reference to our Game instance.
        /// </param>
        /// <param name="tileWidth">
        ///     The width, in pixels, of each tile.
        /// </param>
        /// <param name="tileHeight">
        ///     The height, in pixels, of each tile.
        /// </param>
        public GridScene(GameBase game, int tileWidth, int tileHeight) : base(game)
        {
            TileSize = new Point(tileWidth, tileHeight);
            TotalColumns = _game.Resolution.X / TileSize.X;
            TotalRows = _game.Resolution.Y / TileSize.Y;

            _pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[] { Color.White });
        }

        /// <summary>
        ///     Draws the Scene to its render targert.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance used for rendering.
        /// </param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(Point.Zero, TileSize);

            for (int row = 0; row < TotalRows; row++)
            {
                for (int column = 0; column < TotalColumns; column++)
                {
                    rect.X = column * TileSize.X;
                    rect.Y = row * TileSize.Y;

                    if ((column % 2 == 0 && row % 2 == 0) || (column % 2 != 0 && row % 2 != 0))
                    {
                        spriteBatch.Draw(_pixel, rect, Color.White);
                    }
                    else
                    {

                        spriteBatch.Draw(_pixel, rect, Color.Gray);
                    }

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
