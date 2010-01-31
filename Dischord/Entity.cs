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
        public Entity(Vector2 position, Sprite sprite, float animationInterval)
        {
            this.position           = position;
            this.sprite             = sprite;
            this.animationInterval  = animationInterval;
            this.currentFrame       = 0;
            this.animationTimer     = 0.0f;
            this.destRectangle      = new Rectangle((int)position.X, (int)position.Y, Game.TILE_WIDTH, Game.TILE_HEIGHT);
            this.moving             = false;
            this.isAlive            = true;
            this.facing = 5;
        }

        public Entity(Vector2 position, Sprite sprite) : this(position, sprite, 0.0f) {}

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }
        protected Vector2 position;

        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
        }
        protected Sprite sprite;

        public bool IsAlive {
            get {
                return isAlive;
            }
        }
        protected bool isAlive;

        protected float animationInterval;
        private float animationTimer;
        private int currentFrame;
        private Rectangle destRectangle;
        protected int facing;
        protected bool moving;

        public virtual void Update(GameTime gameTime)
        {
            if(sprite != null) {
                if(animationInterval > 0f && sprite.FrameCount > 1) {
                    animationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if(animationTimer > animationInterval) {
                        currentFrame++;
                        currentFrame %= sprite.FrameCount;
                        animationTimer = 0f;
                    }
                }
                destRectangle = new Rectangle((int)position.X, (int)position.Y, Game.TILE_WIDTH, Game.TILE_HEIGHT);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            if(sprite != null)
                Sprite.Draw(destRectangle, currentFrame);
        }

        public static Entity GetInstance(String data)
        {
            // TODO(jason)
            String[] tokens = data.Split();
            if (tokens.Length >= 4)
            {
                //Char[] chartype = tokens[0].ToLower().ToCharArray();
                //chartype[0] = chartype[0].ToString().ToUpper().ToCharArray()[0];
                //String type = new String(chartype);

                String type = tokens[0];

                int x = int.Parse(tokens[1]);
                int y = int.Parse(tokens[2]);
                char direction = tokens[3][0];

                Type t = Type.GetType("Dischord." + type);
                if(t != null)
                    return (Entity)Activator.CreateInstance(t, new object[] { new Point(x, y), Game.GetInstance().GetSprite(type)});
                else
                    throw new ArgumentException(String.Format("Unknown entity type: {0}\n", type));
            }
            else
            {
                throw new ArgumentException(String.Format("Data has invalid format: {0}\n", data));
            }
        }

        public Engine.Cell Cell
        {
            get
            {
                Engine.TileSet tileSet = Game.GetInstance().GetTileSet();
                int x = (int)(Position.X / tileSet.TileWidth);
                int y = (int)(Position.Y / tileSet.TileHeight);
                return Game.GetInstance().EManager.GetCell(x,y);
            }
        }

        public virtual char toChar()
        {
            return '?';
        }

        public int Facing
        {
            get { return facing; }
        }
    }
}
