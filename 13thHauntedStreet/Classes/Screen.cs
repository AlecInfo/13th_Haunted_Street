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
        private static Screen instance;

        private RenderTarget2D _renderTarget;
        public RenderTarget2D RenderTarget { 
            get => _renderTarget;
        }

        private const int ORIGINALSIZE_X = 1920;
        private const int ORIGINALSIZE_Y = 1080;

        private Vector2 _fullScreenSize;
        public Vector2 FullScreenSize { 
            get => _fullScreenSize;
        }

        private Vector2 _smallScreenSize;
        public Vector2 SmallScreenSize
        {
            get => _smallScreenSize;
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

        public Screen(Vector2 newSize)
        {
            this._editSize = newSize;
        }

        public static Screen Instance(float x, float y)
        {
            // Create a Singleton to avoid duplicates
            if (instance == null)
            {
                instance = new Screen(new Vector2(x, y));
            }
            return instance;
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            this._renderTarget = new RenderTarget2D(graphicsDevice, ORIGINALSIZE_X, ORIGINALSIZE_Y);

            this._fullScreenSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            this._smallScreenSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 1.5f, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1.5f);
        }

        public void Update(GameTime gameTime, GameWindow window, GraphicsDeviceManager graphics)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.G))
            {
                SmallScreen();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                FullScreen();
            }


            // Set Windows in center screen one time
            if (!this._windowsIsChanged && !WindowsSizeIsEqualScreenSize())
            {
                // Update Windows position and Update the size
                window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - ((int)EditSize.X / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - ((int)EditSize.Y / 2));
                
                window.IsBorderless = false;

                window.AllowUserResizing = true;

                this._windowsIsChanged = true;
            }

            if (WindowsSizeIsEqualScreenSize())
            {
                window.Position = Point.Zero;
                window.IsBorderless = true;
                window.AllowUserResizing = false;
            }



            if (window.AllowUserResizing)
            {
                window.ClientSizeChanged += OnResize;
            }
            else
            {
                Game1.graphics.PreferredBackBufferWidth = (int)EditSize.X;
                Game1.graphics.PreferredBackBufferHeight = (int)EditSize.Y;
            }

            Game1.graphics.ApplyChanges();

        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, GameTime gameTime)
        {
            // Get the scale of the screen
            this._scale = 1f / ((float)this.RenderTarget.Height / graphicsDevice.Viewport.Height);
        }

        private bool WindowsSizeIsEqualScreenSize()
        {
            if (EditSize.X == GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width && EditSize.Y == GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
                return true;
            else
                return false;
        }

        public void SmallScreen()
        {
            this._windowsIsChanged = false;
            EditSize = SmallScreenSize;
        }

        public void FullScreen()
        {
            this._windowsIsChanged = false;
            EditSize = FullScreenSize;

        }

        public void OnResize(Object sender, EventArgs e)
        {
            Game1.graphics.PreferredBackBufferWidth = (int)EditSize.X;
            Game1.graphics.PreferredBackBufferHeight = (int)EditSize.Y;

            if ((EditSize.X != Game1.graphics.GraphicsDevice.Viewport.Width) || (EditSize.Y != Game1.graphics.GraphicsDevice.Viewport.Height))
            {
                _editSize.X = Game1.graphics.GraphicsDevice.Viewport.Width;
                _editSize.Y = Game1.graphics.GraphicsDevice.Viewport.Height;
            }
        }
    }
}
