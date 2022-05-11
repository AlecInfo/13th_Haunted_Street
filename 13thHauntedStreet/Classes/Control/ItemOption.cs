/********************************
 * Project : 13th Haunted Street
 * Description : This class ItemOption allows you to change the id 
 *               and this allows to change the value in a textbox
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
    abstract class ItemOption : ItemText
    {
        #region Variables
        public List<string> listValue = new List<string>();

        private string _name;
        public string Name { get => _name; set => _name = value; }

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
        /// Return the value according to id
        /// </summary>
        /// <returns></returns>
        public override string GetValue()
        {
            return listValue[Id];
        }

        public override void ConstructParameterList(ref Dictionary<string, string> parameters)
        {
            parameters.Add(Name, GetValue());
        }
    }
}
