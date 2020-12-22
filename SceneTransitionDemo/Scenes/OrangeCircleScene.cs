using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SceneTransitionDemo
{
    /// <summary>
    ///     A simple scene that displays an orange circle to the screen and the
    ///     text "Orange Circle Scene"
    /// </summary>
    public class OrangeCircleScene : Scene
    {
        //  The sprite font we use to render text.
        private SpriteFont _font;

        //  The orange circle texture we render
        private Texture2D _orangeCircle;

        /// <summary>
        ///     Creates a new OrangeCircleScene instance.
        /// </summary>
        /// <param name="game">
        ///     A reference to the Game
        /// </param>
        public OrangeCircleScene(Game1 game) : base(game) { }

        /// <summary>
        ///     Loads the content for this scene.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            //  We load the font using the content manager from the Game1 reference.
            _font = _game.Content.Load<SpriteFont>("font");

            //  We load the orange circle using the scene specific content manager.
            _orangeCircle = _content.Load<Texture2D>("orange_circle");
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
                _game.ChangeScene(new GreenCircleScene(_game),
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
            spriteBatch.DrawString(_font, "Orange Circle Scene", new Vector2(10, 10), Color.White);

            int centerX = _game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2;
            int centerY = _game.GraphicsDevice.PresentationParameters.BackBufferHeight / 2;

            spriteBatch.Draw(texture: _orangeCircle,
                            position: new Vector2(centerX, centerY),
                            sourceRectangle: null,
                            color: Color.White,
                            rotation: 0.0f,
                            origin: new Vector2(_orangeCircle.Width, _orangeCircle.Height) * 0.5f,
                            scale: Vector2.One,
                            effects: SpriteEffects.None,
                            layerDepth: 0.0f);
        }
    }
}
