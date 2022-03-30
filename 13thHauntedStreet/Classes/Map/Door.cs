﻿/*
 * Author  : Marco Rodrigues
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
    class Door
    {
        // Properties
        private Game1.direction _direction;
        public Rectangle area;
        
        public Door conectedDoor;
        public Scene scene;

        private const int SPAWNOFFSET = 10;
        public Vector2 spawnPos
        {
            get
            {
                Vector2 offset = Vector2.Zero;
                int directionNb = (int)this._direction;
                switch (directionNb)
                {
                    case 1: // left
                        offset.X = SPAWNOFFSET;
                        break;

                    case 2: // right
                        offset.X = -SPAWNOFFSET;
                        break;

                    case 3: // up
                        offset.Y = SPAWNOFFSET;
                        break;

                    case 4: // down
                        offset.Y = -SPAWNOFFSET;
                        break;
                }

                return this.area.Center.ToVector2() + offset;
            }
        }


        // Ctor
        public Door(Game1.direction direction, Vector2 position, Vector2 size, Scene scene) : this(direction, new Rectangle(position.ToPoint(), size.ToPoint()), scene) { }
        public Door(Game1.direction direction, Rectangle area, Scene scene)
        {
            this._direction = direction;
            this.area = area;

            this.scene = scene;
        }


        // Methods
        /// <summary>
        /// checks if the player has entered the door's area
        /// </summary>
        /// <param name="player"></param>
        /// <returns>true if inside door area, else false</returns>
        public bool hasEntered(Player player)
        {
            if (player.rectangle.Right >= this.area.Left &&
                player.rectangle.Left <= this.area.Right &&
                player.rectangle.Bottom >= this.area.Top &&
                player.rectangle.Top <= this.area.Bottom)
            {
                return true;
            }

            return false;
        }
    }
}
