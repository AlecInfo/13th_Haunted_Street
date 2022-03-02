using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;

        private Screen screen;

        private Ghost ghost;
        private GhostAnimationManager ghostAM = new GhostAnimationManager();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            Window.IsBorderless = true;
        }

        protected override void Initialize()
        {
            screen = Screen.Instance(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            screen.LoadContent(GraphicsDevice);

            ghostAM.animationLeft = multipleTextureLoader("TempFiles/GhostSprites/ghostLeft", 6);
            ghostAM.animationRight = multipleTextureLoader("TempFiles/GhostSprites/ghostRight", 6);


            // Ghost
            ghost = new Ghost(
                new Input()
                {
                    Left = Keys.A,
                    Right = Keys.D,
                    Up = Keys.W,
                    Down = Keys.S
                },
                new Vector2(500, 500),
                ghostAM
                );

            // method that loads every texture of an animation
            List<Texture2D> multipleTextureLoader(string filePrefix, int size)
            {
                List<Texture2D> result = new List<Texture2D>();
                for (int x = 0; x < size; x++)
                {
                    result.Add(Content.Load<Texture2D>($"{filePrefix}{x+1:D2}"));
                }

                return result;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            screen.Update(gameTime, Window, graphics);

            ghost.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(screen.RenderTarget);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            ghost.Draw(_spriteBatch);

            _spriteBatch.End();

            // For draw the render target
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.Draw(screen.RenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, screen.Scale, SpriteEffects.None, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
