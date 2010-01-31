using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord {
    class Goal : Obstacle {
        public Goal(Vector2 position) : base(position, Game.GetInstance().GetSprite("Immobilized"),100f) {
            
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void PlayerCollision(Character character)
        {
            base.PlayerCollision(character);
            Game.GetInstance().ControlMode = ControlMode.nextlevel;
        }
    }
}
