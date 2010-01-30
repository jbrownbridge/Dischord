using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dischord.Engine
{
    public class Frame
    {
        private Rectangle rectangle;
        private int rotation;
        private SpriteEffects spriteEffects;

        public Frame(Rectangle rectangle, int rotation, SpriteEffects spriteEffects)
        {
            this.rectangle = rectangle;
            this.rotation = rotation;
            this.spriteEffects = spriteEffects;
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public int Rotation
        {
            get { return rotation; }
        }

        public SpriteEffects SpriteEffects
        {
            get { return spriteEffects; }
        }
    }
}
