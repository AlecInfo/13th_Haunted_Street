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

        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame = 100;



        // Methods
        public abstract void Update(GameTime gameTime);

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
        /// Updates the animation to the next frame
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
    }
}
