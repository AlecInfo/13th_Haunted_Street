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

        private Screen _screen;

        private MainMenu _mainMenu;
        private Texture2D _backgroundMainMenu;

        private Ghost ghost;
        private GhostAnimationManager ghostAM = new GhostAnimationManager();

        private Hunter hunter;
        private HunterAnimationManager hunterAM = new HunterAnimationManager();

        private Texture2D bg;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            Window.IsBorderless = true;
        }

        protected override void Initialize()
        {
            _screen = Screen.Instance(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, Window);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _screen.LoadContent();

            _backgroundMainMenu = Content.Load<Texture2D>("TempFiles/BackgroundMenu");
            _mainMenu = new MainMenu(_backgroundMainMenu);
            _mainMenu.LoadContent();

            ghostAM.animationLeft = multipleTextureLoader("TempFiles/GhostSprites/ghostLeft", 3);
            ghostAM.animationRight = multipleTextureLoader("TempFiles/GhostSprites/ghostRight", 3);


            // Ghost
            ghost = new Ghost(
                new Input()
                {
                    Left = Keys.Left,
                    Right = Keys.Right,
                    Up = Keys.Up,
                    Down = Keys.Down
                },
                new Vector2(500, 500),
                ghostAM
                );

            // Hunter
            hunterAM.walkingLeft = multipleTextureLoader("TempFiles/HunterSprites/walking_left/walking_left", 6);
            hunterAM.walkingRight = multipleTextureLoader("TempFiles/HunterSprites/walking_right/walking_right", 6);
            hunterAM.walkingUp = multipleTextureLoader("TempFiles/HunterSprites/walking_up/walking_up", 6);
            hunterAM.walkingDown = multipleTextureLoader("TempFiles/HunterSprites/walking_down/walking_down", 6);
            hunterAM.idleLeft.Add(Content.Load<Texture2D>("TempFiles/HunterSprites/idle/idle_left"));
            hunterAM.idleRight.Add(Content.Load<Texture2D>("TempFiles/HunterSprites/idle/idle_right"));
            hunterAM.idleUp.Add(Content.Load<Texture2D>("TempFiles/HunterSprites/idle/idle_up"));
            hunterAM.idleDown.Add(Content.Load<Texture2D>("TempFiles/HunterSprites/idle/idle_down"));

            hunter = new Hunter(
                new Input()
                {
                    Left = Keys.A,
                    Right = Keys.D,
                    Up = Keys.W,
                    Down = Keys.S
                },
                new Vector2(500, 500),
                hunterAM
            );

            bg = Content.Load<Texture2D>("TempFiles/bg");


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

            _screen.Update(gameTime);
            _mainMenu.Update(gameTime, _screen);

            hunter.Update(gameTime);
            ghost.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_screen.RenderTarget);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState:SamplerState.PointClamp);


            _mainMenu.Draw(_spriteBatch);
            //_spriteBatch.Draw(bg, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 1f);
            //hunter.Draw(_spriteBatch);
            //ghost.Draw(_spriteBatch);

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_screen.RenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, _screen.Scale, SpriteEffects.None, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
