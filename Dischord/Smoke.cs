using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord {
    class Smoke : VisualSource {
        protected float visibility;
        protected float loopTimer;

        protected float layer;

        public Smoke(Vector2 position, float lifeTimer, float visibility)
            : base(position, Game.GetInstance().GetSprite("Smoke"), lifeTimer, visibility) {
            this.visibility = visibility;
            loopTimer = lifeTimer;
            layer = 0.6f;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if(!isAlive && visibility > 1) {
                visibility /= 2;
                lifeTimer = loopTimer;
                isAlive = true;
                Random rand = new Random(gameTime.TotalRealTime.Milliseconds);
                for(int i = 0; i < 4; i++) {
                    int nx = (int)position.X + (rand.Next(3) - 1) * Game.TILE_WIDTH;
                    int ny = (int)position.Y + (rand.Next(3) - 1) * Game.TILE_HEIGHT;
                    Game.GetInstance().EManager.Add(new Smoke(new Vector2(nx, ny), loopTimer*0.75f, visibility));
                }
            }
        }
    }
}
