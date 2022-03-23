using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;

namespace _13thHauntedStreet
{
    public class Game1 : Game
    {
        // Graphics, spriteBatch and Penumbra
        public static GraphicsDeviceManager graphics;

        public static Game1 self;

        private SpriteBatch _spriteBatch;
        
        private PenumbraComponent _penumbra;

        // Screen
        private Screen _screen;
        
        // Refresh rate limited and display 
        private FrameCounter _frameCounter = new FrameCounter();

        public static bool showFps = true;

        public static float limitedFps = 60f;

        public static float previusLimitedFps;

        // Sound Volume
        public static float sfxVolume = 8f;

        public static float musicVolume = 8f;

        // Menu
        private MainMenu _mainMenu;

        private Texture2D _backgroundMainMenu;

        private Texture2D _arrowButton;

        private SpriteFont _font;

        // Players
        private Ghost ghost;

        private GhostAnimationManager ghostAM = new GhostAnimationManager();

        private Hunter hunter;

        private HunterAnimationManager hunterAM = new HunterAnimationManager();

        // Furnitures
        private Texture2D bedTexture;

        private Texture2D drawerTexture;

        private List<Furniture> furnitureList = new List<Furniture>();

        // remove later
        private Texture2D bg;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            self = this;

            // Create penumbra
            _penumbra = new PenumbraComponent(this);
            
            // Set the fps to 60
            IsFixedTimeStep = true;

            TargetElapsedTime = TimeSpan.FromSeconds(1f / limitedFps);

            previusLimitedFps = limitedFps;

            // Set the windows
            IsMouseVisible = true;

            Window.IsBorderless = true;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // Initialize penumbra
            _penumbra.Initialize();

            // Initialize Screen
            _screen = Screen.Instance(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, Window);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _screen.LoadContent();

            _backgroundMainMenu = Content.Load<Texture2D>("TempFiles/BackgroundMenu");
            _arrowButton = Content.Load<Texture2D>("TempFiles/arrow");
            _font = Content.Load<SpriteFont>("TempFiles/theFont");
            _mainMenu = new MainMenu(Vector2.Zero,_backgroundMainMenu, _font);
            //_mainMenu.LoadContent(_screen, _arrowButton);

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
            /*if (_mainMenu.quitedTheGame)
                Exit();
            */
            // if the fps limit has changed
            if (previusLimitedFps != limitedFps)
            {
                TargetElapsedTime = TimeSpan.FromSeconds(1f / limitedFps);
                previusLimitedFps = limitedFps;
            }

            _screen.Update(gameTime);

            Vector2 posOri = Vector2.Zero;
            _mainMenu.Update(gameTime, _screen, ref posOri);

            hunter.Update(gameTime, furnitureList);
            ghost.Update(gameTime, furnitureList);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _penumbra.BeginDraw();
            GraphicsDevice.SetRenderTarget(_screen.RenderTarget);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _mainMenu.Draw(gameTime, _spriteBatch);
            /*
            if (_mainMenu.Option == MainMenu._RightMenuSelected.NewGame)
            {
                _spriteBatch.Draw(bg, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/2, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2), null, Color.White, 0f, bg.Bounds.Center.ToVector2(), 0.95f, SpriteEffects.None, 1f);

                List<GameObject> gameObjectList = new List<GameObject>();
                gameObjectList.AddRange(furnitureList);
                gameObjectList.Add(hunter);

                foreach (GameObject gameObject in gameObjectList.OrderBy(o => o.position.Y))
                {
                    gameObject.Draw(_spriteBatch);
                }
                ghost.Draw(_spriteBatch);
            }
            else
            {
                _mainMenu.Draw(_spriteBatch);
            }
            
            if (showFps)
            {
                

                _frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                var fps = string.Format("fps: {0}", (int)Decimal.Truncate((decimal)_frameCounter.AverageFramesPerSecond));

                _spriteBatch.DrawString(_font, fps, new Vector2(1, 1), Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }
            */
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
