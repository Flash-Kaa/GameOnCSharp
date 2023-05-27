using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOnCSharp
{
    public static class Commands
    {
        private const double TimeToStartGameAgain = 2;

        private static Vector2 _target;
        private static int _currentIndex;
        private static string[] _commandList;
        private static List<Vector2> _directions;

        public static void SetCommands(string text)
        {
            Initialize();

            var commands = text
                .Split(' ', '\n')
                .Where(x => _commandList.Contains(x.ToLower()) || x.ToLower().Contains('x'));

            _directions = new List<Vector2>(commands.Count());

            foreach (var command in commands)
            {
                var lowCommand = command.ToLower();
                switch (lowCommand)
                {
                    case "right":
                        _directions.Add(new Vector2(PlayMode.BlockSize, 0));
                        break;
                    case "left":
                        _directions.Add(new Vector2(-PlayMode.BlockSize, 0));
                        break;
                    case "up":
                        _directions.Add(new Vector2(0, -PlayMode.BlockSize));
                        break;
                    case "down":
                        _directions.Add(new Vector2(0, PlayMode.BlockSize));
                        break;
                    default:
                        var repeatCount = 0;
                        if (_directions.Count > 0 && lowCommand.Length >= 2 && lowCommand[0] == 'x'
                            && lowCommand.Skip(1).All(x => char.IsDigit(x))
                            && int.TryParse(lowCommand.Substring(1), out repeatCount));
                        {
                            while (--repeatCount > 0)
                                _directions.Add(_directions.Last());
                        }
                        break;
                }
            }
        }

        public static void ShiftPlayer(GameTime gameTime, double lastTimeInSeconds)
        {
            if (_currentIndex == -1 
                || PlayerAnimal.Position == 
                    new Vector2(Maze.Start.X * PlayMode.BlockSize, Maze.Start.Y * PlayMode.BlockSize) 
                || _target == PlayerAnimal.Position)
            {
                _currentIndex++;

                // Завершаем игру, если использовали все команды
                if (_currentIndex >= _directions.Count)
                {
                    if(lastTimeInSeconds > TimeToStartGameAgain)
                        StartOver(gameTime, lastTimeInSeconds);

                    return;
                }

                _target = PlayerAnimal.Position + _directions[_currentIndex];
            }

            var direction = _directions[_currentIndex];

            PlayerAnimal.CurrentSprite = PlayerAnimal.DictSprites[direction];

            var shiftCoef = (gameTime.TotalGameTime.TotalSeconds - lastTimeInSeconds) * PlayerAnimal.Speed;
            var shift = new Vector2((float)(direction.X * shiftCoef), (float)(direction.Y * shiftCoef));

            var lenToTarget = new Vector2(PlayerAnimal.Position.X - _target.X, PlayerAnimal.Position.Y - _target.Y).Length();
            PlayerAnimal.Position = lenToTarget >= shift.Length() ? PlayerAnimal.Position + shift : _target;

            if (!Maze.CanLocatedHere(PlayerAnimal.Position.ToPoint()) && lastTimeInSeconds > TimeToStartGameAgain)
            {
                StartOver(gameTime, lastTimeInSeconds);
            }
        }

        public static void StartOver(GameTime gameTime, double lastTimeInSeconds)
        {
            PlayMode.HaveStartedExecutingCommands = false;
            PlayerAnimal.StartFromBeginning();
        }

        private static void Initialize()
        {
            _currentIndex = -1;
            _commandList = new string[]
            {
                "right",
                "left",
                "up",
                "down"
            };
        }
    }
}
