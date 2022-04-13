using System;
using System.Collections.Generic;
using System.Text;

namespace _13thHauntedStreet
{
    static class Settings
    {
        #region Default values

        private static string _fullscreenDefault = "Enabled";
        private static string _refreshRateDefault = "60";
        private static string _refreshRateDisplayDefault = "Disabled";
        private static string _sfxVolume = "7";
        private static string _musicVolume = "7";

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
            return GetIDValue(_fullscreenDefault, getValuesFullscreen());
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
            return GetIDValue(_refreshRateDefault, getValuesRefreshRate());
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
            return GetIDValue(_refreshRateDisplayDefault, getValuesRefreshRateDisplay());
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
    }
}
