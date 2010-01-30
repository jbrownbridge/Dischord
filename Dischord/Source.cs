using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Dischord
{
    public class Source : Entity
    {
        protected float lifeTimer;

        public Source(Point position, Sprite sprite) : base(position, sprite) {
            lifeTimer = 0;
        }

        public Source(Point position, Sprite sprite, float lifeTimer, SoundEffect sound) : this(position,sprite) {
            if(lifeTimer > 0)
                this.lifeTimer = lifeTimer;
            else
                this.lifeTimer = (float)sound.Duration.TotalMilliseconds;

            sound.Play();
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
