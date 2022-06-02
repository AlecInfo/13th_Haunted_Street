/*
 * Author : David Vieira Luis
 * Project : 13th Haunted Street
 * Details : Class that receives data from the other players and draws them
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using Penumbra;

namespace _13thHauntedStreet
{
    public class foreignPlayer : GameObject
    {
        private const float SPEED = 0.25f;
        public int _id;
        public string playerType;
        public int currentScene;
        public float scale = 1f;
        private bool captured = false;

        // Hunter
        public Light light;
        public Light toolLight;


        public Vector2 _Position
        {
            get { return position; }
            set
            {
                if (!(this.light is null))
                {
                    this.light.Position = value;
                }

                if (!(this.toolLight is null))
                {
                    this.toolLight.Position = value;
                }

                position = value;
            }
        }
        public bool IsObject { get; set; }

        public Texture2D _Texture
        {
            get { return texture; }   // get method
            set { texture = value; }
        }
        public bool IsLightOn
        {
            set
            {
                this.light.Enabled = value;
                this.toolLight.Enabled = value;
            }
        }
        public bool Captured
        {
            get { return captured; }
            set { captured = value; }
        }

        public float radius
        {
            set
            {
                /*Vector2 offset = Vector2.Normalize(Mouse.GetState().Position.ToVector2() - _Position) * Flashlight.POSITIONOFFSET;
                this.toolLight.Position += offset;*/
                this.toolLight.Rotation = value;
            }
        }

        public bool ToolIsFlashlight
        {
            set
            {
                this.toolLight.Scale = new Vector2(this.toolLight.Scale.X, value ? Flashlight.LIGHTGHEIGHT : Vacuum.LIGHTGHEIGHT);
            }
        }

        public foreignPlayer(Vector2 Position, Texture2D Texture, string playerType)
        {
            this._Position = Position;
            this._Texture = Texture;
            this.playerType = playerType;

            this.light = new PointLight
            {
                Scale = new Vector2(400),
                Position = this.position,
                ShadowType = ShadowType.Occluded,
                Intensity = 1f,
                Enabled = false
            };

            this.toolLight = new Spotlight
            {
                Scale = new Vector2(1000, 850),
                Position = this.position,
                ShadowType = ShadowType.Occluded,
                Radius = 25,
                Intensity = 2f,
                Enabled = false
            };
        }
        // TODO a tester
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!(this.texture is null))
            {
                this.scale = 1f;
                if (this.playerType == typeof(Hunter).ToString())
                {
                    this.scale = 3;
                }
                else
                {
                    this.scale = this.IsObject ? Furniture.SCALE : 1.5f;
                }
                if (this.Captured == false)
                {
                    spriteBatch.Draw(this.texture, this.position, null, Color.White, 0f, this.texture.Bounds.Center.ToVector2(), scale, 0, 0);
                }

            }
        }
    }
}