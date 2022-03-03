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
        private const float MOVEMENTSPEED = 0.35f;

        private GhostAnimationManager _animManager;
        private Texture2D currentTexture;

        // Ctor
        public Ghost(Input input, Vector2 initialPos, GhostAnimationManager animationManager)
        {
            this._input = input;
            this._position = initialPos;

            this._animManager = animationManager;
            this._animManager.currentAnim = this._animManager.animationRight;
            this.currentTexture = this._animManager.currentAnim[0];
        }


        // Method
        public override void Update(GameTime gameTime)
        {
            this.readInput();

            this._position += this._movement * MOVEMENTSPEED * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            this.updateAnim(gameTime);
        }

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
            this.currentTexture = this.playAnim(gameTime, this._animManager.currentAnim, this.currentTexture);
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.currentTexture, this._position, null, Color.White, 0f, this.currentTexture.Bounds.Center.ToVector2(), 2f, SpriteEffects.None, 0f);
        }
    }
}
