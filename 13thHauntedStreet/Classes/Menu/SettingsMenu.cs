/********************************
 * Project : 13th Haunted Street
 * Description : This class SettingsMenu allows you to create a parameter menu 
 *               with all the actions and display:
 *               - the menu gameplay (refresh rate, fullscreen, display refresh rate and the volume)
 *               - the menu control (all control the game uses)
 * Date : 13/04/2022
 * Author : Piette Alec
*******************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _13thHauntedStreet
{
    public class SettingsMenu : ItemsMenu
    {
        #region Variables
        private bool _displayMenuGameplay = true;

        private ItemsMenu _menuControl = new ItemsMenu();

        public static ItemsMenu menuControlPage = new ItemsMenu();

        private ItemsMenu _menuGameplay = new ItemsMenu();

        public static List<ItemsMenu> listPages = new List<ItemsMenu>();

        private ItemsMenu _listPageElements = new ItemsMenu();

        Action callback;
        #endregion

        // Ctor
        public SettingsMenu(Vector2 position, Texture2D backgroundTexture, Texture2D buttonTexture, SpriteFont font)
        {
            Texture2D buttonTextureDefault = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);

            this.Position = position;

            _font = font;

            // Button of the global menu settings
            // Action of the gameplay button,
            // which displays all gameplay parameters
            callback = () => { _displayMenuGameplay = true; };
            Func<bool> func = () => false;
            // Create the button gameplay
            Add(SettingsMenu.NewButton("Gameplay", _font, new Vector2(Screen.OriginalScreenSize.X / 0.98f, Screen.OriginalScreenSize.Y / 1.6f), buttonTextureDefault, Color.White, 0.65f, SpriteEffects.None, callback, func, false, 0.65f));

            // Action of the control button,
            // which display all gameplay parameters
            callback = () => { _displayMenuGameplay = false; };
            // Create the button control
            Add(SettingsMenu.NewButton("Control", _font, new Vector2(Screen.OriginalScreenSize.X / 0.98f, Screen.OriginalScreenSize.Y / 1.53f), buttonTextureDefault, Color.White, 0.65f, SpriteEffects.None, callback, func, false, 0.65f));

            // Action of the Back button,
            callback = () =>
            {
                // Which replay the animation and going back to the main menu
                MainMenu.animationStarted = true;

                // Send the dictionnary to the serialise class
                Dictionary<string, string> parameterList = new Dictionary<string, string>();
                this.ConstructParameterList(ref parameterList);

                //When the user returns to the main menu, the settings are saved in an xml file
                XmlSerializer serializer = new XmlSerializer(typeof(Setting[]), new XmlRootAttribute() { ElementName = "settings" });
                using (StreamWriter stream = new StreamWriter(Settings.fileSave))
                {
                    serializer.Serialize(stream, parameterList.Select(kv => new Setting() { id = kv.Key, value = kv.Value }).ToArray());
                }

            };

            // Create the button back
            Add(SettingsMenu.NewButton("Back", _font, new Vector2(Screen.OriginalScreenSize.X / 0.98f, Screen.OriginalScreenSize.Y / 1.47f), buttonTextureDefault, Color.White, 0.65f, SpriteEffects.None, callback, func, false, 0.65f));

            // Item list of the gameplay settings
            // The title
            _menuGameplay.Add(SettingsMenu.NewText(Settings.GetTitleSettings() + "Gameplay", _font, new Vector2(Screen.OriginalScreenSize.X / 0.98f, Screen.OriginalScreenSize.Y / 3.3f), 1f));
            // The fullscreen option
            LineOption(buttonTexture, Settings.GetValuesFullscreen(), Screen.OriginalScreenSize.Y / 2.2f, Settings.GetFullscreenID(), _menuGameplay, Settings.GetTitleFullscreen(), true);
            // The refresh rate option
            LineOption(buttonTexture, Settings.GetValuesRefreshRate(), SpacingOption(_menuGameplay), Settings.GetRefreshRateID(), _menuGameplay, Settings.GetTitleRefreshRate(), true);
            // The refresh rate display option
            LineOption(buttonTexture, Settings.GetValuesRefreshRateDisplay(), SpacingOption(_menuGameplay), Settings.GetRefreshRateDisplayID(), _menuGameplay, Settings.GetTitleRefreshRateDisplay(), true);
            // The effects volume option
            LineOption(buttonTexture, Settings.GetValuesSFXVolume(), SpacingOption(_menuGameplay), Settings.GetSFXVolumeID(), _menuGameplay, Settings.GetTitleSFXVolume(), true);
            // The musics volume option
            LineOption(buttonTexture, Settings.GetValuesMusicVolume(), SpacingOption(_menuGameplay), Settings.GetMusicVolumeID(), _menuGameplay, Settings.GetTitleMusicVolume(), true);

            // Item list of the control settings
            // The title
            _menuControl.Add(SettingsMenu.NewText(Settings.GetTitleSettings() + "Controls", _font, new Vector2(Screen.OriginalScreenSize.X / 0.98f, Screen.OriginalScreenSize.Y / 3.3f), 1f));

            float posX = Screen.OriginalScreenSize.Y / 2.2f;

            // Allows you to count the element in to the page
            int elementsNumber = 1;

            // Get the list of controls and create the list with pages, all pages have at most six elements
            foreach (var item in Settings.listControls)
            {
                // Add the element
                LineOptionControl(Game1.self._controlButton, item.Key, posX, item.Value, _listPageElements);

                // change the x position so that the next one is below
                posX += 70;

                // If the page contains six elements or if the element is the last
                if ((elementsNumber % Settings.GetNumberElementsInPage()) == 0 || elementsNumber == Settings.listControls.Count)
                {
                    // add the page to the list of pages
                    listPages.Add(_listPageElements);

                    // Reset the x position so that it is at the beginning of the page
                    posX = Screen.OriginalScreenSize.Y / 2.2f;
                    // Reset the list elements
                    _listPageElements = new ItemsMenu();
                }
                elementsNumber += 1;
            }

            // Add in to the menu control page the all element of the page one
            foreach (var item in listPages[0].listItems)
            { 
                menuControlPage.listItems.Add(item);
            }

            // If the list of pages is bigger than one, create the button who allows you to change the pages
            if (listPages.Count > 1)
            {
                // Create a list with the all page number
                List<string> listPagesNumber = new List<string>();

                for (int pageNumber = 1; pageNumber <= listPages.Count; pageNumber++)
                {
                    listPagesNumber.Add(pageNumber.ToString());
                }

                // Create the button
                LineOption(buttonTexture, listPagesNumber, 981, 0, _menuControl, Settings.GetTitlePagesButton());
            }

        }

        public override void Update(GameTime gameTime, Screen screen, ref Vector2 changePosition)
        {
            base.Update(gameTime, screen, ref changePosition);

            // Update the menu gameplay or control
            _menuGameplay.Update(gameTime, screen, ref changePosition);
            _menuControl.Update(gameTime, screen, ref changePosition);
            menuControlPage.Update(gameTime, screen, ref changePosition);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            // Display the menu gameplay or the menu control
            if (_displayMenuGameplay)
            {
                _menuGameplay.Draw(gameTime, spriteBatch);
            }
            else
            {
                _menuControl.Draw(gameTime, spriteBatch);
                menuControlPage.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Create a line of item with a title, two buttons 
        /// and in the center text containing a value
        /// </summary>
        /// <param name="buttonTexture"></param>
        /// <param name="listValue"></param>
        /// <param name="titleOption"></param>
        /// <param name="positionY"></param>
        /// <param name="defaultValue"></param>
        /// <param name="addingOption"></param>
        private void LineOption(Texture2D buttonTexture, List<string> listValue, float positionY, int defaultValue, ItemsMenu addingOption, string titleOption = "", bool drawTitle = false)
        {
            // The scale text
            float scale = 0.7f;

            // The position x
            float posX = Screen.OriginalScreenSize.X / 0.79f;

            // The value of the button
            ItemLineOption typeScreen = SettingsMenu.NewOption(listValue, _font, new Vector2(posX + 105, positionY), scale, defaultValue, titleOption);

            // Left button 
            // The action of this
            Action callback = () => { 
                typeScreen.Left(); 
                Settings.ButtonAction(titleOption, typeScreen.GetValue()); 
            };
            Func<bool> enabledButton = () => typeScreen.IsLeft();
            // Add the left button
            addingOption.Add(SettingsMenu.NewButton("", _font, new Vector2(posX, positionY), buttonTexture, Color.White, scale, SpriteEffects.None, callback, enabledButton, false, scale));

            if (drawTitle)
            {
                // The title of the line which adapts its position according to the position of the left button
                addingOption.Add(SettingsMenu.NewText(titleOption, _font, new Vector2(posX - _font.MeasureString(titleOption).X * scale - 20, positionY), scale));
            }


            // Add the value of the button
            addingOption.Add(typeScreen);

            // Right button
            // The action of this
            callback = () => { 
                typeScreen.Right(); 
                Settings.ButtonAction(titleOption, typeScreen.GetValue()); 
            };
            enabledButton = () => typeScreen.IsRight();
            // Add the right button
            addingOption.Add(SettingsMenu.NewButton("", _font, new Vector2(posX + 170, positionY), buttonTexture, Color.White, scale, SpriteEffects.FlipHorizontally, callback, enabledButton, false, scale));
        }

        /// <summary>
        /// Create a line of item with a title and one button, the button as a value 
        /// </summary>
        /// <param name="buttonTexture"></param>
        /// <param name="titleOption"></param>
        /// <param name="positionY"></param>
        /// <param name="defaultValue"></param>
        /// <param name="addingOption"></param>
        private void LineOptionControl(Texture2D buttonTexture, string titleOption, float positionY, string defaultValue, ItemsMenu addingOption)
        {
            // The scale text
            float scaleButton = 2.2f;
            float scaleText = 0.7f;

            // The position x
            float posX = Screen.OriginalScreenSize.X / 0.79f;

            ItemCatchOption buttonAction = SettingsMenu.NewCatch(null, _font);

            // The title of the line which adapts its position according to the position of the button
            addingOption.Add(SettingsMenu.NewText(titleOption, _font, new Vector2(posX - _font.MeasureString(titleOption).X * scaleText - 20, positionY), scaleText));

            // The button
            // The action of this
            Action callback = () => { buttonAction.Catch(); };
            Func<bool> enabledButton = () => false;
            addingOption.Add(SettingsMenu.NewButton(defaultValue, _font, new Vector2(posX, positionY), buttonTexture, Color.Black, scaleButton, SpriteEffects.None, callback, enabledButton, true, scaleText));

            
        }

        /// <summary>
        /// this method allows you to change the page in the control menu
        /// </summary>
        /// <param name="pageNumber"></param>
        public static void ChangePage(int pageNumber)
        {
            // Get the last x position 
            float posX = menuControlPage.listItems[1].Position.X;

            // Remove the all element in the list
            for (int i = menuControlPage.listItems.Count - 1; i >= 0; i--)
            {
                menuControlPage.listItems.RemoveAt(i);
            }

            int cpt = 0;

            // Add all element of the new page to the menuControlPage 
            foreach (var item in listPages[pageNumber].listItems)
            {
                if (cpt % 2 == 0)
                {
                    item.Position = new Vector2(posX - _font.MeasureString(item.Text).X * 0.7f - 20, item.Position.Y);
                }
                else
                {
                    item.Position = new Vector2(posX, item.Position.Y);
                }


                menuControlPage.listItems.Add(item);

                cpt += 1;
            }
        }

        /// <summary>
        /// Retrieve the position of the previous item and change it
        /// </summary>
        /// <param name="itemsMenu"></param>
        /// <returns> flaot </returns>
        private float SpacingOption(ItemsMenu itemsMenu)
        {
            return itemsMenu.listItems[itemsMenu.listItems.Count - 1].Position.Y + 70;
        }

        /// <summary>
        ///  Method for adding a new item Button
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="position"></param>
        /// <param name="texture"></param>
        /// <param name="scale"></param>
        /// <param name="spriteEffect"></param>
        /// <param name="callback"></param>
        /// <param name="enabledButton"></param>
        /// <returns> itemButtonOption </returns>
        static ItemButtonOption NewButton(string text, SpriteFont font, Vector2 position, Texture2D texture, Color color, float scale, SpriteEffects spriteEffect, Action callback, Func<bool> enabledButton, bool DrawImageText, float ScaleText)
        {
            return new ItemButtonOption(texture, font, callback, enabledButton, DrawImageText)
            {
                Text = text,
                Position = position,
                FontColor = color,
                Scale = scale,
                ScaleText = ScaleText,
                Effect = spriteEffect
            };
        }

        /// <summary>
        ///  Method for adding a new item Text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <returns> itemText </returns>
        static ItemText NewText(string text, SpriteFont font, Vector2 position, float scale)
        {
            return new ItemText(font, text)
            {
                Position = position,
                Scale = scale
            };
        }

        /// <summary>
        /// Method for adding a new item Option
        /// </summary>
        /// <param name="values"></param>
        /// <param name="font"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="defaultValue"></param>
        /// <returns> itemOption </returns>
        static ItemLineOption NewOption(List<string> values, SpriteFont font, Vector2 position, float scale, int defaultValue, string titleOption)
        {
            return new ItemLineOption(font, values)
            {
                Position = position,
                Scale = scale,
                align = AlignItem.Center,
                Id = defaultValue,
                Name = titleOption,
            };
        }

        /// <summary>
        /// Under development
        /// Allows you to get the keyboard and mouse state
        /// </summary>
        /// <param name="values"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        static ItemCatchOption NewCatch(List<string> values, SpriteFont font)
        {
            return new ItemCatchOption(font, values) { };
        }

        /// <summary>
        /// This method allows to create the list with menu control and menu gameplay
        /// </summary>
        /// <param name="parameters"></param>
        public override void ConstructParameterList(ref Dictionary<string, string> parameters)
        {
            this._menuControl.ConstructParameterList(ref parameters);
            this._menuGameplay.ConstructParameterList(ref parameters);
        }
    }
}
