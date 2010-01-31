using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Dischord.Engine {
    class Baddie : Mobile{
        protected Enemy enemy;
        protected float nextChange;

        private Map map;
        private TileSet tileSet;

        public Baddie(Vector2 position, float maxSpeed, Facing facing, Map map, TileSet tileSet) : base(position, maxSpeed, facing,map,tileSet) {
            this.map = map;
            this.tileSet = tileSet;
            direction = Direction.Stand;
            enemy = new Enemy(Position, Game.GetInstance().GetSprite("Wall"));
            Game.GetInstance().EManager.Add(enemy);
            nextChange = 0f;
        }

        public override void SetDirection(Direction direction) {
            this.direction = direction;
        }

        public override void UpdatePositionAndFrame(float elapsedTime, Viewport viewport) {
            /*nextChange -= elapsedTime;
            if(nextChange <= 0) {
                Random rand = new Random((int)elapsedTime);
                switch(0) {
                    case 0: direction = Direction.Up; break;
                    case 1: direction = Direction.Down; break;
                    case 2: direction = Direction.Left; break;
                    case 3: direction = Direction.Right; break;
                    case 4: direction = Direction.Stand; break;
                }
                nextChange = 1f;
            }*/

            if(enemy.IsMobile()) {
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
            }
            enemy.ChangePos(Position);
        }
    }
}
