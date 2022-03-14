/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Player abstract class
 * Date    : 10.03.2022
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    abstract class Player
    {
        // Properties
        protected Input _input;

        protected Vector2 _position;
        protected Vector2 _movement;
        protected Texture2D _currentTexture;
        protected float _scale;

        protected Vector2 _collisionPos;
        protected Vector2 _collisionSize;

        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 100;


        // Methods
        public abstract void Update(GameTime gameTime, List<Furniture> furnitureList);

        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Updates movement 
        /// </summary>
        protected void readInput()
        {
            KeyboardState kbdState = Keyboard.GetState();
            this._movement = Vector2.Zero;

            // X
            if (kbdState.IsKeyDown(this._input.Left))
            {
                _movement.X = -1;
            }

            if (kbdState.IsKeyDown(this._input.Right))
            {
                _movement.X = +1;
            }

            // Y
            if (kbdState.IsKeyDown(this._input.Up))
            {
                _movement.Y = -1;
            }

            if (kbdState.IsKeyDown(this._input.Down))
            {
                _movement.Y = +1;
            }

            // diagonal fix
            if (_movement.X != 0 && _movement.Y != 0) // if is moving diagonally
            {
                _movement /= 1.4f;
            }
        }

        /// <summary>
        /// plays the animation
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="animation">animation that is currently playing</param>
        /// <param name="currentTexture">texture that is currently beeing drawn</param>
        /// <returns>next frame</returns>
        protected Texture2D playAnim(GameTime gameTime, List<Texture2D> animation, Texture2D currentTexture)
        {
            this.timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (this.timeSinceLastFrame > this.millisecondsPerFrame)
            {
                this.timeSinceLastFrame -= this.millisecondsPerFrame;
                
                // find current frame id
                int nextTextureId = animation.FindIndex(item => item == currentTexture);

                if (nextTextureId < animation.Count - 1)
                {
                    return animation[nextTextureId + 1];
                }

                // else
                return animation[0];
            }

            // else
            return currentTexture;
        }

        /// <summary>
        /// draw the player collision box to help debug
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected void drawCollisionBox(SpriteBatch spriteBatch)
        {
            Texture2D defaultTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            defaultTexture.SetData(new Color[] { Color.White });

            spriteBatch.Draw(defaultTexture, new Rectangle(this._collisionPos.ToPoint(), (this._collisionSize).ToPoint()), null, Color.White * 0.5f, 0f, new Vector2(0.5f), SpriteEffects.None, 1f);
            spriteBatch.Draw(defaultTexture, new Rectangle(this._collisionPos.ToPoint(), new Vector2(5).ToPoint()), null, Color.Red, 0f, new Vector2(0.5f), SpriteEffects.None, 1f);
        }

        protected Vector2 objectCollision(List<Furniture> furnitureList, Vector2 distance)
        {
            foreach (Furniture item in furnitureList)
            {
                if (this._collisionPos.X + this._collisionSize.X / 2 + distance.X > item.collisionBox.Left &&
                    this._collisionPos.X - this._collisionSize.X / 2 < item.collisionBox.Left &&
                    this._collisionPos.Y + this._collisionSize.Y / 2 > item.collisionBox.Top &&
                    this._collisionPos.Y - this._collisionSize.Y / 2 < item.collisionBox.Bottom) // Left
                {
                    distance.X = 0;
                }

                if (this._collisionPos.X - this._collisionSize.X / 2 + distance.X < item.collisionBox.Right &&
                    this._collisionPos.X + this._collisionSize.X / 2 > item.collisionBox.Right &&
                    this._collisionPos.Y + this._collisionSize.Y / 2 > item.collisionBox.Top &&
                    this._collisionPos.Y - this._collisionSize.Y / 2 < item.collisionBox.Bottom) // Right
                {
                    distance.X = 0;
                }

                if (this._collisionPos.Y + this._collisionSize.Y / 2 + distance.Y > item.collisionBox.Top &&
                    this._collisionPos.Y - this._collisionSize.Y / 2 < item.collisionBox.Top &&
                    this._collisionPos.X + this._collisionSize.X / 2 > item.collisionBox.Left &&
                    this._collisionPos.X - this._collisionSize.X / 2 < item.collisionBox.Right) // Top
                {
                    distance.Y = 0;
                }

                if (this._collisionPos.Y - this._collisionSize.Y / 2 + distance.Y < item.collisionBox.Bottom &&
                    this._collisionPos.Y + this._collisionSize.Y / 2 > item.collisionBox.Bottom &&
                    this._collisionPos.X + this._collisionSize.X / 2 > item.collisionBox.Left &&
                    this._collisionPos.X - this._collisionSize.X / 2 < item.collisionBox.Right) // Bottom
                {
                    distance.Y = 0;
                }
            }

            return distance;
        }
    }
}
