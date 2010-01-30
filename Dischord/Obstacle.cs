using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord
{
    public class Obstacle : Entity
    {
        public Obstacle(Point position, Sprite sprite) : base(position, sprite) { }

        public override char toChar()
        {
            return 'O';
        }
    }
}
