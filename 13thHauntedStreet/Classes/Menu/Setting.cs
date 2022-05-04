/********************************
 * Project : 13th Haunted Street
 * Description : This class SaveSettings allows you to save the settings
 * 
 * Date : 13/04/2022
 * Author : Piette Alec
*******************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace _13thHauntedStreet
{
    [Serializable]
    public class Setting
    {
        #region Variables

        [XmlAttribute]
        public string id;

        [XmlAttribute]
        public string value;

        #endregion
    }
}
