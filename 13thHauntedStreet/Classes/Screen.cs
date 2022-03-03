using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class Screen
    {
        // Varriables
        private static Screen instance;

        private GameWindow _window;

        private RenderTarget2D _renderTarget;
        public RenderTarget2D RenderTarget { 
            get => _renderTarget;
        }

        private const float _SCREENDIFFERENCE = 1.5f;
        private const int _ORIGINALSIZE_X = 1920;
        private const int _ORIGINALSIZE_Y = 1080;
        private const int _MINSCREENSIZE = 640;

        private Vector2 _fullScreenSize;
        public Vector2 FullScreenSize { 
            get => _fullScreenSize;
        }

        private Vector2 _windowedSize;
        public Vector2 WindowedSize
        {
            get => _windowedSize;
        }

        private float _scale;
        public float Scale 
        { 
            get => _scale; 
        }

        private Vector2 _editSize;
        public Vector2 EditSize 
        { 
            get => _editSize;
            set => _editSize = value;
        }

        private bool _windowsIsChanged = false;


        // Ctor
        public Screen(Vector2 newSize, GameWindow window)
        {
            this._editSize = newSize;
            this._window = window;
        }

        public static Screen Instance(float x, float y, GameWindow window)
        {
            // Create a Singleton to avoid duplicates
            if (instance == null)
            {
                instance = new Screen(new Vector2(x, y), window);
            }
            return instance;
        }

        // Methods
        public void LoadContent()
        {
            // Get the render target size according to the original size
            this._renderTarget = new RenderTarget2D(Game1.graphics.GraphicsDevice, _ORIGINALSIZE_X, _ORIGINALSIZE_Y);
            // Get the windowed and full screen size according to screen size
            this._fullScreenSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            this._windowedSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / _SCREENDIFFERENCE, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / _SCREENDIFFERENCE);
        }

        public void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                this.WindowedScreen();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                this.FullScreen();
            }

            // Get the scale of the screen
            this._scale = 1f / ((float)this.RenderTarget.Width / Game1.graphics.GraphicsDevice.Viewport.Width); ;

            // Put in windowed
            if (!this._windowsIsChanged && !this.WindowsSizeIsEqualScreenSize())
            {
                // Update Windows position and Update the size
                this._window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - ((int)this.EditSize.X / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - ((int)this.EditSize.Y / 2));
                // Add border on the window
                this._window.IsBorderless = false;
                // Add the possibility to the user to modify the window
                this._window.AllowUserResizing = true;
                this._window.ClientSizeChanged += this.ChangeScreenSize;

                this._windowsIsChanged = true;

                // Change the size
                Game1.graphics.PreferredBackBufferWidth = (int)this.EditSize.X;
                Game1.graphics.PreferredBackBufferHeight = (int)this.EditSize.Y;
            }

            // Put in full screen
            if (this.WindowsSizeIsEqualScreenSize())
            {
                _window.Position = Point.Zero;
                _window.IsBorderless = true;
                _window.AllowUserResizing = false;
                this._windowsIsChanged = true;
            }

            // When the user can't modify the window
            if (!_window.AllowUserResizing)
            {
                // Change de window size
                Game1.graphics.PreferredBackBufferWidth = (int)EditSize.X;
                Game1.graphics.PreferredBackBufferHeight = (int)EditSize.Y;               
            }

            // Apply the change size of the window
            Game1.graphics.ApplyChanges();
        }

        /// <summary>
        /// Check if the window size is full screen or not
        /// </summary>
        /// <returns></returns>
        private bool WindowsSizeIsEqualScreenSize()
        {
            if (EditSize.X == GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width && EditSize.Y == GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Set the window to screen windowed
        /// </summary>
        public void WindowedScreen()
        {
            this._windowsIsChanged = false;
            this.EditSize = this.WindowedSize;
        }

        /// <summary>
        /// Set the window to full screen
        /// </summary>
        public void FullScreen()
        {
            this._windowsIsChanged = false;
            this.EditSize = this.FullScreenSize;
        }

        /// <summary>
        /// This method is an event that allows to change the window size according to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeScreenSize(Object sender, EventArgs e)
        {
            // if the screen is to small
            if (this._window.ClientBounds.Width <= _MINSCREENSIZE)
                Game1.graphics.PreferredBackBufferWidth = _MINSCREENSIZE;
            else
                Game1.graphics.PreferredBackBufferWidth = this._window.ClientBounds.Width;

            // Set the window height according the window width
            Game1.graphics.PreferredBackBufferHeight = (this._window.ClientBounds.Width * _ORIGINALSIZE_Y) / _ORIGINALSIZE_X;
            // Apply the change size of the window
            Game1.graphics.ApplyChanges();
        }
    }
}
