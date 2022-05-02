/********************************
 * Project : 13th Haunted Street
 * Description : This class MainMenu allows you to create the main menu
 *               of the game with button, animation, ...
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
    class MainMenu : ItemsMenu
    {
        #region Variables
        private Texture2D _background;

        public static bool animationStarted = false;

        private Vector2 _backgroundPosition = Vector2.Zero;

        private float _backgroudScale;

        private bool _isOnTheLeftWall = true;

        Action callback;
        #endregion

        // Ctor
        public MainMenu(Vector2 position, Texture2D background, SpriteFont font)
        {
            this.Position = position; 

            this._background = background;

            this._font = font;

            this._backgroudScale = Screen.OriginalScreenSize.Y / background.Height;

            // Title of the application
            Add(MainMenu.NewText("13th Haunted Street", _font, new Vector2(Screen.OriginalScreenSize.X / 8.5f, Screen.OriginalScreenSize.Y/ 2.6f)));
            
            // Button of the menu
            // Action of new game
            callback = () => { animationStarted = true; Game1.self.displayMainMenu = false;  };
            // Create the button new game
            Add(MainMenu.NewButton("New game", _font, new Vector2(Screen.OriginalScreenSize.X / 7, Screen.OriginalScreenSize.Y / 1.8f), callback, false));
            
            // Action of settings
            callback = () => { 
                animationStarted = true; 
                Settings.SetDefautlValue();
                Game1.self.settingsMenu = new SettingsMenu( position,  background, Game1.self._arrowButton, font);
            };
            // Create the button new game
            Add(MainMenu.NewButton("Settings", _font, GetButtonPosition(), callback, false));

            // Action of quit
            callback = () => { QuitProgram.isQuit = true; };
            // Create the button quit
            Add(MainMenu.NewButton("Quit", _font, GetButtonPosition(), callback, false));
            
            // Action of join
            callback = () => { animationStarted = true; };
            // Create the button join
            Add(MainMenu.NewButton("Join", _font, new Vector2(Screen.OriginalScreenSize.X / 3.05f, Screen.OriginalScreenSize.Y / 1.46f), callback, false));
        }

        public override void Update(GameTime gameTime, Screen screen, ref Vector2 changePosition)
        {
            //zzz pour tester
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                animationStarted = true;
            }

            // Start the animation between the left and right side of the wall
            if (animationStarted)
            {
                // If the camera is on the left wall
                if (_isOnTheLeftWall)
                {
                    // Make the animation from left to right
                    // Moves all menu items to the left
                    changePosition = new Vector2(-1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0 * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                }
                // If the camera is on the right wall
                else
                {
                    // Make the animation from right to left
                    // Moves all menu items to the right
                    changePosition = new Vector2(1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0 * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                }
                // Activation of the animation according to changePosition
                this.PlayLateralAnimation(gameTime, screen, ref changePosition);
            }

            base.Update(gameTime, screen,ref changePosition);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Display the background of the menu
            spriteBatch.Draw(this._background, this._backgroundPosition, null, Color.White, 0f, Vector2.Zero, _backgroudScale, SpriteEffects.None, 0f);

            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Move the position of the background to create an animation between the two walls
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="screen"></param>
        private void PlayLateralAnimation(GameTime gameTime, Screen screen, ref Vector2 changePosition)
        {
            // Get the game time in milliseconds
            this._currentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // When the game time has reached timer tick
            if (this._currentTime >= _TIMERTICK)
            {
                // Calculate the final position of the animation
                float finalPosition = Screen.OriginalScreenSize.X - (this._background.Width * _backgroudScale);

                // Move the menu to the left
                if (this._backgroundPosition.X > finalPosition || this._backgroundPosition.X < 0)
                {
                    // Move the background
                    Vector2 newPosition = new Vector2(this._backgroundPosition.X + changePosition.X, this._backgroundPosition.Y + changePosition.Y);
                    this._backgroundPosition = newPosition;
                    // Move the Title

                    // To make sure the menu position doesn't move too far
                    if (this._backgroundPosition.X <= finalPosition) { 
                        this._backgroundPosition.X = finalPosition;
                        this._isOnTheLeftWall = false;
                        animationStarted = false;
                        changePosition = Vector2.Zero;
                    }
                    if (this._backgroundPosition.X >= 0)
                    {
                        this._backgroundPosition.X = 0;
                        this._isOnTheLeftWall = true;
                        animationStarted = false;
                        changePosition = Vector2.Zero;
                    }
                }
                // Reset current time
                this._currentTime -= _TIMERTICK;
            }
        }

        /// <summary>
        /// Method for adding a new item Button
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="position"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        static ItemButton NewButton(string text, SpriteFont font, Vector2 position, Action callback, bool DrawImageText)
        {
            Texture2D buttonTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);

            return new ItemButton(buttonTexture, font, callback, DrawImageText)
            {
                Text = text,
                Position = position,
                FontColor = Color.White
            };
        }

        /// <summary>
        /// Method for adding a new item Text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        static ItemText NewText(string text, SpriteFont font, Vector2 position)
        {
            return new ItemText(font, text)
            {
                Position = position,
            };
        }

        /// <summary>
        /// This method retrieves the position of a button 
        /// and returns another position aligned according to the given button
        /// </summary>
        /// <returns></returns>
        private Vector2 GetButtonPosition()
        {
            return new Vector2(
                listItems[listItems.Count -1].Position.X,
                listItems[listItems.Count - 1].Position.Y + _font.MeasureString(listItems[listItems.Count - 1].Text).Y * 1.5f
                );
        }
    }
}
