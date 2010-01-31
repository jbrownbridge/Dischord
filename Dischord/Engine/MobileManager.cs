using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dischord.Engine {
    public class MobileManager : SpriteManager {
        public MobileManager(Game game, string imagePath, int columns, int rows, string behaviourPath) : base(game, imagePath, columns, rows, behaviourPath) { }

        public override void Update(GameTime gameTime) {
            if(!reset) {
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Viewport viewport = Game.GraphicsDevice.Viewport;

                foreach(Mobile mobile in spriteList) {
                    mobile.UpdatePositionAndFrame(elapsedTime, viewport);
                }
            }
            else {
                spriteList.Clear();
            }
        }

        public override void Draw(GameTime gameTime) {
            foreach(Mobile mobile in spriteList) {
                mobile.Draw();
            }
        }
    }
}
