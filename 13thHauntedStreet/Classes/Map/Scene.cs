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
    public class Scene
    {
        // Properties
        public int id;

        private Texture2D _ground;
        public Texture2D walls;
        public Vector2 backgroundScale;

        public Rectangle groundArea;

        public Player player;
        public List<Furniture> furnitureList;
        public List<Lantern> lanternList;


        // Ctor
        public Scene(int id, Texture2D ground, Texture2D walls, Rectangle groundArea, Player player, List<Furniture> furnitureList, List<Lantern> lanternList) : this(id, ground, walls, Vector2.One, groundArea, player, furnitureList, lanternList) { }

        public Scene(int id, Texture2D ground, Texture2D walls, Vector2 backgroundScale, Rectangle groundArea, Player player, List<Furniture> furnitureList, List<Lantern> lanternList)
        {
            this.id = id;
            this._ground = ground;
            this.walls = walls;
            this.backgroundScale = backgroundScale;
             
            this.groundArea = groundArea;

            this.player = player;
            this.furnitureList = furnitureList;
            this.lanternList = lanternList;
        }


        // Methods
        public void Update(GameTime gameTime)
        {

            player.Update(gameTime, this.furnitureList, this);

            // lights
            Game1.penumbra.Lights.AddRange(player.lights);
            foreach (foreignPlayer otherPlayer in Client.listOtherPlayer) 
            {
                if (otherPlayer._id != player.id && otherPlayer.currentScene == this.id)
                {
                    if (otherPlayer.light.Enabled)
                    {
                        Game1.penumbra.Lights.Add(otherPlayer.light);
                    }

                    if (otherPlayer.toolLight.Enabled)
                    {
                        Game1.penumbra.Lights.Add(otherPlayer.toolLight);
                    }
                }
            }

            foreach (Furniture furniture in this.furnitureList)
            {
                Game1.penumbra.Hulls.Add(furniture.hull);

                // Update furniture hulls (hunter only)
                // if the player is inside a furniture object, add the furniture hull to the players IgnoredHulls list, else remove it from the list
                if (isInside(furniture, player))
                {
                    if(player.GetType() == typeof(Hunter))
                    {
                        Hunter hunter = player as Hunter;
                        if (!hunter.currentTool.light.IgnoredHulls.Contains(furniture.hull))
                        {
                            hunter.currentTool.light.IgnoredHulls.Add(furniture.hull);
                        }
                    }
                    else
                    {
                        Ghost ghost = player as Ghost;
                        if (!ghost.light.IgnoredHulls.Contains(furniture.hull))
                        {
                            ghost.light.IgnoredHulls.Add(furniture.hull);
                        }
                    }
                }
                else
                {
                    if (player.GetType() == typeof(Hunter))
                        (player as Hunter).currentTool.light.IgnoredHulls.Remove(furniture.hull);
                    else
                        (player as Ghost).light.IgnoredHulls.Remove(furniture.hull);
                }
            }

            // Update lanterns
            foreach (Lantern lantern in this.lanternList)
            {
                if (lantern.isOn)       
                {
                    Game1.penumbra.Lights.Add(lantern.light);
                }

                lantern.Update(gameTime);
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
            
            if (player.rectangle.Right+5 >= furnitureRect.Left-5 &&
                player.rectangle.Left-5 <= furnitureRect.Right+5 &&
                player.rectangle.Bottom >= furnitureRect.Top &&
                player.rectangle.Top <= furnitureRect.Bottom)
            {
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw background image
            spriteBatch.Draw(this._ground, Screen.OriginalScreenSize / 2, null, Color.White, 0, this._ground.Bounds.Center.ToVector2(), this.backgroundScale, 0, 0);

            // draw game objects (player, furniture)
            List<GameObject> gameObjectList = new List<GameObject>();
            List<GameObject> ghostList = new List<GameObject>();

            gameObjectList.AddRange(furnitureList);

            List<foreignPlayer> otherPlayersToDraw = new List<foreignPlayer>();
            foreach (foreignPlayer otherPlayer in Client.listOtherPlayer)
            {
                if (otherPlayer._id != player.id && otherPlayer.currentScene == this.id)
                {
                    otherPlayersToDraw.Add(otherPlayer);
                }
            }
            gameObjectList.AddRange(otherPlayersToDraw);

            // filter by player type
            if (this.player.GetType() == typeof(Hunter))
            {
                gameObjectList.Add(player);
            }

            if(this.player.GetType() == typeof(Ghost))
            {
                ghostList.Add(player);
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

            // Draw Lanterns
            foreach (Lantern lantern in this.lanternList)
            {
                lantern.Draw(spriteBatch);
            }

            // draw collision box
            //spriteBatch.Draw(Game1.defaultTexture, this.walls, null, Color.Purple * 0.5f);
        }
    }
}
