/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Base game object Class
 * Date    : 14.03.2022
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    abstract class GameObject
    {
        // Properties
        public Vector2 position;
        public Texture2D texture;

        // Methods
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
