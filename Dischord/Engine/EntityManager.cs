using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dischord.Engine {
    public class Cell {
        List<Entity> entities;

        public Cell() {
            entities = new List<Entity>();
        }

        public void Clear() {
            entities.Clear();
        }

        public void addEntity(Entity e) {
            entities.Add(e);
        }

        public IEnumerable<Entity> Entities {
            get {
                foreach(Entity e in entities) {
                    yield return e;
                }
            }
        }
    }
    
    public class EntityManager {
        private List<Entity> entities;
        private List<Entity> additions;
        private Cell[,] cells;
        private int rows, columns;

        public IEnumerable<Entity> Entities {
            get {
                foreach(Entity e in entities) {
                    yield return e;
                }
            }
        }

        public EntityManager(int rows, int columns) {
            entities = new List<Entity>();
            additions = new List<Entity>();
            cells = new Cell[rows, columns];
            for(int i = 0; i < rows; i++)
                for(int j = 0; j < columns; j++)
                    cells[i, j] = new Cell();

            this.rows = rows;
            this.columns = columns;
        }

        public Cell GetCell(int x, int y) {
            if(x < 0 || x >= columns || y < 0 || y >= rows)
                return null;
            return cells[y, x];
        }

        public void Add(Entity e) {
            additions.Add(e);
        }

        public void Del(Entity e) {
            additions.Remove(e);
        }

        public void Update() {

            for(int i = 0; i < rows; i++)
                for(int j = 0; j < columns; j++)
                    cells[i, j].Clear();

            List<Entity> survivors = new List<Entity>();

            foreach(Entity e in entities) {
                if(e.IsAlive) {
                    Cell tmpcell = e.Cell;
                    if(tmpcell != null) {
                        e.Cell.addEntity(e);
                        survivors.Add(e);
                    }
                }
            }

            foreach(Entity e in additions) {
                if(e.IsAlive) {
                    Cell tmpcell = e.Cell;
                    if(tmpcell != null) {
                        e.Cell.addEntity(e);
                        survivors.Add(e);
                    }
                }
            }

            additions.Clear();

            entities = survivors;
        }

        public void Reset() {
            entities.Clear();
            additions.Clear();
        }
    }
}
