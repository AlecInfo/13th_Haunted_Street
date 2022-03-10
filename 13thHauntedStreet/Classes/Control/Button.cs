using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace _13thHauntedStreet
{
    public class Button
    {
        #region Fields

        private MouseState _currentMouse;

        private SpriteFont _font;

        private bool _iSHovering;

        private MouseState _previusMouse;

        private Texture2D _texture;

        private Rectangle _rectangle;

        #endregion

        #region Proporties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle Rectangle
        {
            get => _rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)_font.MeasureString(this.Text).X, (int)_font.MeasureString(this.Text).Y);
            set => _rectangle = value;
        }

        public string Text { get; set; }

        public SpriteEffects Effect { get; set; }

        #endregion

        #region Methods

        public Button(Texture2D texture, SpriteFont font)
        {
            this._texture = texture;

            this._font = font;

            this.PenColour = Color.Black;

            this.Effect = SpriteEffects.None;
        }


        public void Update(GameTime gameTime, Screen screen)
        {
            this._previusMouse = this._currentMouse;
            this._currentMouse = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle((int)(this._currentMouse.X / screen.Scale), (int)(this._currentMouse.Y / screen.Scale), 1, 1);

            this._iSHovering = false;

            if (mouseRectangle.Intersects(this.Rectangle))
            {
                this._iSHovering = true;

                if (this._currentMouse.LeftButton == ButtonState.Released && this._previusMouse.LeftButton == ButtonState.Pressed)
                {
                    this.Click?.Invoke(this, new EventArgs());
                }
            }


            if (this._iSHovering)
                this.PenColour = Color.White;
            else
                this.PenColour = Color.LightGray;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._texture, new Vector2(this.Rectangle.X, this.Rectangle.Y), null, Color.White * 0f, 0f, Vector2.Zero, new Vector2(this.Rectangle.Width, this.Rectangle.Height), this.Effect, 0f);

            if (!string.IsNullOrEmpty(this.Text))
            {
                float x = (this.Rectangle.X + (this.Rectangle.Width / 2)) - (this._font.MeasureString(Text).X / 2);
                float y = (this.Rectangle.Y + (this.Rectangle.Height / 2)) - (this._font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(this._font, this.Text, new Vector2(x, y), this.PenColour);
            }
        }

        #endregion
    }
}
