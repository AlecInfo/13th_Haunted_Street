using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class ItemsMenu : FormItem
    {
        public List<FormItem> listItems = new List<FormItem>();

        public override void Update(GameTime gameTime, Screen screen, ref Vector2 changePosition)
        {
            foreach (var item in this.listItems)
            {
                item.Update(gameTime, screen, ref changePosition);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var item in this.listItems)
            {
                item.Draw(gameTime, spriteBatch);
            }
        }

        public void Add(FormItem newItem)
        {
            listItems.Add(newItem);
        }

        public void Remove(FormItem removeItem)
        {
            for (int i = listItems.Count - 1; i >= 0; i--)
            {
                if (listItems[i] == removeItem)
                {
                    listItems.RemoveAt(i);
                }
            }
        }
    }
}
