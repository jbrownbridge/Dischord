using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord
{
    public class Character : Entity
    {
        protected Controls controls;

        public Character(Point position, Sprite sprite) : base(position, sprite) {
            controls = Game.GetInstance().GetCharacterControls();
        }

        public Character(Point position, Sprite sprite, float animationInterval) : base(position, sprite, animationInterval) {
            controls = Game.GetInstance().GetCharacterControls();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            
            if(controls.Direction > 1)
                position.X += 1;
            else if(controls.Direction < -1)
                position.X -= 1;

            if(controls.Direction == -2 || controls.Direction == 1 || controls.Direction == 4)
                position.Y -= 1;
            else if(controls.Direction == -4 || controls.Direction == -1 || controls.Direction == 2)
                position.Y += 1;

        }

        public override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
        }
    }
}
