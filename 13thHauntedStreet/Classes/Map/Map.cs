/*
 * Author  : Marco Rodrigues
 * Project : 13th Haunted Street
 * Details : Map class
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class Map
    {
        // Properties
        private Player player;

        // first scene in the list is also the first scene displayed
        public List<Scene> listScenes;
        public Scene currentScene;

        public List<Door> doorList = new List<Door>();
        public List<Door> currentSceneDoors = new List<Door>();


        // Ctor
        public Map(Player player, List<Scene> listScenes)
        {
            this.player = player;

            this.listScenes = listScenes;
            this.currentScene = listScenes[0];
        }


        // Methods
        public void Update(GameTime gametime)
        {
            this.currentScene.Update(gametime);

            // select from all door, the doors from the current scene
            this.currentSceneDoors = this.doorList.Where(door => door.scene == this.currentScene).ToList();

            foreach (Door door in this.currentSceneDoors)
            {
                // if player has entered a door
                if (door.hasEntered() && door.connectedDoor != null)
                {
                    GoThroughDoor(door);
                }
            }
        }

        /// <summary>
        /// change the scene and the player position to simulate going through a door
        /// </summary>
        private void GoThroughDoor(Door door)
        {
            this.currentScene = door.connectedDoor.scene;
            player.position = door.connectedDoor.spawnPos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.currentScene.Draw(spriteBatch);

            //foreach (Door door in currentSceneDoors)
            //{
            //    spriteBatch.Draw(Game1.defaultTexture, door.area, null, Color.Yellow * 0.5f);
            //}
        }
    }
}
