/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Flashlight class (inherits from Tool abstract class)
 * Tool    : Used to catch ghost, by sucking them, its light is narrower than the default flashlight
 * Sources : https://www.geeksforgeeks.org/check-whether-a-given-point-lies-inside-a-triangle-or-not/
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
        public bool _isOn = false;

        private const int POSITIONOFFSET = 50;
        public const int LIGHTGHEIGHT = 300;

        private Vector2 playerPosition;
        public Vector2 oppositePoint1;
        public Vector2 oppositePoint2;

        //test
        Color color = Color.White;


        // Ctor
        public Vacuum(Texture2D icon)
        {
            this.icon = icon;

            this.light = new Spotlight
            {
                Scale = new Vector2(600, LIGHTGHEIGHT),
                Position = Vector2.Zero,
                ShadowType = ShadowType.Occluded,
                Radius = 25,
                Intensity = 2f
            };
        }


        // Methods
        public override void Update(GameTime gameTime, Vector2 playerPosition) 
        {
            this.playerPosition = playerPosition;
            Use();

            // find light angle
            MouseState msState = Mouse.GetState();
            this.angle = (float)Math.Atan2(msState.Y - playerPosition.Y, msState.X - playerPosition.X);
            this.light.Rotation = this.angle;

            // find position
            this.position = Vector2.Normalize(Mouse.GetState().Position.ToVector2() - playerPosition) * POSITIONOFFSET;
            this.light.Position = playerPosition + this.position;

            float sideLength = (float)Math.Sqrt(Math.Pow(this.light.Scale.Y / 4, 2) + Math.Pow(this.light.Scale.X, 2));
            float angle1 = (float)Math.Asin(this.light.Scale.X / sideLength);
            float angle2 = ((float)Math.PI / 2) - angle1;
            oppositePoint1 = playerPosition + new Vector2(
                (float)Math.Cos(this.light.Rotation + angle2),
                (float)Math.Sin(this.light.Rotation + angle2)
            ) * sideLength;
            oppositePoint2 = playerPosition + new Vector2(
                (float)Math.Cos(this.light.Rotation - angle2),
                (float)Math.Sin(this.light.Rotation - angle2)
            ) * sideLength;
        }

        protected override void Use()
        {
            color = Color.White;
            if (Game1.knm.isButtonPressed(Game1.input.Use1))
            {
                this._isOn = true;

                foreach (foreignPlayer otherPlayer in Client.listOtherPlayer)
                {
                    if (otherPlayer.playerType == typeof(Ghost).ToString() && otherPlayer.currentScene == Game1.player.currentScene.id && !otherPlayer.Captured)
                    {
                        if (IsInside(this.playerPosition + this.position, oppositePoint1, oppositePoint2, otherPlayer.position))
                        {
                            float distanceBetween = (float)Math.Sqrt(Math.Pow(this.playerPosition.X - otherPlayer.position.X, 2) + Math.Pow(this.playerPosition.Y - otherPlayer.position.Y, 2));
                            if (distanceBetween <= 200)
                            {
                                Game1.client.envoieMessage($"{otherPlayer._id}, capturer");
                            }
                            else
                            {
                                Game1.client.envoieMessage($"{otherPlayer._id}, se fait aspirer");
                            }
                        }
                    }
                }
            }

            if (!Game1.knm.isButtonPressed(Game1.input.Use1))
            {
                this._isOn = false;
            }
        }

        /// <summary>
        /// Calculates the area of a triangle, from its points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <returns>Area of the triangle</returns>
        private static float Area(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            return Math.Abs((point1.X * (point2.Y - point3.Y) +
                             point2.X * (point3.Y - point1.Y) +
                             point3.X * (point1.Y - point2.Y)) / 2f);
        }

        /// <summary>
        /// Checks if a point is inside a triangle
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <param name="pointToCheck"></param>
        /// <returns>true if the point is inside, else false</returns>
        private bool IsInside(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 pointToCheck)
        {
            // Area of the triangle
            float MainArea = Area(point1, point2, point3);

            // Triangles formed by point to check, and 2 main traingle points
            float Area1 = Area(pointToCheck, point2, point3);

            float Area2 = Area(point1, pointToCheck, point3);

            float Area3 = Area(point1, point2, pointToCheck);

            return (Math.Abs(MainArea - (Area1 + Area2 + Area3)) <= 0.5f);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 playerPosition) 
        {
            spriteBatch.Draw(this._isOn?Game1.vacuumIconOn:Game1.vacuumIconOff, playerPosition + this.position, null, color, this.angle, Game1.vacuumIconOff.Bounds.Center.ToVector2(), 3, 0, 1f);

            spriteBatch.Draw(this.icon, Screen.OriginalScreenSize - new Vector2(Hunter.UIFRAMEBORDER), null, Color.White, 0f, Game1.uiFrame.Bounds.Size.ToVector2(), Hunter.UIFRAMESCALE, 0, 0);


            // Debug
            /*Game1.DrawLine(spriteBatch, playerPosition + this.position, oppositePoint1, Color.Red, 2f);
            Game1.DrawLine(spriteBatch, playerPosition + this.position, oppositePoint2, Color.Blue, 2f);
            Game1.DrawLine(spriteBatch, oppositePoint1, oppositePoint2, Color.Green, 2f);*/
        }
    }
}
