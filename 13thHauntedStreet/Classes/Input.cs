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
        public Game1.KnMButtons Up;
        public Game1.KnMButtons Down;
        public Game1.KnMButtons Left;
        public Game1.KnMButtons Right;

        // Uses
        public Game1.KnMButtons Use1;
        public Game1.KnMButtons Use2;
        public Game1.KnMButtons ItemUp;
        public Game1.KnMButtons ItemDown;
    }
}