using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dischord
{
    public abstract class Entity
    {
        public Entity(Point position, Sprite sprite, float animationInterval)
        {
            this.position           = position;
            this.sprite             = sprite;
            this.animationInterval  = animationInterval;
            this.currentFrame       = 0;
            this.animationTimer     = 0.0f;
            this.destRectangle      = new Rectangle(position.X, position.Y, Game.TILE_WIDTH, Game.TILE_HEIGHT);
        }

        public Entity(Point position, Sprite sprite) : this(position, sprite, 0.0f) {}

        public Point Position
        {
            get
            {
                return position;
            }
        }
        protected Point position;

        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
        }
        protected Sprite sprite;

        private float animationInterval;
        private float animationTimer;
        private int currentFrame;
        private Rectangle destRectangle;

        public virtual void Update(GameTime gameTime)
        {
            if (animationInterval > 0f && sprite.FrameCount > 1) {
                animationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (animationTimer > animationInterval)
                {
                    currentFrame++;
                    currentFrame %= sprite.FrameCount;
                    animationTimer = 0f;
                }
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            Sprite.Draw(destRectangle, currentFrame);
        }

        public static Entity GetInstance(String data)
        {
            // TODO(jason)
            throw new NotImplementedException();
        }

        public MapCell MapCell
        {
            get
            {
                int x = Position.X / Game.TILE_WIDTH;
                int y = Position.Y / Game.TILE_HEIGHT;
                return Game.GetInstance().Map.getCell(x + 1, y + 1);
            }
        }
    }
}
