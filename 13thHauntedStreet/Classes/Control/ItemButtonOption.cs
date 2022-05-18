/********************************
 * Project : 13th Haunted Street
 * Description : This class ItemButtonOption allows you to change
 *               whatever details of the ItemButton class
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
    class ItemButtonOption : ItemButton
    {
        #region Variables
        Func<bool> enabledButton = () => { return true; };
     
        private Color _disableColor = Color.Gray;
        #endregion

        // Ctor
        public ItemButtonOption(Texture2D texture, SpriteFont font, Action<int, GameTime> eventButton, Func<bool> enabledButton, bool DrawImageText) : base(texture, font, eventButton, DrawImageText)
        {
            this.enabledButton = enabledButton;
        }

        public override void Draw(SpriteBatch spriteBatch)
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
            base.Draw(spriteBatch);
        }
    }
}
