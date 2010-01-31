using System.Collections.Generic;

namespace Dischord.Engine
{
    public class Terrain
    {
        private List<TileRow> tileRowList;

        public Terrain()
        {
            tileRowList = new List<TileRow>();
        }

        public List<TileRow> TileRowList
        {
            get { return tileRowList; }
        }

        public TileRow this[int index]
        {
            get { return tileRowList[index]; }
        }
    }
}
