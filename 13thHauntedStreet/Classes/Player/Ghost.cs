/*
 * Author  : Marco Rodrigues, Alec Piette
 * Project : 13th Haunted Street
 * Details : Ghost class (inherits from player abstract class)
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
    class Ghost : Player
    {
        // Properties
        private const float DEFAULTSCALE = 1.5f;

        private const float MOVEMENTSPEED_GHOST = 0.4f;
        private const float MOVEMENTSPEED_OBJECT = 0.2f;

        private float movementSpeed = MOVEMENTSPEED_GHOST;

        public GhostAnimationManager animManager;

        private List<ItemButton> _listButton = new List<ItemButton>();

        private Vector2 _floatOffset = Vector2.Zero;
        private float _floatTimer;
        private const int FLOATSPEED = 4;
        private const float FLOATSIZE = 0.25f;

        private float _objectBar_xPosition = Screen.OriginalScreenSize.X / 18;
        private float _objectBar_yPosition = Screen.OriginalScreenSize.Y / 10;
        private const int _SPACING_OBJECTBAR = 30;

        private bool _previusMouse;
        private bool _currentMouse;

        //public bool isObject = false;

        // Ctor
        public Ghost(Vector2 initialPos, GhostAnimationManager animationManager)
        {
            this.position = initialPos;
            this.scale = DEFAULTSCALE;

            this.animManager = animationManager;
            this.animManager.currentAnim = this.animManager.animationRight;
            this.texture = this.animManager.currentAnim[0];

            this.light = new PointLight
            {
                Scale = new Vector2(1000),
                Position = this.position,
                ShadowType = ShadowType.Occluded,
                Radius = 10,
                Intensity = 1f
            };

            int indexCount = 0;
            foreach (Furniture item in Game1.self.furnitureList)
            {
                float posX = this._objectBar_xPosition - item.texture.Width / 2;

                Action<int> callback = (indexButton) => 
                {
                    if (!isObject || this.texture != item.texture)
                    {
                        Transform(indexButton, item);
                    }
                    else
                    {
                        Detransform();
                    }
                };
                this._listButton.Add(NewButton(Game1.self.font, new Vector2(posX, this._objectBar_yPosition), item.texture, callback, indexCount));
                this._objectBar_yPosition += item.texture.Height + _SPACING_OBJECTBAR;

                indexCount += 1;
            }
        }


        // Method
        public override void Update(GameTime gameTime, List<Furniture> furnitureList, Scene scene)
        {
            this._gameTime = gameTime;
            this._furnitureList = furnitureList;
            this.currentScene = scene;

            this._previusMouse = this._currentMouse;
            this._currentMouse = Game1.knm.isButtonPressed(Game1.input.Use2);

            if (this._currentMouse && !this._previusMouse && isObject)
            {
                Detransform();
            }

            foreach (ItemButton item in this._listButton)
            {
                Vector2 changePosition = Vector2.Zero;
                item.Update(gameTime, Game1.self.screen, ref changePosition);
            }

            this.ReadInput();

            // if the player is not moving in the y axis, make the ghost float
            if (this.movement.Y == 0 && !isObject)
            {
                this._floatTimer += (float)this._gameTime.ElapsedGameTime.TotalSeconds * FLOATSPEED;
                this._floatOffset.Y += (float)Math.Sin(this._floatTimer) * FLOATSIZE;

            }
            
            // if player has moved update position
            if (this.movement.X != 0 || this.movement.Y != 0)
            {
                this.UpdatePosition();
            }

            this.UpdateAnim();
        }

        public override void UpdatePosition()
        {
            Vector2 distance = this.movement * this.movementSpeed * (float)this._gameTime.ElapsedGameTime.TotalMilliseconds;


            this.WallCollision(ref distance);

            this.position += distance;
            this.light.Position = this.position;

            this.collisionBox = new Rectangle((int)this.position.X - this.texture.Width / 2, (int)(this.position.Y), (int)this.texture.Width, (int)(this.texture.Height / 2));
        }

        /// <summary>
        /// Updates the animation that is playing 
        /// </summary>
        private void UpdateAnim()
        {
            if (!isObject)
            {
                if (this.movement.X > 0)
                {
                    this.animManager.currentAnim = this.animManager.animationRight;
                }
                else if (this.movement.X < 0)
                {
                    this.animManager.currentAnim = this.animManager.animationLeft;
                }
                
                this.PlayAnim(this.animManager.currentAnim, ref this.texture);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position + this._floatOffset, null, Color.White, 0f, this.texture.Bounds.Center.ToVector2(), this.scale, SpriteEffects.None, 0f);
            this.DrawCollisionBox(spriteBatch);
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {
            foreach (ItemButton item in this._listButton)
            {
                item.Draw(spriteBatch);
            }
        }

        private void Transform(int indexButton, Furniture furniture)
        {
            this.animManager.currentAnim = this.animManager.furniture;
            this.texture = this.animManager.furniture[indexButton];

            foreach (ItemButton item in this._listButton)
            {
                item.IsOn = false;
            }
            this._listButton[indexButton].IsOn = true;

            this.movementSpeed = MOVEMENTSPEED_OBJECT;
            this.scale = furniture.scale;

            isObject = true;
        }

        private void Detransform()
        {
            this.animManager.currentAnim = this.animManager.animationRight;
            this.texture = this.animManager.currentAnim[0];

            foreach (ItemButton item in this._listButton)
            {
                item.IsOn = false;
            }

            this.scale = DEFAULTSCALE;

            isObject = false;
            
            this.movementSpeed = MOVEMENTSPEED_GHOST;
        }

        static ItemButton NewButton(SpriteFont font, Vector2 position, Texture2D texture, Action<int> callback, int parameterCallback, bool DrawImageText = true)
        {
            return new ItemButton(texture, font, callback, DrawImageText)
            {
                Position = position,
                ButtonColor = Color.Gray,
                ParameterClick = parameterCallback,
            };
        }

    }
}
