using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Dischord
{
    public class Character : Entity
    {
        public Character(Vector2 position) : base(position, Game.GetInstance().GetSprite("Wall")) {
            this.position = position;
        }

        public void ChangePos(Vector2 position) {
            this.position = position;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            //base.Draw(gameTime);
        }

        public bool God
        {
            get
            {
                return true;
            }
        }
    }
}
