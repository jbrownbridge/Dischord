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

            if(controls.Direction != 0) {
                animationInterval = 100f;
                moving = true;
                facing = controls.Direction;
            }
            else {
                animationInterval = 0f;
                moving = false;
            }
            
            if(moving) {
                if(facing > 1 && facing < 5)
                    --position.X;
                else if(facing > 5 && facing < 9)
                    ++position.X;

                if(facing > 3 && facing < 7)
                    --position.Y;
                else if(facing == 2 || facing == 1 || facing == 8)
                    ++position.Y;
            }
        }

        public override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
        }
    }
}
