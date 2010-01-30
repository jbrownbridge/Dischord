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
        public Point pos;

        public PQEntry(int cost, Point pos, Point target)
        {
            this.cost = cost;
            this.pos = pos;
            this.heuristic = cost + Math.Abs(pos.X - target.X) + Math.Abs(pos.Y - target.Y);
        }
    }

    public class ai
    {
        public static Direction findPath(Map map, Point pos, Point target)
        {
            if (pos == target)
                return Direction.still;
            int[,] d = {{0, 1}, {0, -1}, {1, 0}, {-1, 0}};
            bool[,] vis = new bool[map.width+2, map.height+2];
            Direction[] dir = { Direction.up, Direction.down, Direction.left, Direction.right };
            PriorityQueue<int, PQEntry> queue = new PriorityQueue<int,PQEntry>();
            queue.Enqueue(0, new PQEntry(0, target, pos)); // going from target to pos, purposefully in reverse
            vis[target.X, target.Y] = true;
            while (!queue.IsEmpty)
            {
                PQEntry e = queue.Dequeue();
                //Console.WriteLine(e.pos.X + " " + e.pos.Y);
                for (int i = 0; i <= d.GetUpperBound(0); i++)
                {
                    MapCell cell = map.getCell(e.pos.X + d[i,0], e.pos.Y + d[i,1]); // cell we would move onto
                    Point p = new Point(e.pos.X + d[i, 0], e.pos.Y + d[i, 1]);
                    if (cell.Type == MapCell.MapCellType.floor && !vis[p.X, p.Y]) // can only go through floors
                    {
                        //Console.WriteLine("> " + p.X + " " + p.Y + ": " + cell.toChar());
                        PQEntry newEntry = new PQEntry(e.cost + cell.cost(), new Point(e.pos.X + d[i, 0], e.pos.Y + d[i, 1]), pos);
                        queue.Enqueue(newEntry.heuristic, newEntry);
                        if (p == pos) // if we've reached our "target", then this must be the best path; we only care about one move for now, since the obstacles might change before reaching the target
                        {
                            return dir[i];
                        }
                        vis[p.X, p.Y] = true;
                    }
                }
            }
            throw new InvalidProgramException("Couldn't find a valid path");
        }
    }
}
