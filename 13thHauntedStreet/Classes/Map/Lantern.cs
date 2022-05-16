/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Lantern Class
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
    public class Lantern : GameObject
    {
        // Attributs
        private float scale;
        public Light light;
        private Random rnd = new Random();

        private bool _flickers;
        public bool isOn = true;
        private int _timeSinceLastFrame = 0;
        private int _millisecondsPerFrame = 1000;


        // Ctor
        public Lantern(Vector2 position, Texture2D texture = null, float textureScale = 1,  int lightScale = 300, bool flickers = false)
        {
            this.position = position;
            this.texture = texture;
            this.scale = textureScale;
            this._flickers = flickers;

            this.light = new PointLight
            {
                Scale = new Vector2(lightScale),
                Position = this.position,
                ShadowType = ShadowType.Occluded,
                Intensity = 1f
            };
        }


        // Methods
        public void Update(GameTime gameTime)
        {
            if (this._flickers)
            {
                if (this._timeSinceLastFrame >= this._millisecondsPerFrame)
                {
                    this._timeSinceLastFrame -= this._millisecondsPerFrame;

                    this.isOn = !this.isOn;

                    if (this.isOn)
                    {
                        this._millisecondsPerFrame = rnd.Next(7500, 10000);
                    }else
                        this._millisecondsPerFrame = rnd.Next(100, 300);
                }

                this._timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!(this.texture is null))
            {
                spriteBatch.Draw(this.texture, this.position, null, Color.White, 0f, this.texture.Bounds.Center.ToVector2(), this.scale, 0, 0);
            }
        }
    }
}
