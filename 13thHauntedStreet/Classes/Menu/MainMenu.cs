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

        private SpriteFont _font;

        private List<Button> _leftMenuButtonList = new List<Button>();
        private List<Button> _rightMenuButtonList = new List<Button>();
        private Texture2D _buttonTexture;

        private Texture2D _background;
        private Vector2 _backgroundPosition = Vector2.Zero;
        private float _backgroudScale = 4.32f;
        private Vector2 _titlePosition = new Vector2(Game1.graphics.PreferredBackBufferWidth / 7.5f, Game1.graphics.PreferredBackBufferHeight / 2.6f);

        private bool _animationStarted = false;
        private bool _isOnTheLeftWall = true;

        private float _currentTime;
        private const float _TIMERTICK = 1f;

        private enum _RightMenuSelected
        {
            None,
            NewGame,
            JoinGame,
            Settings,
            Quit
        }
        private _RightMenuSelected Option { get; set; }

        // Ctor
        public MainMenu(Texture2D background, SpriteFont font)
        {
            this._background = background;
            this._font = font;
        }

        public static MainMenu Instence(Texture2D background, SpriteFont font)
        {
            if (_instance == null)
            {
                _instance = new MainMenu(background, font);
            }
            return _instance;
        }

        // Methods
        public void LoadContent(Screen screen)
        {
            _buttonTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            _buttonTexture.SetData(new Color[] { Color.White });

            #region Create Button

            Button btnNewGame = new Button(_buttonTexture, _font)
            {
                Text = "New Game",
                Position = new Vector2(screen.OriginalScreenSize.X / 7, screen.OriginalScreenSize.Y / 1.8f),
                PenColour = Color.White
            };
            btnNewGame.Click += BtnNewGame_Click;
            _leftMenuButtonList.Add(btnNewGame);

            Button btnSettings = new Button(_buttonTexture, _font)
            {
                Text = "Settings",
                Position = GetButtonPosition(),
                PenColour = Color.White
            };
            btnSettings.Click += BtnSettings_Click;
            _leftMenuButtonList.Add(btnSettings);

            Button btnQuit = new Button(_buttonTexture, _font)
            {
                Text = "Quit",
                Position = GetButtonPosition(),
                PenColour = Color.White
            };
            btnQuit.Click += BtnQuit_Click;
            _leftMenuButtonList.Add(btnQuit);

            Button btnJoinGame = new Button(_buttonTexture, _font)
            {
                Text = "Join",
                Position = new Vector2(screen.OriginalScreenSize.X / 3.05f, screen.OriginalScreenSize.Y / 1.46f),
                PenColour = Color.White
            };
            btnJoinGame.Click += BtnJoinGame_Click;
            _leftMenuButtonList.Add(btnJoinGame);

            #endregion
        }


        public void Update(GameTime gameTime, Screen screen)
        {
            

            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                this._animationStarted = true;
            }

            foreach (Button item in _leftMenuButtonList)
            {
                item.Update(gameTime, screen);
            }

            if (_animationStarted)
            {
                this.PlayLateralAnimation(gameTime, screen);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._background, this._backgroundPosition, null, Color.White, 0f, Vector2.Zero,this._backgroudScale, SpriteEffects.None, 0f);

            spriteBatch.DrawString(_font, "13th Haunted St", this._titlePosition, Color.White);

            foreach (Button item in _leftMenuButtonList)
            {
                item.Draw(spriteBatch);
            }
        }

        #region Menu Animation
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
                        this._backgroundPosition.X -= 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        this._titlePosition.X -= 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        foreach  (Button item in _leftMenuButtonList)
                        {
                            item.Position = new Vector2(item.Position.X - (1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds), item.Position.Y);
                        }
                        
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
                        this._titlePosition.X += 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        foreach (Button item in _leftMenuButtonList)
                        {
                            item.Position = new Vector2(item.Position.X + (1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds), item.Position.Y);
                        }

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
        #endregion


        #region Button Event and more
        private Vector2 GetButtonPosition()
        {
            return new Vector2(
                Game1.graphics.PreferredBackBufferWidth / 7, 
                _leftMenuButtonList[_leftMenuButtonList.Count - 1].Position.Y + _font.MeasureString(_leftMenuButtonList[_leftMenuButtonList.Count - 1].Text).Y * 1.5f
                );
        }

        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            this._animationStarted = true;
            this.Option = _RightMenuSelected.NewGame;
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            this._animationStarted = true;
            this.Option = _RightMenuSelected.Settings;

            this._rightMenuButtonList.Add(
                new Button(_buttonTexture, _font)
                {
                    Position = new Vector2()
                }
                );
        }

        private void BtnQuit_Click(object sender, EventArgs e)
        {
            this._animationStarted = true;
            this.Option = _RightMenuSelected.Quit;
        }

        private void BtnJoinGame_Click(object sender, EventArgs e)
        {
            this._animationStarted = true;
            this.Option = _RightMenuSelected.JoinGame;
        }
        #endregion
    }
}
