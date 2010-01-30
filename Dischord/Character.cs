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
    }
}
