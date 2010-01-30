using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord {
    class Fire : VisualSource {
        protected const float VISIBILITY = 3f;
        protected const float SMOKE_TIME = 3000f;

        protected float loopTimer;

        public Fire(Point position, float lifeTimer) : base(position, Game.GetInstance().GetSprite("Enemy"), lifeTimer, VISIBILITY) {
            loopTimer = lifeTimer;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if(!isAlive && strength > 0) {
                --strength;
                isAlive = true;
                lifeTimer = loopTimer;
                Game.GetInstance().Map.Add(new Smoke(new Point(position.X, position.Y), SMOKE_TIME, VISIBILITY));
            }
        }
    }
}
