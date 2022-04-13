using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class ItemOption : ItemText
    {
        #region Variables
        List<string> listValue = new List<string>();

        private int _id = 0;
        public int Id { get => _id; set => _id = value; }
        #endregion

        // Ctor
        public ItemOption(SpriteFont font, List<string> values) : base(font, "")
        {
            this._font = font;

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

        /// <summary>
        /// Return the value according to id
        /// </summary>
        /// <returns></returns>
        public override string GetValue()
        {
            return listValue[Id];
        }
    }
}
