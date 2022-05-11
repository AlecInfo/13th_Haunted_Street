/********************************
 * Project : 13th Haunted Street
 * Description : This class Settings allows you to list all 
 *               value for the menus
 * Date : 13/04/2022
 * Author : Piette Alec
*******************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace _13thHauntedStreet
{
    static class Settings
    {
        #region Default values

        // Name of the xml file and it's located "13th Haunted Street\13th_Haunted_Street\13thHauntedStreet\bin\Debug\netcoreapp3.1\BackupMenu.xml"
        public static string fileSave = "BackupMenu.xml";

        // Dictionary with the default values
        private static Dictionary<string, string> _defaultValues = new Dictionary<string, string>()
        {
            { GetTitleFullscreen(), "Enabled" },
            { GetTitleRefreshRate(), "60" },
            { GetTitleRefreshRateDisplay(), "Disabled" },
            { GetTitleSFXVolume(), "7" },
            { GetTitleMusicVolume(), "7" },
        };


        // Dictionary with the list of the controls
        public static Dictionary<string, string> listControls = new Dictionary<string, string>()
        {
            { GetTitleMoveUp(), Game1.input.Up.ToString() },
            { GetTitleMoveLeft(), Game1.input.Left.ToString() },
            { GetTitleMoveDown(), Game1.input.Down.ToString() },
            { GetTitleMoveRight(), Game1.input.Right.ToString() },
            { GetTitleWeaponOne(), Game1.input.ItemUp.ToString() },
            { GetTitleWeaponTwo(), Game1.input.ItemDown.ToString() },
            { GetTitleAttack(), Game1.input.Use1.ToString() },
            { GetTitleTransform(), Game1.input.Use1.ToString() },
            { GetTitleDetransform(), Game1.input.Use2.ToString() },
        };

        #endregion

        #region Variables

        private static string _fullscreen;

        private static string _refreshRate;

        private static string _refreshRateDisplay;

        private static string _sfxVolume;

        private static string _musicVolume;

        #endregion

        /// <summary>
        /// This method allows to return the id from the default value according to the list of value,
        /// the default value and the list of value are parametres
        /// </summary>
        /// <param name="defaultSetting"></param>
        /// <param name="listValues"></param>
        /// <returns></returns>
        private static int GetIDValue(string defaultSetting, List<string> listValues)
        {
            // Browse the list of values
            for (int i = 0; i < listValues.Count; i++)
            {
                // Check if the default value is the same as the list value
                if (defaultSetting == listValues[i])
                {
                    // Return its value
                    return i;
                }
            }
            return 0;
        }

        #region Set the values 

        /// <summary>
        /// This method allows to define the varriables depending on whether there is a backup
        /// </summary>
        public static void SetDefautlValue()
        {
            // If the file exists 
            if (File.Exists(fileSave))
            {
                // Recover the data int to parameterList
                Dictionary<string, string> parameterList = new Dictionary<string, string>();
                XmlSerializer serializer = new XmlSerializer(typeof(Setting[]), new XmlRootAttribute() { ElementName = "settings" });

                using (StreamReader stream = new StreamReader(Settings.fileSave))
                {
                    parameterList = ((Setting[])serializer.Deserialize(stream)).ToDictionary(i => i.id, i => i.value);
                }

                // And apply the parameterList values 
                ChangeValues(parameterList);
            }
            else
            {
                // Apply the default values 
                ChangeValues(_defaultValues);
            }

        }

        public static void ChangeValues(Dictionary<string, string> dictio)
        {
            // Set the variables with the default values
            foreach (var item in dictio)
            {
                if (item.Key == Settings.GetTitleFullscreen())
                {
                    _fullscreen = item.Value;
                    FullscreenAction(item.Value);
                }
                else if (item.Key == Settings.GetTitleRefreshRate())
                {
                    _refreshRate = item.Value;
                    RefreshRateAction(item.Value);
                }
                else if (item.Key == Settings.GetTitleRefreshRateDisplay())
                {
                    _refreshRateDisplay = item.Value;
                    RefreshRateDisplayAction(item.Value);
                }
                else if (item.Key == Settings.GetTitleSFXVolume())
                {
                    _sfxVolume = item.Value;
                    SFXVolumeAction(item.Value);
                }
                else if (item.Key == Settings.GetTitleMusicVolume())
                {
                    _musicVolume = item.Value;
                    MusicVolumeAction(item.Value);
                }
            }
        }

        #endregion

        #region Get the button action
        /// <summary>
        /// This method is used to retrieve the action of the button for the setting menu,
        /// the parametres of the method are the name of the button and the value
        /// </summary>
        /// <param name="button"></param>
        /// <param name="state"></param>
        public static void ButtonAction(string button, string state)
        {
            // The action of the fullscreen menu
            if (button == GetTitleFullscreen())
            {
                // Put the window in fullscreen
                FullscreenAction(state);
            }

            // The action of the refresh rate menu
            if (button == GetTitleRefreshRate())
            {
                // Get the value and apply the limit
                RefreshRateAction(state);
            }

            // The action of the refresh rate display menu
            if (button == GetTitleRefreshRateDisplay())
            {
                RefreshRateDisplayAction(state);
            }

            // The action of the effect volume menu
            if (button == GetTitleSFXVolume())
            {
                // Get the value and apply the volume
                SFXVolumeAction(state);
            }

            // The action of the music volume menu
            if (button == GetTitleMusicVolume())
            {
                // Get the value and apply the volume
                MusicVolumeAction(state);
            }

            // The action of the change pages button
            if (button == GetTitlePagesButton())
            {
                SettingsMenu.ChangePage(Convert.ToInt32(state) - 1);
            }
        }

        #endregion

        #region Get Title of the pages

        /// <summary>
        /// This method returns the first part of the menu title
        /// </summary>
        /// <returns></returns>
        public static string GetTitleSettings()
        {
            // Return the title
            return "Settings : ";
        }
        #endregion

        #region Get Fullscreen

        /// <summary>
        /// This method allows to return the title text of the button full screen
        /// </summary>
        /// <returns></returns>
        public static string GetTitleFullscreen()
        {
            // Return the title
            return "Full Screen";
        }

        /// <summary>
        /// This method allows to return the list of values ​​that the button has
        /// </summary>
        /// <returns></returns>
        public static List<string> GetValuesFullscreen()
        {
            // Returns a list with the value disabled and enabled
            return new List<string>() { "Disabled", "Enabled" };
        }

        /// <summary>
        /// This method allows to get the id of the default value and return it
        /// </summary>
        /// <returns></returns>
        public static int GetFullscreenID()
        {
            // Return the id
            return GetIDValue(_fullscreen, GetValuesFullscreen());
        }

        /// <summary>
        /// This method is the action after clicking the button, it changes the screen either in fullscreen or windowed
        /// </summary>
        /// <param name="state"></param>
        private static void FullscreenAction(string state)
        {
            // Put the window in fullscreen
            if (state == "Enabled")
                Game1.self.screen.FullScreen();
            // Put the window in windowed
            else if (state == "Disabled")
                Game1.self.screen.WindowedScreen();
        }
        #endregion

        #region Get Refresh rate


        /// <summary>
        /// This method allows to return the title text of the button refresh rate
        /// </summary>
        /// <returns></returns>
        public static string GetTitleRefreshRate()
        {
            // Return the title
            return "Refresh rate";
        }

        /// <summary>
        /// This method allows to return the list of values ​​that the button has
        /// </summary>
        /// <returns></returns>
        public static List<string> GetValuesRefreshRate()
        {
            // Create the list
            List<string> listValue = new List<string>();

            // The max value 
            int maxValue = 120;
            // The min value
            int minValue = 30;
            // The spacing between list values
            int spacingValue = 10;


            // Add to the list all the values ​​between the max and min value
            for (int i = minValue; i <= maxValue; i += spacingValue)
            {
                listValue.Add(i.ToString());
            }

            // Return the list with the values
            return listValue;
        }

        /// <summary>
        /// This method allows to get the id of the default value and return it
        /// </summary>
        /// <returns></returns>
        public static int GetRefreshRateID()
        {
            // Return the id
            return GetIDValue(_refreshRate, GetValuesRefreshRate());
        }

        /// <summary>
        /// This method is the action after clicking the button, it changes the limit fps 
        /// </summary>
        /// <param name="state"></param>
        private static void RefreshRateAction(string state)
        {
            // Get the value and apply the limit
            Game1.self.limitedFps = Convert.ToInt32(state);
        }
        #endregion

        #region Get Refresh rate Display

        /// <summary>
        /// This method allows to return the title text of the button refresh rate display
        /// </summary>
        /// <returns></returns>
        public static string GetTitleRefreshRateDisplay()
        {
            // Return the title
            return "Refresh rate display";
        }

        /// <summary>
        /// This method allows to return the list of values ​​that the button has
        /// </summary>
        /// <returns></returns>
        public static List<string> GetValuesRefreshRateDisplay()
        {
            // Returns a list with the value disabled and enabled
            return new List<string>() { "Disabled", "Enabled" };
        }

        /// <summary>
        /// This method allows to get the id of the default value and return it
        /// </summary>
        /// <returns></returns>
        public static int GetRefreshRateDisplayID()
        {
            // Return the id
            return GetIDValue(_refreshRateDisplay, GetValuesRefreshRateDisplay());
        }

        /// <summary>
        /// This method is the action after clicking the button, it changes whether or not it displays fps
        /// </summary>
        /// <param name="state"></param>
        private static void RefreshRateDisplayAction(string state)
        {
            // Display the rifresh rate 
            if (state == "Enabled")
                Game1.self.showFps = true;
            // Undisplay the rifresh rate
            else if (state == "Disabled")
                Game1.self.showFps = false;
        }
        #endregion

        #region Get SFX volume

        /// <summary>
        /// This method allows to return the title text of the button effect volume
        /// </summary>
        /// <returns></returns>
        public static string GetTitleSFXVolume()
        {
            // Return the title
            return "Effect volume";
        }

        /// <summary>
        /// This method allows to return the list of values ​​that the button has
        /// </summary>
        /// <returns></returns>
        public static List<string> GetValuesSFXVolume()
        {
            // Create the list
            List<string> listValue = new List<string>();

            // The max value 
            int maxValue = 10;
            // The min value
            int minValue = 0;
            // The spacing between list values
            int spacingValue = 1;


            // Add to the list all the values ​​between the max and min value
            for (int i = minValue; i <= maxValue; i += spacingValue)
            {
                listValue.Add(i.ToString());
            }

            // Return the list with the values
            return listValue;
        }

        /// <summary>
        /// This method allows to get the id of the default value and return it
        /// </summary>
        /// <returns></returns>
        public static int GetSFXVolumeID()
        {
            // Return the id
            return GetIDValue(_sfxVolume, GetValuesSFXVolume());
        }

        /// <summary>
        /// This method is the action after clicking the button, it changes the SFX volume values
        /// </summary>
        /// <param name="state"></param>
        private static void SFXVolumeAction(string state)
        {
            // Get the value and apply the volume
            Game1.self.sfxVolume = Convert.ToInt32(state);
        }

        #endregion

        #region Get Music volume

        /// <summary>
        /// This method allows to return the title text of the button music volume
        /// </summary>
        /// <returns></returns>
        public static string GetTitleMusicVolume()
        {
            // Return the title
            return "Music volume";
        }

        /// <summary>
        /// This method allows to return the list of values ​​that the button has
        /// </summary>
        /// <returns></returns>
        public static List<string> GetValuesMusicVolume()
        {
            // Create the list
            List<string> listValue = new List<string>();

            // The max value 
            int maxValue = 10;
            // The min value
            int minValue = 0;
            // The spacing between list values
            int spacingValue = 1;

            // Add to the list all the values ​​between the max and min value
            for (int i = minValue; i <= maxValue; i += spacingValue)
            {
                listValue.Add(i.ToString());
            }

            // Return the list with the values
            return listValue;
        }

        /// <summary>
        /// This method allows to get the id of the default value and return it
        /// </summary>
        /// <returns></returns>
        public static int GetMusicVolumeID()
        {
            // Return the id
            return GetIDValue(_musicVolume, GetValuesSFXVolume());
        }

        /// <summary>
        /// This method is the action after clicking the button, it changes the music volume values
        /// </summary>
        /// <param name="state"></param>
        private static void MusicVolumeAction(string state)
        {
            // Get the value and apply the volume
            Game1.self.musicVolume = Convert.ToInt32(state);
        }
        #endregion

        #region Get the control title
        public static string GetTitlePagesButton()
        {
            return "Pages Control";
        }

        public static string GetTitleMoveUp()
        {
            return "Move up";
        }
        public static string GetTitleMoveLeft()
        {
            return "Move left";
        }
        public static string GetTitleMoveDown()
        {
            return "Move down";
        }
        public static string GetTitleMoveRight()
        {
            return "Move right";
        }
        public static string GetTitleWeaponOne()
        {
            return "Weapon one";
        }
        public static string GetTitleWeaponTwo()
        {
            return "Weapon two";
        }
        public static string GetTitleAttack()
        {
            return "Attack";
        }
        public static string GetTitleTransform()
        {
            return "Transform";
        }
        public static string GetTitleDetransform()
        {
            return "Detransform";
        }
        #endregion


    }
}
