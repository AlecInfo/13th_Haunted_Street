/*
 * Author  : Marco Rodrigues, Alec Piette
 * Project : 13th Haunted Street
 * Details : Player abstract class
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;

namespace _13thHauntedStreet
{
    public abstract class Player : GameObject
    {
        // Properties
        public int id;

        public bool isObject = false;

        public Color color = Color.White;

        protected GameTime _gameTime;
        public Scene currentScene;
        public Light light;

        public Vector2 movement;
        public float scale;
        public static Rectangle collisionBox;

        protected List<Furniture> _furnitureList;

        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 100;

        public Rectangle rectangle
        {
            get
            {
                return new Rectangle(
                    this.position.ToPoint() - (this.texture.Bounds.Size.ToVector2() * this.scale / 2).ToPoint(), 
                    (this.texture.Bounds.Size.ToVector2() * this.scale).ToPoint());
            }
        }

        public List<Light> lights
        {
            get
            {
                List<Light> lights = new List<Light>();
                lights.Add(this.light);

                // add tool light if player is hunter
                if (this.GetType() == typeof(Hunter))
                {
                    lights.Add((this as Hunter).currentTool.light);
                }

                return lights;
            }
        }


        // Methods
        public abstract void Update(GameTime gameTime, List<Furniture> furnitureList, Scene scene);
        public virtual void DrawUI(SpriteBatch spriteBatch) { }

        /// <summary>
        /// Updates movement with input
        /// </summary>
        protected void ReadInput()
        {
            this.movement = Vector2.Zero;

            // X
            if (Game1.knm.isButtonPressed(Game1.input.Left))
            {
                this.movement.X += -1;
            }

            if (Game1.knm.isButtonPressed(Game1.input.Right))
            {
                this.movement.X += 1;
            }

            // Y
            if (Game1.knm.isButtonPressed(Game1.input.Up))
            {
                this.movement.Y += -1;
            }

            if (Game1.knm.isButtonPressed(Game1.input.Down))
            {
                this.movement.Y += 1;
            }

            // diagonal fix
            if (this.movement.X != 0 && this.movement.Y != 0) // if is moving diagonally
            {
                this.movement /= 1.4f;
            }
        }

        /// <summary>
        /// Updates the player position
        /// </summary>
        public abstract void UpdatePosition();

        /// <summary>
        /// plays the animation
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="animation">animation that is currently playing</param>
        /// <param name="currentTexture">texture that is currently beeing drawn</param>
        protected void PlayAnim(List<Texture2D> animation, ref Texture2D currentTexture)
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
        protected void DrawCollisionBox(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.defaultTexture, collisionBox, null, Color.White * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// collision between the player and game objects
        /// </summary>
        /// <param name="distance"></param>
        protected void ObjectCollision(ref Vector2 distance)
        {
            foreach (Furniture item in this._furnitureList)
            {
                if (collisionBox.Right + distance.X > item.collisionBox.Left &&
                    collisionBox.Left < item.collisionBox.Left &&
                    collisionBox.Bottom > item.collisionBox.Top &&
                    collisionBox.Top < item.collisionBox.Bottom) // Left
                {
                    distance.X = 0;
                }

                if (collisionBox.Left + distance.X < item.collisionBox.Right &&
                    collisionBox.Right > item.collisionBox.Right &&
                    collisionBox.Bottom > item.collisionBox.Top &&
                    collisionBox.Top < item.collisionBox.Bottom) // Right
                {
                    distance.X = 0;
                }

                if (collisionBox.Bottom + distance.Y > item.collisionBox.Top &&
                    collisionBox.Top < item.collisionBox.Top &&
                    collisionBox.Right > item.collisionBox.Left &&
                    collisionBox.Left < item.collisionBox.Right) // Top
                {
                    distance.Y = 0;
                }

                if (collisionBox.Top + distance.Y < item.collisionBox.Bottom &&
                    collisionBox.Bottom > item.collisionBox.Bottom &&
                    collisionBox.Right > item.collisionBox.Left &&
                    collisionBox.Left < item.collisionBox.Right) // Bottom
                {
                    distance.Y = 0;
                }
            }
        }

        /// <summary>
        /// collision between the current scene walls and the player
        /// </summary>
        /// <param name="distance"></param>
        protected void WallCollision(ref Vector2 distance)
        {
            if (collisionBox.Left + distance.X < this.currentScene.groundArea.Left) // Left
            {
                distance.X = 0;
            }
            else if (collisionBox.Right + distance.X > this.currentScene.groundArea.Right) // Right
            {
                distance.X = 0;
            }

            if (collisionBox.Top + distance.Y < this.currentScene.groundArea.Top) // Top
            {
                distance.Y = 0;
            }
            else if (collisionBox.Bottom + distance.Y > this.currentScene.groundArea.Bottom) // Bottom
            {
                distance.Y = 0;
            }
        }

        protected bool PlayerIsCollide(ref Vector2 distance)
        {
            foreach (Furniture item in this._furnitureList)
            {
                if (((collisionBox.Top + distance.Y >= item.collisionBox.Top && collisionBox.Top + distance.Y <= item.collisionBox.Bottom) ||
                    (collisionBox.Bottom + distance.Y >= item.collisionBox.Top && collisionBox.Bottom + distance.Y <= item.collisionBox.Bottom) ||
                    (collisionBox.Top + distance.Y  <= item.collisionBox.Top && collisionBox.Bottom + distance.Y >= item.collisionBox.Bottom)) && 
                    ((collisionBox.Right + distance.X <= item.collisionBox.Right && collisionBox.Right + distance.X >= item.collisionBox.Left) ||
                    (collisionBox.Left + distance.X >= item.collisionBox.Left && collisionBox.Left + distance.X <= item.collisionBox.Right) ||
                    (collisionBox.Left + distance.X <= item.collisionBox.Left && collisionBox.Right + distance.X >= item.collisionBox.Right)))
                {
                    return true;
                }
            }

            if (collisionBox.Left + distance.X < this.currentScene.groundArea.Left) // Left
            {
                return true;
            }
            else if (collisionBox.Right + distance.X > this.currentScene.groundArea.Right) // Right
            {
                return true;
            }

            if (collisionBox.Top + distance.Y < this.currentScene.groundArea.Top) // Top
            {
                return true;
            }
            else if (collisionBox.Bottom + distance.Y > this.currentScene.groundArea.Bottom) // Bottom
            {
                return true;
            }

            return false;
        }
    }
}
