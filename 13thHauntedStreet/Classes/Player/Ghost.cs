/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Ghost class (inherits from player abstract class)
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;

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
            this.scale = 1.5f;

            this._animManager = animationManager;
            this._animManager.currentAnim = this._animManager.animationRight;
            this.texture = this._animManager.currentAnim[0];

            this.light = new PointLight
            {
                Scale = new Vector2(1000),
                Position = this.collisionBox.Center.ToVector2(),
                ShadowType = ShadowType.Occluded,
                Radius = 10,
                Intensity = 1f
            };
        }


        // Method
        public override void Update(GameTime gameTime, List<Furniture> furnitureList, Scene scene)
        {
            this._gameTime = gameTime;
            this._furnitureList = furnitureList;
            this.currentScene = scene;

            this.readInput();

            // if the player is not moving in the y axis, make the ghost float
            if (this._movement.Y == 0)
            {
                this._floatTimer += (float)this._gameTime.ElapsedGameTime.TotalSeconds * FLOATSPEED;
                this._movement.Y += (float)Math.Sin(this._floatTimer) * FLOATSIZE;
            }
            
            // if player has moved update position
            if (this._movement.X != 0 || this._movement.Y != 0)
            {
                this.updatePosition();
            }

            this.updateAnim();
        }

        public override void updatePosition()
        {
            Vector2 distance = this._movement * MOVEMENTSPEED * (float)this._gameTime.ElapsedGameTime.TotalMilliseconds;

            this.wallCollision(ref distance);

            this.position += distance;
            this.light.Position = this.position;

            this.collisionBox = new Rectangle((int)this.position.X - this.texture.Width / 2, (int)(this.position.Y), (int)this.texture.Width, (int)(this.texture.Height / 2));
        }

        /// <summary>
        /// Updates the animation that is playing 
        /// </summary>
        private void updateAnim()
        {
            if (this._movement.X > 0)
            {
                this._animManager.currentAnim = this._animManager.animationRight;
            }
            else if (this._movement.X < 0)
            {
                this._animManager.currentAnim = this._animManager.animationLeft;
            }

            this.playAnim(this._animManager.currentAnim, ref this.texture);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, null, Color.White, 0f, this.texture.Bounds.Center.ToVector2(), this.scale, SpriteEffects.None, 0f);
            //this.drawCollisionBox(spriteBatch);
        }
    }
}
