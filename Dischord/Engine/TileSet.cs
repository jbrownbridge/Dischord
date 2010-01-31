using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dischord.Engine
{
    public class TileSet
    {
        private Rectangle[,] rectangles;
        private Texture2D texture;
        private int columns;
        private int rows;
        private int tileWidth;
        private int tileHeight;

        public TileSet(Texture2D texture, int columns, int rows)
        {
            this.texture = texture;
            this.columns = columns;
            this.rows = rows;
            tileWidth = texture.Width / columns;
            tileHeight = texture.Height / rows;
            rectangles = new Rectangle[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    rectangles[i, j] = new Rectangle(j * tileWidth, i * tileHeight, tileWidth, tileHeight);
                }
            }
        }
        
        public Texture2D Texture
        {
            get { return texture; }
        }

        public int TileWidth
        {
            get { return tileWidth; }
        }

        public int TileHeight
        {
            get { return tileHeight; }
        }

        public int Width
        {
            get { return TileWidth * columns; }
        }

        public int Height
        {
            get { return TileHeight * rows; }
        }

        public Rectangle Rectangle(int position)
        {
            return rectangles[((position - 1) / columns), ((position - 1) % columns)];
        }
    }
}
