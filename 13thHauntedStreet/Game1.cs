﻿using System;
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
        public PenumbraComponent penumbra;

        public enum direction
        {
            none,
            left,
            right,
            up,
            down
        }
        
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

        public static Texture2D defaultTexture;

        // Players
        private Ghost ghost;

        private GhostAnimationManager ghostAM = new GhostAnimationManager();

        private Hunter hunter;

        private HunterAnimationManager hunterAM = new HunterAnimationManager();
        private List<Player> playerList = new List<Player>();

        // Furnitures
        private Texture2D bedTexture;

        private Texture2D drawerTexture;

        private List<Furniture> furnitureList = new List<Furniture>();

        private Scene testScene;

        // remove later
        private Texture2D bg;
        private Texture2D ground;
        private Texture2D walls;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            penumbra = new PenumbraComponent(this) {
                AmbientColor = Color.Black
            };
            Content.RootDirectory = "Content";

            self = this;
            
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
            penumbra.Initialize();

            // Initialize Screen
            _screen = Screen.Instance(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, Window);

            penumbra.Initialize();

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

            defaultTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            defaultTexture.SetData(new Color[] { Color.White });

            ghostAM.animationLeft = multipleTextureLoader("TempFiles/GhostSprites/ghostLeft", 3);
            ghostAM.animationRight = multipleTextureLoader("TempFiles/GhostSprites/ghostRight", 3);


            // Ghost
            playerList.Add(new Ghost(
                new Input()
                {
                    Left = Keys.Left,
                    Right = Keys.Right,
                    Up = Keys.Up,
                    Down = Keys.Down
                },
                new Vector2(500, 500),
                ghostAM
            ));

            // Hunter
            hunterAM.walkingLeft = multipleTextureLoader("TempFiles/HunterSprites/walking_left/walking_left", 6);
            hunterAM.walkingRight = multipleTextureLoader("TempFiles/HunterSprites/walking_right/walking_right", 6);
            hunterAM.walkingUp = multipleTextureLoader("TempFiles/HunterSprites/walking_up/walking_up", 6);
            hunterAM.walkingDown = multipleTextureLoader("TempFiles/HunterSprites/walking_down/walking_down", 6);
            hunterAM.idleLeft.Add(Content.Load<Texture2D>("TempFiles/HunterSprites/idle/idle_left"));
            hunterAM.idleRight.Add(Content.Load<Texture2D>("TempFiles/HunterSprites/idle/idle_right"));
            hunterAM.idleUp.Add(Content.Load<Texture2D>("TempFiles/HunterSprites/idle/idle_up"));
            hunterAM.idleDown.Add(Content.Load<Texture2D>("TempFiles/HunterSprites/idle/idle_down"));

            playerList.Add(new Hunter(
                new Input()
                {
                    Left = Keys.A,
                    Right = Keys.D,
                    Up = Keys.W,
                    Down = Keys.S
                },
                new Vector2(500, 500),
                hunterAM
            ));

            bedTexture = Content.Load<Texture2D>("TempFiles/Furniture/bed");
            drawerTexture = Content.Load<Texture2D>("TempFiles/Furniture/drawer");
            furnitureList.Add(new Furniture(new Vector2(1000, 500), bedTexture));
            furnitureList.Add(new Furniture(new Vector2(1400, 750), drawerTexture));

            bg = Content.Load<Texture2D>("TempFiles/bg");
            ground = Content.Load<Texture2D>("TempFiles/Ground");
            walls = Content.Load<Texture2D>("TempFiles/Walls");

            testScene = new Scene(bg, walls, Vector2.One/1.125f, new Rectangle(360, 215, 1200, 650), playerList, furnitureList);

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

            // clear hulls and lights
            penumbra.Hulls.Clear();
            penumbra.Lights.Clear();

            // make penumbra visible if in the game
            //penumbra.Visible = (_mainMenu.Option == MainMenu._RightMenuSelected.NewGame);
            
            testScene.Update(gameTime, penumbra);

            //Vector2 posOri = Vector2.Zero;
            //_mainMenu.Update(gameTime, _screen, ref posOri);

            _screen.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_screen.RenderTarget);
            /*if (_mainMenu.Option == MainMenu._RightMenuSelected.NewGame)
            {*/
                penumbra.BeginDraw();
                GraphicsDevice.Clear(Color.Black);
            
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
  
                testScene.Draw(_spriteBatch);

                _spriteBatch.End();
                penumbra.Draw(gameTime);

                // Draw infront of shadow
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _spriteBatch.End();
            /*}
            else
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _mainMenu.Draw(gameTime, _spriteBatch);
                _spriteBatch.End();
            }
            
            if (showFps)
            {
                

                _frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                var fps = string.Format("fps: {0}", (int)Decimal.Truncate((decimal)_frameCounter.AverageFramesPerSecond));

                _spriteBatch.DrawString(_font, fps, new Vector2(1, 1), Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }*/
            

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            _spriteBatch.Draw(_screen.RenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, _screen.Scale, SpriteEffects.None, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
