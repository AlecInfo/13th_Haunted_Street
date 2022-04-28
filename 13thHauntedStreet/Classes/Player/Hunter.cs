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
        private bool hasReleasedItemKey = true;
        private List<Tool> tools = new List<Tool>();
        public Tool tool;

        private const float MOVEMENTSPEED = 0.3f;

        private HunterAnimationManager _animManager;

        private Game1.direction _currentDirection = Game1.direction.down;


        // Ctor
        public Hunter(Vector2 initialPos, HunterAnimationManager animationManager)
        {
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
                Scale = new Vector2(400),
                Position = this.position,
                ShadowType = ShadowType.Occluded,
                Intensity = 1f
            };

            // Tools
            this.tools.Add(new Flashlight());
            this.tools.Add(new Vacuum());
            this.tool = this.tools[0];
        }


        // Methods
        public override void Update(GameTime gameTime, List<Furniture> furnitureList, Scene scene)
        {
            this._gameTime = gameTime;
            this._furnitureList = furnitureList;
            this.currentScene = scene;

            this.ReadInput();
            this.ReadItemChangingKey();

            // if player has moved update position
            if (this._movement.X != 0 || this._movement.Y != 0)
            {
                this.UpdatePosition();
            }

            this.tool.Update(gameTime, this.position);

            this.UpdateAnim();
        }

        public override void UpdatePosition()
        {
            Vector2 distance = this._movement * MOVEMENTSPEED * (float)this._gameTime.ElapsedGameTime.TotalMilliseconds;

            this.ObjectCollision(ref distance);

            this.WallCollision(ref distance);

            this.position += distance;

            this.collisionBox = new Rectangle((int)(this.position.X - this.texture.Width * this.scale / 2), (int)this.position.Y, (int)(this.texture.Width * this.scale), (int)(this.texture.Height / 2 * this.scale));
            this.light.Position = new Vector2(this.collisionBox.Center.X, this.collisionBox.Top);
        }

        /// <summary>
        /// Updates the animation that is playing
        /// </summary>
        private void UpdateAnim()
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

            this.PlayAnim(this._animManager.currentAnim, ref this.texture);
        }

        /// <summary>
        /// Read keys that change the tool that is currently being used
        /// </summary>
        /// <remarks>Differs from ReakKey() because this method is Hunter only</remarks>
        private void ReadItemChangingKey()
        {
            int? index = null;

            // Item Up Key
            if (Keyboard.GetState().IsKeyDown(Game1.input.ItemUp) && this.hasReleasedItemKey)
            {
                this.hasReleasedItemKey = false;

                index = tools.FindIndex(x => x == this.tool) + 1;
            }

            // Item Down Key
            if (Keyboard.GetState().IsKeyDown(Game1.input.ItemDown) && this.hasReleasedItemKey)
            {
                this.hasReleasedItemKey = false;

                index = tools.FindIndex(x => x == this.tool) - 1;
            }

            // if index not null
            if(!(index is null))
            {
                // check if too high or too low
                if (index == tools.Count)
                {
                    index = 0;
                }
                else if (index < 0)
                {
                    index = tools.Count - 1;
                }
                
                // then apply changement
                this.tool = this.tools[index ?? default(int)];
            }

            // Release Key
            if (Keyboard.GetState().IsKeyUp(Game1.input.ItemUp) && Keyboard.GetState().IsKeyUp(Game1.input.ItemDown))
            {
                this.hasReleasedItemKey = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, null, Color.White, 0f, this.texture.Bounds.Center.ToVector2(), this.scale, SpriteEffects.None, 1f);

            //this.drawCollisionBox(spriteBatch);
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {
            tool.Draw(spriteBatch, this.position);
        }
    }
}
