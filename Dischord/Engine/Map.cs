using System;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Linq;


namespace Dischord.Engine
{
    public class Map
    {
        public const int TRANSPARENT_INDEX = 1244;
        public const int ABOVE_PLAYER_LAYER_START = 8;

        private int columns;
        private int rows;
        private int[,] walkable;
        private List<Terrain> terrainList;
        private int tileSize;

        public Map(string xmlFile, bool convert)
        {
            Terrain lastTerrainAdded;
            TileRow lastTileRowAdded;
            terrainList = new List<Terrain>();
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(xmlFile))
            {
                throw new FileNotFoundException();
            }
            xmlDoc.Load(xmlFile);
    

            rows = int.Parse(xmlDoc.SelectSingleNode("MAP/MAP_DIMENSIONS/Height").FirstChild.Value);
            columns = int.Parse(xmlDoc.SelectSingleNode("MAP/MAP_DIMENSIONS/Width").FirstChild.Value);
            tileSize = int.Parse(xmlDoc.SelectSingleNode("MAP/MAP_DIMENSIONS/Tile_Size").FirstChild.Value);

            walkable = new int[rows, columns];
            XmlNodeList walkableTileRows = xmlDoc.SelectNodes("MAP/LAYOUT/CollisionLayer/RowInfo");

            int x = 0, y = 0;

            // Setup Layers
            XmlNodeList terrains = xmlDoc.SelectNodes("MAP/LAYOUT/Layer");
            foreach (XmlNode terrain in terrains)
            {
                lastTerrainAdded = new Terrain();
                terrainList.Add(lastTerrainAdded);

                XmlNodeList tileRows = terrain.SelectNodes("RowInfo");

                foreach (XmlNode tileRow in tileRows)
                {
                    lastTileRowAdded = new TileRow();
                    lastTerrainAdded.TileRowList.Add(lastTileRowAdded);

                    String[] tiles = tileRow.FirstChild.Value.Split(',');
                    foreach (String tile in tiles)
                    {
                        int tileCode = int.Parse(tile);
                        if (tileCode == -1)
                            tileCode = TRANSPARENT_INDEX;
                        
                        lastTileRowAdded.TileList.Add(new Tile(tileCode, 0));
                    }
                }
                //break;
            }

            // Setup Walkable
            y = 0;
            foreach (XmlNode walkableTileRow in walkableTileRows)
            {
                String[] walkableTiles = walkableTileRow.FirstChild.Value.Split(',');
                x = 0;
                foreach (String walkableTile in walkableTiles)
                {
                    walkable[y, x] = int.Parse(walkableTile);
                    x++;
                }
                y++;
            }
        }

        public Map(string xmlFile)
        {
            Terrain lastTerrainAdded;
            TileRow lastTileRowAdded;
            XmlDocument xmlDoc;
            int x, y;

            terrainList = new List<Terrain>();
            xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);

            XmlNodeList terrains = xmlDoc.SelectNodes("Map/TerrainLayer");
            foreach (XmlNode terrain in terrains)
            {
                lastTerrainAdded = new Terrain();
                terrainList.Add(lastTerrainAdded);

                XmlNodeList tileRows = terrain.SelectNodes("TileRow");
                rows = tileRows.Count;
                foreach (XmlNode tileRow in tileRows)
                {
                    lastTileRowAdded = new TileRow();
                    lastTerrainAdded.TileRowList.Add(lastTileRowAdded);

                    XmlNodeList tiles = tileRow.SelectNodes("Tile");
                    columns = tiles.Count;
                    foreach (XmlNode tile in tiles)
                    {
                        lastTileRowAdded.TileList.Add(
                            new Tile(
                                int.Parse(tile.Attributes["Position"].Value),
                                int.Parse(tile.Attributes["Rotation"].Value)));
                    }

                    walkable = new int[rows, columns];
                    XmlNodeList walkableTileRows = xmlDoc.SelectNodes("Map/WalkableLayer/TileRow");

                    y = 0;
                    foreach (XmlNode walkableTileRow in walkableTileRows)
                    {
                        XmlNodeList walkableTiles = walkableTileRow.SelectNodes("Tile");
                        x = 0;
                        foreach (XmlNode walkableTile in walkableTiles)
                        {
                            walkable[y, x] = int.Parse(walkableTile.Attributes["Walkable"].Value);
                            x++;
                        }
                        y++;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, TileSet tileset, Vector2 position)
        {
            float randyRow = (float)position.Y / tileset.TileHeight;
            float randyCol = (float)position.X / tileset.TileWidth;
            float viewportRows = spriteBatch.GraphicsDevice.Viewport.Height / tileset.TileHeight;
            float viewportCols = spriteBatch.GraphicsDevice.Viewport.Width / tileset.TileWidth;
            float screenRows = Rows;
            float screenCols = Columns;

            float top = Math.Max(0, randyRow - viewportRows / 2);
            if (randyRow > screenRows - viewportRows / 2)
                top = screenRows - viewportRows;
            float left = Math.Max(0, randyCol - viewportCols / 2);
            if (randyCol > screenCols - viewportCols / 2)
                left = screenCols - viewportCols;

            int x = 0;
            int y = 0;

            int layer = 0;

            foreach (Terrain terrain in terrainList)
            {
                float depth = layer * 0.02f;

                if (layer < Map.ABOVE_PLAYER_LAYER_START)
                {
                    depth += 0.1f;
                }
                else
                {
                    depth += 0.5f;
                }
                layer++;
                y = 0;
                foreach (TileRow tileRow in terrain.TileRowList)
                {
                    x = 0;
                    /*
                    if (y < top - 1)
                    {
                        y++;
                        continue;
                    }

                    if (y > bottom)
                    {
                        break;
                    }
                    */
                    foreach (Tile tile in tileRow.TileList) 
                    {
                        /*
                        if (x < left - 1)
                        {
                            x++;
                            continue;
                        }

                        if (x > right)
                        {
                            break;
                        }*/
                        if (randyRow < viewportRows / 2)
                        {

                        }
                        spriteBatch.Draw(
                            tileset.Texture,
                            new Rectangle(
                                (int)((x * tileset.TileWidth) + (tileset.TileWidth / 2) - (left * tileset.TileWidth)),
                                (int)((y * tileset.TileHeight) + (tileset.TileHeight / 2) - (top * tileset.TileHeight)),
                                tileset.TileWidth, tileset.TileHeight),
                                tileset.Rectangle(tile.Position),
                                Color.White,
                                MathHelper.ToRadians(tile.Rotation),
                                new Vector2(tileset.TileWidth / 2, tileset.TileHeight / 2),
                                SpriteEffects.None, depth);
                        x++;
                    }
                    y++;
                }
            }
        }

        public void CheckCollision(Sprite sprite, TileSet tileSet)
        {
            int leftTile, rightTile, topTile, bottomTile;
            leftTile = (int)MathHelper.Clamp(
                Utility.PixelToTile(
                    sprite.Position.X + sprite.Movement.X -
                    sprite.Animation.AnimationManager.Origin.X + 0.1f, tileSet.TileWidth
                ),
                1, columns
            );

            rightTile = (int)MathHelper.Clamp(
                Utility.PixelToTile(
                    sprite.Position.X + sprite.Movement.X +
                    sprite.Animation.AnimationManager.Origin.X - 0.1f, tileSet.TileWidth
                ),
                1, columns
            );

            topTile = (int)MathHelper.Clamp(
                Utility.PixelToTile(
                    sprite.Position.Y + sprite.Movement.Y -
                    sprite.Animation.AnimationManager.Origin.Y + 0.1f, tileSet.TileHeight
                ),
                1, rows
            );

            bottomTile = (int)MathHelper.Clamp(
                Utility.PixelToTile(
                    sprite.Position.Y + sprite.Movement.Y +
                    sprite.Animation.AnimationManager.Origin.Y - 0.1f, tileSet.TileHeight
                ),
                1, rows
            );

            if (sprite.Movement.X < 0)
            {
                for (int i = topTile; i <= bottomTile; i++)
                {
                    if (walkable[i - 1, leftTile - 1] == 0)
                    {
                        sprite.Movement = new Vector2(
                            Utility.TileToPixel(leftTile, tileSet.TileWidth) +
                            sprite.Animation.AnimationManager.Origin.X - sprite.Position.X, 0
                        );
                        break;
                    }
                }
            }

            else if (sprite.Movement.X > 0)
            {
                for (int i = topTile; i <= bottomTile; i++)
                {
                    if (walkable[i - 1, rightTile - 1] == 0)
                    {
                        sprite.Movement = new Vector2(
                            Utility.TileToPixel(leftTile, tileSet.TileWidth) -
                            sprite.Animation.AnimationManager.Origin.X - sprite.Position.X, 0
                        );
                        break;
                    }
                }
            }

            else if (sprite.Movement.Y < 0)
            {
                for (int i = leftTile; i <= rightTile; i++)
                {
                    if (walkable[topTile - 1, i - 1] == 0)
                    {
                        sprite.Movement = new Vector2(
                            0,
                            Utility.TileToPixel(topTile, tileSet.TileHeight) +
                            sprite.Animation.AnimationManager.Origin.Y - sprite.Position.Y
                        );
                        break;
                    }
                }
            }

            else if (sprite.Movement.Y > 0)
            {
                for (int i = leftTile; i <= rightTile; i++)
                {
                    if (walkable[bottomTile - 1, i - 1] == 0)
                    {
                        sprite.Movement = new Vector2(
                            0, 
                            Utility.TileToPixel(topTile, tileSet.TileHeight) -
                            sprite.Animation.AnimationManager.Origin.Y - sprite.Position.Y
                        );
                        break;
                    }
                }
            }
        }

        public int Columns
        {
            get { return columns; }
        }

        public int Rows
        {
            get { return rows; }
        }

        public int Width
        {
            get { return Columns * this.tileSize; }
        }

        public int Height
        {
            get { return Rows * this.tileSize; }
        }

        public int[,] Walkable
        {
            get { return walkable; }
            set { walkable = value; }
        }

        public List<Terrain> TerrainList
        {
            get { return terrainList; }
        }

        public Terrain this[int index]
        {
            get { return terrainList[index]; }
        }
    }
}
