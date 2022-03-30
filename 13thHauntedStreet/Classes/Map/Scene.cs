/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Scene Class
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;

namespace _13thHauntedStreet
{
    class Scene
    {
        // Properties
        private Texture2D _ground;
        public Texture2D walls;
        private Vector2 _backgroundScale;

        public Rectangle groundArea;

        public List<Player> playerList;
        public List<Furniture> furnitureList;

        private PenumbraComponent _penumbra;


        // Ctor
        public Scene(Texture2D ground, Texture2D walls, Rectangle groundArea, List<Player> playerList, List<Furniture> furnitureList) : this(ground, walls, Vector2.One, groundArea, playerList, furnitureList) { }

        public Scene(Texture2D ground, Texture2D walls, Vector2 backgroundScale, Rectangle groundArea, List<Player> playerList, List<Furniture> furnitureList)
        {
            this._ground = ground;
            this.walls = walls;
            this._backgroundScale = backgroundScale;
             
            this.groundArea = groundArea;

            this.playerList = playerList;
            this.furnitureList = furnitureList;
        }


        // Methods
        public void Update(GameTime gameTime, PenumbraComponent penumbra)
        {
            this._penumbra = penumbra;

            // Update player
            foreach (Player player in this.playerList)
            {
                player.Update(gameTime, this.furnitureList, this);

                // lights
                penumbra.Lights.Add(player.light);
                if (player.GetType() == typeof(Hunter))
                {
                    penumbra.Lights.Add((player as Hunter).flashLight);
                }
            }

            // Update furniture hulls
            foreach (Furniture furniture in this.furnitureList)
            {
                penumbra.Hulls.Add(furniture.hull);
                foreach (Player player in this.playerList)
                {
                    // if the player is inside a furniture object, add the furniture hull to the players IgnoredHulls list, else remove it from the list
                    if (isInside(furniture, player))
                    {
                        if (!player.light.IgnoredHulls.Contains(furniture.hull))
                        {
                            player.light.IgnoredHulls.Add(furniture.hull);

                            if (player.GetType() == typeof(Hunter))
                            {
                                (player as Hunter).flashLight.IgnoredHulls.Add(furniture.hull);
                            }
                        }
                    }
                    else
                    {
                        player.light.IgnoredHulls.Remove(furniture.hull);

                        if (player.GetType() == typeof(Hunter))
                        {
                            (player as Hunter).flashLight.IgnoredHulls.Remove(furniture.hull);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if a player is inside a furniture object
        /// </summary>
        /// <param name="furniture"></param>
        /// <param name="player"></param>
        /// <returns>true if it's inside, else false</returns>
        private bool isInside(Furniture furniture, Player player)
        {
            Rectangle furnitureRect = new Rectangle(furniture.position.ToPoint(), furniture.texture.Bounds.Size);
            Rectangle playerRect = new Rectangle(player.position.ToPoint() - (player.texture.Bounds.Size.ToVector2() * player.scale / 2).ToPoint(), (player.texture.Bounds.Size.ToVector2() + player.texture.Bounds.Size.ToVector2() / 2 * player.scale).ToPoint());
            
            if (playerRect.Right >= furnitureRect.Left &&
                playerRect.Left <= furnitureRect.Right &&
                playerRect.Bottom >= furnitureRect.Top &&
                playerRect.Top <= furnitureRect.Bottom)
            {
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw background image
            spriteBatch.Draw(this._ground, Screen.OriginalScreenSize / 2, null, Color.White, 0, this._ground.Bounds.Center.ToVector2(), this._backgroundScale, 0, 0);

            // draw game objects (player, furniture)
            List<GameObject> gameObjectList = new List<GameObject>();
            List<GameObject> ghostList = new List<GameObject>();

            gameObjectList.AddRange(furnitureList);

            // filter by player type
            foreach (Player player in this.playerList)
            {
                if (player.GetType() == typeof(Hunter))
                {
                    gameObjectList.Add(player);
                }

                if(player.GetType() == typeof(Ghost))
                {
                    ghostList.Add(player);
                }
            }

            // Draw game objects ordered by Y
            foreach (GameObject gameObject in gameObjectList.OrderBy(o => o.position.Y))
            {
                gameObject.Draw(spriteBatch);  
            }

            // Draw Ghosts on top of everything else
            foreach (GameObject ghost in ghostList)
            {
                ghost.Draw(spriteBatch);
            }


            // draw collision box
            //spriteBatch.Draw(Game1.defaultTexture, this.walls, null, Color.Purple * 0.5f);
        }
    }
}
