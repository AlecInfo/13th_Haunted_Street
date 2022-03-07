/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Ghost class (inherits from player abstract class)
 * Date    : 03.03.2022
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class Ghost : Player
    {
        // Properties
        private const float MOVEMENTSPEED = 0.4f;

        private GhostAnimationManager _animManager;
        private Texture2D _currentTexture;

        private float _floatTimer;
        private const int FLOATSPEED = 4;
        private const int FLOATSIZE = 3;


        // Ctor
        public Ghost(Input input, Vector2 initialPos, GhostAnimationManager animationManager)
        {
            this._input = input;
            this._position = initialPos;

            this._animManager = animationManager;
            this._animManager.currentAnim = this._animManager.animationRight;
            this._currentTexture = this._animManager.currentAnim[0];
        }


        // Method
        public override void Update(GameTime gameTime)
        {
            this.readInput();

            // float
            if (this._movement.Y == 0)
            {
                this._floatTimer += (float)gameTime.ElapsedGameTime.TotalSeconds * FLOATSPEED;
                this._position.Y = this._position.Y + (float)Math.Sin(_floatTimer) * FLOATSIZE;
            }

            this._position += this._movement * MOVEMENTSPEED * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            this.updateAnim(gameTime);
        }

        /// <summary>
        /// Updates the animation that is playing
        /// </summary>
        /// <param name="gameTime"></param>
        private void updateAnim(GameTime gameTime)
        {
            if (this._movement.X > 0)
            {
                this._animManager.currentAnim = this._animManager.animationRight;
            }
            else if (this._movement.X < 0)
            {
                this._animManager.currentAnim = this._animManager.animationLeft;
            }
            this._currentTexture = this.playAnim(gameTime, this._animManager.currentAnim, this._currentTexture);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._currentTexture, this._position, null, Color.White, 0f, this._currentTexture.Bounds.Center.ToVector2(), 1.5f, SpriteEffects.None, 0f);
        }
    }
}
