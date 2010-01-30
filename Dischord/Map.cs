using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Dischord
{
    public class MapCell {
        public enum MapCellType {
            none,       // ?
            wall,       // #
            floor,      // .
        };

        public MapCellType Type
        {
            get 
            {
                return type;
            }
        }
        MapCellType type;
        List<Entity> entities = new List<Entity>();

        public MapCell() {
            type = MapCellType.none;
        }

        public MapCell(MapCellType type) {
            this.type = type;
        }

        public MapCell(char c)
        {
            setType(c);
        }

        public void setType(char c) {
            switch (c) {
                case '?':
                    type = MapCellType.none;
                    break;
                case '#':
                    type = MapCellType.wall;
                    break;
                case '.':
                    type = MapCellType.floor;
                    break;
                case 'X':
                    break;
                default:
                    throw new ArgumentException("Unexpected map cell: " + c);
            }
        }

        public char toChar() {
            switch (Type) {
                case MapCellType.none:
                    return ' ';
                case MapCellType.wall:
                    return '#';
                case MapCellType.floor:
                    foreach (Entity e in entities)
                    {
                        return e.toChar();
                    }
                    return '.';
            }
            throw new ArgumentException("Unexpected map cell");
        }

        public void clearEntities()
        {
            entities.Clear();
        }

        public void addEntity(Entity e)
        {
            entities.Add(e);
        }

        public int cost()
        {
            // TODO
            return 1;
        }
    };

    public class Map
    {
        public int width, height;
        private MapCell[,] map;
        private List<Entity> entities = new List<Entity>();

        public IEnumerable<Entity> Entities
        {
            get
            {
                foreach (Entity e in entities)
                {
                    yield return e;
                }
            }
        }

        public MapCell getCell(int x, int y)
        {
            return map[y, x];
        }

        public Map(): this(0, 0) {}

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            map = new MapCell[width+2,height+2];
        }

        public Map(String filename)
        {
            StreamReader fin = new StreamReader(File.Open(filename, FileMode.Open));
            String inline = fin.ReadLine();
            if (String.IsNullOrEmpty(inline))
                throw new ArgumentException("Map file borked, ffuuuuuuuuuuuuuu");
            String[] lineArray = inline.Split();
            if (lineArray.Length != 2)
                throw new ArgumentException("Map file borked, ffuuuuuuuuuuuuuu");
            width = int.Parse(lineArray[0]);
            height = int.Parse(lineArray[1]);
            //init(int.Parse(lineArray[0]), int.Parse(lineArray[1]));
            map = new MapCell[height + 2, width + 2];
            for(int i = 0; i < height + 2; i++)
                for(int j = 0; j < width + 2; j++)
                    map[i, j] = new MapCell();

            for (int i = 0; i < height; i++)
            {
                String line = fin.ReadLine();
                if (String.IsNullOrEmpty(line) || line.Length != width)
                    throw new ArgumentException("Map file borked, ffuuuuuuuuuuuuuu");
                for (int j = 0; j < width; j++)
                    map[i + 1, j + 1] = new MapCell(line[j]);
            }
            while (true)
            {
                String line = fin.ReadLine();
                if (String.IsNullOrEmpty(line))
                    break;
                entities.Add(Entity.GetInstance(line));
            }
            fin.Close();
            //Update();
        }

        public void Add(Entity e) {
            entities.Add(e);
        }

        public void Update()
        {
            
            for (int i = 0; i < height + 2; i++)
                for (int j = 0; j < width + 2; j++)
                    map[i, j].clearEntities();

            List<Entity> survivors = new List<Entity>();
            
            foreach(Entity e in entities) {
                if(e.IsAlive) {
                    e.MapCell.addEntity(e);
                    survivors.Add(e);
                }
            }
            entities = survivors;
        }

        public void draw() {
            for (int i = 0; i < height+2; i++) {
                for (int j = 0; j < width+2; j++)
                    Console.Write(map[i, j].toChar());
                Console.WriteLine();
            }
        }

        // For Testing
        /*public static void Main()
        {
            Map map = new Map("maps/simple4.map");
            map.draw();
        }*/
    }
}
