using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord {
    class GlueTrap : Obstacle {
        private Boolean triggered;
        private float traptime;
        private Enemy target;

        public GlueTrap(Point position,Sprite sprite) : base(position,sprite) {
            triggered = false;
            traptime = 8000f;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if(triggered)
                traptime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(traptime < 0) {
                target.Mobilize();
                isAlive = false;
            }
        }

        public override void Draw(GameTime gameTime) {
            if(!triggered)
                base.Draw(gameTime);
        }

        public override void EnemyCollision(Enemy enemy) {
            if(!triggered) {
                triggered = true;
                enemy.Immobilize();
                target = enemy;
            }
        }
    }
}
