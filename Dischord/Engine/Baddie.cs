using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Dischord.Engine {
    class Baddie : Mobile{
        protected Enemy enemy;
        protected float nextChange;
        protected Boolean blind;

        private Map map;
        private TileSet tileSet;
        Random rand;

        public Baddie(Vector2 position, float maxSpeed, Facing facing, Map map, TileSet tileSet) : base(position, maxSpeed, facing,map,tileSet) {
            this.map = map;
            this.tileSet = tileSet;
            direction = Direction.Stand;
            enemy = new Enemy(Position, Game.GetInstance().GetSprite("Wall"));
            Game.GetInstance().EManager.Add(enemy);
            nextChange = 0f;
            rand = new Random();
            blind = false;
        }

        public override void SetDirection(Direction direction) {
            this.direction = direction;
        }

        public override void UpdatePositionAndFrame(float elapsedTime, Viewport viewport) {
            if(enemy.IsMobile()) {
                base.UpdatePositionAndFrame(elapsedTime, viewport);
                nextChange -= elapsedTime;

                if(nextChange <= 0) {
                    switch(rand.Next(5)) {
                        case 0: direction = Direction.Up; break;
                        case 1: direction = Direction.Down; break;
                        case 2: direction = Direction.Left; break;
                        case 3: direction = Direction.Right; break;
                        case 4: direction = Direction.Stand; break;
                    }
                    nextChange = 1f;
                }

                EntityManager manager = Game.GetInstance().EManager;
                int xval = (int)(Position.X / tileSet.TileWidth);
                int yval = (int)(Position.Y / tileSet.TileHeight);

                int tx = xval;
                int ty = yval;
                float strength = 0;

                foreach(Entity e in enemy.Cell.Entities) {
                    if(e is Smoke) {
                        blind = true;
                        break;
                    }
                    if(e is Character) {

                    }
                }

                for(int i = yval - 6; i < yval + 7; i++) {
                    for(int j = xval - 6; j < xval + 7; j++) {
                        if(i > 0 && i < map.Rows && j > 0 && j < map.Columns) {
                            float str = 0;
                            foreach(Entity e in manager.GetCell(j, i).Entities) {
                                if(e is Source) {
                                    str += ((e as Source).Strength + 1);
                                }
                                else if(e is Character && !blind) {
                                    str = 999;
                                }
                            }
                            if(str > strength) {
                                strength = str;
                                tx = j;
                                ty = i;
                            }
                        }
                    }
                }

                if(Math.Abs(tx - xval) > Math.Abs(ty - yval)) {
                    if(tx < xval)
                        direction = Direction.Left;
                    else
                        direction = Direction.Right;
                }
                else if(Math.Abs(tx - xval) < Math.Abs(ty - yval)) {
                    if(ty < yval)
                        direction = Direction.Up;
                    else
                        direction = Direction.Down;
                }

                enemy.ChangePos(Position);
            }
            blind = false;
        }
    }
}
