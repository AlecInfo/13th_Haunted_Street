using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class ArrowButton
    {
        private SpriteFont _font;

        private List<Button> _buttonList = new List<Button>();

        private const int TEXTSPACING = 10;

        private const int ARROWSPACING = 30;

        public string Text { get; set; }

        public float Value { get; set; }

        public bool EnableMode { get; set; }

        public bool State { get; set; }

        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        public ArrowButton(Texture2D texture, SpriteFont font)
        {
            this._font = font;

            this._buttonList.Add(
                new Button(texture, font)
                {
                    Position = this.Position
                }
                );

            this._buttonList.Add(
                new Button(texture, font)
                {
                    Position = new Vector2(this.Position.X + ARROWSPACING, this.Position.Y),
                    Effect = SpriteEffects.FlipVertically
                }
                );

            this.PenColour = Color.Black;
        }

        public void Update(GameTime gameTime)
        {
            if (this._buttonList[0].Clicked)
            {
                this.ChangeTheFieldValue(-1);
            }
            else if (this._buttonList[this._buttonList.Count -1].Clicked)
            {
                this.ChangeTheFieldValue(1);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this._font, this.Text, new Vector2(this.Position.X - (this._font.MeasureString(this.Text).X + TEXTSPACING), this.Position.Y), this.PenColour);

            _buttonList[0].Draw(spriteBatch);

            spriteBatch.DrawString(this._font, this.Text, new Vector2(this.Position.X + ARROWSPACING / 2, this.Position.Y), this.PenColour);

            _buttonList[_buttonList.Count -1].Draw(spriteBatch);
        }

        private void ChangeTheFieldValue(float numberValue)
        {
            if (this.EnableMode)
            {
                this.EnableMode = !this.EnableMode;
            }
            else
            {
                this.Value += numberValue;
            }
        }
    }
}
