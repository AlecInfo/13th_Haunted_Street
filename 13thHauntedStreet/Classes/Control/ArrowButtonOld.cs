using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class ArrowButtonOld 
    {
        private SpriteFont _font;

        private Texture2D _texture;

        private List<ButtonOld> _buttonList = new List<ButtonOld>();

        private string _state;

        private bool _didPassOnce = false;

        private Color _disableColor = Color.Gray;

        private const int ARROWSPACING = 170;

        private const int TEXTSPACING = 40;


        public string Text { get; set; }

        public float Value { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public bool EnableMode { get; set; }

        public bool State { get; set; }

        public Color PenColor { get; set; }

        public Vector2 Position { get; set; }

        public float Scale { get; set; }

        public bool Clicked { get; set; }

        public ArrowButtonOld(Texture2D texture = null, SpriteFont font = null)
        {
            this._texture = texture;

            this._font = font;

            this.PenColor = Color.Black;
        }
        /*

        public override void Update(GameTime gameTime, Screen screen, float changePosition)
        {
            this.Clicked = false;

            if (!this._didPassOnce)
            {
                Button btnLeft = new Button(this._texture, this._font)
                {
                    Position = new Vector2(this.Position.X, this.Position.Y),
                    Scale = 0.6f
                };
                // Assign the event
                btnLeft.Click += BtnLeft_Click;
                // Add the button in the list
                this._buttonList.Add(btnLeft);

                Button btnRight = new Button(this._texture, this._font)
                {
                    Position = new Vector2(this.Position.X + ARROWSPACING, this.Position.Y),
                    Effect = SpriteEffects.FlipHorizontally,
                    Scale = 0.6f
                };
                // Assign the event
                btnRight.Click += BtnRight_Click;
                // Add the button in the list
                this._buttonList.Add(btnRight);

                this._didPassOnce = true;
            }



            foreach (var item in this._buttonList)
            {
                item.Update(gameTime, screen);

                item.Position = new Vector2(item.Position.X + changePosition, this.Position.Y);
            }

            if (this.EnableMode)
            {
                if (this.State)
                {
                    this._state = "Enable";
                }
                else
                {
                    this._state = "Disable";
                }
            }

            ColorButton(this.Value, this.State);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(this._font, this.Text, new Vector2(this.Position.X - (this._font.MeasureString(this.Text).X + TEXTSPACING), this.Position.Y), this.PenColour, 0f, Vector2.Zero, 0.60f, SpriteEffects.None, 1f);
            
        
            spriteBatch.DrawString(this._font, this.Text, new Vector2(this.Position.X - (this._font.MeasureString(this.Text).X * this.Scale) - TEXTSPACING, this.Position.Y), this.PenColor, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 1f);
        
            if (EnableMode)
            {
                spriteBatch.DrawString(this._font, this._state, new Vector2(this.Position.X + (ARROWSPACING + this._buttonList[1].Rectangle.Width) / 2 - this._font.MeasureString(this._state).X * this.Scale / 2, this.Position.Y), this.PenColor, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 1f);
            }
            else
            {
                spriteBatch.DrawString(this._font, this.Value.ToString(), new Vector2(this.Position.X + (ARROWSPACING + this._buttonList[1].Rectangle.Width) / 2 - this._font.MeasureString(this.Value.ToString()).X * this.Scale / 2, this.Position.Y), this.PenColor, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 1f);
            }

            foreach (var item in this._buttonList)
            {
                item.Draw(spriteBatch);
            }
        }

        private void ChangeTheFieldValue(float numberValue, bool state)
        {
            if (this.EnableMode)
            {
                if (state && numberValue == -1)
                {
                    this.State = false;
                }
                else if (!state && numberValue == 1)
                {
                    this.State = true; 
                }
            }
            else
            {
                this.Value += numberValue;

                if (this.Value <= this.MinValue)
                {
                    this.Value = this.MinValue;
                }
                else if (this.Value >= this.MaxValue)
                {
                    this.Value = this.MaxValue;
                }

            }
        }

        private void ColorButton(float value, bool state)
        {
            foreach (var item in this._buttonList)
            {
                item.MaxValueReached = false;
            }

            if (value <= this.MinValue && !this.EnableMode)
            {
                this._buttonList[0].ButtonColor = this._disableColor;
                this._buttonList[0].MaxValueReached = true;
            }
            else if (value >= this.MaxValue && !this.EnableMode)
            {
                this._buttonList[1].ButtonColor = this._disableColor;
                this._buttonList[1].MaxValueReached = true;
            }

            if (this.EnableMode)
            {
                if (state)
                {
                    this._buttonList[1].ButtonColor = this._disableColor;
                    this._buttonList[1].MaxValueReached = true;
                }
                else
                {
                    this._buttonList[0].ButtonColor = this._disableColor;
                    this._buttonList[0].MaxValueReached = true;
                }
            }
        }


        private void BtnLeft_Click(object sender, EventArgs e)
        {
            this.Clicked = true;

            this.ChangeTheFieldValue(-1, this.State);
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            this.Clicked = true;

            this.ChangeTheFieldValue(1, this.State);
        }*/
    }
}
