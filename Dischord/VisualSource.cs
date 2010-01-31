using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord {
    class VisualSource : Source {
        public VisualSource(Vector2 position, Sprite sprite, float lifeTimer, float visibility)
            : base(position, sprite) {
            if(sprite != null) {
                this.lifeTimer = lifeTimer;
                strength = visibility;
            }
            else {
                this.lifeTimer = 0;
                this.isAlive = false;
            }
        }
    }
}
