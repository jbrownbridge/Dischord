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
        public Enemy(Point position, Sprite sprite) : base(position, sprite) { }
    }
}
