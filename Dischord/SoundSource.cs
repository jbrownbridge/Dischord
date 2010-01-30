using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Dischord {
    class SoundSource : Source{
        public SoundSource(Point position, SoundEffect sound, int volume) : base(position,null) {
            if(sound != null) {
                this.lifeTimer = (float)sound.Duration.TotalMilliseconds;
            }
            else {
                this.lifeTimer = 0;
                this.isAlive = false;
            }

            if(this.isAlive && volume > 0) {
                int x = position.X / Game.TILE_WIDTH;
                int y = position.Y / Game.TILE_HEIGHT;
                for(int i = x - volume; i <= x + volume; i++) {
                    for(int j = y - volume; j <= y + volume; j++) {
                        if(i != x || j != y) {
                            Game.GetInstance().Map.Add(new Source(new Point(i * Game.TILE_WIDTH, j * Game.TILE_HEIGHT), null, lifeTimer + (lifeTimer * 1f / (Math.Abs(x - i) + Math.Abs(y - j))), volume - (Math.Abs(x - i) + Math.Abs(y - j)) + 0.5f));
                        }
                    }
                }
                lifeTimer *= 2.25f;
                strength = volume;
            }

            if(sound != null) {
                sound.Play();
            }

        }
    }
}
