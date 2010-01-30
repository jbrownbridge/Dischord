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
            int[,] d = {{0, 1}, {0, -1}, {1, 0}, {-1, 0}};
            Direction[] dir = { Direction.up, Direction.down, Direction.left, Direction.right };
            PriorityQueue<int, PQEntry> queue = new PriorityQueue<int,PQEntry>();
            queue.Enqueue(0, new PQEntry(0, target, pos)); // going from target to pos, purposefully in reverse
            while (!queue.IsEmpty)
            {
                PQEntry e = queue.Dequeue();
                for (int i = 0; i < d.Length; i++)
                {
                    MapCell cell = map.getCell(e.pos.X + d[i,0], e.pos.Y + d[i,1]); // cell we would move onto
                    if (cell.Type == MapCell.MapCellType.floor) // can only go through floors
                    {
                        PQEntry newEntry = new PQEntry(e.cost + cell.cost(), new Point(e.pos.X + d[i, 0], e.pos.Y + d[i, 1]), pos);
                        queue.Enqueue(newEntry.heuristic, newEntry);
                        Point p = new Point(pos.X + d[i,0], pos.Y + d[i,1]);
                        if (p == pos) // if we've reached our "target", then this must be the best path; we only care about one move for now, since the obstacles might change before reaching the target
                        {
                            return dir[i];
                        }
                    }
                }
            }
            throw new InvalidProgramException("Couldn't find a valid path");
        }
    }
}
