using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SceneTransitionDemo.Scenes
{
    /// <summary>
    ///     A Scene that renders horizontal strips on the screen.
    /// </summary>
    public class HorizontalStripScene : Scene
    {
        // A 1x1 pixel texture used for rendering the strips.
        private Texture2D _pixel;

        /// <summary>
        ///     Gets the height, in pixels, of each row.
        /// </summary>
        public int RowHeight { get; private set; }

        /// <summary>
        ///     Gets the total number of rows.
        /// </summary>
        public int TotalRows { get; private set; }

        /// <summary>
        ///     Creates a new HorizontalStripScene instance.
        /// </summary>
        /// <param name="game"> 
        ///     A reference to our Game instance.
        /// </param>
        /// <param name="rowHeight">
        ///     The height, in pixels, of each row
        /// </param>
        public HorizontalStripScene(GameBase game, int rowHeight) : base(game)
        {
            RowHeight = rowHeight;
            TotalRows = _game.Resolution.Y / RowHeight;

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
            Rectangle rect = new Rectangle(0, 0, _game.Resolution.X, RowHeight);


            for (int row = 0; row < TotalRows; row++)
            {
                rect.Y = row * RowHeight;

                if (row % 2 == 0)
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
