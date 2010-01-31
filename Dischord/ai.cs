using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dischord
{
    public enum Direction {
        up,
        down,
        left,
        right,
        still,
    }

    public class PQEntry
    {
        public int cost, heuristic;
        public Vector2 pos;

        public PQEntry(int cost, Vector2 pos, Vector2 target)
        {
            this.cost = cost;
            this.pos = pos;
            this.heuristic = cost + Math.Abs((int)pos.X - (int)target.X) + Math.Abs((int)pos.Y - (int)target.Y);
        }
    }

    public class ai
    {
        public static Vector2 getTarget(Map map, Vector2 pos)
        {
            float strongest = -1;
            Vector2 target = new Vector2(-1, -1);
            foreach (Entity e in map.Entities)
                if (e is Source)
                {
                    double dis = Math.Sqrt(Math.Pow(e.Position.X - pos.X, 2) + Math.Pow(e.Position.Y - pos.Y, 2));
                    if ((!(e is SoundSource) || dis <= 4 * Game.TILE_WIDTH) && (e as Source).Strength > strongest)
                    {
                        strongest = (e as Source).Strength;
                        int x = (int)e.Position.X / Game.TILE_WIDTH;
                        int y = (int)e.Position.Y / Game.TILE_HEIGHT;
                        target = new Vector2(x+1, y+1);
                    }
                }
            return target;
        }

        public static Direction findPath(Map map, Vector2 pos, Enemy c)
        {
            if (c.Wait > 0)
            {
                c.Wait--;
                return Direction.still;
            }
            Vector2 target = getTarget(map, pos);
            if (target.X != -1)
                return findPath(map, pos, target);
            // no sound source
            Random rg = new Random();
            double r = rg.NextDouble();
            if (r < 0.006)
            {
                c.Wait = 20;
                switch (c.Facing)
                {
                    case 5:
                        return Direction.left;
                    case 1:
                        return Direction.right;
                    case 3:
                        return Direction.down;
                    case 7:
                        return Direction.up;
                }
            }
            else if (r < 0.012)
            {
                c.Wait = 20;
                switch (c.Facing)
                {
                    case 5:
                        return Direction.right;
                    case 1:
                        return Direction.left;
                    case 3:
                        return Direction.up;
                    case 7:
                        return Direction.down;
                }
            }
            else
            {
                switch (c.Facing)
                {
                    case 5:
                        return Direction.up;
                    case 1:
                        return Direction.down;
                    case 3:
                        return Direction.left;
                    case 7:
                        return Direction.right;
                }
            }
            return Direction.still;
        }

        public static Direction findPath(Map map, Vector2 pos, Vector2 target)
        {
            if (pos == target)
                return Direction.still;
            int[,] d = {{0, 1}, {0, -1}, {1, 0}, {-1, 0}};
            bool[,] vis = new bool[map.width+2, map.height+2];
            Direction[] dir = { Direction.up, Direction.down, Direction.left, Direction.right };
            PriorityQueue<int, PQEntry> queue = new PriorityQueue<int,PQEntry>();
            queue.Enqueue(0, new PQEntry(0, target, pos)); // going from target to pos, purposefully in reverse
            vis[(int)target.X, (int)target.Y] = true;
            while (!queue.IsEmpty)
            {
                PQEntry e = queue.Dequeue();
                //Console.WriteLine(e.pos.X + " " + e.pos.Y);
                for (int i = 0; i <= d.GetUpperBound(0); i++)
                {
                    MapCell cell = map.getCell((int)e.pos.X + d[i,0], (int)e.pos.Y + d[i,1]); // cell we would move onto
                    Vector2 p = new Vector2(e.pos.X + d[i, 0], e.pos.Y + d[i, 1]);
                    if (cell.Type == MapCell.MapCellType.floor && !vis[(int)p.X, (int)p.Y]) // can only go through floors
                    {
                        //Console.WriteLine("> " + p.X + " " + p.Y + ": " + cell.toChar());
                        PQEntry newEntry = new PQEntry(e.cost + cell.cost(), new Vector2(e.pos.X + d[i, 0], e.pos.Y + d[i, 1]), pos);
                        queue.Enqueue(newEntry.heuristic, newEntry);
                        if (p == pos) // if we've reached our "target", then this must be the best path; we only care about one move for now, since the obstacles might change before reaching the target
                        {
                            return dir[i];
                        }
                        vis[(int)p.X, (int)p.Y] = true;
                    }
                }
            }
            throw new InvalidProgramException("Couldn't find a valid path");
        }
    }
}
