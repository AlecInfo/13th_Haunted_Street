/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Singleton class that manages player input
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    public sealed class Input
    {
        private Input() { }
        private static Input _instance;

        public static Input GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Input();
            }
            return _instance;
        }


        // Directional
        public Keys Up;
        public Keys Down;
        public Keys Left;
        public Keys Right;

        // Uses
        public Keys Use1;
        public Keys Use2;
        public Keys ItemUp;
        public Keys ItemDown;
    }
}