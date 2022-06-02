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
        private const float MOVEMENTSPEED_STUN = 0.1f;

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

        // For detrasform the ghost
        private bool _isDetransform = false;
        private bool _isStun = false;
        private const float _COUNTDURATION = 800f; //every 0.8s.
        private float _currentTime = 0f;
        private Color _stunColor = Color.Gray;

        public bool isBeingVacuumed = false;


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

                Action<int, GameTime> callback = (indexButton, gameTime) =>
                {
                    if (!_isDetransform)
                    {
                        if ((!isObject || this.texture != item.texture))
                        {
                            Transform(indexButton, item);
                        }
                        else
                        {
                            Detransform(_gameTime);
                        }

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

            this.ReadInput();

            // If the ghost was trasformed in object or not
            if (isObject)
            {
                // Object collision box
                collisionBox = new Rectangle((int)(
                    this.position.X - this.texture.Width / 2), (int)(this.position.Y - this.texture.Height / 2 + this.texture.Height / 3.5f),
                    (int)this.texture.Width, (int)(this.texture.Height - this.texture.Height / 3.5f));

                // Reset the ghost float 
                this._floatOffset.Y = 0;

                // Get the mouse state
                this._previusMouse = this._currentMouse;
                this._currentMouse = Game1.knm.isButtonPressed(Game1.input.Use2);

                // The ghost can detransform with the right click
                if (this._currentMouse && !this._previusMouse)
                {
                    Detransform(gameTime);
                }

            }
            else
            {
                if (!this._isStun && this._isDetransform)
                {
                    this.movementSpeed = MOVEMENTSPEED_STUN;
                    color = this._stunColor;

                    _currentTime += (float)_gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (_currentTime >= _COUNTDURATION)
                    {
                        _currentTime -= _COUNTDURATION;

                        this._isStun = true;
                        this._isDetransform = false;

                        this.movementSpeed = MOVEMENTSPEED_GHOST;
                        color = Color.White;
                    }
                }

                // Ghost collision box
                collisionBox = new Rectangle((int)this.position.X - this.texture.Width / 2, (int)(this.position.Y), (int)this.texture.Width, (int)(this.texture.Height / 2));

                // if the player is not moving in the y axis, make the ghost float
                if (this.movement.Y == 0)
                {
                    this._floatTimer += (float)this._gameTime.ElapsedGameTime.TotalSeconds * FLOATSPEED;
                    this._floatOffset.Y += (float)Math.Sin(this._floatTimer) * FLOATSIZE;
                }

                if (this.isBeingVacuumed)
                {
                    foreignPlayer hunter = Client.listOtherPlayer.Find(x => x.playerType == typeof(Hunter).ToString());

                    this.movement += Vector2.Normalize(hunter.position - Game1.player.position) * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.25f;
                }

                this.UpdateAnim();
            }

            // Update the button who allows to you transform the ghost
            foreach (ItemButton item in this._listButton)
            {
                Vector2 changePosition = Vector2.Zero;
                item.Update(gameTime, Game1.self.screen, ref changePosition);
            }

            // if player has moved update position
            if (this.movement.X != 0 || this.movement.Y != 0)
            {
                this.UpdatePosition();
            }
        }

        public override void UpdatePosition()
        {
            Vector2 distance = this.movement * this.movementSpeed * (float)this._gameTime.ElapsedGameTime.TotalMilliseconds;

            if (isObject)
            {
                this.ObjectCollision(ref distance);
            }

            this.WallCollision(ref distance);

            this.position += distance;
            this.light.Position = this.position;
            this.isBeingVacuumed = false;
        }

        /// <summary>
        /// Updates the animation that is playing 
        /// </summary>
        private void UpdateAnim()
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            float opactiy = Game1.captured ? 0.5f : 1f;
            spriteBatch.Draw(this.texture, this.position + this._floatOffset, null, color * opactiy, 0f, this.texture.Bounds.Center.ToVector2(), this.scale, SpriteEffects.None, 0f);
            //this.DrawCollisionBox(spriteBatch);
        }

        public override void DrawUI(SpriteBatch spriteBatch)
        {
            if (Game1.captured != true)
            {
                foreach (ItemButton item in this._listButton)
                {
                    item.Draw(spriteBatch);
                }
            }
        }

        private void Transform(int indexButton, Furniture furniture)
        {
            // Object collision box
            collisionBox = new Rectangle((int)(
                this.position.X - furniture.texture.Width / 2), (int)(this.position.Y - furniture.texture.Height / 2 + furniture.texture.Height / 3.5f),
                (int)furniture.texture.Width, (int)(furniture.texture.Height - furniture.texture.Height / 3.5f));


            Vector2 distance = this.movement * movementSpeed * (float)this._gameTime.ElapsedGameTime.TotalMilliseconds;
            if (!PlayerIsCollide(ref distance))
            {
                this.animManager.currentAnim = this.animManager.furniture;
                this.texture = this.animManager.furniture[indexButton];

                foreach (ItemButton item in this._listButton)
                {
                    item.IsOn = false;
                }
                this._listButton[indexButton].IsOn = true;

                this.movementSpeed = MOVEMENTSPEED_OBJECT;
                this.scale = Furniture.SCALE;

                this._isStun = false;

                isObject = true;
            }

        }

        private void Detransform(GameTime gameTime)
        {
            this.animManager.currentAnim = this.animManager.animationRight;
            this.texture = this.animManager.currentAnim[0];

            foreach (ItemButton item in this._listButton)
            {
                item.IsOn = false;
            }

            this.scale = DEFAULTSCALE;

            isObject = false;
            this._isDetransform = true;
        }

        static ItemButton NewButton(SpriteFont font, Vector2 position, Texture2D texture, Action<int, GameTime> callback, int parameterCallback, bool DrawImageText = true)
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