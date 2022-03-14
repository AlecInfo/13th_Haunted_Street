using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _13thHauntedStreet
{
    class MainMenuSetting
    {
        private Texture2D _buttonTexture;

        private SpriteFont _font;

        private bool _isOnControls = false;

        private List<Button> _buttonList = new List<Button>();
        private List<ArrowButton> _arrowButtonList = new List<ArrowButton>();


        public MainMenuSetting()
        {
        
        }

        public void Load(Screen screen)
        {
            // Create a rectangle texture
            this._buttonTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            this._buttonTexture.SetData(new Color[] { Color.White });

            _arrowButtonList.Add(
                new ArrowButton(this._buttonTexture, this._font)
                {
                    Text = "Full screen",
                    Position = new Vector2(screen.OriginalScreenSize.X / 3.05f, screen.OriginalScreenSize.Y / 1.46f),
                    EnableMode = true,
                    State = true
                }
                );
        }

        public void Update(GameTime gameTime)
        {
            foreach (var item in this._arrowButtonList)
            {
                item.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
