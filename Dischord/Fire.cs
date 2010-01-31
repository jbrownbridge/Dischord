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

        public Fire(Vector2 position, float lifeTimer)
            : base(position, Game.GetInstance().GetSprite("Fire"), lifeTimer, VISIBILITY) {
            loopTimer = lifeTimer;
            Game.GetInstance().EManager.Add(new SoundSource(position, Game.GetInstance().GetSound("Crackle"), 2));
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if(!isAlive && strength > 0) {
                --strength;
                isAlive = true;
                lifeTimer = loopTimer;
                Game.GetInstance().EManager.Add(new Smoke(new Vector2(position.X, position.Y), SMOKE_TIME, VISIBILITY));
                Game.GetInstance().EManager.Add(new SoundSource(position, Game.GetInstance().GetSound("Crackle"), 2));
            }
        }
    }
}
