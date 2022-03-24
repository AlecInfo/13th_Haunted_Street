﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class ItemButton : FormItem
    {

        private Texture2D _texture;

        private Texture2D _defaultTexture;

        private MouseState _currentMouse;

        private MouseState _previusMouse;

        public Action Click;

        //zzz ajouter
        public ItemOption ShowValue;

        public SpriteEffects Effect { get; set; }

        /// <summary>
        /// permet d'afficher un carré à côté du bouton
        /// </summary>
        public bool IsSelected { get; set; }

        public Rectangle Rectangle { get; set; }

        public Color ButtonColor { get; set; }


        public ItemButton(Texture2D texture, SpriteFont font, Action eventButton)
        {
            // Create a rectangle texture
            this._defaultTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            this._defaultTexture.SetData(new Color[] { Color.White });
            this.Click = eventButton;

            this._texture = texture;

            this._font = font;

            this.FontColor = Color.Black;

            this.Effect = SpriteEffects.None;

            this.Scale = 1f;

            this.IsSelected = false;
        }

        public override void Update(GameTime gameTime, Screen screen,ref Vector2 changePosition)
        {
            Vector2 newPosition = new Vector2(this.Position.X + changePosition.X, this.Position.Y + changePosition.Y);
            this.Position = newPosition;

            if (string.IsNullOrEmpty(this.Text))
            {
                this.Rectangle = new Rectangle((int)(this.Position.X), (int)this.Position.Y, (int)(this._texture.Width * this.Scale), (int)(this._texture.Height * this.Scale));
            }
            else
            {
                this.ButtonColor = Color.White * 0f;
                this.Rectangle = new Rectangle((int)(Position.X), (int)Position.Y, (int)(_font.MeasureString(this.Text).X * this.Scale), (int)(_font.MeasureString(this.Text).Y * this.Scale));
            }

            this._previusMouse = this._currentMouse;
            this._currentMouse = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle((int)(this._currentMouse.X / screen.Scale), (int)(this._currentMouse.Y / screen.Scale), 1, 1);

            
            bool isHovering = false;

            if (mouseRectangle.Intersects(this.Rectangle))
            {
                isHovering = true;

                if (this._currentMouse.LeftButton == ButtonState.Released && this._previusMouse.LeftButton == ButtonState.Pressed)
                {
                    this.Click();
                }
            }


            if (isHovering && !string.IsNullOrEmpty(this.Text))
            {
                this.FontColor = Color.White;
            }
            else
            {
                this.FontColor = Color.LightGray;
            }


            if (isHovering && string.IsNullOrEmpty(this.Text) && !MaxValueReached())
            {
                this.ButtonColor = Color.LightGray;
            }
            else if (!isHovering && string.IsNullOrEmpty(this.Text) && !MaxValueReached())
            {
                this.ButtonColor = Color.White;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._texture, this.Rectangle, null, this.ButtonColor, 0f, Vector2.Zero, this.Effect, 0f);

            if (this.IsSelected)
            {
                this.ButtonColor = Color.White;
                Rectangle rectangle = new Rectangle((int)(Position.X - 20), (int)Position.X, 10, 10);
                rectangle.Y = (int)(Position.Y + (this._font.MeasureString(this.Text).Y * this.Scale) / 2 - rectangle.Height / 2);

                spriteBatch.Draw(this._defaultTexture, rectangle, null, this.FontColor, 0f, Vector2.Zero, this.Effect, 0f);
            }

            if (!string.IsNullOrEmpty(this.Text))
            {
                float x = (this.Rectangle.X + (this.Rectangle.Width / 2)) - (this._font.MeasureString(Text).X * this.Scale / 2);
                float y = (this.Rectangle.Y + (this.Rectangle.Height / 2)) - (this._font.MeasureString(Text).Y * this.Scale / 2);

                spriteBatch.DrawString(this._font, this.Text, new Vector2(x, y), this.FontColor, 0f, Vector2.Zero, this.Scale, SpriteEffects.None, 1f);
            }
        }

        /// <summary>
        /// Si le bouton doit être acctif ou pas
        /// </summary>
        /// <returns>bool</returns>
        private bool MaxValueReached()
        {
            return false;
        }
    }
}