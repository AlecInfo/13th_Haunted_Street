/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Furniture class
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
    class Furniture : GameObject
    {
        // Properties
        public Rectangle collisionBox;


        // Ctor
        public Furniture(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;

            this.collisionBox = new Rectangle(
                (int)this.position.X, (int)this.position.Y + this.texture.Height / 4,
                this.texture.Width, this.texture.Height - this.texture.Height / 4);
        }


        // Methods
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, null, Color.White, 0f, Vector2.Zero, 1f, 0, 0f);

            Texture2D defaultTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            defaultTexture.SetData(new Color[] { Color.White });
            //spriteBatch.Draw(defaultTexture, this.collisionBox, null, Color.Black * 0.5f, 0f, Vector2.Zero, 0, 0f);
        }
    }
}
