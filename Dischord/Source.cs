using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord
{
    public class Source : Entity
    {
        protected float lifeTimer;
        protected float strength;
        public float Strength
        {
            get { return strength; }
        }

        public Source(Vector2 position, Sprite sprite) : base(position, sprite) {
            lifeTimer = 0;
        }

        public Source(Vector2 position, Sprite sprite, float lifeTimer, float strength)
            : this(position, sprite) {
            this.lifeTimer = lifeTimer;
            this.strength = strength;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            lifeTimer -= gameTime.ElapsedGameTime.Milliseconds;
            if(lifeTimer < 0) {
                isAlive = false;
            }
        }
    }
}
