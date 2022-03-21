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
        #region Fields

        private static MainMenu _instance;

        private MainMenuSetting _mainMenuSettings = new MainMenuSetting();

        private SpriteFont _font;

        private Texture2D _background;

        private List<Button> _leftMenuButtonList = new List<Button>();
        private List<Button> _rightMenuButtonList = new List<Button>();
        private Texture2D _buttonTexture;

        private Vector2 _backgroundPosition = Vector2.Zero;
        private float _backgroudScale = 4.32f;
        private Vector2 _titlePosition = new Vector2(Game1.graphics.PreferredBackBufferWidth / 7.5f, Game1.graphics.PreferredBackBufferHeight / 2.6f);

        public bool animationStarted = false;
        private bool _isOnTheLeftWall = true;

        private float _currentTime;
        private const float _TIMERTICK = 1f;

        public bool quitedTheGame = false;

        private bool _isAlreadyBack = false;

        public enum _RightMenuSelected
        {
            None,
            NewGame,
            JoinGame,
            Settings,
            Quit
        }
        public _RightMenuSelected Option { get; set; }

        #endregion



        #region Methods

        // Ctor
        public MainMenu(Texture2D background, SpriteFont font)
        {
            this._background = background;
            this._font = font;
        }

        // singleton
        public static MainMenu Instence(Texture2D background, SpriteFont font)
        {
            if (_instance == null)
            {
                _instance = new MainMenu(background, font);
            }
            return _instance;
        }

        // Methods
        public void LoadContent(Screen screen, Texture2D arrowButton)
        {
            // Create a rectangle texture
            _buttonTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            _buttonTexture.SetData(new Color[] { Color.White });

            #region Create Button

            // Initialize the new game button
            Button btnNewGame = new Button(_buttonTexture, _font)
            {
                Text = "New Game",
                Position = new Vector2(screen.OriginalScreenSize.X / 7, screen.OriginalScreenSize.Y / 1.8f),
                PenColor = Color.White
            };
            // Assign the event
            btnNewGame.Click += BtnNewGame_Click;
            // Add the button in the list
            _leftMenuButtonList.Add(btnNewGame);


            // Initialize the settings button
            Button btnSettings = new Button(_buttonTexture, _font)
            {
                Text = "Settings",
                Position = GetButtonPosition(),
                PenColor = Color.White
            };
            // Assign the event
            btnSettings.Click += BtnSettings_Click;
            // Add the button in the list
            _leftMenuButtonList.Add(btnSettings);


            // Initialize the Quit button
            Button btnQuit = new Button(_buttonTexture, _font)
            {
                Text = "Quit",
                Position = GetButtonPosition(),
                PenColor = Color.White
            };
            // Assign the event
            btnQuit.Click += BtnQuit_Click;
            // Add the button in the list
            _leftMenuButtonList.Add(btnQuit);

            // Initialize the Join game button
            Button btnJoinGame = new Button(_buttonTexture, _font)
            {
                Text = "Join",
                Position = new Vector2(screen.OriginalScreenSize.X / 3.05f, screen.OriginalScreenSize.Y / 1.46f),
                PenColor = Color.White
            };
            // Assign the event
            btnJoinGame.Click += BtnJoinGame_Click;
            // Add the button in the list
            _leftMenuButtonList.Add(btnJoinGame);

            #endregion

            this._mainMenuSettings.Load(screen, this._font, arrowButton);
        }


        public void Update(GameTime gameTime, Screen screen)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                this.animationStarted = true;
            }

            // Updates all buttons that are in the list 
            foreach (Button item in _leftMenuButtonList)
            {
                item.Update(gameTime, screen);
            }

            this._mainMenuSettings.Update(gameTime, screen);

            if (this._mainMenuSettings.Back)
            {
                this.animationStarted = this._mainMenuSettings.Back;
                this._mainMenuSettings.Back = false;
            }

            // Start the animation 
            if (animationStarted)
            {
                this.PlayLateralAnimation(gameTime, screen);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Background
            spriteBatch.Draw(this._background, this._backgroundPosition, null, Color.White, 0f, Vector2.Zero,this._backgroudScale, SpriteEffects.None, 0f);

            // Title
            spriteBatch.DrawString(_font, "13th Haunted St", this._titlePosition, Color.White);

            // All button
            foreach (Button item in _leftMenuButtonList)
            {
                item.Draw(spriteBatch);
            }

            if (this.Option == _RightMenuSelected.Settings)
            {
                _mainMenuSettings.Draw(spriteBatch);
            }
        }

        #region Menu Animation

        /// <summary>
        /// Move the position of the background to create an animation between the two walls
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="screen"></param>
        private void PlayLateralAnimation(GameTime gameTime, Screen screen)
        {
            // Get the game time in milliseconds
            this._currentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // When the game time has reached timer tick
            if (this._currentTime >= _TIMERTICK)
            {   
                if (_isOnTheLeftWall)
                {
                    float finalPosition = screen.OriginalScreenSize.X - (this._background.Width * this._backgroudScale);

                    // Move the menu to the left
                    if (this._backgroundPosition.X > finalPosition)
                    {
                        // Move the background
                        this._backgroundPosition.X -= 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        // Move the Title
                        this._titlePosition.X -= 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        // Move all buttons 
                        foreach (Button item in _leftMenuButtonList)
                        {
                            item.Position = new Vector2(item.Position.X - (1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds), item.Position.Y);
                        }

                        this._mainMenuSettings.ChangePosition = -1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        // To make sure the menu position doesn't move too far
                        if (this._backgroundPosition.X <= finalPosition)
                            this._backgroundPosition.X = finalPosition;
                    }
                    else
                    {
                        // Reset the value 
                        this.animationStarted = false;
                        this._isOnTheLeftWall = false;
                        this._mainMenuSettings.ChangePosition = 0;
                    }
                }
                else
                {
                    // Move the menu to the right
                    if (this._backgroundPosition.X < 0)
                    {
                        // Move the background
                        this._backgroundPosition.X += 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        // Move the title
                        this._titlePosition.X += 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                        // Move all button
                        foreach (Button item in _leftMenuButtonList)
                        {
                            item.Position = new Vector2(item.Position.X + (1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds), item.Position.Y);
                        }

                        this._mainMenuSettings.ChangePosition = 1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                        // To make sure the menu position doesn't move too far
                        if (this._backgroundPosition.X >= 0)
                            this._backgroundPosition.X = 0;
                    }
                    else
                    {
                        // Reset the value
                        this.animationStarted = false;
                        this._isOnTheLeftWall = true;
                        this._mainMenuSettings.ChangePosition = 0;
                        this.Option = _RightMenuSelected.None;
                    }
                }

                this._currentTime -= _TIMERTICK;
            }
        }

        #endregion


        #region Button Event and more

        /// <summary>
        /// Retrieves the position of the previous button in the list and adapts this position
        /// </summary>
        /// <returns></returns>
        private Vector2 GetButtonPosition()
        {
            return new Vector2(
                Game1.graphics.PreferredBackBufferWidth / 7, 
                _leftMenuButtonList[_leftMenuButtonList.Count - 1].Position.Y + _font.MeasureString(_leftMenuButtonList[_leftMenuButtonList.Count - 1].Text).Y * 1.5f
                );
        }


        /// <summary>
        /// New game button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            this.animationStarted = true;
            this.Option = _RightMenuSelected.NewGame;
        }

        /// <summary>
        /// Settings button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSettings_Click(object sender, EventArgs e)
        {
            this.animationStarted = true;
            this.Option = _RightMenuSelected.Settings;
        }

        /// <summary>
        /// Join the game button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnJoinGame_Click(object sender, EventArgs e)
        {
            this.animationStarted = true;
            this.Option = _RightMenuSelected.JoinGame;
        }

        /// <summary>
        /// Quit button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnQuit_Click(object sender, EventArgs e)
        {
            this.quitedTheGame = true;
        }

        #endregion

        #endregion
    }
}
