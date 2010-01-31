using System;
namespace Dischord.Engine
{
    public class Utility
    {
        public static int TileToPixel(int tilePosition, int tileSize)
        {
            return (tilePosition * tileSize);
        }

        public static int PixelToTile(float pixelPosition, int tileSize)
        {
            return (int)Math.Floor(pixelPosition / tileSize) + 1;
        }
    }
}
