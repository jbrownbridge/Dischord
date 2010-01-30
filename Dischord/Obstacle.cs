using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord
{
    public class Obstacle : Entity
    {
        
        public Obstacle(Point position, Sprite sprite) : base(position, sprite) { }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            foreach(Entity e in base.MapCell.Entities) {
                if(e is Character)
                    PlayerCollision();
                else if(e is Enemy)
                    EnemyCollision();
            }
        }

        public virtual void PlayerCollision() { }

        public virtual void EnemyCollision() { }

        public override char toChar()
        {
            return 'O';
        }
    }
}
