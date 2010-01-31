using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord
{
    public class StaticEntity : Entity
    {
        public StaticEntity(Vector2 position, Sprite sprite) : base(position, sprite) { }

        public StaticEntity(Vector2 position, Sprite sprite, float animationInterval) : base(position, sprite) { }
    }
}
