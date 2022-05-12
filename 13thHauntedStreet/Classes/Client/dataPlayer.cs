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
        private string texturePath;
        private int id;
        private string playerType;
        private string animName;
        private int animFrame;

        #endregion

        #region Properties
        [XmlElement]
        public string Position
        {
            get { return position; }   // get method
            set { position = value; }
        }
        [XmlElement]
        public string Texture
        {
            get { return texturePath; }   // get method
            set { texturePath = value; }
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
        public string AnimName
        {
            get { return animName; }
            set { animName = value; }
        }
        [XmlElement]
        public int AnimFrame
        {
            get { return animFrame; }
            set { animFrame = value; }
        }
        #endregion


        public dataPlayer()
        {
        }
    }
}