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
        protected int speed;

        private const int SPEED = 2;

        public void Immobilize() {
            speed = 0;
        }

        public void Mobilize() {
            speed = SPEED;
        }

        public Enemy(Vector2 position, Sprite sprite)
            : base(position, sprite) {
            wait = 0;
            speed = SPEED;
        }

        public void move(Direction d) {
            switch(d) {
                case Direction.up:
                    position = new Vector2(Position.X, Position.Y - speed);
                    facing = 5;
                    break;
                case Direction.down:
                    position = new Vector2(Position.X, Position.Y + speed);
                    facing = 1;
                    break;
                case Direction.left:
                    position = new Vector2(Position.X - speed, Position.Y);
                    facing = 3;
                    break;
                case Direction.right:
                    position = new Vector2(Position.X + speed, Position.Y);
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
