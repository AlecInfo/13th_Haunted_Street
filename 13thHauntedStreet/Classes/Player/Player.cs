/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Player abstract class
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
    abstract class Player : GameObject
    {
        // Properties
        protected Input _input;
        protected GameTime _gameTime;
        public Scene currentScene;
        public Light light;

        protected Vector2 _movement;
        public float scale;
        protected Rectangle _collisionBox;

        protected List<Furniture> _furnitureList;

        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 100;

        public Rectangle rectangle
        { 
            get
            {
                return new Rectangle(
                    this.position.ToPoint() - (this.texture.Bounds.Size.ToVector2() * this.scale / 2).ToPoint(), 
                    (this.texture.Bounds.Size.ToVector2() + this.texture.Bounds.Size.ToVector2() / 2 * this.scale).ToPoint());
            }
        }


        // Methods
        public abstract void Update(GameTime gameTime, List<Furniture> furnitureList, Scene scene);

        /// <summary>
        /// Updates movement with input
        /// </summary>
        protected void readInput()
        {
            KeyboardState kbdState = Keyboard.GetState();
            this._movement = Vector2.Zero;

            // X
            if (kbdState.IsKeyDown(this._input.Left))
            {
                this._movement.X += -1;
            }

            if (kbdState.IsKeyDown(this._input.Right))
            {
                this._movement.X += 1;
            }

            // Y
            if (kbdState.IsKeyDown(this._input.Up))
            {
                this._movement.Y += -1;
            }

            if (kbdState.IsKeyDown(this._input.Down))
            {
                this._movement.Y += 1;
            }

            // diagonal fix
            if (this._movement.X != 0 && this._movement.Y != 0) // if is moving diagonally
            {
                this._movement /= 1.4f;
            }
        }

        /// <summary>
        /// Updates the player position
        /// </summary>
        public abstract void updatePosition();

        /// <summary>
        /// plays the animation
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="animation">animation that is currently playing</param>
        /// <param name="currentTexture">texture that is currently beeing drawn</param>
        protected void playAnim(List<Texture2D> animation, ref Texture2D currentTexture)
        {
            this.timeSinceLastFrame += this._gameTime.ElapsedGameTime.Milliseconds;
            if (this.timeSinceLastFrame > this.millisecondsPerFrame)
            {
                this.timeSinceLastFrame -= this.millisecondsPerFrame;

                // find current frame id
                Texture2D texture = currentTexture;
                int nextTextureId = animation.FindIndex(item => item == texture);

                if (nextTextureId < animation.Count - 1)
                {
                    currentTexture = animation[nextTextureId + 1];
                }
                else
                    currentTexture = animation[0];
            }
        }

        /// <summary>
        /// Draw the player collision box to help debug
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected void drawCollisionBox(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.defaultTexture, this._collisionBox, null, Color.White * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// collision between the player and game objects
        /// </summary>
        /// <param name="distance"></param>
        protected void objectCollision(ref Vector2 distance)
        {
            foreach (Furniture item in this._furnitureList)
            {
                if (this._collisionBox.Right + distance.X > item.collisionBox.Left &&
                    this._collisionBox.Left < item.collisionBox.Left &&
                    this._collisionBox.Bottom > item.collisionBox.Top &&
                    this._collisionBox.Top < item.collisionBox.Bottom) // Left
                {
                    distance.X = 0;
                }

                if (this._collisionBox.Left + distance.X < item.collisionBox.Right &&
                    this._collisionBox.Right > item.collisionBox.Right &&
                    this._collisionBox.Bottom > item.collisionBox.Top &&
                    this._collisionBox.Top < item.collisionBox.Bottom) // Right
                {
                    distance.X = 0;
                }

                if (this._collisionBox.Bottom + distance.Y > item.collisionBox.Top &&
                    this._collisionBox.Top < item.collisionBox.Top &&
                    this._collisionBox.Right > item.collisionBox.Left &&
                    this._collisionBox.Left < item.collisionBox.Right) // Top
                {
                    distance.Y = 0;
                }

                if (this._collisionBox.Top + distance.Y < item.collisionBox.Bottom &&
                    this._collisionBox.Bottom > item.collisionBox.Bottom &&
                    this._collisionBox.Right > item.collisionBox.Left &&
                    this._collisionBox.Left < item.collisionBox.Right) // Bottom
                {
                    distance.Y = 0;
                }
            }
        }

        /// <summary>
        /// collision between the current scene walls and the player
        /// </summary>
        /// <param name="distance"></param>
        protected void wallCollision(ref Vector2 distance)
        {
            if (this._collisionBox.Left + distance.X < this.currentScene.groundArea.Left) // Left
            {
                distance.X = 0;
            }
            else if (this._collisionBox.Right + distance.X > this.currentScene.groundArea.Right) // Right
            {
                distance.X = 0;
            }

            if (this._collisionBox.Top + distance.Y < this.currentScene.groundArea.Top) // Top
            {
                distance.Y = 0;
            }
            else if (this._collisionBox.Bottom + distance.Y > this.currentScene.groundArea.Bottom) // Bottom
            {
                distance.Y = 0;
            }
        }
    }
}
