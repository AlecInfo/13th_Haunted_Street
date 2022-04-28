/********************************
 * Project : 13th Haunted Street
 * Description : This class QuitProgram allows you to quit the program
 * 
 * Date : 13/04/2022
 * Author : Piette Alec
*******************************/

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class QuitProgram
    {
        #region Variables
        public static bool isQuit = false;
        #endregion

        public void Update(GameTime gameTime, Game1 game)
        {
            // If its true quit the program
            if (isQuit)
            {
                game.Exit();
            }
        }
    }
}
