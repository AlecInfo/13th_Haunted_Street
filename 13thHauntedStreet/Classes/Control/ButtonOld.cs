using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace _13thHauntedStreet
{
    public class ButtonOld //: FormItem
    {
        #region Fields

        private MouseState _currentMouse;
        private MouseState _previusMouse;

        private SpriteFont _font;

        private bool _iSHovering;


        private Texture2D _texture;

        private Texture2D _defaultTexture;

        private Rectangle _rectangle;

        private bool _didPassOnce = false;

        #endregion

        #region Proporties

        public event EventHandler Click;

        public bool Clicked { get; set; }

        public bool MaxValueReached { get; set; }

        public bool IsSelected { get; set; }

        public Color PenColor { get; set; }

        public Color ButtonColor { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle Rectangle
        {
            get => _rectangle;
            set => _rectangle = value;
        }

        public string Text { get; set; }

        public SpriteEffects Effect { get; set; }

        public float Scale { get; set; }

        #endregion

        #region Methods

        public ButtonOld(Texture2D texture, SpriteFont font)
        {
            // Create a rectangle texture
            this._defaultTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            this._defaultTexture.SetData(new Color[] { Color.White });


            this._texture = texture;

            this._font = font;

            this.PenColor = Color.Black;

            this.Effect = SpriteEffects.None;

            this.Scale = 1f;

            this.MaxValueReached = false;

            this.IsSelected = false;
        }
        /*

        public override void Update(GameTime gameTime, Screen screen, float changePosition)
        {
            if (string.IsNullOrEmpty(this.Text))
            { 
                this.Rectangle = new Rectangle((int)(this.Position.X), (int)this.Position.Y, (int)(this._texture.Width * this.Scale), (int)(this._texture.Height * this.Scale));
            }
            else 
            {
                this.ButtonColor = Color.White * 0f;
                this.Rectangle = new Rectangle((int)(Position.X), (int)Position.Y, (int)(_font.MeasureString(this.Text).X * this.Scale), (int)(_font.MeasureString(this.Text).Y * this.Scale));
            }

            this._didPassOnce = true;

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


            if (this._iSHovering && !string.IsNullOrEmpty(this.Text))
            {
                this.PenColor = Color.White;
            }
            else
            {
                this.PenColor = Color.LightGray;
            }


            if (this._iSHovering && string.IsNullOrEmpty(this.Text) && !MaxValueReached)
            {
                this.ButtonColor = Color.LightGray;
            }
            else if (!this._iSHovering && string.IsNullOrEmpty(this.Text) && !MaxValueReached)
            {
                this.ButtonColor = Color.White;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._texture, this.Rectangle, null, this.ButtonColor, 0f, Vector2.Zero, this.Effect, 0f);

            if (this.IsSelected)
            {
                this.ButtonColor = Color.White;
                Rectangle rectangle = new Rectangle((int)(Position.X - 20), (int)Position.X, 10, 10);
                rectangle.Y = (int)(Position.Y + (this._font.MeasureString(this.Text).Y * this.Scale) / 2 - rectangle.Height / 2);

                spriteBatch.Draw(this._defaultTexture, rectangle, null, this.PenColor, 0f, Vector2.Zero, this.Effect, 0f);
            }

            if (!string.IsNullOrEmpty(this.Text))
            {
                float x = (this.Rectangle.X + (this.Rectangle.Width / 2)) - (this._font.MeasureString(Text).X * this.Scale / 2);
                float y = (this.Rectangle.Y + (this.Rectangle.Height / 2)) - (this._font.MeasureString(Text).Y * this.Scale / 2);

                spriteBatch.DrawString(this._font, this.Text, new Vector2(x, y), this.PenColor, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 1f);
            }
        }*/
        #endregion
    }
}
