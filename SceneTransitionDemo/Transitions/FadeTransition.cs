using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SceneTransitionDemo.Transitions
{
    /// <summary>
    ///     A transition that fades the scene out/in.
    /// </summary>
    public class FadeTransition : Transition
    {
        /// <summary>
        ///     Creates a new FadeTransition instance.
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
        public FadeTransition(GameBase game, TimeSpan transitionTime, TransitionKind kind)
            : base(game, transitionTime, kind) { }

        /// <summary>
        ///     Renders this transition.
        /// </summary>
        /// <param name="spriteBatch">
        ///     The SpriteBatch instance used for rendering.
        /// </param>
        protected override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: SourceTexture,
                             destinationRectangle: SourceTexture.Bounds,
                             sourceRectangle: SourceTexture.Bounds,
                             color: Color.White * GetAlpha());
        }

        /// <summary>
        ///     Gets the alpha value to use for the color mask when rendering.
        /// </summary>
        /// <returns>
        ///     The value to use for the color mask alpha
        /// </returns>
        private float GetAlpha()
        {
            double timeLeft = TransitionTimeRemaining.TotalSeconds;

            if (Kind == TransitionKind.Out)
            {
                return (float)(timeLeft / TransitionTime.TotalSeconds);
            }
            else
            {
                return (float)(1.0 - (timeLeft / TransitionTime.TotalSeconds));
            }
        }
    }
}
