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
            this.moving             = false;
            this.isAlive            = true;
            this.facing = 5;
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
                destRectangle = new Rectangle(position.X, position.Y, Game.TILE_WIDTH, Game.TILE_HEIGHT);
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
                Char[] chartype = tokens[0].ToLower().ToCharArray();
                chartype[0] = chartype[0].ToString().ToUpper().ToCharArray()[0];
                String type = new String(chartype);

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

        public MapCell MapCell
        {
            get
            {
                int x = Position.X / Game.TILE_WIDTH;
                int y = Position.Y / Game.TILE_HEIGHT;
                return Game.GetInstance().Map.getCell(x + 1, y + 1);
            }
        }

        public virtual char toChar()
        {
            return '?';
        }

        public void move(Direction d)
        {
            switch (d)
            {
                case Direction.up:
                    position = new Point(Position.X, Position.Y-1);
                    facing = 5;
                    break;
                case Direction.down:
                    position = new Point(Position.X, Position.Y + 1);
                    facing = 1;
                    break;
                case Direction.left:
                    position = new Point(Position.X-1, Position.Y);
                    facing = 3;
                    break;
                case Direction.right:
                    position = new Point(Position.X+1, Position.Y);
                    facing = 7;
                    break;
                case Direction.still:
                    break;
                default:
                    throw new ArgumentException("This should *never* print");
            }
        }

        public int Facing
        {
            get { return facing; }
        }
    }
}
