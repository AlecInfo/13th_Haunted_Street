﻿/*
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
        public float angle;

        private bool _isLit = true;
        private bool _hasReleasedUseKey = true;

        public const int POSITIONOFFSET = 50;


        // Ctor
        public Flashlight()
        {
            this.light = new Spotlight
            {
                Scale = new Vector2(1000, 850),
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
            this.light.Enabled = this._isLit;

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
            KeyboardState kbdState = Keyboard.GetState();
            if (kbdState.IsKeyDown(Game1.input.Use1) && this._hasReleasedUseKey)
            {
                this._isLit = !this._isLit;
                this._hasReleasedUseKey = false;
            }

            if (kbdState.IsKeyUp(Game1.input.Use1))
            {
                this._hasReleasedUseKey = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            spriteBatch.Draw(Game1.flashlightIcon, playerPosition + this.position, null, this._isLit?Color.White:Color.Gray, this.angle, Game1.flashlightIcon.Bounds.Center.ToVector2(), 4, 0, 1f);

            //spriteBatch.Draw(Game1.flashlightFrameIcon, );
        }
    }
}