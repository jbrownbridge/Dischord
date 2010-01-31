using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Dischord.Engine
{
    public class Randy : Mobile
    {
        private Map map;
        private TileSet tileSet;
        private Vector2 renderPos;
        private Boolean moving;
        private Character character;
        private float nextGlue;
        private float nextLyre;
        private float nextFire;

        protected SoundEffectInstance walkingSound;
        protected float walkingSoundDuration;

        public Vector2 RenderPos {
            get {
                return renderPos;
            }
        }

        public void useGlue() {
            Game.GetInstance().EManager.Add(new GlueTrap(Position, Game.GetInstance().GetSprite("GlueTrap")));
            Game.GetInstance().EManager.Add(new SoundSource(Position, Game.GetInstance().GetSound("GlueTrap"),0));
            nextGlue = 20000f;
        }

        public void useLyre() {
            Game.GetInstance().EManager.Add(new SoundSource(Position, Game.GetInstance().GetSound("Lyre"),12));
            nextLyre = 5000f;
        }

        public void useFire() {
            Game.GetInstance().EManager.Add(new Fire(Position, 6000f));
            nextFire = 25000f;
        }

        public Randy(Vector2 position, float maxSpeed, Facing facing, Map map, TileSet tileSet) : base(position, maxSpeed, facing,map,tileSet) {
            this.direction = direction;
            this.map = map;
            this.tileSet = tileSet;
            renderPos = new Vector2(0, 0);
            
            walkingSound = Game.GetInstance().GetSound("Walk").CreateInstance();
            walkingSoundDuration = (float)Game.GetInstance().GetSound("Walk").Duration.TotalMilliseconds;
            walkingSound.Volume = 1f;
            moving = false;
            character = new Character(Position);
            Game.GetInstance().EManager.Add(character);
            nextLyre = 0;
            nextGlue = 0;
            nextFire = 0;
            Game.GetInstance().EManager.Add(new Goal(Position));
        }

        public override void Initialize()
        {
            switch (this.Facing)
            {
                case Facing.Left:
                    this.Animation.Behaviour = "WalkLeft";
                    break;
                case Facing.Right:
                    this.Animation.Behaviour = "WalkRight";
                    break;
                case Facing.Up:
                    this.Animation.Behaviour = "WalkUp";
                    break;
                case Facing.Down:
                    this.Animation.Behaviour = "WalkDown";
                    break;
            }
        }

        public override void UpdatePositionAndFrame(float elapsedTime, Viewport viewport) {
            base.UpdatePositionAndFrame(elapsedTime, viewport);
            if(direction != Direction.Stand) {
                if(moving == false)
                    Game.GetInstance().EManager.Add(new SoundSource(Position, walkingSound, walkingSoundDuration, 1));

                //Restart the sound if it has finished
                if(walkingSound.State == SoundState.Stopped)
                    Game.GetInstance().EManager.Add(new SoundSource(Position, walkingSound, walkingSoundDuration, 1));

                moving = true;
            }
            else {
                //Stop the walking sound
                if(moving == true)
                    walkingSound.Stop();

                moving = false;
            }

            character.ChangePos(Position);
        }

        public override void Draw()
        {
            Texture2D sprite = this.SpriteManager.SpriteImage;
            Vector2 pos = new Vector2();

            SpriteBatch spriteBatch = Game.GetInstance().SpriteBatch;
            
            float viewportHeight = spriteBatch.GraphicsDevice.Viewport.Height;
            float viewportWidth = spriteBatch.GraphicsDevice.Viewport.Width;
            //float left = Math.Max(0,Math.Min((spriteBatch.GraphicsDevice.Viewport.Width - sprite.Width)/2, Position.X));

            pos.X = viewportWidth / 2;
            pos.Y = viewportHeight / 2;

            if (Position.Y < pos.Y)
            {
                pos.Y = Math.Max(0, Position.Y);
            }
            else if (Position.Y > map.Height - (viewportHeight / 2))
            {
                pos.Y = Math.Min(viewportHeight, viewportHeight - map.Height + Position.Y);// Math.Min(map.Height, Position.Y);
            }
            if (Position.X < pos.X)
            {
                pos.X = Math.Max(0, Position.X);
            }
            else if (Position.X > map.Width - (viewportWidth / 2))
            {
                pos.X = Math.Min(viewportWidth, viewportWidth - map.Width + Position.X);// Math.Min(map.Height, Position.Y);
            }

            renderPos = pos;

            this.SpriteManager.SpriteBatch.Draw(
                this.SpriteManager.SpriteImage, pos,
                this.Animation.Frame.Rectangle,
                Color.White, this.Animation.Frame.Rotation,
                this.Animation.AnimationManager.Origin, 1.0f,
                this.Animation.Frame.SpriteEffects, 0.5f
            );                
                

        }
    }
}
