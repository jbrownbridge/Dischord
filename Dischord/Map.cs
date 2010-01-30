﻿using System;
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
                default:
                    throw new ArgumentException("Unexpected map cell: " + c);
            }
        }

        public void clearEntities()
        {
            entities.Clear();
        }

        public void addEntity(Entity e)
        {
            entities.Add(e);
        }
    };

    public class Map
    {
        private int width, height;
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
            map = new MapCell[width + 2, height + 2];
            for (int i = 0; i < height; i++)
            {
                String line = fin.ReadLine();
                if (String.IsNullOrEmpty(line) || line.Length != width)
                    throw new ArgumentException("Map file borked, ffuuuuuuuuuuuuuu");
                for (int j = 0; j < width; j++)
                    map[i + 1, j + 1] = new MapCell(line[i]);
            }
            while (true)
            {
                String line = fin.ReadLine();
                if (String.IsNullOrEmpty(line))
                    break;
                entities.Add(Entity.GetInstance(line));
            }
            fin.Close();
            Update();
        }

        public void Update()
        {
            for (int i = 0; i < height + 2; i++)
                for (int j = 0; j < width + 2; j++)
                    map[i, j].clearEntities();

            foreach (Entity e in entities)
                e.MapCell.addEntity(e);
        }

        // For Testing
        /*public static void Main()
        {
            string filename = "test_marco.txt";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using (StreamWriter fout = new StreamWriter(File.Open(filename, FileMode.CreateNew)))
            {
                fout.WriteLine("5 5");
                for (int i = 0; i < 5; i++)
                {
                    fout.WriteLine("?#?..");
                }
            }
            Map map = new Map(filename);
            File.Delete(filename);
        }*/
    }
}