using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dischord
{
    public class Enemy : Entity
    {
        protected int wait;

        public Enemy(Point position, Sprite sprite) : base(position, sprite) {
            wait = 0;
        }

        public void move(Direction d) {
            switch(d) {
                case Direction.up:
                    position = new Point(Position.X, Position.Y - 1);
                    facing = 5;
                    break;
                case Direction.down:
                    position = new Point(Position.X, Position.Y + 1);
                    facing = 1;
                    break;
                case Direction.left:
                    position = new Point(Position.X - 1, Position.Y);
                    facing = 3;
                    break;
                case Direction.right:
                    position = new Point(Position.X + 1, Position.Y);
                    facing = 7;
                    break;
                case Direction.still:
                    break;
                default:
                    throw new ArgumentException("This should *never* print");
            }
        }

        public override char toChar()
        {
            return 'E';
        }

        public int Wait
        {
            get { return wait; }
            set { wait = value; }
        }
    }
}
