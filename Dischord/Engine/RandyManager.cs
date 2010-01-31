using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Dischord.Engine
{    
    public class RandyManager : MobileManager
    {
        public RandyManager(
            Game game, string imagePath, int columns, int rows, string behaviourPath) : base(game, imagePath, columns, rows, behaviourPath)
        {
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Viewport viewport = Game.GraphicsDevice.Viewport;

            foreach (Randy randy in spriteList)
            {
                randy.UpdatePositionAndFrame(elapsedTime, viewport);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Randy randy in spriteList)
            {
                randy.Draw();
            }
        }
    }
}
