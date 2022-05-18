/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Flashlight class (inherits from Tool abstract class)
 * Tool    : Default tool used to search for ghost, can be turned on and off
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;

namespace _13thHauntedStreet
{
    class Flashlight : Tool
    {
        // Properties
        public bool isLit = true;
        private bool _hasReleasedUseKey = true;

        public const int POSITIONOFFSET = 50;
        public const int LIGHTGHEIGHT = 850;


        // Ctor
        public Flashlight(Texture2D icon)
        {
            this.icon = icon;

            this.light = new Spotlight
            {
                Scale = new Vector2(1000, LIGHTGHEIGHT),
                Position = Vector2.Zero,
                ShadowType = ShadowType.Occluded,
                Radius = 25,
                Intensity = 2f
            };
        }


        // Methods
        public override void Update(GameTime gameTime, Vector2 playerPosition)
        {
            // use
            Use();
            this.light.Enabled = this.isLit;

            // find light angle
            MouseState msState = Mouse.GetState();
            this.angle = (float)Math.Atan2(msState.Y - playerPosition.Y, msState.X - playerPosition.X);
            this.light.Rotation = this.angle;

            // find position
            this.position = Vector2.Normalize(Mouse.GetState().Position.ToVector2() - playerPosition) * POSITIONOFFSET;
            this.light.Position = playerPosition + this.position;
        }

        protected override void Use()
        {
            if (Game1.knm.isButtonPressed(Game1.input.Use1) && this._hasReleasedUseKey)
            {
                this.isLit = !this.isLit;
                this._hasReleasedUseKey = false;
            }

            if (!Game1.knm.isButtonPressed(Game1.input.Use1))
            {
                this._hasReleasedUseKey = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            spriteBatch.Draw(Game1.flashlightIcon, playerPosition + this.position, null, this.isLit?Color.White:Color.Gray, this.angle, Game1.flashlightIcon.Bounds.Center.ToVector2(), 4, 0, 1f);

            spriteBatch.Draw(this.icon, Screen.OriginalScreenSize - new Vector2(Hunter.UIFRAMEBORDER), null, Color.White, 0f, Game1.uiFrame.Bounds.Size.ToVector2(), Hunter.UIFRAMESCALE, 0, 0);
        }
    }
}
