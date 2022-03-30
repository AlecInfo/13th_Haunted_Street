using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    abstract class ItemOption : ItemText
    {
        List<ItemText> listValue = new List<ItemText>();

        public ItemOption(SpriteFont font, string text) : base(font, text)
        {
                
        }

        public void Left()
        {

        }

        public void Right()
        {

        }

        public void Application()
        {

        }
    }
}
