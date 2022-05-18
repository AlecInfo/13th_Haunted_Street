/*
 * Author  : Marco Rodrigues, Alec Piette
 * Project : 13th Haunted Street
 * Details : Furniture class
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
    public class Furniture : GameObject
    {
        // Properties
        public Rectangle collisionBox;
        public Hull hull;
        public const float SCALE = 1;

        // Ctor
        public Furniture(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;

            this.collisionBox = new Rectangle(
                (int)this.position.X, (int)(this.position.Y + this.texture.Height / 3.5f),
                this.texture.Width, (int)(this.texture.Height - this.texture.Height / 3.5f));

            this.hull = new Hull(new Vector2(0.49f), new Vector2(-0.49f, 0.49f), new Vector2(-0.49f), new Vector2(0.49f, -0.49f))
            {
                Position = this.position,
                Scale = this.texture.Bounds.Size.ToVector2(),
                Origin = new Vector2(-0.5f)
            };
        }


        // Methods
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, null, Color.White, 0f, Vector2.Zero, SCALE, 0, 0f);
            //spriteBatch.Draw(Game1.defaultTexture, this.collisionBox, null, Color.Black * 0.5f, 0f, Vector2.Zero, 0, 0f);
        }
    }
}
