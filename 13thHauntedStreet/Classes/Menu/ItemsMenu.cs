/********************************
 * Project : 13th Haunted Street
 * Description : This class ItemsMenu allows you to create menu items
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
    public class ItemsMenu : FormItem
    {
        #region Variables
        public List<FormItem> listItems = new List<FormItem>();
        #endregion

        public override void Update(GameTime gameTime, Screen screen, ref Vector2 changePosition)
        {
            // Browse all item in the list and uptade
            try
            {
                foreach (var item in this.listItems)
                {
                    item.Update(gameTime, screen, ref changePosition);
                }
            }
            catch (Exception)
            {
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Browse all item in the list and draw
            foreach (var item in this.listItems)
            {
                item.Draw(spriteBatch);
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

        /// <summary>
        /// Trasfert the dictionary parameters for saveing it
        /// </summary>
        /// <param name="parameters"></param>
        public override void ConstructParameterList(ref Dictionary<string, string> parameters)
        {
            foreach (var item in this.listItems)
            {
                item.ConstructParameterList(ref parameters);
            }
        }
    }
}
