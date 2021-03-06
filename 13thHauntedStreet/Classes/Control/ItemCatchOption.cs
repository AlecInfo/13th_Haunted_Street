/********************************
 * Project : 13th Haunted Street
 * Description : Allows you to get the keyboard and mouse state
 * 
 * Date : 05/05/2022
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
    class ItemCatchOption : ItemOption
    {
        public ItemCatchOption(SpriteFont font, List<string> values) : base(font, null)
        {
            this.listValue = values;
        }
        
        public string Catch(bool doThis = false)
        {
            return "";
        }
    }
}
