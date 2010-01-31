using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord {
    class Wall : StaticEntity {
        public Wall(Vector2 position) : base(position, Game.GetInstance().GetSprite("Wall")) { }
    }
}
