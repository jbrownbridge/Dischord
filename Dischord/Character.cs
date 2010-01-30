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
        protected Controls controls;
        protected SoundEffectInstance walkingSound;
        protected float walkingSoundDuration;

        public Character(Point position, Sprite sprite) : base(position, sprite) {
            controls = Game.GetInstance().GetCharacterControls();
            walkingSound = Game.GetInstance().GetSound("Walk").CreateInstance();
            walkingSoundDuration = (float)Game.GetInstance().GetSound("Walk").Duration.Milliseconds;
            walkingSound.Volume = 1f;
        }

        public Character(Point position, Sprite sprite, float animationInterval) : base(position, sprite, animationInterval) {
            controls = Game.GetInstance().GetCharacterControls();
            walkingSound = Game.GetInstance().GetSound("Walk").CreateInstance();
            walkingSoundDuration = (float)Game.GetInstance().GetSound("Walk").Duration.Milliseconds;
            walkingSound.Volume = 1f;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if(controls.Direction != 0) {
                animationInterval = 100f;
                //Start playing walk sound if we were stationary
                if(moving == false)
                    Game.GetInstance().Map.Add(new SoundSource(position, walkingSound, walkingSoundDuration, 1));
                
                //Restart the sound if it has finished
                if(walkingSound.State == SoundState.Stopped)
                    Game.GetInstance().Map.Add(new SoundSource(position, walkingSound, walkingSoundDuration, 1));

                moving = true;
                facing = controls.Direction;
            }
            else {
                animationInterval = 0f;
                //Stop the walking sound
                if(moving == true)
                    walkingSound.Stop();

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

        public override char toChar() {
            return 'C';
        }
    }
}
