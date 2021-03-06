/*
 * Author  : Marco Rodrigues, Alec Piette
 * Project : 13th Haunted Street
 * Details : Door class
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    public class Door
    {
        // Properties
        private Game1.direction _direction;
        public Rectangle area;
        public const int LENGTH = 50;

        public Door connectedDoor;

        public Scene scene;

        private const int SPAWNOFFSET = 50;
        public Vector2 spawnPos
        {
            get
            {
                Vector2 offset = Vector2.Zero;
                int directionNb = (int)this._direction;
                switch (directionNb)
                {
                    case 1: // left
                        offset.X = SPAWNOFFSET + Player.collisionBox.Width;
                        break;

                    case 2: // right
                        offset.X = -SPAWNOFFSET - Player.collisionBox.Width;
                        break;

                    case 3: // up
                        offset.Y = SPAWNOFFSET + Player.collisionBox.Height;
                        break;

                    case 4: // down
                        offset.Y = -SPAWNOFFSET - Player.collisionBox.Height;
                        break;
                }

                return this.area.Center.ToVector2() + offset;
            }
        }


        // Ctor
        public Door(Game1.direction direction, Scene scene)
        {
            this._direction = direction;
            this.scene = scene;

            // find area
            switch (this._direction)
            {
                case Game1.direction.left:
                    this.area = new Rectangle(this.scene.groundArea.Left + 5, this.scene.groundArea.Top + (this.scene.groundArea.Height - LENGTH) / 2, 5, LENGTH);
                    break;

                case Game1.direction.right:
                    this.area = new Rectangle(this.scene.groundArea.Right - 5, this.scene.groundArea.Top + (this.scene.groundArea.Height - LENGTH) / 2, 5, LENGTH);
                    break;

                case Game1.direction.up:
                    this.area = new Rectangle(this.scene.groundArea.Left + (this.scene.groundArea.Width - LENGTH) / 2, this.scene.groundArea.Top + 5, LENGTH, 5);
                    break;

                case Game1.direction.down:
                    this.area = new Rectangle(this.scene.groundArea.Left + (this.scene.groundArea.Width - LENGTH) / 2, this.scene.groundArea.Bottom - 5, LENGTH, 5);
                    break;
            }
        }


        // Methods
        /// <summary>
        /// checks if the player has entered the door's area
        /// </summary>
        /// <param name="player"></param>
        /// <returns>true if inside door area, else false</returns>
        public bool hasEntered()
        {
            if (Player.collisionBox.Right >= this.area.Left &&
                Player.collisionBox.Left <= this.area.Right &&
                Player.collisionBox.Bottom >= this.area.Top &&
                Player.collisionBox.Top <= this.area.Bottom)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// connects two doors together
        /// </summary>
        /// <param name="door1"></param>
        /// <param name="door2"></param>
        public static void connectDoors(Door door1, Door door2)
        {
            door1.connectedDoor = door2;
            door2.connectedDoor = door1;
        }
    }
}
