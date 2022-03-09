using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{ 
    class MainMenu
    {
        // Varriables
        private static MainMenu _instance;

        private Texture2D _background;
        private Vector2 _backgroundPosition = Vector2.Zero;
        private float _backgroudScale = 4.32f;

        private bool _animationStarted = false;
        private bool _isOnTheLeftWall = true;

        private float _currentTime;
        private const float _TIMERTICK = 1f;



        // Ctor
        public MainMenu(Texture2D background)
        {
            this._background = background;
        }

        public static MainMenu Instence(Texture2D background)
        {
            if (_instance == null)
            {
                _instance = new MainMenu(background);
            }
            return _instance;
        }

        // Methods
        public void LoadContent()
        {

        }

        public void Update(GameTime gameTime, Screen screen)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                this._animationStarted = true;
            }

            if (_animationStarted)
            {
                this.PlayLateralAnimation(gameTime, screen);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._background, this._backgroundPosition, null, Color.White, 0f, Vector2.Zero,this._backgroudScale, SpriteEffects.None, 0f);
        }

        private void PlayLateralAnimation(GameTime gameTime, Screen screen)
        {
            this._currentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this._currentTime >= _TIMERTICK)
            {
                if (_isOnTheLeftWall)
                {
                    float finalPosition = screen.OriginalScreenSize.X - (this._background.Width * this._backgroudScale);

                    if (this._backgroundPosition.X > finalPosition)
                    {
                        this._backgroundPosition.X -= 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1;
                        
                        if (this._backgroundPosition.X <= finalPosition)
                            this._backgroundPosition.X = finalPosition;
                    }
                    else
                    {
                        this._animationStarted = false;
                        this._isOnTheLeftWall = false;
                    }
                }
                else
                {
                    if (this._backgroundPosition.X < 0)
                    {
                        this._backgroundPosition.X += 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        if (this._backgroundPosition.X >= 0)
                            this._backgroundPosition.X = 0;
                    }
                    else
                    {
                        this._animationStarted = false;
                        this._isOnTheLeftWall = true;
                    }
                }

                this._currentTime -= _TIMERTICK;
            }
        }
    }
}
