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
