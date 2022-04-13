using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class ItemButtonOption : ItemButton
    {
        #region Variables
        Func<bool> enabledButton = () => { return true; };
     
        private Color _disableColor = Color.Gray;
        #endregion

        // Ctor
        public ItemButtonOption(Texture2D texture, SpriteFont font, Action eventButton, Func<bool> enabledButton) : base(texture, font, eventButton)
        {
            this.enabledButton = enabledButton;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Change the color of the button if the value is at maximum
            if (!enabledButton())
            {
                this.ButtonColor = Color.White;

                //zzz
                // Faire en sorte que quand on passe la souris par dessu le bouton selui si change de couleur
            }
            else
            {
                this.ButtonColor = _disableColor;
            }
            base.Draw(gameTime, spriteBatch);
        }
    }
}
