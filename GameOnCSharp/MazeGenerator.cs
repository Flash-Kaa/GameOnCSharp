using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOnCSharp
{
    public class MazeGenerator
    {
        public MazeCell[,] Maze;
        public int Height { get; }
        public int Width { get; }

        public Point Start { get; private set; } = Point.Null;
        public Point End { get; private set; } = Point.Null;

        public MazeGenerator(int height, int width)
        {
            Height = height;
            Width = width;

            Maze = new MazeCell[Width, Height];

            Generate();
        }

        private void Generate()
        {
            GenerateWalls();

            // SetFinishPoint()
            // var traps = GenerateTraps().ToList() - лист с точками, где были поставлены ловушки
            // while(!HaveWayFromStartToFinish()) с помощью A* или поиска в длину
            // заменить все поставленные ловушки на MazeCell.Wall и переставить ловушки
        }

        private void GenerateWalls()
        {
            var random = new Random();
            var startPoint = new Point(0, 0);
            Start = startPoint;
            Maze[0, 0] = MazeCell.Way;

            var points = new Queue<Point>();
            points.Enqueue(startPoint);

            while (points.Count > 0)
            {
                var thisPoint = points.Dequeue();
                var nearby = FindPointsNearby(thisPoint).ToArray();

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

                    NotGeneratingWayAreas(randomNearbyPoint, thisPoint);
                }
            }
        }

        private void NotGeneratingWayAreas(Point nearbyPoint, Point point)
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

        private bool InBounds(Point point)
        {
            return point.X < Width && point.Y < Height
                && point.X >= 0 && point.Y >= 0;
        }

        private IEnumerable<Point> FindPointsNearby(Point point)
        {
            foreach (var p in GeneratePointsNearby())
            {
                var newPoint = point + p;

                if (InBounds(newPoint) && Maze[newPoint.X, newPoint.Y] == MazeCell.Null)
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
