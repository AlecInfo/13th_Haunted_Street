using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class MainMenuSetting
    {
        private Texture2D _buttonTexture;

        private SpriteFont _font;

        private string _title = "Game option : ";

        private string _tmpTitle = "";

        public Vector2 TitlePosition { get; set; }

        public bool Back { get; set; }

        private float _changePosition = 0;
        public float ChangePosition { get => _changePosition; set => _changePosition = value; }

        private List<Button> _buttonList = new List<Button>();
        public List<Button> ButtonList { get => _buttonList; set => _buttonList = value; }


        private List<ArrowButton> _arrowButtonList = new List<ArrowButton>();
        public List<ArrowButton> ArrowButtonList { get => _arrowButtonList; set => _arrowButtonList = value; }

        public void Load(Screen screen, SpriteFont font, Texture2D arrowButton)
        {
            this._font = font;

            // Create a rectangle texture
            this._buttonTexture = arrowButton;

            this.Back = false;

            this.TitlePosition = new Vector2(screen.OriginalScreenSize.X / 0.98f, screen.OriginalScreenSize.Y / 3.3f);

            

            ArrowButtonList.Add(
                new ArrowButton(this._buttonTexture, font)
                {
                    Text = "Full screen",
                    Position = new Vector2(screen.OriginalScreenSize.X / 0.78f, screen.OriginalScreenSize.Y / 1.80f),
                    EnableMode = true,
                    State = true,
                    Scale = 0.7f
                }
                );

            ArrowButtonList.Add(
                new ArrowButton(this._buttonTexture, font)
                {
                    Text = "Refresh rate",
                    Position = new Vector2(this.ArrowButtonList[0].Position.X, GetArrowButtonPosition()),
                    Value = 60,
                    MinValue = 10,
                    MaxValue = 140,
                    Scale = 0.7f
                }
                ); ;

            ArrowButtonList.Add(
                new ArrowButton(this._buttonTexture, font)
                {
                    Text = "Refresh rate display",
                    Position = new Vector2(this.ArrowButtonList[0].Position.X, GetArrowButtonPosition()),
                    EnableMode = true,
                    State = false,
                    Scale = 0.7f
                }
                );

            ArrowButtonList.Add(
                new ArrowButton(this._buttonTexture, font)
                {
                    Text = "SFX volume",
                    Position = new Vector2(this.ArrowButtonList[0].Position.X, GetArrowButtonPosition()),
                    Value = 10,
                    MinValue = 0,
                    MaxValue = 15,
                    Scale = 0.7f
                }
                );

            ArrowButtonList.Add(
                new ArrowButton(this._buttonTexture, font)
                {
                    Text = "Music volume",
                    Position = new Vector2(this.ArrowButtonList[0].Position.X, GetArrowButtonPosition()),
                    Value = 10,
                    MinValue = 0,
                    MaxValue = 15,
                    Scale = 0.7f
                }
                );

            // Create a rectangle texture
            _buttonTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            _buttonTexture.SetData(new Color[] { Color.White });


            Button btnSettings = new Button(_buttonTexture, font)
            {
                Text = "Settings",
                Position = new Vector2(screen.OriginalScreenSize.X / 0.98f, screen.OriginalScreenSize.Y / 1.6f),
                IsSelected = true,
                Scale = 0.65f
            };
            // Assign the event
            btnSettings.Click += BtnSettings_Click;
            // Add the button in the list
            this._buttonList.Add(btnSettings);

            Button btnControls = new Button(_buttonTexture, font)
            {
                Text = "Controls",
                Position = new Vector2(this.ButtonList[0].Position.X, GetButtonPosition()),
                Scale = 0.65f
            };
            // Assign the event
            btnControls.Click += BtnControls_Click;
            // Add the button in the list
            this.ButtonList.Add(btnControls);


            Button btnBack = new Button(_buttonTexture, font)
            {
                Text = "Back",
                Position = new Vector2(this.ButtonList[0].Position.X, GetButtonPosition()),
                Scale = 0.65f
            };
            // Assign the event
            btnBack.Click += BtnBack_Click;
            // Add the button in the list
            this.ButtonList.Add(btnBack);


            GetTitle();
        }

        public void Update(GameTime gameTime, Screen screen)
        {
            this.TitlePosition = new Vector2(this.TitlePosition.X + this.ChangePosition, this.TitlePosition.Y);

            // Scroll through the arrow button list
            int cpt = 0;
            foreach (var item in this.ArrowButtonList)
            {
                item.Update(gameTime, screen, this.ChangePosition);

                item.Position = new Vector2(item.Position.X + this.ChangePosition, item.Position.Y);

                if (item.Clicked)
                {
                    switch (cpt)
                    {
                        // Windowed screen or Full screen
                        case 0:
                            if (item.State)
                                screen.FullScreen();
                            else
                                screen.WindowedScreen();
                            break;

                        // Limit the refresh rate
                        case 1:
                            break;

                        // The refresh rate display
                        case 2:
                            if (item.State)
                                Game1.showFps = true;
                            else
                                Game1.showFps = false;
                            break;

                        // SFX volume
                        case 3:
                            break;

                        // Music volume
                        case 4:
                            break;

                        default:
                            break;
                    }
                }

                cpt += 1;
            }

            // Scroll through the button list
            cpt = 0;
            foreach (var item in this.ButtonList)
            {
                item.Update(gameTime, screen);

                item.Position = new Vector2(item.Position.X + this.ChangePosition, item.Position.Y);

                if (item.Clicked)
                {
                    switch (cpt)
                    {
                        case 0:
                            break;

                        case 1:
                            break;

                        case 2:
                            this.Back = true;
                            this.ButtonList[cpt].Clicked = false;
                            break;

                        default:
                            break;
                    }
                }

                cpt += 1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this._font, this._tmpTitle, this.TitlePosition, Color.White);

            foreach (var item in this.ArrowButtonList)
            {
                item.Draw(spriteBatch);
            }

            foreach (var item in this.ButtonList)
            {
                item.Draw(spriteBatch);
            }
        }


        private float GetArrowButtonPosition()
        {
            return this.ArrowButtonList[this.ArrowButtonList.Count -1].Position.Y + 50;
        }

        private float GetButtonPosition()
        {
            return this.ButtonList[this.ButtonList.Count - 1].Position.Y + 35;
        }

        private string GetTitle()
        {
            foreach (var item in this.ButtonList)
            {
                if (item.IsSelected)
                {
                    return this._tmpTitle = String.Format("{0}{1}", this._title, item.Text);
                }
            }

            return "";
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            foreach (var item in ButtonList)
            {
                item.IsSelected = false;
            }

            btn.IsSelected = true;
            
            GetTitle();
        }

        private void BtnControls_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            foreach (var item in ButtonList)
            {
                item.IsSelected = false;
            }

            btn.IsSelected = true;

            GetTitle();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            btn.Clicked = true;
        }
    }
}
