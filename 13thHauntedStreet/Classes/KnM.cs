/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Class that manages Keyboard and Mouse button state
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    public class KnM
    {
        // -- Attributs --
        // Keyboard
        private KeyboardState kbdState;

        // Mouse
        private MouseState msState;
        private int previousScrollWheelValue;


        // -- Ctor --
        public KnM()
        {
            this.kbdState = Keyboard.GetState();
            this.msState = Mouse.GetState();
            this.previousScrollWheelValue = this.msState.ScrollWheelValue;
        }


        // -- Methods --
        public void Update()
        {
            if (this.msState.ScrollWheelValue != this.previousScrollWheelValue)
            {
                this.previousScrollWheelValue = this.msState.ScrollWheelValue;
            }
        }

        /// <summary>
        /// check if a button (Keyboard or mouse) is pressed
        /// </summary>
        /// <param name="key">button to check</param>
        /// <returns>true if pressed, else false</returns>
        public bool isButtonPressed(Game1.KnMButtons button)
        {
            this.kbdState = Keyboard.GetState();
            this.msState = Mouse.GetState();


            if ((int)button < 300) // Keyboard
            {
                if (this.kbdState.IsKeyDown((Keys)(int)button))
                {
                    return true;
                }
            }
            else if ((int)button >= 300) // Mouse
            {
                // Right Click
                if (button == Game1.KnMButtons.RightClick && this.msState.RightButton == ButtonState.Pressed)
                {
                    return true;
                }

                // Left Click
                if (button == Game1.KnMButtons.LeftClick && this.msState.LeftButton == ButtonState.Pressed)
                {
                    return true;
                }

                //Scroll Click
                if (button == Game1.KnMButtons.ScrollButton && this.msState.MiddleButton == ButtonState.Pressed)
                {
                    return true;
                }

                // Scroll Up
                if (button == Game1.KnMButtons.ScrollUp && this.msState.ScrollWheelValue > this.previousScrollWheelValue)
                {
                    return true;
                }

                // Scroll Down
                if (button == Game1.KnMButtons.ScrollDown && this.msState.ScrollWheelValue < this.previousScrollWheelValue)
                {
                    return true;
                }
            }

            // not pressed
            return false;
        }
    }
}
