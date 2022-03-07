/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Hunter class (inherits from player abstract class)
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
    class Hunter : Player
    {        
        // Properties
        private const float MOVEMENTSPEED = 0.45f;

        private HunterAnimationManager _animManager;
        private Texture2D _currentTexture;

        private enum direction
        {
            none,
            left,
            right,
            up,
            down
        }

        private direction _currentDirection = direction.down;


        // Ctor
        public Hunter(Input input, Vector2 initialPos, HunterAnimationManager animationManager)
        {
            this._input = input;
            this._position = initialPos;

            this._animManager = animationManager;
            this._animManager.currentAnim = this._animManager.walkingDown;
            this._currentTexture = this._animManager.currentAnim[0];
        }


        // Methods
        public override void Update(GameTime gameTime)
        {
            this.readInput();

            this._position += this._movement * MOVEMENTSPEED * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            this.updateAnim(gameTime);
        }

        /// <summary>
        /// Updates the animation that is playing
        /// </summary>
        /// <param name="gameTime"></param>
        private void updateAnim(GameTime gameTime)
        {
            // if is moving
            if (this._movement.X != 0 || this._movement.Y != 0)
            {
                if (this._movement.X < 0)
                {
                    this._animManager.currentAnim = this._animManager.walkingLeft;
                    this._currentDirection = direction.left;
                }
                else if (this._movement.X > 0)
                {
                    this._animManager.currentAnim = this._animManager.walkingRight;
                    this._currentDirection = direction.right;
                }
                else
                {
                    if (this._movement.Y < 0)
                    {
                        this._animManager.currentAnim = this._animManager.walkingUp;
                        this._currentDirection = direction.up;
                    }
                    else if (this._movement.Y > 0)
                    {
                        this._animManager.currentAnim = this._animManager.walkingDown;
                        this._currentDirection = direction.down;
                    }
                }
            }
            else // if its not moving
            {
                switch (this._currentDirection)
                {
                    case direction.left:
                        this._animManager.currentAnim = this._animManager.idleLeft;
                        break;

                    case direction.right:
                        this._animManager.currentAnim = this._animManager.idleRight;
                        break;

                    case direction.up:
                        this._animManager.currentAnim = this._animManager.idleUp;
                        break;

                    case direction.down:
                        this._animManager.currentAnim = this._animManager.idleDown;
                        break;
                }
            }

            this._currentTexture = this.playAnim(gameTime, _animManager.currentAnim, this._currentTexture);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._currentTexture, this._position, null, Color.White, 0f, this._currentTexture.Bounds.Center.ToVector2(), 3f, SpriteEffects.None, 0f);
        }
    }
}
