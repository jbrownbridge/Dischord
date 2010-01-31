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
        private float layer;
        public Sprite(Texture2D spriteSheet, int spriteWidth, int spriteHeight, int frameCount)
        {
            this.spriteSheet    = spriteSheet;  
            this.frameCount     = frameCount;
            this.spriteWidth    = spriteWidth;
            this.spriteHeight   = spriteHeight;
            layer = 0.5f;
        }

        public Sprite(Texture2D spriteSheet, int spriteWidth, int spriteHeight, int frameCount, float layer) : this(spriteSheet,spriteWidth,spriteHeight,frameCount){
            this.layer = layer;
        }

        public void Draw(Rectangle destRectangle, int frameNumber)
        {
            Vector2 pos = new Vector2();
            Vector2 randyPos = Game.GetInstance().CharacterPosition();

            SpriteBatch spriteBatch = Game.GetInstance().SpriteBatch;
            

            float viewportHeight = spriteBatch.GraphicsDevice.Viewport.Height;
            float viewportWidth = spriteBatch.GraphicsDevice.Viewport.Width;

            if(Math.Abs(randyPos.X - destRectangle.X) < spriteBatch.GraphicsDevice.Viewport.Width) {
                if(Math.Abs(randyPos.Y - destRectangle.Y) < spriteBatch.GraphicsDevice.Viewport.Height) {
                    Vector2 renderPos = Game.GetInstance().RenderPosition();


                    pos.X = renderPos.X - (randyPos.X - destRectangle.X);
                    pos.Y = renderPos.Y - (randyPos.Y - destRectangle.Y);

                    frameNumber = frameNumber % frameCount;
                    Rectangle sourceRect = new Rectangle(frameNumber * spriteWidth, 0, spriteWidth, spriteHeight);
                    destRectangle.X = (int)pos.X;
                    destRectangle.Y = (int)pos.Y;

                    spriteBatch.Draw(spriteSheet, destRectangle, sourceRect, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, layer);

                }
            }
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
