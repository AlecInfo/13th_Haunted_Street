﻿/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Hunter class (inherits from player abstract class)
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
    class Hunter : Player
    {
        // Properties
        public float lightAngle;
        public Light flashLight;

        private const float MOVEMENTSPEED = 0.45f;

        private HunterAnimationManager _animManager;

        private Game1.direction _currentDirection = Game1.direction.down;


        // Ctor
        public Hunter(Input input, Vector2 initialPos, HunterAnimationManager animationManager)
        {
            this._input = input;
            this.position = initialPos;
            this.scale = 3f;

            this._animManager = animationManager;
            this._animManager.currentAnim = this._animManager.walkingDown;
            this.texture = this._animManager.currentAnim[0];

            // Collision box
            this.collisionBox = new Rectangle(
                (int)(this.position.X - this.texture.Width * this.scale / 2), (int)this.position.Y,
                (int)(this.texture.Width * this.scale), (int)(this.texture.Height / 2 * this.scale));

            // Lights
            this.light = new PointLight
            {
                Scale = new Vector2(250),
                Position = this.position,
                ShadowType = ShadowType.Occluded,
                Intensity = 0.5f
            };
            this.flashLight = new Spotlight
            {
                Scale = new Vector2(1000, 750),
                Position = new Vector2(this.collisionBox.Center.X, this.collisionBox.Top),
                ShadowType = ShadowType.Occluded,
                Radius = 10,
                Intensity = 1.5f
            };

        }


        // Methods
        public override void Update(GameTime gameTime, List<Furniture> furnitureList, Scene scene)
        {
            this._gameTime = gameTime;
            this._furnitureList = furnitureList;
            this.currentScene = scene;

            this.readInput();

            // if player has moved update position
            if (this._movement.X != 0 || this._movement.Y != 0)
            {
                this.updatePosition();
            }

            // update flashlight angle with mouse position
            MouseState msState = Mouse.GetState();
            this.lightAngle = (float)Math.Atan2(msState.Y - this.position.Y, msState.X - this.position.X);
            this.flashLight.Rotation = this.lightAngle;

            this.updateAnim();
        }

        public override void updatePosition()
        {
            Vector2 distance = this._movement * MOVEMENTSPEED * (float)this._gameTime.ElapsedGameTime.TotalMilliseconds;

            this.objectCollision(ref distance);

            this.wallCollision(ref distance);

            this.position += distance;

            this.collisionBox = new Rectangle((int)(this.position.X - this.texture.Width * this.scale / 2), (int)this.position.Y, (int)(this.texture.Width * this.scale), (int)(this.texture.Height / 2 * this.scale));
            this.light.Position = new Vector2(this.collisionBox.Center.X, this.collisionBox.Top);
            this.flashLight.Position = this.position;
        }

        /// <summary>
        /// Updates the animation that is playing
        /// </summary>
        private void updateAnim()
        {
            // if is moving
            if (this._movement.X != 0 || this._movement.Y != 0)
            {
                if (this._movement.X < 0)
                {
                    this._animManager.currentAnim = this._animManager.walkingLeft;
                    this._currentDirection = Game1.direction.left;
                }
                else if (this._movement.X > 0)
                {
                    this._animManager.currentAnim = this._animManager.walkingRight;
                    this._currentDirection = Game1.direction.right;
                }
                else
                {
                    if (this._movement.Y < 0)
                    {
                        this._animManager.currentAnim = this._animManager.walkingUp;
                        this._currentDirection = Game1.direction.up;
                    }
                    else if (this._movement.Y > 0)
                    {
                        this._animManager.currentAnim = this._animManager.walkingDown;
                        this._currentDirection = Game1.direction.down;
                    }
                }
            }
            else // if its not moving
            {
                switch (this._currentDirection)
                {
                    case Game1.direction.left:
                        this._animManager.currentAnim = this._animManager.idleLeft;
                        break;

                    case Game1.direction.right:
                        this._animManager.currentAnim = this._animManager.idleRight;
                        break;

                    case Game1.direction.up:
                        this._animManager.currentAnim = this._animManager.idleUp;
                        break;

                    case Game1.direction.down:
                        this._animManager.currentAnim = this._animManager.idleDown;
                        break;
                }
            }

            this.playAnim(this._animManager.currentAnim, ref this.texture);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, null, Color.White, 0f, this.texture.Bounds.Center.ToVector2(), this.scale, SpriteEffects.None, 1f);

            Vector2 flashCursorPos = Vector2.Normalize(Mouse.GetState().Position.ToVector2() - this.position) * 75;
            spriteBatch.Draw(Game1.flashlightIcon, this.position+flashCursorPos, null, Color.White, this.lightAngle, Game1.flashlightIcon.Bounds.Center.ToVector2(), 4, 0, 1f); // a finir

            //this.drawCollisionBox(spriteBatch);
        }
    }
}
