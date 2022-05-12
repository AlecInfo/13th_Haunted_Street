using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    public class GhostAnimationManager
    {
        public List<Texture2D> animationLeft = new List<Texture2D>();
        public List<Texture2D> animationRight = new List<Texture2D>();

        public List<Texture2D> currentAnim = new List<Texture2D>();
    }
}
