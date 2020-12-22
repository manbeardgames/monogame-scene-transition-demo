using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SceneTransitionDemo
{
    /// <summary>
    ///     A simple scene that displays an green circle to the screen and the
    ///     text "Green Circle Scene"
    /// </summary>
    public class GreenCircleScene : Scene
    {
        //  The sprite font we use to render text.
        private SpriteFont _font;

        //  The green circle texture we render
        private Texture2D _greenCircle;

        /// <summary>
        ///     Creates a new GreenCircleScene instance.
        /// </summary>
        /// <param name="game">
        ///     A reference to the Game
        /// </param>
        public GreenCircleScene(Game1 game) : base(game) { }

        /// <summary>
        ///     Loads the content for this scene.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            //  We load the font using the content manager from the Game1 reference.
            _font = _game.Content.Load<SpriteFont>("font");

            //  We load the green circle using the scene specific content manager.
            _greenCircle = _content.Load<Texture2D>("green_circle");
        }

        /// <summary>
        ///     Updates this scene.
        /// </summary>
        /// <param name="gameTime">
        ///     A snapshot of the frame specific timing values.
        /// </param>
        public override void Update(GameTime gameTime)
        {
            if (_game.CurKeyboardState.IsKeyDown(Keys.Space) && _game.PrevKeyboardState.IsKeyUp(Keys.Space))
            {
                //  Tell the game to change to the OrangeCircleScene
                _game.ChangeScene(new OrangeCircleScene(_game),
                                    new EvenOddTileTransition(_game, 32, TimeSpan.FromSeconds(1), Transition.TransitionKind.Out),
                                    new EvenOddTileTransition(_game, 32, TimeSpan.FromSeconds(1), Transition.TransitionKind.In));
            }
        }

        /// <summary>
        ///     Draws this scene.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance used for rendering.
        /// </param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "Green Circle Scene", new Vector2(10, 10), Color.White);

            int centerX = _game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2;
            int centerY = _game.GraphicsDevice.PresentationParameters.BackBufferHeight / 2;

            spriteBatch.Draw(texture: _greenCircle,
                            position: new Vector2(centerX, centerY),
                            sourceRectangle: null,
                            color: Color.White,
                            rotation: 0.0f,
                            origin: new Vector2(_greenCircle.Width, _greenCircle.Height) * 0.5f,
                            scale: Vector2.One,
                            effects: SpriteEffects.None,
                            layerDepth: 0.0f);
        }
    }
}
