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
using System.Text;
using System.Xml.Serialization;

namespace _13thHauntedStreet
{
    static class Settings
    {
        #region Default values

        public static string fileSave = "BackupMenu.xml";

        private static string _fullscreenDefault = "Enabled";

        private static string _refreshRateDefault = "60";

        private static string _refreshRateDisplayDefault = "Disabled";

        private static string _sfxVolumeDefault = "7";

        private static string _musicVolumeDefault = "7";

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

        #region Set the default value 

        /// <summary>
        /// This method allows to define the varriables depending on whether there is a backup
        /// </summary>
        public static void SetDefautlValue()
        {
            // if the file exists 
            if (File.Exists(fileSave))
            {
                SaveSettings saveSettings;

                XmlSerializer restore = new XmlSerializer(typeof(SaveSettings));

                // Restore the data
                using (StreamReader item = new StreamReader(fileSave))
                {
                    saveSettings = (SaveSettings)restore.Deserialize(item);
                }

                // And set the variables with him
                _fullscreen = saveSettings.Fullscreen;

                _refreshRate = saveSettings.RefreshRate;

                _refreshRateDisplay = saveSettings.RefreshRateDisplay;

                _sfxVolume = saveSettings.SfxVolume;

                _musicVolume = saveSettings.MusicVolume;
            }
            else
            {
                // Set the variables with the default values
                _fullscreen = _fullscreenDefault;

                _refreshRate = _refreshRateDefault;

                _refreshRateDisplay = _refreshRateDisplayDefault;

                _sfxVolume = _sfxVolumeDefault;

                _musicVolume = _musicVolumeDefault;
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
            if (button == getTitleFullscreen())
            {
                // Put the window in fullscreen
                if (state == "Enabled")
                    Game1.self.screen.FullScreen();
                // Put the window in windowed
                else if (state == "Disabled")
                    Game1.self.screen.WindowedScreen();
            }

            // The action of the refresh rate menu
            if (button == getTitleRefreshRate())
            {
                // Get the value and apply the limit
                Game1.self.limitedFps = Convert.ToInt32(state);
            }

            // The action of the refresh rate display menu
            if (button == getTitleRefreshRateDisplay())
            {
                // Display the rifresh rate 
                if (state == "Enabled")
                    Game1.self.showFps = true;
                // Undisplay the rifresh rate
                else if (state == "Disabled")
                    Game1.self.showFps = false;
            }

            // The action of the effect volume menu
            if (button ==getTitleSFXVolume())
            {
                // Get the value and apply the volume
                Game1.self.sfxVolume = Convert.ToInt32(state);
            }

            // The action of the music volume menu
            if (button == getTitleMusicVolume())
            {
                // Get the value and apply the volume
                Game1.self.musicVolume = Convert.ToInt32(state);
            }
        }
        #endregion

        #region Get Title settings

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
        public static string getTitleFullscreen() 
        {
            // Return the title
            return "Full Screen";
        }

        /// <summary>
        /// This method allows to return the list of values ​​that the button has
        /// </summary>
        /// <returns></returns>
        public static List<string> getValuesFullscreen()
        {
            // Returns a list with the value disabled and enabled
            return new List<string>() { "Disabled", "Enabled" };
        }

        /// <summary>
        /// This method allows to get the id of the default value and return it
        /// </summary>
        /// <returns></returns>
        public static int getFullscreenID()
        {
            // Return the id
            return GetIDValue(_fullscreen, getValuesFullscreen());
        }
        #endregion

        #region Get Refresh rate


        /// <summary>
        /// This method allows to return the title text of the button refresh rate
        /// </summary>
        /// <returns></returns>
        public static string getTitleRefreshRate()
        {
            // Return the title
            return "Refresh rate";
        }

        /// <summary>
        /// This method allows to return the list of values ​​that the button has
        /// </summary>
        /// <returns></returns>
        public static List<string> getValuesRefreshRate()
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
        public static int getRefreshRateID()
        {
            // Return the id
            return GetIDValue(_refreshRate, getValuesRefreshRate());
        }
        #endregion

        #region Get Refresh rate Display

        /// <summary>
        /// This method allows to return the title text of the button refresh rate display
        /// </summary>
        /// <returns></returns>
        public static string getTitleRefreshRateDisplay()
        {
            // Return the title
            return "Refresh rate display";
        }

        /// <summary>
        /// This method allows to return the list of values ​​that the button has
        /// </summary>
        /// <returns></returns>
        public static List<string> getValuesRefreshRateDisplay()
        {
            // Returns a list with the value disabled and enabled
            return new List<string>() { "Disabled", "Enabled" };
        }

        /// <summary>
        /// This method allows to get the id of the default value and return it
        /// </summary>
        /// <returns></returns>
        public static int getRefreshRateDisplayID()
        {
            // Return the id
            return GetIDValue(_refreshRateDisplay, getValuesRefreshRateDisplay());
        }
        #endregion

        #region Get SFX volume

        /// <summary>
        /// This method allows to return the title text of the button effect volume
        /// </summary>
        /// <returns></returns>
        public static string getTitleSFXVolume()
        {
            // Return the title
            return "Effect volume";
        }

        /// <summary>
        /// This method allows to return the list of values ​​that the button has
        /// </summary>
        /// <returns></returns>
        public static List<string> getValuesSFXVolume()
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
        public static int getSFXVolumeID()
        {
            // Return the id
            return GetIDValue(_sfxVolume, getValuesSFXVolume());
        }

        #endregion

        #region Get Music volume

        /// <summary>
        /// This method allows to return the title text of the button music volume
        /// </summary>
        /// <returns></returns>
        public static string getTitleMusicVolume()
        {
            // Return the title
            return "Music volume";
        }

        /// <summary>
        /// This method allows to return the list of values ​​that the button has
        /// </summary>
        /// <returns></returns>
        public static List<string> getValuesMusicVolume()
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
        public static int getMusicVolumeID()
        {
            // Return the id
            return GetIDValue(_musicVolume, getValuesSFXVolume());
        }
        #endregion

    }
}
