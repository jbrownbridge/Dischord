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
        public static bool can_see(Map map, Point pos, int facing, Point target, bool[,] vis)
        {
            if (pos == target)
                return true;
            if (map.getCell(pos.X, pos.Y).Type != MapCell.MapCellType.floor || vis[pos.X, pos.Y])
                return false;
            vis[pos.X, pos.Y] = true;
            int dx = 0, dy = 0, sx = 0, sy = 0;
            switch (facing)
            {
                case 1:
                    dx = 0; dy = 1;
                    sx = 1; sy = 0;
                    break;
                case 3:
                    dx = -1; dy = 0;
                    sx = 0; sy = 1;
                    break;
                case 5:
                    dx = 0; dy = -1;
                    sx = 1; sy = 0;
                    break;
                case 7:
                    dx = 1; dy = 0;
                    sx = 0; sy = 1;
                    break;
                default:
                    throw new ArgumentException("Shouldn't be here!!!");
            }
            Point[] ds = {
                          new Point(pos.X + dx, pos.Y + dy),
                          new Point(pos.X + dx + sx, pos.Y + dy + sy),
                          new Point(pos.X + dx - sx, pos.Y + dy - sy),
                      };
            foreach (Point d in ds) {
                if (can_see(map, d, facing, target, vis))
                    return true;
            }
            return false;
        }

        public static Point getTarget(Map map, Point pos, int facing)
        {
            int x = pos.X / Game.TILE_WIDTH;
            int y = pos.Y / Game.TILE_HEIGHT;
            pos = new Point(x + 1, y + 1);
            float strongest = -1;
            Point target = new Point(-1, -1);
            foreach (Entity e in map.Entities)
                if (e is Source)
                {
                    bool known = false;
                    x = e.Position.X / Game.TILE_WIDTH;
                    y = e.Position.Y / Game.TILE_HEIGHT;
                    Point source = new Point(x + 1, y + 1);
                    if (e is SoundSource)
                    {
                        double dis = Math.Sqrt(Math.Pow(e.Position.X - pos.X, 2) + Math.Pow(e.Position.Y - pos.Y, 2));
                        if (dis <= 4 * Game.TILE_WIDTH)
                            known = true;
                    }
                    else if (e is VisualSource) {
                        bool[,] vis = new bool[map.width+2, map.height+2];
                        if (can_see(map, pos, facing, source, vis))
                            known = true;
                    }
                    if (known && (e as Source).Strength > strongest)
                    {
                        strongest = (e as Source).Strength;
                        target = source;
                    }
                }
            return target;
        }

        public static Direction findPath(Map map, Point pos, Enemy c)
        {
            if (c.Wait > 0)
            {
                c.Wait--;
                return Direction.still;
            }
            Point target = getTarget(map, pos, c.Facing);
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
