using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord
{
    public class Obstacle : Entity
    {

        public Obstacle(Vector2 position, Sprite sprite) : base(position, sprite) { }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            foreach(Entity e in base.Cell.Entities) {
                if(e is Character)
                    PlayerCollision((e as Character));
                else if(e is Enemy)
                    EnemyCollision((e as Enemy));
            }
        }

        public virtual void PlayerCollision(Character character) { }

        public virtual void EnemyCollision(Enemy enemy) { }

        public override char toChar()
        {
            return 'O';
        }
    }
}
