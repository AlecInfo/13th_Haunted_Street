/*
 * Author : David Vieira Luis
 * Project : 13th Haunted Street
 * Details : Serializable class that converts the player data to send to the server after
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
namespace _13thHauntedStreet
{
    [Serializable()]
    public class dataPlayer
    {
        #region Attributs

        private string position;
        private int id;
        private string playerType;
        private string _textureName;
        private bool _isObject;

        #endregion

        #region Properties
        [XmlElement]
        public string Position
        {
            get { return position; }   // get method
            set { position = value; }
        }

        [XmlAttribute]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [XmlElement]
        public string PlayerType {
            get { return playerType; }
            set { playerType = value; } 
        }
        [XmlElement]
        public string TextureName
        {
            get { return _textureName; }
            set { _textureName = value; }
        }

        [XmlElement]
        public bool IsObject
        {
            get { return _isObject; }
            set { _isObject = value; }
        }
        #endregion


        public dataPlayer()
        {
        }
    }
}