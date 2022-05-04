/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Abstract Tool class
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
    abstract class Tool
    {
        // Properties
        public Texture2D icon;
        public Vector2 position;
        public Light light;


        // Methods
        public abstract void Update(GameTime gameTime, Vector2 playerPosition);

        /// <summary>
        /// Tool action
        /// </summary>
        protected abstract void Use();

        public abstract void Draw(SpriteBatch spriteBatch, Vector2 playerPosition);
    }
}
