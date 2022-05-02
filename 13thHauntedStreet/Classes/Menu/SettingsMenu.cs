/********************************
 * Project : 13th Haunted Street
 * Description : This class SettingsMenu allows you to create a parameter menu 
 *               with all the actions and display
 * Date : 13/04/2022
 * Author : Piette Alec
*******************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PostSharp.Serialization.Serializers;
using Rebus.Serialization;
using SharpYaml.Serialization.Serializers;

namespace _13thHauntedStreet
{
    public class SettingsMenu : ItemsMenu
    {
        #region Variables
        private bool _displayMenuGameplay = true;

        private ItemsMenu menuControl = new ItemsMenu();

        private ItemsMenu menuGameplay = new ItemsMenu();

        Action callback;
        #endregion

        // Ctor
        public SettingsMenu(Vector2 position, Texture2D backgroundTexture, Texture2D buttonTexture, SpriteFont font)
        {
            Texture2D buttonTextureDefault = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);

            this.Position = position;

            this._font = font;

            // Button of the global menu settings
            // Action of the gameplay button,
            // which displays all gameplay parameters
            callback = () => { _displayMenuGameplay = true; };
            Func<bool> func = () => false;
            // Create the button gameplay
            Add(SettingsMenu.NewButton("Gameplay", _font, new Vector2(Screen.OriginalScreenSize.X / 0.98f, Screen.OriginalScreenSize.Y / 1.6f), buttonTextureDefault, 0.65f, SpriteEffects.None, callback, func));

            // Action of the control button,
            // which display all gameplay parameters
            callback = () => { _displayMenuGameplay = false; };
            // Create the button control
            Add(SettingsMenu.NewButton("Control", _font, new Vector2(Screen.OriginalScreenSize.X / 0.98f, Screen.OriginalScreenSize.Y / 1.53f), buttonTextureDefault, 0.65f, SpriteEffects.None, callback, func));

            // Action of the back button,
            // which replay the animation and going back to the main menu
            callback = () => { 
                MainMenu.animationStarted = true;

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
            Add(SettingsMenu.NewButton("Back", _font, new Vector2(Screen.OriginalScreenSize.X / 0.98f, Screen.OriginalScreenSize.Y / 1.47f), buttonTextureDefault, 0.65f, SpriteEffects.None, callback, func));

            // Item list of the gameplay settings
            // The title
            menuGameplay.Add(SettingsMenu.NewText(Settings.GetTitleSettings() + "Gameplay", _font, new Vector2(Screen.OriginalScreenSize.X / 0.98f, Screen.OriginalScreenSize.Y / 3.3f), 1f));
            // The fullscreen option
            LineOption(buttonTexture, Settings.GetValuesFullscreen(), Settings.GetTitleFullscreen(), Screen.OriginalScreenSize.Y / 1.80f, Settings.GetFullscreenID(), menuGameplay);
            // The refresh rate option
            LineOption(buttonTexture, Settings.GetValuesRefreshRate(), Settings.GetTitleRefreshRate(), SpacingOption(menuGameplay), Settings.GetRefreshRateID(), menuGameplay);
            // The refresh rate display option
            LineOption(buttonTexture, Settings.GetValuesRefreshRateDisplay(), Settings.GetTitleRefreshRateDisplay(), SpacingOption(menuGameplay), Settings.GetRefreshRateDisplayID(), menuGameplay);
            // The effects volume option
            LineOption(buttonTexture, Settings.GetValuesSFXVolume(), Settings.GetTitleSFXVolume(), SpacingOption(menuGameplay), Settings.GetSFXVolumeID(), menuGameplay);
            // The musics volume option
            LineOption(buttonTexture, Settings.GetValuesMusicVolume(), Settings.GetTitleMusicVolume(), SpacingOption(menuGameplay), Settings.GetMusicVolumeID(), menuGameplay);

            // Item list of the control settings
            //int positionX 
            // The title
            menuControl.Add(SettingsMenu.NewText(Settings.GetTitleSettings() + "Controls", _font, new Vector2(Screen.OriginalScreenSize.X / 0.98f, Screen.OriginalScreenSize.Y / 3.3f), 1f));

        }

        public override void Update(GameTime gameTime, Screen screen, ref Vector2 changePosition)
        {
            base.Update(gameTime, screen, ref changePosition);

            // Update the menu gameplay or control
            menuGameplay.Update(gameTime, screen, ref changePosition);
            menuControl.Update(gameTime, screen, ref changePosition);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            // Display the menu gameplay or the menu control
            if (_displayMenuGameplay)
            {
                menuGameplay.Draw(gameTime, spriteBatch);
            }
            else
            {
                menuControl.Draw(gameTime, spriteBatch);
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
        private void LineOption(Texture2D buttonTexture, List<string> listValue, string titleOption, float positionY, int defaultValue, ItemsMenu addingOption)
        {
            // The scale text
            float scale = 0.7f;

            // The value of the button
            ItemOption typeScreen = SettingsMenu.NewOption(listValue, _font, new Vector2(Screen.OriginalScreenSize.X / 0.8f + 105, positionY), scale, defaultValue, titleOption);

            // Left button 
            // The action of this
            Action callback = () => { typeScreen.Left(); Settings.ButtonAction(titleOption, typeScreen.GetValue()); };
            Func<bool> enabledButton = () => typeScreen.IsLeft();
            // Add the left button
            addingOption.Add(SettingsMenu.NewButton("", _font, new Vector2(Screen.OriginalScreenSize.X / 0.8f, positionY), buttonTexture, scale, SpriteEffects.None, callback, enabledButton));

            // The title of the line which adapts its position according to the position of the left button
            addingOption.Add(SettingsMenu.NewText(titleOption, _font, new Vector2(Screen.OriginalScreenSize.X / 0.8f - this._font.MeasureString(titleOption).X * scale - 20, positionY), scale));

            // Add the value of the button
            addingOption.Add(typeScreen);

            // Right button
            // The action of this
            callback = () => { typeScreen.Right(); Settings.ButtonAction(titleOption, typeScreen.GetValue()); };
            enabledButton = () => typeScreen.IsRight();
            // Add the right button
            addingOption.Add(SettingsMenu.NewButton("", _font, new Vector2(Screen.OriginalScreenSize.X / 0.8f + 170, positionY), buttonTexture, scale, SpriteEffects.FlipHorizontally, callback, enabledButton));
        }

        /// <summary>
        /// Retrieve the position of the previous item and change it
        /// </summary>
        /// <param name="itemsMenu"></param>
        /// <returns> flaot </returns>
        private float SpacingOption(ItemsMenu itemsMenu)
        {
            return itemsMenu.listItems[itemsMenu.listItems.Count - 1].Position.Y + 50;
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
        static ItemButtonOption NewButton(string text, SpriteFont font, Vector2 position, Texture2D texture, float scale, SpriteEffects spriteEffect, Action callback, Func<bool> enabledButton)
        {
            return new ItemButtonOption(texture, font, callback, enabledButton)
            {
                Text = text,
                Position = position,
                FontColor = Color.White,
                Scale = scale,
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
        static ItemOption NewOption(List<string> values, SpriteFont font, Vector2 position, float scale, int defaultValue, string titleOption)
        {
            return new ItemOption(font, values)
            {
                Position = position,
                Scale = scale,
                align = AlignItem.Center,
                Id = defaultValue,
                Name = titleOption,
            };
        }

        /// <summary>
        /// This method allows to create the list with menu control and menu gameplay
        /// </summary>
        /// <param name="parameters"></param>
        public override void ConstructParameterList(ref Dictionary<string, string> parameters)
        {
            this.menuControl.ConstructParameterList(ref parameters);
            this.menuGameplay.ConstructParameterList(ref parameters);
        }
    }
}
