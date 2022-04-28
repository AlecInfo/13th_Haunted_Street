/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Flashlight class (inherits from Tool abstract class)
 * Tool    : Used to catch ghost, by sucking them, its light is narrower than the default flashlight
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
    class Vacuum : Tool
    {
        // Properties
        public float angle;

        public bool _isOn = false;

        private const int POSITIONOFFSET = 50;


        // Ctor
        public Vacuum()
        {
            this.light = new Spotlight
            {
                Scale = new Vector2(1000, 600),
                Position = Vector2.Zero,
                ShadowType = ShadowType.Occluded,
                Radius = 25,
                Intensity = 2f
            };
        }


        // Methods
        public override void Update(GameTime gameTime, Vector2 playerPosition) 
        {
            Use();

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
            if (kbdState.IsKeyDown(Game1.input.Use1))
            {
                this._isOn = true;
            }

            if (kbdState.IsKeyUp(Game1.input.Use1))
            {
                this._isOn = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 playerPosition) 
        {
            spriteBatch.Draw(this._isOn?Game1.vacuumIconOn:Game1.vacuumIconOff, playerPosition + this.position, null, Color.White, this.angle, Game1.vacuumIconOff.Bounds.Center.ToVector2(), 3, 0, 1f);
        }
    }
}
