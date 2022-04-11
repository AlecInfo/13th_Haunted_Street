using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class MainMenuSettingOld
    {
        #region Fields

        private Texture2D _buttonTexture;

        private SpriteFont _font;

        private string _title = "Game option : ";

        private string _tmpTitle = "";

        private float _changePosition = 0;

        private List<ButtonOld> _tabList = new List<ButtonOld>();

        private List<ArrowButtonOld> _settingsList = new List<ArrowButtonOld>();

        private List<object> _controlsList = new List<object>();

        private List<object> _displayedList = new List<object>();

        #endregion

        #region Proporties
        public Vector2 TitlePosition { get; set; }

        public bool Back { get; set; }

        public float ChangePosition { get => _changePosition; set => _changePosition = value; }

        public List<ButtonOld> TabList { get => _tabList; set => _tabList = value; }

        public List<ArrowButtonOld> SettingsList { get => _settingsList; set => _settingsList = value; }

        public List<object> ControlsList { get => _controlsList; set => _controlsList = value; }

        public List<object> DisplayedList { get => _displayedList; set => _displayedList = value; }

        #endregion

        public void Load(Screen screen, SpriteFont font, Texture2D arrowButton)
        {
            // Get the font
            this._font = font;

            // Get arrow texture and create a rectangle with it
            this._buttonTexture = arrowButton;

            // Initialize the back variable
            this.Back = false;

            // Initialize the title position
            this.TitlePosition = new Vector2(Screen.OriginalScreenSize.X / 0.98f, Screen.OriginalScreenSize.Y / 3.3f);

            // Add Full screen arrow button to the list
            SettingsList.Add(
                new ArrowButtonOld(this._buttonTexture, font)
                {
                    Text = "Full screen",
                    Position = new Vector2(Screen.OriginalScreenSize.X / 0.78f, Screen.OriginalScreenSize.Y / 1.80f),
                    EnableMode = true,
                    State = screen.WindowsSizeIsEqualScreenSize(),
                    Scale = 0.7f
                }
                );

            // Add Refresh rate arrow button to the list
            SettingsList.Add(
                new ArrowButtonOld(this._buttonTexture, font)
                {
                    Text = "Refresh rate",
                    Position = new Vector2(this.SettingsList[0].Position.X, GetArrowButtonPosition()),
                    //Value = Game1.limitedFps,
                    MinValue = 10,
                    MaxValue = 144,
                    Scale = 0.7f
                }
                );

            // Add Refresh rate display arrow button to the list
            SettingsList.Add(
                new ArrowButtonOld(this._buttonTexture, font)
                {
                    Text = "Refresh rate display",
                    Position = new Vector2(this.SettingsList[0].Position.X, GetArrowButtonPosition()),
                    EnableMode = true,
                    //State = Game1.showFps,
                    Scale = 0.7f
                }
                );

            // Add SFX volume arrow button to the list
            SettingsList.Add(
                new ArrowButtonOld(this._buttonTexture, font)
                {
                    Text = "SFX volume",
                    Position = new Vector2(this.SettingsList[0].Position.X, GetArrowButtonPosition()),
                    //Value = Game1.sfxVolume,
                    MinValue = 0,
                    MaxValue = 10,
                    Scale = 0.7f
                }
                );

            // Add Music volume arrow button to the list
            SettingsList.Add(
                new ArrowButtonOld(this._buttonTexture, font)
                {
                    Text = "Music volume",
                    Position = new Vector2(this.SettingsList[0].Position.X, GetArrowButtonPosition()),
                    //Value = Game1.musicVolume,
                    MinValue = 0,
                    MaxValue = 10,
                    Scale = 0.7f
                }
                );


            // Create a rectangle texture to one per one pixel
            _buttonTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            _buttonTexture.SetData(new Color[] { Color.White });

            // Create the settings button
            ButtonOld btnSettings = new ButtonOld(_buttonTexture, font)
            {
                Text = "Settings",
                Position = new Vector2(Screen.OriginalScreenSize.X / 0.98f, Screen.OriginalScreenSize.Y / 1.6f),
                IsSelected = true,
                Scale = 0.65f
            };
            // Assign the event
            btnSettings.Click += BtnSettings_Click;
            // Add the button in the list
            this._tabList.Add(btnSettings);

            // Create the controls button
            ButtonOld btnControls = new ButtonOld(_buttonTexture, font)
            {
                Text = "Controls",
                Position = new Vector2(this.TabList[0].Position.X, GetButtonPosition()),
                Scale = 0.65f
            };
            // Assign the event
            btnControls.Click += BtnControls_Click;
            // Add the button in the list
            this.TabList.Add(btnControls);

            // Create the back button
            ButtonOld btnBack = new ButtonOld(_buttonTexture, font)
            {
                Text = "Back",
                Position = new Vector2(this.TabList[0].Position.X, GetButtonPosition()),
                Scale = 0.65f
            };
            // Assign the event
            btnBack.Click += BtnBack_Click;
            // Add the button in the list
            this.TabList.Add(btnBack);

            // Change the title text according to the button selected like Settings or Controls
            GetTitle();

            // Define which tab to display
            //this.DisplayedList = (objecthis.SettingsList;
        }
        /*
        public void Update(GameTime gameTime, Screen screen)
        {
            // Update the title position 
            this.TitlePosition = new Vector2(this.TitlePosition.X + this.ChangePosition, this.TitlePosition.Y);

            // Scroll through the arrow button list
            int cpt = 0;
            foreach (var item in this.SettingsList)
            {
                // Update the arrow button 
                item.Update(gameTime, screen, this.ChangePosition);
                // Update the position
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
                            Game1.limitedFps = item.Value;
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
                            Game1.sfxVolume = item.Value;
                            break;

                        // Music volume
                        case 4:
                            Game1.musicVolume = item.Value;
                            break;

                        default:
                            break;
                    }
                }

                cpt += 1;
            }

            // Scroll through the button list
            cpt = 0;
            foreach (var item in this.TabList)
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
                            this.TabList[cpt].Clicked = false;
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

            foreach (var item in this.SettingsList)
            {
                item.Draw(spriteBatch);
            }

            foreach (var item in this.TabList)
            {
                item.Draw(spriteBatch);
            }
        }
        */

        private float GetArrowButtonPosition()
        {
            return this.SettingsList[this.SettingsList.Count -1].Position.Y + 50;
        }

        private float GetButtonPosition()
        {
            return this.TabList[this.TabList.Count - 1].Position.Y + 35;
        }

        private string GetTitle()
        {
            foreach (var item in this.TabList)
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
            ButtonOld btn = (ButtonOld)sender;

            foreach (var item in TabList)
            {
                item.IsSelected = false;
            }

            btn.IsSelected = true;
            
            GetTitle();
        }

        private void BtnControls_Click(object sender, EventArgs e)
        {
            ButtonOld btn = (ButtonOld)sender;

            foreach (var item in TabList)
            {
                item.IsSelected = false;
            }

            btn.IsSelected = true;

            GetTitle();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            ButtonOld btn = (ButtonOld)sender;

            btn.Clicked = true;
        }
    }
}
