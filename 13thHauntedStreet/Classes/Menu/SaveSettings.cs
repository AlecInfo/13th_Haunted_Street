﻿/********************************
 * Project : 13th Haunted Street
 * Description : This class SaveSettings allows you to save the settings
 * 
 * Date : 13/04/2022
 * Author : Piette Alec
*******************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace _13thHauntedStreet
{
    [Serializable]
    public class SaveSettings
    {
        #region Variables

        public string Fullscreen { get; set; }

        public string RefreshRate { get; set; }

        public string RefreshRateDisplay { get; set; }

        public string SfxVolume { get; set; }

        public string MusicVolume { get; set; }

        #endregion

        public SaveSettings(){}
        public SaveSettings(string fullscreen, string refreshrate, string refreshratedisplay, string sfxvolume, string musicvolume)
        {
            this.Fullscreen = fullscreen;

            this.RefreshRate = refreshrate;

            this.RefreshRateDisplay = refreshratedisplay;

            this.SfxVolume = sfxvolume;

            this.MusicVolume = musicvolume;
        }
    }
}