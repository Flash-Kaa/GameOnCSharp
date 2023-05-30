using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameOnCSharp.Tests
{
    [TestFixture]
    public class MazeTests
    {
        [TestCase(10, 10, 0, 0, 0)]
        [TestCase(30, 10, 0, 0, 0)]
        [TestCase(1, 1, 0.1, 0.1, 5)]
        [TestCase(1, 20, 0.5, 0.1, 50)]
        [TestCase(100, 100, 0.5, 0.5, 20)]
        [TestCase(50, 10, int.MaxValue, int.MinValue, 0)]
        public void CanMoveToFinish(int height, int width,
            double chanceOfTrap, double chanceOfWall, int countPointToVisit)
        {
            var maze = new MazeGenerator(height, width, chanceOfTrap, chanceOfWall, countPointToVisit);
            Assert.IsTrue(HavePath(maze.Maze, maze.Start, maze.End));
        }

        [TestCase(10, 10, 0, 0, 0, 10)]
        [TestCase(30, 10, 0, 0, 0, 10)]
        [TestCase(30, 30, 1, 1, 0, 10)]
        [TestCase(1, 1, 0.1, 0.1, 5, 10)]
        [TestCase(1, 20, 0.5, 0.1, 50, 20)]
        [TestCase(100, 100, 0.5, 0.5, 20, 200)]
        [TestCase(50, 10, int.MaxValue, int.MinValue, 0, 10)]
        public void CanQuicklyGenerateMaze(int height, int width,
            double chanceOfTrap, double chanceOfWall, int countPointToVisit, int expectedMaxMillisecond)
        {
            var iterationCount = 25;
            var timer = new Stopwatch();
            timer.Start();

            for(int i = 0;  i < iterationCount; i++)
                new MazeGenerator(height, width, chanceOfTrap, chanceOfWall, countPointToVisit);

            timer.Stop();

            Assert.LessOrEqual(timer.Elapsed.TotalMilliseconds / iterationCount, expectedMaxMillisecond);
        }

        [Test]
        public void RandomCanQuicklyGenerateMaze()
        {
            var testCount = 20;
            var expectedMaxMillisecond = 1000;

            var minLen = 1;
            var maxLen = 25;
            var maxPointToVisit = 10;

            var random = new Random();

            for (int i = 0; i < testCount; i++)
            {
                CanQuicklyGenerateMaze(
                    height: random.Next(minLen, maxLen),
                    width: random.Next(minLen, maxLen),
                    chanceOfTrap: random.NextDouble(),
                    chanceOfWall: random.NextDouble(),
                    countPointToVisit: random.Next(maxPointToVisit),
                    expectedMaxMillisecond: expectedMaxMillisecond);
            }
        }

        [Test]
        public void RandomCanMoveToFinish()
        {
            var testCount = 20;

            var minLen = 1;
            var maxLen = 40;
            var maxPointToVisit = 10;

            var random = new Random();

            for (int i = 0; i < testCount; i++)
            {
                CanMoveToFinish(
                    height: random.Next(minLen, maxLen),
                    width: random.Next(minLen, maxLen),
                    chanceOfTrap: random.NextDouble(),
                    chanceOfWall: random.NextDouble(),
                    countPointToVisit: random.Next(maxPointToVisit));
            }
        }

        private bool HavePath(MazeCell[,] maze, Point startPoint, Point endPoint)
        {
            var toVisit = new Stack<Point>();
            var visited = new HashSet<Point> { startPoint };
            toVisit.Push(startPoint);

            int width = maze.GetLength(0);
            int height = maze.GetLength(1);

            while (toVisit.Count > 0)
            { 
                var thisPoint = toVisit.Pop();

                if (thisPoint == endPoint)
                    return true;

                foreach (var newPoint in FindPointsNearby(thisPoint))
                {
                    if (InBounds(newPoint, width, height) 
                        && maze[newPoint.X, newPoint.Y] == MazeCell.Way 
                        && !visited.Contains(newPoint))
                    {
                        toVisit.Push(newPoint);
                        visited.Add(newPoint);
                    }
                }
            }

            return false;
        }

        private IEnumerable<Point> FindPointsNearby(Point point)
        {
            foreach (var p in GeneratePointsNearby())
            {
                var newPoint = point + p;
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

        private bool InBounds(Point point, int width, int height)
            => point.X < width && point.Y < height && point.X >= 0 && point.Y >= 0;
    }
}
