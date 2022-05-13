/********************************
 * Project : 13th Haunted Street
 * Description : This class FormItem allows you to list all 
 *               the items of a form such as buttons, text , ...
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
    public enum AlignItem
    {
        Left,
        Center,
        Right
    }

    public abstract class FormItem
    {
        #region Variables 
        public string Text { get; set; }

        public Vector2 Position { get; set; }

        public Color FontColor { get; set; }

        public float Scale { get; set; }

        protected static SpriteFont _font;

        protected float _currentTime;

        protected const int TEXTSPACING = 40;

        protected const float _TIMERTICK = 1f;

        public AlignItem align = AlignItem.Left;

        #endregion

        public virtual void Update(GameTime gameTime, Screen screen, ref Vector2 changePosition) 
        {
            Console.WriteLine("update");
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Console.WriteLine("draw");

            // Draw the text 
            string temp = this.GetValue();
            spriteBatch.DrawString(_font, this.GetValue(), new Vector2(Alignement(), this.Position.Y), this.FontColor, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// This method allows to create alignments (left, center and right) for text
        /// </summary>
        /// <returns></returns>
        public virtual float Alignement() 
        {
            float x = 0;

            // Return the text position
            switch (align)
            {
                // Align Center
                case AlignItem.Center:
                    x = this.Position.X - _font.MeasureString(this.GetValue()).X * this.Scale / 2;
                    break;

                // Align Right
                case AlignItem.Right:
                    x = this.Position.X - _font.MeasureString(this.GetValue()).X * this.Scale;
                    break;

                // Align Left
                default:
                    x = this.Position.X;
                    break;
            }

            return x;
        }

        /// <summary>
        /// This method allows to get the value of the text
        /// </summary>
        /// <returns></returns>
        public virtual string GetValue()
        {
            return this.Text;
        }

        /// <summary>
        /// This method allows to create a transfert the parametters in to the list fort save in the xml file
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void ConstructParameterList(ref Dictionary<string, string> parameters)
        {
            
        }
    }
}
