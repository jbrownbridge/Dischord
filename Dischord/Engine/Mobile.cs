using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Dischord.Engine {
    public class Mobile : Sprite {
        public enum Direction {
            Stand,
            Left,
            Right,
            Up,
            Down,
        };

        protected Direction direction;

        private Map map;
        private TileSet tileSet;

        public Mobile(Vector2 position, float maxSpeed, Facing facing, Map map, TileSet tileSet) : base(position, maxSpeed, facing) {
            this.map = map;
            this.tileSet = tileSet;
            direction = Direction.Stand;
        }

        public override void Initialize() {
            switch(this.Facing) {
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

        public virtual void SetDirection(Direction direction) {
            this.direction = direction;
        }

        public virtual void UpdatePositionAndFrame(float elapsedTime, Viewport viewport) {
            if(direction == Direction.Left) {
                if(this.Facing != Facing.Left) {
                    this.Facing = Facing.Left;
                    this.Animation.Behaviour = "WalkLeft";
                }
                this.Movement = new Vector2(-this.MaxSpeed * elapsedTime, 0);
                map.CheckCollision(this, tileSet);
                this.Position += this.Movement;
                this.Animation.UpdateFrame(elapsedTime);
            }
            else if(direction == Direction.Right) {
                if(this.Facing != Facing.Right) {
                    this.Facing = Facing.Right;
                    this.Animation.Behaviour = "WalkRight";
                }
                this.Movement = new Vector2(this.MaxSpeed * elapsedTime, 0);
                map.CheckCollision(this, tileSet);
                this.Position += this.Movement;
                this.Animation.UpdateFrame(elapsedTime);
            }
            else if(direction == Direction.Up) {
                if(this.Facing != Facing.Up) {
                    this.Facing = Facing.Up;
                    this.Animation.Behaviour = "WalkUp";
                }
                this.Movement = new Vector2(0, -this.MaxSpeed * elapsedTime);
                map.CheckCollision(this, tileSet);
                this.Position += this.Movement;
                this.Animation.UpdateFrame(elapsedTime);
            }
            else if(direction == Direction.Down) {
                if(this.Facing != Facing.Down) {
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

        public virtual void Draw() {
            Texture2D sprite = this.SpriteManager.SpriteImage;
            Vector2 pos = new Vector2();
            Vector2 randyPos = Game.GetInstance().CharacterPosition();

            SpriteBatch spriteBatch = Game.GetInstance().SpriteBatch;
            

            float viewportHeight = spriteBatch.GraphicsDevice.Viewport.Height;
            float viewportWidth = spriteBatch.GraphicsDevice.Viewport.Width;

            if(Math.Abs(randyPos.X - Position.X) < spriteBatch.GraphicsDevice.Viewport.Width) {
                if(Math.Abs(randyPos.Y - Position.Y) < spriteBatch.GraphicsDevice.Viewport.Height) {
                    Vector2 renderPos = Game.GetInstance().RenderPosition();
                    

                    pos.X = renderPos.X - (randyPos.X - Position.X);
                    pos.Y = renderPos.Y - (randyPos.Y - Position.Y);

                    this.SpriteManager.SpriteBatch.Draw(
                        this.SpriteManager.SpriteImage, pos,
                        this.Animation.Frame.Rectangle,
                        Color.Red, this.Animation.Frame.Rotation,
                        this.Animation.AnimationManager.Origin, 1.0f,
                        this.Animation.Frame.SpriteEffects, 0.5f
                    );
                }
            }

        }
    }
}
