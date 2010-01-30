using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord {
    class Smoke : VisualSource {
        protected float visibility;
        protected float loopTimer;

        public Smoke(Point position, float lifeTimer, float visibility) : base(position, Game.GetInstance().GetSprite("Smoke"), lifeTimer, visibility) {
            this.visibility = visibility;
            loopTimer = lifeTimer;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if(!isAlive && visibility > 1) {
                visibility /= 2;
                lifeTimer = loopTimer;
                isAlive = true;
                Random rand = new Random(gameTime.TotalRealTime.Milliseconds);
                for(int i = 0; i < 4; i++) {
                    int nx = position.X + (rand.Next(3) - 1) * Game.TILE_WIDTH;
                    int ny = position.Y + (rand.Next(3) - 1) * Game.TILE_HEIGHT;
                    Game.GetInstance().Map.Add(new Smoke(new Point(nx, ny), loopTimer*0.75f, visibility));
                }
            }
        }
    }
}
