using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOnCSharp
{
    public class MazeGenerator
    {
        public MazeCell[,] Maze { get; private set; }
        public Point Start { get; private set; } = new Point(-1, -1);
        public Point End { get; private set; } = new Point(-1, -1);

        public List<Point> Traps { get; private set; }
        public List<Point> Walls { get; private set; }

        private int _height { get; }
        private int _width { get; }

        public MazeGenerator(int height, int width, double chanceOfTrap, double chanceOfWall, int countPointToVisit)
        {
            _height = height;
            _width = width;

            Traps = new List<Point>();
            Walls = new List<Point>();
            Generate(chanceOfTrap, chanceOfWall, countPointToVisit);

            AddObjectInList(MazeCell.Trap, Traps);
            AddObjectInList(MazeCell.Null, Walls);
            AddObjectInList(MazeCell.Wall, Walls);
        }

        private void Generate(double chanceOfTrap, double chanceOfWall, int countPointToVisit)
        {
            SetRightBorder();
            GenerateWallsAndWays();
            SetStartAndEnd();
            SetTrapsAndWallsWithIgnorPath(
                CreatePath(countPointToVisit), chanceOfTrap, chanceOfWall);
        }

        private void SetRightBorder()
        {
            Maze = new MazeCell[_width + 1, _height];
            for (int i = 0; i < _height; i++)
                Maze[_width, i] = MazeCell.Wall;
        }

        private void AddObjectInList(MazeCell obj, List<Point> list)
        {
            for (int i = 0; i < Maze.GetLength(1); i++)
            {
                for (int j = 0; j < Maze.GetLength(0); j++)
                {
                    if (Maze[j, i] == obj)
                        list.Add(new Point(j, i));
                }
            }
        }

        private void SetStartAndEnd()
        {
            End = FindNeighbordWayPoint(new Point(_width - 1, _height - 1));
            Start = FindNeighbordWayPoint(new Point(0, 0));

            if (End == new Point(-1, -1) || Start == new Point(-1, -1))
                throw new Exception("Can't set start or end point");
        }

        private List<Point> CreatePath(int countPointToVisit)
        {
            List<Point> path;

            if (countPointToVisit > 0)
            {
                var toVisit = new List<Point>();

                while (countPointToVisit > 0)
                {
                    var random = new Random();
                    var randomPoint = new Point(random.Next(_width), random.Next(_height));

                    if (!InBounds(randomPoint) || Maze[randomPoint.X, randomPoint.Y] != MazeCell.Way)
                        continue;

                    toVisit.Add(randomPoint);
                    countPointToVisit--;
                }

                path = FindPath(Start, toVisit[0]);

                for (int i = 0; i < toVisit.Count - 1; i++)
                    path.AddRange(FindPath(toVisit[i], toVisit[i + 1]));

                path.AddRange(FindPath(toVisit[toVisit.Count - 1], End));
            }
            else
            {
                path = FindPath(Start, End);
            }

            if (path == null)
                throw new Exception("Can't find path from start to end");

            return path;
        }

        private void GenerateWallsAndWays()
        {
            var random = new Random();
            var startPoint = new Point(random.Next(_width), random.Next(_height));
            Maze[startPoint.X, startPoint.Y] = MazeCell.Way;

            var points = new Queue<Point>();
            points.Enqueue(startPoint);

            while (points.Count > 0)
            {
                var thisPoint = points.Dequeue();
                var nearby =
                    FindPointsNearby(thisPoint,
                        newPoint => Maze[newPoint.X, newPoint.Y] == MazeCell.Null)
                    .ToArray();

                if (nearby.Length == 0)
                    continue;

                var minPathCount = 1;
                var pathCount = random.Next(minPathCount, nearby.Length + 1);

                for (int i = 0; i < pathCount; i++)
                {
                    var randomNearbyPoint = nearby[random.Next(0, nearby.Length)];

                    while (points.Contains(randomNearbyPoint))
                        randomNearbyPoint = nearby[random.Next(0, nearby.Length)];

                    Maze[randomNearbyPoint.X, randomNearbyPoint.Y] = MazeCell.Way;
                    points.Enqueue(randomNearbyPoint);

                    SetWallIfCanGeneratingWayAreas(randomNearbyPoint, thisPoint);
                }
            }
        }

        private void SetWallIfCanGeneratingWayAreas(Point nearbyPoint, Point point)
        {
            var direction = nearbyPoint - point;

            var nearbyPointsToNearbyPoint = new List<Point>
        {
            nearbyPoint + new Point(direction.Y, direction.X),
            nearbyPoint - new Point(direction.Y, direction.X)
        };

            foreach (var nearbyP in nearbyPointsToNearbyPoint)
            {
                var newPoint = nearbyP + direction;
                var newPoint2 = nearbyP - direction;

                if (InBounds(newPoint2) && Maze[newPoint2.X, newPoint2.Y] == MazeCell.Way
                    || InBounds(newPoint) && Maze[newPoint.X, newPoint.Y] == MazeCell.Way)
                {
                    Maze[nearbyP.X, nearbyP.Y] = MazeCell.Wall;
                }
            }
        }

        private Point FindNeighbordWayPoint(Point point)
        {
            var queue = new Queue<Point>();
            queue.Enqueue(point);

            while (queue.Count > 0)
            {
                var thisPoint = queue.Dequeue();

                if (!InBounds(thisPoint))
                    continue;

                if (Maze[thisPoint.X, thisPoint.Y] == MazeCell.Way)
                    return thisPoint;

                foreach (var p in FindPointsNearby(thisPoint, newPoint => true))
                    queue.Enqueue(p);
            }

            return new Point(-1, -1);
        }

        private void SetTrapsAndWallsWithIgnorPath(List<Point> path, double chanceOfTrap, double chanceOfWall)
        {
            var rand = new Random();

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    var point = new Point(j, i);

                    if (!path.Contains(point) && Maze[j, i] == MazeCell.Way)
                    {
                        switch (rand.Next(_width))
                        {
                            case int n when n <= (int)(_width * chanceOfWall):
                                Maze[j, i] = MazeCell.Wall;
                                break;
                            case int n when n < (int)(_width * (chanceOfTrap + chanceOfWall)):
                                Maze[j, i] = MazeCell.Trap;
                                break;
                        }
                    }
                }
            }
        }

        // A*
        private List<Point> FindPath(Point start, Point end)
        {
            var openList = new List<Point> { start };
            var cameFrom = new Dictionary<Point, Point>();
            var gScore = new Dictionary<Point, int> { [start] = 0 };
            var fScore = new Dictionary<Point, int> { [start] = ManhattanDistance(Start, End) };

            while (openList.Count > 0)
            {
                var current = FindLowestFScore(openList, fScore);

                if (current == end)
                    return ReconstructPath(cameFrom, current, start);

                openList.Remove(current);

                foreach (var neighbor in
                    FindPointsNearby(current,
                        neighbor => InBounds(neighbor) && Maze[neighbor.X, neighbor.Y] == MazeCell.Way))
                {
                    var tentativeGScore = gScore[current] + 1;

                    if (tentativeGScore < gScore.GetValueOrDefault(neighbor, int.MaxValue))
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + ManhattanDistance(neighbor, end);

                        if (!openList.Contains(neighbor))
                            openList.Add(neighbor);
                    }
                }
            }

            return new List<Point>();
        }

        private List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current, Point start)
        {
            List<Point> path = new List<Point>();

            while (cameFrom.ContainsKey(current))
            {
                path.Add(current);
                current = cameFrom[current];
            }

            path.Add(start);
            path.Reverse();
            return path;
        }

        private Point FindLowestFScore(List<Point> openList, Dictionary<Point, int> fScore)
        {
            Point lowest = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (fScore.ContainsKey(openList[i]) && fScore[openList[i]] < fScore[lowest])
                {
                    lowest = openList[i];
                }
            }
            return lowest;
        }

        private int ManhattanDistance(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

        private bool InBounds(Point point)
        {
            return point.X < _width && point.Y < _height
                && point.X >= 0 && point.Y >= 0;
        }

        private IEnumerable<Point> FindPointsNearby(Point point, Func<Point, bool> condition)
        {
            foreach (var p in GeneratePointsNearby())
            {
                var newPoint = point + p;

                if (InBounds(newPoint) && condition(newPoint))
                    yield return newPoint;
            }
        }

        private IEnumerable<Point> GeneratePointsNearby()
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (Math.Abs(i) + Math.Abs(j) == 1)
                        yield return new Point(i, j);
                }
            }
        }

    }

    public enum MazeCell
    {
        Null,
        Way,
        Wall,
        Trap
    }
}