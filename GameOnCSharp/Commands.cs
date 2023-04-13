using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnCSharp
{
    public static class Commands
    {
        static Vector2[] Directions;

        private static Vector2 _lastPlayerPosition;
        private static double _doubleArea;
        private static int _currentIndex;

        static void Initialize()
        {
            _lastPlayerPosition = new Vector2(-1, -1);
            _currentIndex = 0;
            _doubleArea = 0.5;
        }

        public static Vector2 GetDirection(Vector2 playerPosition)
        {
            if(_lastPlayerPosition == new Vector2(-1, -1))
                _lastPlayerPosition = playerPosition;

            if (_currentIndex < Directions.Length && 
                Math.Abs(playerPosition.X - _lastPlayerPosition.X) > (Directions[_currentIndex].X - _doubleArea) * Game1.BrickSize
                && Math.Abs(playerPosition.Y - _lastPlayerPosition.Y) > (Directions[_currentIndex].Y - _doubleArea) * Game1.BrickSize
                && Math.Abs(playerPosition.X - _lastPlayerPosition.X) < (Directions[_currentIndex].X + _doubleArea) * Game1.BrickSize
                && Math.Abs(playerPosition.Y - _lastPlayerPosition.Y) < (Directions[_currentIndex].Y + _doubleArea) * Game1.BrickSize)
            {
                _currentIndex++;
            }
            if (_currentIndex >= Directions.Length)
            {
                Game1.HaveStartedExecutingCommands = false;
                return new Vector2(-1, -1);
            }
            return Directions[_currentIndex];
        }

        public static void SetCommands(string text)
        {
            Initialize();

            var t = text.Split(' ', '\n');
            Directions = new Vector2[t.Length];
            var index = 0;

            t.AsParallel().ForAll(
                k => 
                {
                    switch (k.ToLower())
                    {
                        case "right":
                            Directions[index++] = new Vector2(1, 0);
                            break;
                        case "left":
                            Directions[index++] = new Vector2(-1, 0);
                            break;
                        case "up":
                            Directions[index++] = new Vector2(0, -1);
                            break;
                        case "down":
                            Directions[index++] = new Vector2(0, 1);
                            break;
                    }
                });
        }
    }
}
