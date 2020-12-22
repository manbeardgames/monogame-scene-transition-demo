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
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SceneTransitionDemo
{
    /// <summary>
    ///     A transition that divides the scene into a checkerboard and spins out/in the odd tiles
    ///     then the even tiles
    /// </summary>
    public class EvenOddTileTransition : Transition
    {
        //  Have the transition time.
        private double _transitionHalfTime;

        //  The width and height, in pixels,  of a tile.
        private int _tileSize;

        //  The total number of columns.
        private int _columns;

        //  The total number of rows.
        private int _rows;

        /// <summary>
        ///     Creates a new EvenOddTransition instance.
        /// </summary>
        /// <param name="game">
        ///     A reference to our Game instance.
        /// </param>
        /// <param name="tileSize">
        ///     The width and height, in pixels, of a tile.
        /// </param>
        /// <param name="transitionTime">
        ///     The total amount of time the transition will take.
        /// </param>
        /// <param name="kind">
        ///     The type of transition.
        /// </param>
        public EvenOddTileTransition(Game1 game, int tileSize, TimeSpan transitionTime, TransitionKind kind)
            : base(game, transitionTime, kind)
        {
            _transitionHalfTime = TransitionTime.TotalSeconds / 2;
            _tileSize = tileSize;
        }

        /// <summary>
        ///     Starts the transition.
        /// </summary>
        /// <param name="sourceTexture">
        ///     A reference to the RenderTarget2D instance of the scene being transitioned.
        /// </param>
        public override void Start(RenderTarget2D sourceTexture)
        {
            base.Start(sourceTexture);

            _columns = (int)Math.Ceiling(SourceTexture.Width / (float)_tileSize);
            _rows = (int)Math.Ceiling(SourceTexture.Height / (float)_tileSize);
        }

        /// <summary>
        ///     Renders this transition.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance used for rendering.
        /// </param>
        protected override void Render(SpriteBatch spriteBatch)
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns; column++)
                {
                    int size = GetSize(IsOdd(column, row));
                    int xPos = ((column * _tileSize) + (_tileSize - size) / 2) + (size / 2);
                    int yPos = ((row * _tileSize) + (_tileSize - size) / 2) + (size / 2);

                    spriteBatch.Draw(texture: SourceTexture,
                                        destinationRectangle: new Rectangle(xPos, yPos, size, size),
                                        sourceRectangle: new Rectangle(column * _tileSize, row * _tileSize, _tileSize, _tileSize),
                                        color: Color.White,
                                        rotation: GetRotation(IsOdd(column, row)),
                                        origin: new Vector2(_tileSize, _tileSize) * 0.5f,
                                        effects: SpriteEffects.None,
                                        layerDepth: 0.0f);
                }
            }
        }

        /// <summary>
        ///     Calculates and returns the value to use for the tiles rotation.
        /// </summary>
        /// <param name="isOdd">
        ///     Is the tile one of the odd tiles. An odd tile is one that is ina  row and column that is both
        ///     even numbers, or both odd numbers.
        /// </param>
        /// <returns>
        ///     The rotation value to use for the tile.
        /// </returns>
        private float GetRotation(bool isOdd)
        {
            double timeLeft = TransitionTimeRemaining.TotalSeconds;

            if (isOdd)
            {
                timeLeft = Math.Min(timeLeft, _transitionHalfTime);
            }
            else
            {
                timeLeft = Math.Max(timeLeft - _transitionHalfTime, 0);
            }

            if (Kind == TransitionKind.Out)
            {
                return 5.0f * (float)Math.Sin((timeLeft / _transitionHalfTime) - 1.0);
            }
            else
            {
                return 5.0f * (float)Math.Sin((timeLeft / _transitionHalfTime));
            }
        }

        /// <summary>
        ///     Calculates and returns the size value to use for a tile.
        /// </summary>
        /// <param name="isOdd">
        ///     Is the tile one of the odd tiles. An odd tile is one that is ina  row and column that is both
        ///     even numbers, or both odd numbers.
        /// </param>
        /// <returns>
        ///     The size value to use for the tile.
        /// </returns>
        private int GetSize(bool isOdd)
        {
            double timeLeft = TransitionTimeRemaining.TotalSeconds;

            if (isOdd)
            {
                timeLeft = Math.Min(timeLeft, _transitionHalfTime);
            }
            else
            {
                timeLeft = Math.Max(timeLeft - _transitionHalfTime, 0);
            }

            if (Kind == TransitionKind.Out)
            {
                return (int)((_tileSize) * (timeLeft / _transitionHalfTime));
            }
            else
            {
                return (int)((_tileSize) * (1 - (timeLeft / _transitionHalfTime)));
            }
        }

        /// <summary>
        ///     Given a column (x) and row (y) of a tile, determines if it is an 
        ///     odd tile.
        /// </summary>
        /// <remarks>
        ///     An odd tile is one where both the row and column are even numbers or
        ///     both the row and column are odd numbers
        /// </remarks>
        /// <param name="column">
        ///     The column the tile is in.
        /// </param>
        /// <param name="row">
        ///     The row the tile is in.
        /// </param>
        /// <returns></returns>
        private bool IsOdd(int column, int row)
        {
            return (column % 2 == 0 && row % 2 == 0) || (column % 2 != 0 && row % 2 != 0);
        }
    }
}
