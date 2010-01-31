using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord.Engine
{
    public enum Facing
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public abstract class Sprite
    {
        private Vector2 position;
        private float maxSpeed;
        private Facing facing;
        private Vector2 movement;
        
        private SpriteManager spriteManager;
        private Animation animation;

        public Sprite(Vector2 position, float maxSpeed, Facing facing)
        {
            this.position = position;
            this.maxSpeed = maxSpeed;
            this.facing = facing;
            this.movement = Vector2.Zero;
        }

        public abstract void Initialize();

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float MaxSpeed
        {
            get { return maxSpeed; }
        }

        public Facing Facing
        {
            get { return facing; }
            set { facing = value; }
        }

        public Vector2 Movement
        {
            get { return movement; }
            set { movement = value; }
        }

        public Animation Animation
        {
            get { return animation; }
            set { animation = value; }
        }

        public SpriteManager SpriteManager
        {
            get { return spriteManager; }
            set { spriteManager = value; }
        }
    }
}
