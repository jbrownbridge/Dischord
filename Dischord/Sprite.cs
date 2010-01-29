using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Dischord
{
    public class Sprite
    {
        public Sprite(Texture2D spriteSheet, int spriteWidth, int spriteHeight, int frameCount)
        {
            this.spriteSheet    = spriteSheet;  
            this.frameCount     = frameCount;
            this.spriteWidth    = spriteWidth;
            this.spriteHeight   = spriteHeight;
        }

        public void Draw(Rectangle destRectangle, int frameNumber)
        {
            frameNumber = frameNumber % frameCount;
            Rectangle sourceRect = new Rectangle(frameNumber * spriteWidth, 0, spriteWidth, spriteHeight);
            Game.GetInstance().SpriteBatch.Draw(spriteSheet, destRectangle, sourceRect, Color.White);
        }

        private Texture2D spriteSheet;

        public int SpriteWidth
        {
            get
            {
                return spriteWidth;
            }
        }
        private int spriteWidth;

        public int SpriteHeight
        {
            get
            {
                return spriteHeight;
            }
        }
        private int spriteHeight;

        public int FrameCount
        {
            get
            {
                return frameCount;
            }
        }
        private int frameCount;
    }
}
