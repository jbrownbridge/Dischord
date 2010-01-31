using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Dischord {
    class SoundSource : Source {
        SoundEffectInstance sound;
        public SoundSource(Vector2 position, SoundEffectInstance sound, float duration, int volume) : base(position, null) {
            if(sound != null) {
                this.sound = sound;
                this.lifeTimer = duration;
            }
            else {
                this.lifeTimer = 0;
                this.isAlive = false;
            }

            if(this.isAlive && volume > 0) {
                int x = (int)position.X / Game.GetInstance().GetTileSet().TileWidth;
                int y = (int)position.Y / Game.GetInstance().GetTileSet().TileHeight;
                for(int i = x - volume; i <= x + volume; i++) {
                    for(int j = y - volume; j <= y + volume; j++) {
                        if(i != x || j != y) {
                            Game.GetInstance().EManager.Add(new Source(new Vector2(i * Game.GetInstance().GetTileSet().TileWidth, j * Game.GetInstance().GetTileSet().TileHeight), sprite, lifeTimer + (lifeTimer * 1f / (Math.Abs(x - i) + Math.Abs(y - j))), volume - (Math.Abs(x - i) + Math.Abs(y - j)) + 0.5f));
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
        public SoundSource(Vector2 position, SoundEffect sound, int volume) : this(position, sound.CreateInstance(), (float)sound.Duration.TotalMilliseconds, volume) { }
    }
}
