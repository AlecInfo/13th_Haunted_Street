/********************************
 * Project : 13th Haunted Street
 * Description : 
 * Date : 02/05/2022
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
    class ItemLineOption : ItemOption
    {
        public ItemLineOption(SpriteFont font, List<string> values) : base(font, null)
        {
            _font = font;

            this.listValue = values;
        }

        /// <summary>
        /// lower the list id
        /// </summary>
        public void Left()
        {
            if (Id <= 0)
            {
                Id = 0;
            }
            else
            {
                Id -= 1;
            }
        }

        /// <summary>
        /// Increase the list id
        /// </summary>
        public void Right()
        {
            if (Id >= listValue.Count - 1)
            {
                Id = listValue.Count - 1;
            }
            else
            {
                Id += 1;
            }
        }

        /// <summary>
        /// Return true if id is at max
        /// </summary>
        /// <returns></returns>
        public bool IsRight()
        {
            return Id >= listValue.Count - 1;
        }

        /// <summary>
        /// Return true if id is at min
        /// </summary>
        /// <returns></returns>
        public bool IsLeft()
        {
            return Id <= 0;
        }
    }
}
