using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Penumbra;

namespace _13thHauntedStreet
{
    public partial class Game1 : Game
    {
        // Graphics, spriteBatch and Penumbra
        public static GraphicsDeviceManager graphics; 

        public static Game1 self;

        private SpriteBatch _spriteBatch;
        public static PenumbraComponent penumbra;

        private QuitProgram _quit = new QuitProgram();

        public static KnM knm = new KnM();
        public static Input input;
        public enum direction
        {
            none,
            left,
            right,
            up,
            down
        }

        // Screen
        public Screen screen;
        
        // Refresh rate limited and display 
        private FrameCounter _frameCounter = new FrameCounter();

        public bool showFps;

        public int limitedFps;

        public static float previusLimitedFps;
         
        // Sound Volume
        public float sfxVolume;

        public float musicVolume;

        // Menu
        public bool displayMainMenu = true;

        private MainMenu _mainMenu;

        public SettingsMenu settingsMenu;

        private Texture2D _backgroundMainMenu;

        public Texture2D _arrowButton;

        public Texture2D _controlButton;

        public SpriteFont font;

        public static Texture2D defaultTexture;

        // Players
        public static GhostAnimationManager ghostAM = new GhostAnimationManager();
        public static HunterAnimationManager hunterAM = new HunterAnimationManager();
        public static Player player;
        public static bool captured = false;

        public Texture2D crosshair;
        public static Texture2D flashlightIcon;
        public static Texture2D flashlightFrameIcon;
        public static Texture2D vacuumIconOff;
        public static Texture2D vacuumIconOn;
        public static Texture2D vacuumFrameIcon;
        public static Texture2D uiFrame;
        public static Texture2D uiSmallFrame;

        // Furnitures
        private Texture2D bedTexture;
        private Texture2D drawerTexture;
        private Texture2D bigBedTexture;
        private Texture2D smallTableTexture;

        public List<Furniture> furnitureList = new List<Furniture>();
        public List<Furniture> drawabledFurnitureList = new List<Furniture>();

        // remove later
        private Texture2D bg;
        private Texture2D bg2;
        private Texture2D ground;
        private Texture2D walls;
        public Map testMap;

        // Server
        private int id = 1;
        public static Client client;
        public static dataPlayer dataPlayer;
        public DateTime clientLastUpdate;
        public string serializeToStringPlayer;


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

            previusLimitedFps = 60 ;

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
            screen = Screen.Instance(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, Window);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            screen.LoadContent();

            _backgroundMainMenu = Content.Load<Texture2D>("TempFiles/BackgroundMenu");
            _arrowButton = Content.Load<Texture2D>("TempFiles/arrow");
            _controlButton = Content.Load<Texture2D>("TempFiles/buttonControl");
            font = Content.Load<SpriteFont>("TempFiles/theFont");

            _mainMenu = new MainMenu(Vector2.Zero, _backgroundMainMenu, font);

            defaultTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            defaultTexture.SetData(new Color[] { Color.White });

            // Furniture

            bedTexture = Content.Load<Texture2D>("TempFiles/Furniture/bed");
            bigBedTexture = Content.Load<Texture2D>("TempFiles/Furniture/big_bed");
            drawerTexture = Content.Load<Texture2D>("TempFiles/Furniture/drawer");
            smallTableTexture = Content.Load<Texture2D>("TempFiles/Furniture/small_table");

            furnitureList.Add(new Furniture(Vector2.Zero, bedTexture));
            furnitureList.Add(new Furniture(Vector2.Zero, bigBedTexture));
            furnitureList.Add(new Furniture(Vector2.Zero, drawerTexture));
            furnitureList.Add(new Furniture(Vector2.Zero, smallTableTexture));

            drawabledFurnitureList.Add(new Furniture(new Vector2(1000, 500), bedTexture));
            drawabledFurnitureList.Add(new Furniture(new Vector2(1400, 750), drawerTexture));
            drawabledFurnitureList.Add(new Furniture(new Vector2(500, 200), bigBedTexture));
            drawabledFurnitureList.Add(new Furniture(new Vector2(1200, 400), smallTableTexture));
            drawabledFurnitureList.Add(new Furniture(new Vector2(370, 600), drawerTexture));
            drawabledFurnitureList.Add(new Furniture(new Vector2(1350, 350), bigBedTexture));
            

            // Ghost
            ghostAM.animationLeft = multipleTextureLoader("TempFiles/GhostSprites/left/ghostLeft", 3);
            ghostAM.animationRight = multipleTextureLoader("TempFiles/GhostSprites/right/ghostRight", 3);
            foreach (Furniture item in furnitureList)
            {
                ghostAM.furniture.Add(item.texture);
            }

            // Hunter
            hunterAM.walkingLeft = multipleTextureLoader("TempFiles/HunterSprites/walking_left/walking_left", 6);
            hunterAM.walkingRight = multipleTextureLoader("TempFiles/HunterSprites/walking_right/walking_right", 6);
            hunterAM.walkingUp = multipleTextureLoader("TempFiles/HunterSprites/walking_up/walking_up", 6);
            hunterAM.walkingDown = multipleTextureLoader("TempFiles/HunterSprites/walking_down/walking_down", 6);
            hunterAM.idleLeft.Add(Content.Load<Texture2D>("TempFiles/HunterSprites/idle_left/idle_left"));
            hunterAM.idleRight.Add(Content.Load<Texture2D>("TempFiles/HunterSprites/idle_right/idle_right"));
            hunterAM.idleUp.Add(Content.Load<Texture2D>("TempFiles/HunterSprites/idle_up/idle_up"));
            hunterAM.idleDown.Add(Content.Load<Texture2D>("TempFiles/HunterSprites/idle_down/idle_down"));

            // Player
            crosshair = Content.Load<Texture2D>("TempFiles/crosshair");
            Mouse.SetCursor(MouseCursor.FromTexture2D(crosshair, crosshair.Bounds.Center.X, crosshair.Bounds.Center.Y));

            flashlightIcon = Content.Load<Texture2D>("TempFiles/Ui/Hunter/flashlightIcon");
            flashlightFrameIcon = Content.Load<Texture2D>("TempFiles/Ui/Hunter/flashlightFrameIcon");
            vacuumIconOff = Content.Load<Texture2D>("TempFiles/Ui/Hunter/vacuumOff");
            vacuumIconOn = Content.Load<Texture2D>("TempFiles/Ui/Hunter/vacuumOn");
            vacuumFrameIcon = Content.Load<Texture2D>("TempFiles/Ui/Hunter/vacuumFrameIcon");
            uiFrame = Content.Load<Texture2D>("TempFiles/Ui/Hunter/frame");
            uiSmallFrame = Content.Load<Texture2D>("TempFiles/Ui/Hunter/frame2");

            input = Input.GetInstance();
            input.Left = KnMButtons.A;
            input.Right = KnMButtons.D;
            input.Up = KnMButtons.W;
            input.Down = KnMButtons.S;
            input.Use1 = KnMButtons.LeftClick;
            input.Use2 = KnMButtons.RightClick;
            input.ItemUp = KnMButtons.ScrollUp;
            input.ItemDown = KnMButtons.ScrollDown;

            player = new Hunter(
                new Vector2(500, 500),
                hunterAM
            );

            // Map
            bg = Content.Load<Texture2D>("TempFiles/bg");
            bg2 = Content.Load<Texture2D>("TempFiles/bg2");
            ground = Content.Load<Texture2D>("TempFiles/Ground");
            walls = Content.Load<Texture2D>("TempFiles/Walls");

            testMap = new Map(player,
                new List<Scene>() {
                    new Scene(1, bg, walls, Vector2.One / 1.125f, new Rectangle(360, 215, 1200, 650), player, drawabledFurnitureList, new List<Lantern>()),
                    new Scene(2, bg2, walls, Vector2.One / 1.125f, new Rectangle(360, 215, 1200, 650), player, new List<Furniture>(), new List<Lantern>())
                }
            );

            testMap.doorList.Add(new Door(direction.up, testMap.listScenes[0]));
            testMap.doorList.Add(new Door(direction.down, testMap.listScenes[1]));
            Door.connectDoors(testMap.doorList[0], testMap.doorList[1]);

            // Client
            dataPlayer = new dataPlayer();


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

            // Set the default parameters like the refresh rate, music volume or fullscreen
            Settings.SetDefautlValue();
        }

        protected override void Update(GameTime gameTime)
        {
            _quit.Update(gameTime, self);

            // if the fps limit has changed
            if (previusLimitedFps != limitedFps)
            {
                TargetElapsedTime = TimeSpan.FromSeconds(1f / limitedFps);
                previusLimitedFps = limitedFps;
            }

            screen.Update(gameTime);

            if (displayMainMenu)
            {
                // Display the main menu
                Vector2 posOri = Vector2.Zero;
                _mainMenu.Update(gameTime, screen, ref posOri);
                if (settingsMenu != null)
                {
                    settingsMenu.Update(gameTime, screen, ref posOri);
                }
            }
            else
            {
                // Display the game
                penumbra.Hulls.Clear();
                penumbra.Lights.Clear();
                penumbra.Visible = true; // remove later

                testMap.Update(gameTime);

                TimeSpan ts = DateTime.Now - clientLastUpdate;
                if (ts.Milliseconds >= 62)
                {
                    // Test if changed, Hunter only
                    bool hasChangedHunter = false;
                    do
                    {
                        if (player.GetType() == typeof(Hunter))
                        {
                            Hunter hunter = (player as Hunter);
                            if (hunter.currentTool.GetType() == typeof(Flashlight))
                            {
                                if ((hunter.currentTool as Flashlight).isLit != dataPlayer.IsLightOn)
                                {
                                    hasChangedHunter = true;
                                    break;
                                }
                            }

                            if (hunter.currentTool.light.Radius != dataPlayer.Radius)
                            {
                                hasChangedHunter = true;
                                break;
                            }

                            bool isFlashlight = hunter.currentTool.GetType() == typeof(Flashlight) ? true : false;
                            if (isFlashlight != dataPlayer.ToolIsFlashlight)
                            {
                                hasChangedHunter = true;
                                break;
                            }
                        }

                        break;
                    } while (false);

                    if (player.movement.X != 0 || player.movement.Y != 0 || dataPlayer.TextureName != player.texture.Name || hasChangedHunter)
                    {
                        dataPlayer.Id = player.id;
                        dataPlayer.Position = player.position.ToString();

                        dataPlayer.PlayerType = player.GetType().ToString();
                        dataPlayer.CurrentScene = testMap.currentScene.id;

                        dataPlayer.IsObject = player.isObject;

                        dataPlayer.TextureName = player.texture.Name;
                        dataPlayer.Captured = captured;
                        if (player.GetType() == typeof(Hunter))
                        {
                            Hunter hunter = player as Hunter;
                            if (hunter.currentTool.GetType() == typeof(Flashlight))
                            {
                                dataPlayer.IsLightOn = (hunter.currentTool as Flashlight).isLit;
                            }
                            else
                                dataPlayer.IsLightOn = true;

                            dataPlayer.Radius = hunter.currentTool.angle;
                            dataPlayer.ToolIsFlashlight = hunter.currentTool.GetType() == typeof(Flashlight) ? true : false;
                        }
                        else
                        {
                            dataPlayer.IsLightOn = false;
                            dataPlayer.Radius = 0;
                            dataPlayer.ToolIsFlashlight = false;
                        }

                        serializeToStringPlayer = SerializeObject();
                        client.envoieMessage(serializeToStringPlayer);
                    }
                }
            }

            knm.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(screen.RenderTarget);


            // If the menu should be displayed
            if (displayMainMenu)
            {
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                
                // Display the main menu
                _mainMenu.Draw(_spriteBatch);

                // If the gameplay menu is instantiated
                if (settingsMenu != null)
                {
                    // Display the gameplay menu
                    settingsMenu.Draw(_spriteBatch);
                }
                _spriteBatch.End();
            }
            else
            {
                penumbra.BeginDraw();
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

                testMap.Draw(_spriteBatch);

                _spriteBatch.End();
                penumbra.Draw(gameTime);
            }


            // draw UI
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            if (showFps)
            {
                // Display the FPS
                _frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

                var fps = string.Format("fps: {0}", (int)Decimal.Truncate((decimal)_frameCounter.AverageFramesPerSecond));

                _spriteBatch.DrawString(font, fps, new Vector2(1, 1), Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }

            if (!displayMainMenu)
            {
                player.DrawUI(_spriteBatch);
            }
            _spriteBatch.End();



            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            _spriteBatch.Draw(screen.RenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, screen.Scale, SpriteEffects.None, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // Server Methods
        /// <summary>
        /// Serializes dataPlayer
        /// </summary>
        /// <returns>serialized dataPlayer</returns>
        public static string SerializeObject()
        {
            string xmlStr = string.Empty;

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            settings.NewLineChars = string.Empty;
            settings.NewLineHandling = NewLineHandling.None;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, string.Empty);

                    XmlSerializer serializer = new XmlSerializer(dataPlayer.GetType());
                    serializer.Serialize(xmlWriter, dataPlayer, namespaces);

                    xmlStr = stringWriter.ToString();
                    xmlWriter.Close();
                }

                stringWriter.Close();
            }

            return xmlStr;
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            client.envoieMessage("Je me deconnecte :" + Game1.player.id);
        }


        // Draw line
        private static Texture2D _texture;

        private static Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _texture.SetData(new[] { Color.White });
            }

            return _texture;
        }
        public static void DrawLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }
    }
}