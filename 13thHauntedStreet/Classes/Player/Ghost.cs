/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Ghost class (inherits from player abstract class)
 * Date    : 09.03.2022
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

        private float _floatTimer;
        private const int FLOATSPEED = 4;
        private const float FLOATSIZE = 0.1f;


        // Ctor
        public Ghost(Input input, Vector2 initialPos, GhostAnimationManager animationManager)
        {
            this._input = input;
            this.position = initialPos;

            this._animManager = animationManager;
            this._animManager.currentAnim = this._animManager.animationRight;
            this.texture = this._animManager.currentAnim[0];

            this._collisionPos = new Vector2(this.position.X, this.position.Y + this.texture.Height / 2);
            this._collisionSize = new Vector2(this.texture.Width, this.texture.Height / 2);

            this._scale = 1.5f;
        }


        // Method
        public override void Update(GameTime gameTime, List<Furniture> furnitureList)
        {
            this.readInput();

            this.updatePosition(gameTime, furnitureList);

            this.updateAnim(gameTime);
        }

        /// <summary>
        /// Updates the player position
        /// </summary>
        private void updatePosition(GameTime gameTime, List<Furniture> furnitureList)
        {
            if (this._movement.Y == 0)
            {
                this._floatTimer += (float)gameTime.ElapsedGameTime.TotalSeconds * FLOATSPEED;
                this._movement.Y += (float)Math.Sin(this._floatTimer) * FLOATSIZE;
            }

            this.position += this._movement * MOVEMENTSPEED * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            this._collisionPos = new Vector2(this.position.X, this.position.Y + this.texture.Height / 2);
            this._collisionSize = new Vector2(this.texture.Width, this.texture.Height / 2);
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
            this.texture = this.playAnim(gameTime, this._animManager.currentAnim, this.texture);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, null, Color.White, 0f, this.texture.Bounds.Center.ToVector2(), this._scale, SpriteEffects.None, 0f);
            //this.drawCollisionBox(spriteBatch);
        }
    }
}
