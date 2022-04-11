using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class ItemText : FormItem
    {
        public ItemText(SpriteFont font, string text)
        {
            this._font = font;

            this.FontColor = Color.Black;

            this.Scale = 1f;

            this.Text = text;
        }

        public override void Update(GameTime gameTime, Screen screen, ref Vector2 changePosition)
        {
            // Change text position
            Vector2 newPosition = new Vector2(this.Position.X + changePosition.X, this.Position.Y + changePosition.Y);
            this.Position = newPosition;
        }
    }
}
