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
