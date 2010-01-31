using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dischord {
    class HudItem {
        protected Texture2D texture;
        protected Point position;
        public HudItem(Texture2D texture, Point position) {
            this.position = position;
            this.texture = texture;
        }

        public Texture2D Texture {
            get {
                return texture;
            }
        }

        public Point Position {
            get {
                return position;
            }
        }
    }
}
