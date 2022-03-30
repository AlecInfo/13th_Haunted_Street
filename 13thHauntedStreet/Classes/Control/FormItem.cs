using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    abstract class FormItem
    {
        public string Text { get; set; }

        public Vector2 Position { get; set; }

        public Color FontColor { get; set; }

        public float Scale { get; set; }

        protected SpriteFont _font;

        protected float _currentTime;

        protected const int TEXTSPACING = 40;

        protected const float _TIMERTICK = 1f;


        public virtual void Update(GameTime gameTime, Screen screen, ref Vector2 changePosition) 
        {
            Console.WriteLine("update");
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Console.WriteLine("draw");
            spriteBatch.DrawString(this._font, this.Text, new Vector2(this.Position.X - (this._font.MeasureString(this.Text).X * this.Scale) - TEXTSPACING, this.Position.Y), this.FontColor, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 1f);
        }
    }
}
