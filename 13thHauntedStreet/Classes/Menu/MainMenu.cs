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
        private Texture2D _background;

        private bool animationStarted = false;

        private Vector2 _backgroundPosition = Vector2.Zero;

        private float _backgroudScale;

        private bool _isOnTheLeftWall = true;

        Action callback;

        //public List<FormItem> listItems = new List<FormItem>();

        static ItemButton NewButton(string text, SpriteFont font, Vector2 position, Action callback)
        {
            Texture2D buttonTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);

            return new ItemButton(buttonTexture, font, callback)
            {
                Text = text,
                Position = position,
                FontColor = Color.White
            };
        }

        static ItemText NewText(string text, SpriteFont font, Vector2 position)
        {
            return new ItemText(font, text)
            {
                Position = position,
            };
        }

        public MainMenu(Vector2 position, Texture2D background, SpriteFont font)
        {
            this.Position = position; //A vérifier pour la suite
            this._background = background;
            this._font = font;
            this._backgroudScale = Screen.OriginalScreenSize.Y / background.Height;

            //zzz Vérifier problème de taille écran plus cours != 1920
            //Titre de l'app
            Add(MainMenu.NewText("13th Haunted Street", _font, new Vector2(Screen.OriginalScreenSize.X / 3.15f, Screen.OriginalScreenSize.Y/ 2.6f)));
            //Menu des boutons
            callback = () => { this.animationStarted = true; };
            Add(MainMenu.NewButton("New game", _font, new Vector2(Screen.OriginalScreenSize.X / 7, Screen.OriginalScreenSize.Y / 1.8f), callback));
            callback = () => { this.animationStarted = true; };
            Add(MainMenu.NewButton("Settings", _font, GetButtonPosition(), callback));
            callback = () => { Game1.self.Exit();  };
            Add(MainMenu.NewButton("Quit", _font, GetButtonPosition(), callback));
            callback = () => { this.animationStarted = true; };
            Add(MainMenu.NewButton("Join", _font, new Vector2(Screen.OriginalScreenSize.X / 3.05f, Screen.OriginalScreenSize.Y / 1.46f), callback));
        }

        public override void Update(GameTime gameTime, Screen screen, ref Vector2 changePosition)
        {
            // pour tester
            if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                animationStarted = true;
            }

            // Start the animation 
            if (animationStarted)
            {
                if (_isOnTheLeftWall)
                {
                    changePosition = new Vector2(-1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0 * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                }
                else {
                    changePosition = new Vector2(1 * (float)gameTime.ElapsedGameTime.TotalMilliseconds, 0 * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                }
                this.PlayLateralAnimation(gameTime, screen, ref changePosition);
            }

            base.Update(gameTime, screen,ref changePosition);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this._background, this._backgroundPosition, null, Color.White, 0f, Vector2.Zero, _backgroudScale, SpriteEffects.None, 0f);

            base.Draw(gameTime, spriteBatch);
        }

        //public override void Add(ItemsMenu newItem)
        //{
        //    listItems.Add(newItem);
        //}

        //public override void Remove(ItemsMenu removeItem)
        //{
        //    for (int i = listItems.Count - 1; i >= 0; i--)
        //    {
        //        if (listItems[i] == removeItem)
        //        {
        //            listItems.RemoveAt(i);
        //        }
        //    }
        //}

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
                        this.animationStarted = false;
                        changePosition = Vector2.Zero;
                    }

                    if (this._backgroundPosition.X >= 0)
                    {
                        this._backgroundPosition.X = 0;
                        this._isOnTheLeftWall = true;
                        this.animationStarted = false;
                        changePosition = Vector2.Zero;
                    }
                }

                this._currentTime -= _TIMERTICK;
            }
        }

        private Vector2 GetButtonPosition()
        {
            return new Vector2(
                listItems[listItems.Count -1].Position.X,
                listItems[listItems.Count - 1].Position.Y + _font.MeasureString(listItems[listItems.Count - 1].Text).Y * 1.5f
                );
        }
    }
}
