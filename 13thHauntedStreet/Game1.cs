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

        private Hunter hunter;
        private HunterAnimationManager hunterAM = new HunterAnimationManager();

        private Texture2D bedTexture;
        private Texture2D drawerTexture;
        private List<Furniture> furnitureList = new List<Furniture>();

        // remove later
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
            screen = Screen.Instance(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, Window);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            screen.LoadContent();

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

            bedTexture = Content.Load<Texture2D>("TempFiles/Furniture/bed");
            drawerTexture = Content.Load<Texture2D>("TempFiles/Furniture/drawer");
            furnitureList.Add(new Furniture(new Vector2(1000, 500), bedTexture));
            furnitureList.Add(new Furniture(new Vector2(1400, 750), drawerTexture));

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

            hunter.Update(gameTime, furnitureList);
            screen.Update(gameTime);

            ghost.Update(gameTime, furnitureList);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(screen.RenderTarget);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            _spriteBatch.Draw(bg, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/2, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2), null, Color.White, 0f, bg.Bounds.Center.ToVector2(), 0.95f, SpriteEffects.None, 1f);

            foreach (Furniture item in furnitureList)
            {
                item.Draw(_spriteBatch);
            }
            
            hunter.Draw(_spriteBatch);
            ghost.Draw(_spriteBatch);

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.Draw(screen.RenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, screen.Scale, SpriteEffects.None, 0f);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
