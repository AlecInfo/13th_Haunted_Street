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

        // Methods
        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

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
    }
}
