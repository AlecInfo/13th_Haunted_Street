using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    public class ItemsMenu : FormItem
    {
        #region Variables
        public List<FormItem> listItems = new List<FormItem>();
        #endregion

        public override void Update(GameTime gameTime, Screen screen, ref Vector2 changePosition)
        {
            // Browse all item in the list and uptade
            foreach (var item in this.listItems)
            {
                item.Update(gameTime, screen, ref changePosition);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Browse all item in the list and draw
            foreach (var item in this.listItems)
            {
                item.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Add item in to the list of item menu
        /// </summary>
        /// <param name="newItem"></param>
        public void Add(FormItem newItem)
        {
            listItems.Add(newItem);
        }

        /// <summary>
        /// delete an item in the list
        /// </summary>
        /// <param name="removeItem"></param>
        public void Remove(FormItem removeItem)
        {
            // Browse the items in the list
            for (int i = listItems.Count - 1; i >= 0; i--)
            {
                // Checked if the item removed at the same to the item list
                if (listItems[i] == removeItem)
                {
                    // Delete the item
                    listItems.RemoveAt(i);
                }
            }
        }
    }
}
