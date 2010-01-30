using System.Collections.Generic;

namespace Dischord.Engine
{
    public class TileRow
    {
        private List<Tile> tileList;

        public TileRow()
        {
            tileList = new List<Tile>();
        }        

        public List<Tile> TileList
        {
            get { return tileList; }            
        }

        public Tile this[int index]
        {
            get { return tileList[index]; }
        }
    }
}