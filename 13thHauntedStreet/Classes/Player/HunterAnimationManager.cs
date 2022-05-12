using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    public class HunterAnimationManager
    {
        public List<Texture2D> walkingLeft = new List<Texture2D>();
        public List<Texture2D> walkingRight = new List<Texture2D>();
        public List<Texture2D> walkingUp = new List<Texture2D>();
        public List<Texture2D> walkingDown = new List<Texture2D>();

        public List<Texture2D> idleLeft = new List<Texture2D>();
        public List<Texture2D> idleRight = new List<Texture2D>();
        public List<Texture2D> idleUp = new List<Texture2D>();
        public List<Texture2D> idleDown = new List<Texture2D>();


        public List<Texture2D> currentAnim = new List<Texture2D>();
    }
}
