/*
 * Author : David Vieira Luis
 * Project : 13th Haunted Street
 * Details : Class that receives data from the other players and draws them
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
    public class foreignPlayer : GameObject
    {
        private const float SPEED = 0.25f;
        public int _id;
        public string playerType;

        public Vector2 _Position
        {
            get { return position; }   // get method
            set { position = value; }
        }
        public Texture2D _Texture
        {
            get { return texture; }   // get method
            set { texture = value; }
        }

        public bool IsObject { get; set; }

        public foreignPlayer(Vector2 Position, Texture2D Texture, string playerType)
        {
            this._Position = Position;
            this._Texture = Texture;
            this.playerType = playerType;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!(this.texture is null))
            {
                float scale = this.playerType == typeof(Hunter).ToString() ? 3 : 1.5f;
                spriteBatch.Draw(this.texture, this.position, null, Color.White, 0f, this.texture.Bounds.Center.ToVector2(), scale, 0, 0);
            }
        }
    }
}
