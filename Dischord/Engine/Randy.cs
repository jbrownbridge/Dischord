using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Dischord.Engine
{
    public class Randy : Sprite
    {
        private Map map;
        private TileSet tileSet;

        public Randy(Vector2 position, float maxSpeed, Facing facing, Map map, TileSet tileSet)
            : base(position, maxSpeed, facing)
        {
            this.map = map;
            this.tileSet = tileSet;
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

        public void UpdatePositionAndFrame(float elapsedTime, Viewport viewport)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Left)) {
                if (this.Facing != Facing.Left)
                {
                    this.Facing = Facing.Left;
                    this.Animation.Behaviour = "WalkLeft";
                }
                this.Movement = new Vector2(-this.MaxSpeed * elapsedTime, 0);
                map.CheckCollision(this, tileSet);
                this.Position += this.Movement;
                this.Animation.UpdateFrame(elapsedTime);
            }
            else if (keyboard.IsKeyDown(Keys.Right))
            {
                if (this.Facing != Facing.Right)
                {
                    this.Facing = Facing.Right;
                    this.Animation.Behaviour = "WalkRight";
                }
                this.Movement = new Vector2(this.MaxSpeed * elapsedTime, 0);
                map.CheckCollision(this, tileSet);
                this.Position += this.Movement;
                this.Animation.UpdateFrame(elapsedTime);
            }
            else if (keyboard.IsKeyDown(Keys.Up))
            {
                if (this.Facing != Facing.Up)
                {
                    this.Facing = Facing.Up;
                    this.Animation.Behaviour = "WalkUp";
                }
                this.Movement = new Vector2(0, -this.MaxSpeed * elapsedTime);
                map.CheckCollision(this, tileSet);
                this.Position += this.Movement;
                this.Animation.UpdateFrame(elapsedTime);
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                if (this.Facing != Facing.Down)
                {
                    this.Facing = Facing.Down;
                    this.Animation.Behaviour = "WalkDown";
                }
                this.Movement = new Vector2(0, this.MaxSpeed * elapsedTime);
                map.CheckCollision(this, tileSet);
                this.Position += this.Movement;
                this.Animation.UpdateFrame(elapsedTime);
            }
            this.Position = new Vector2(
                MathHelper.Clamp(
                    this.Position.X,
                    viewport.X + this.Animation.AnimationManager.Origin.X,
                    map.Width - this.Animation.AnimationManager.Origin.X
                ),
                MathHelper.Clamp(
                    this.Position.Y,
                    viewport.Y + this.Animation.AnimationManager.Origin.Y,
                    map.Height - this.Animation.AnimationManager.Origin.Y
                )
            );
        }

        /*public void Draw()
        {
            this.SpriteManager.SpriteBatch.Draw(
                this.SpriteManager.SpriteImage, this.Position,
                this.Animation.Frame.Rectangle,
                Color.White, this.Animation.Frame.Rotation,
                this.Animation.AnimationManager.Origin, 1.0f,
                this.Animation.Frame.SpriteEffects, 0.5f
            );                
        }*/

        public void Draw()
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
