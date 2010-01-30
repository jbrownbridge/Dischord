using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord
{
    public class Character : Entity
    {
        public Character(Point position, Sprite sprite) : base(position, sprite) {}
        public Character(Point position, Sprite sprite, Controls controls) : base(position, sprite) { 
        
        }
    }
}
