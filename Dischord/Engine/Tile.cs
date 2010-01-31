namespace Dischord.Engine
{
    public class Tile
    {
        private int position;
        private int rotation;

        public Tile(int position, int rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        public int Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
    }
}
