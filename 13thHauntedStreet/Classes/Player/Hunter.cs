/*
 * Author  : Marco Rodrigues, Alec Piette
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
        private Tool[] tools = new Tool[2];
        public int currentToolNb = 0;
        public const int UIFRAMEBORDER = 50;
        public const float UIFRAMESCALE = 1.5f;

        private const float MOVEMENTSPEED = 0.3f;

        private HunterAnimationManager _animManager;

        private Game1.direction _currentDirection = Game1.direction.down;

        public Tool currentTool { get { return this.tools[this.currentToolNb]; } }


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
            this.tools[0] = new Flashlight(Game1.flashlightFrameIcon);
            this.tools[1] = new Vacuum(Game1.vacuumFrameIcon);
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

            this.tools[this.currentToolNb].Update(gameTime, this.position);

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
            // Item Up or Down Key
            if ((Game1.knm.isButtonPressed(Game1.input.ItemUp) || Game1.knm.isButtonPressed(Game1.input.ItemDown)) && this.hasReleasedItemKey)
            {
                this.hasReleasedItemKey = false;
                this.currentToolNb = this.currentToolNb == 0 ? 1 : 0;
            }

            // Release Key
            if (!Game1.knm.isButtonPressed(Game1.input.ItemUp) && !Game1.knm.isButtonPressed(Game1.input.ItemDown))
            {
                this.hasReleasedItemKey = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, null, Color.White, 0f, this.texture.Bounds.Center.ToVector2(), this.scale, SpriteEffects.None, 1f);

            //this.DrawCollisionBox(spriteBatch);
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {
            tools[this.currentToolNb].Draw(spriteBatch, this.position);

            // Draw main item frame
            spriteBatch.Draw(Game1.uiFrame, 
                Screen.OriginalScreenSize - new Vector2(UIFRAMEBORDER),
                null, Color.White, 0f, 
                Game1.uiFrame.Bounds.Size.ToVector2(),
                UIFRAMESCALE, 0, 0);

            // Draw secondary item
            int otherToolNb = this.currentToolNb==0?1:0;

            spriteBatch.Draw(this.tools[otherToolNb].icon,
                Screen.OriginalScreenSize - new Vector2(Game1.uiFrame.Bounds.Size.X * UIFRAMESCALE + UIFRAMEBORDER * 1.5f, UIFRAMEBORDER),
                null, Color.White, 0f,
                this.tools[otherToolNb].icon.Bounds.Size.ToVector2(),
                ((float)Game1.uiSmallFrame.Bounds.Size.X / (float)Game1.uiFrame.Bounds.Size.X)*UIFRAMESCALE, 0, 0);

            spriteBatch.Draw(Game1.uiSmallFrame,
                Screen.OriginalScreenSize - new Vector2(Game1.uiFrame.Bounds.Size.X*UIFRAMESCALE + UIFRAMEBORDER * 1.5f, UIFRAMEBORDER),
                null, Color.White, 0f,
                Game1.uiSmallFrame.Bounds.Size.ToVector2(),
                UIFRAMESCALE, 0, 0);
        }
    }
}
