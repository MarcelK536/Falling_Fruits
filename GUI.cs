using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Falling_Fruits
{
    class GUI
    {
        private Texture2D GUITexture;
        private Rectangle GUIRectangle;

        private string assetName;
        public string AssetName { get => assetName; set => assetName = value; }

        public delegate void ElementClicked(string element);

        public event ElementClicked clickEvent;

        bool delay = false;
        double delayTime = 0.15;

        public GUI(string assetName)
        {
            this.assetName = assetName;
        }

        public void LoadContent(Texture2D texture2D)
        {
            GUITexture = texture2D;
            GUIRectangle = new Rectangle(0, 0, GUITexture.Width, GUITexture.Height);
        }

        public bool Update(GameTime gameTime)
        {
            if (delay == false)
            {
                delay = true;
                delayTime = 0.1;
                return (GUIRectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed);
            }
            delayTime -= gameTime.ElapsedGameTime.TotalSeconds;
            if (delayTime <= 0)
            {
                delay = false;
            }
            return false;
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GUITexture, GUIRectangle, Color.White);
        }

        public void CenterElement(int width, int height)
        {
            GUIRectangle = new Rectangle((width / 2) - (this.GUITexture.Width / 2), (height / 2) - (this.GUITexture.Height / 2), this.GUITexture.Width, this.GUITexture.Height);
        }

        public void MoveElement(int x,int y)
        {
            GUIRectangle = new Rectangle(GUIRectangle.X += x, GUIRectangle.Y += y,GUIRectangle.Width, GUIRectangle.Height);
        }
    }
}
