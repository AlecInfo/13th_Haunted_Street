/********************************
 * Project : 13th Haunted Street
 * Description : This class ItemText allows you to create text 
 *              
 * Date : 13/04/2022
 * Author : Piette Alec
*******************************/

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
        // Ctor
        public ItemText(SpriteFont font, string text)
        {
            _font = font;

            this.FontColor = Color.Black;

            this.Scale = 1f;

            this.Text = text;
        }

        public override void Update(GameTime gameTime, Screen screen, ref Vector2 changePosition)
        {
            // Change text position for the menu animation
            Vector2 newPosition = new Vector2(this.Position.X + changePosition.X, this.Position.Y + changePosition.Y);
            this.Position = newPosition;
        }
    }
}
