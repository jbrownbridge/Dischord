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
        public static bool intersection(Point p1, Point p2, Point p3, Point p4)
        {
            double xD1, yD1, xD2, yD2, xD3, yD3;
            double dot, deg, len1, len2;
            double segmentLen1, segmentLen2;
            double ua, ub, div;

            // calculate differences
            xD1 = p2.X - p1.X;
            xD2 = p4.X - p3.X;
            yD1 = p2.Y - p1.Y;
            yD2 = p4.Y - p3.Y;
            xD3 = p1.X - p3.X;
            yD3 = p1.Y - p3.Y;

            // calculate the lengths of the two lines
            len1 = Math.Sqrt(xD1 * xD1 + yD1 * yD1);
            len2 = Math.Sqrt(xD2 * xD2 + yD2 * yD2);

            // calculate angle between the two lines.
            dot = (xD1 * xD2 + yD1 * yD2); // dot product
            deg = dot / (len1 * len2);

            // if abs(angle)==1 then the lines are parallell,
            // so no intersection is possible
            if (Math.Abs(deg) == 1) return false;

            // find intersection Pt between two lines
            div = yD2 * xD1 - xD2 * yD1;
            ua = (xD2 * yD3 - yD2 * xD3) / div;
            ub = (xD1 * yD3 - yD1 * xD3) / div;
            double ptX = p1.X + ua * xD1;
            double ptY = p1.Y + ua * yD1;

            // calculate the combined length of the two segments
            // between Pt-p1 and Pt-p2
            xD1 = ptX - p1.X;
            xD2 = ptX - p2.X;
            yD1 = ptY - p1.Y;
            yD2 = ptY - p2.Y;
            segmentLen1 = Math.Sqrt(xD1 * xD1 + yD1 * yD1) + Math.Sqrt(xD2 * xD2 + yD2 * yD2);

            // calculate the combined length of the two segments
            // between Pt-p3 and Pt-p4
            xD1 = ptX - p3.X;
            xD2 = ptX - p4.X;
            yD1 = ptY - p3.Y;
            yD2 = ptY - p4.Y;
            segmentLen2 = Math.Sqrt(xD1 * xD1 + yD1 * yD1) + Math.Sqrt(xD2 * xD2 + yD2 * yD2);

            // if the lengths of both sets of segments are the same as
            // the lenghts of the two lines the point is actually
            // on the line segment.

            // if the point isn't on the line, return null
            if (Math.Abs(len1 - segmentLen1) > 0.01 || Math.Abs(len2 - segmentLen2) > 0.01)
                return false;

            // return the valid intersection
//            Console.WriteLine(p1 + " " + p2 + " " + p3 + " " + p4 + ": " + ptX + " " + ptY);
            return true; // pt is the actual point of intersection
        }

        public static bool intersection(Point lineA, Point lineB, Point squareA, int squreSize)
        {
            return
                intersection(lineA, lineB, squareA, new Point(squareA.X + Game.TILE_WIDTH, squareA.Y)) ||
                intersection(lineA, lineB, squareA, new Point(squareA.X + Game.TILE_WIDTH, squareA.Y)) ||
                intersection(lineA, lineB, new Point(squareA.X + Game.TILE_WIDTH, squareA.Y), new Point(squareA.X + Game.TILE_WIDTH, squareA.Y + Game.TILE_HEIGHT)) ||
                intersection(lineA, lineB, new Point(squareA.X, squareA.Y + Game.TILE_HEIGHT), new Point(squareA.X + Game.TILE_WIDTH, squareA.Y + Game.TILE_HEIGHT));
        }

        public static bool can_see(Map map, Point pos, int facing, Point target)
        {
            int dx = 0, dy = 0;
            switch (facing)
            {
                case 1:
                    dx = 0; dy = 1;
                    break;
                case 3:
                    dx = -1; dy = 0;
                    break;
                case 5:
                    dx = 0; dy = -1;
                    break;
                case 7:
                    dx = 1; dy = 0;
                    break;
                default:
                    throw new ArgumentException("Shouldn't be here!!!");
            }
            
            for (int x = 0; x < map.width+2; x++)
                for (int y = 0; y < map.height + 2; y++)
                    if (map.getCell(x, y).Type != MapCell.MapCellType.floor &&
                            intersection(pos, target, new Point(x, y), 1))
                    {
//                        Console.WriteLine("Intersection: " + x + " " + y + "(" + pos + ", " + target + ")");
                        return false;
                    }
            return true;
        }

        public static Point getTarget(Map map, Point pos, int facing)
        {
            int foo = pos.X + pos.Y; // TODO: remove
            int x = pos.X / Game.TILE_WIDTH;
            int y = pos.Y / Game.TILE_HEIGHT;
            Point pos_tile = new Point(x + 1, y + 1);
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
                        double dis = Math.Sqrt(Math.Pow(e.Position.X - pos_tile.X, 2) + Math.Pow(e.Position.Y - pos_tile.Y, 2));
                        if (dis <= 2 * Game.TILE_WIDTH)
                            known = true;
                    }
                    else if (e is VisualSource && can_see(map, pos, facing, e.Position)) {
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
            {
                int x = pos.X / Game.TILE_WIDTH + 1;
                int y = pos.Y / Game.TILE_HEIGHT + 1;
                return findPath(map, new Point(x, y), target);
            }
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
