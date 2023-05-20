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
                        if (_directions.Count > 0
                            && lowCommand.Length >= 2
                            && lowCommand[0] == 'x'
                            && lowCommand.Skip(1).All(x => char.IsDigit(x))
                            && int.TryParse(lowCommand.Substring(1), out repeatCount));
                        {
                            while (--repeatCount > 0)
                            {
                                _directions.Add(_directions.Last());
                            }
                        }
                        break;
                }
            }
        }

        public static void ShiftPlayer(GameTime gameTime, PlayerAnimal player, Maze maze, double lastTimeInSeconds)
        {
            if (_currentIndex == -1 || player.Position == new Vector2(maze.Start.X * PlayMode.BlockSize, maze.Start.Y * PlayMode.BlockSize) 
                || _target == player.Position)
            {
                _currentIndex++;

                // Завершаем игру, если использовали все команды
                if (_currentIndex >= _directions.Count)
                {
                    if(lastTimeInSeconds > TimeToStartGameAgain)
                        StartOver(gameTime, player, maze, lastTimeInSeconds);

                    return;
                }

                // Устанавливаем следующую точку
                _target = player.Position + _directions[_currentIndex];
            }

            var direction = _directions[_currentIndex];

            // Изменение направления текущего спрайта
            player.CurrentSprite = player.DictSprites[direction];

            // Направление и величина движения
            var coef = (gameTime.TotalGameTime.TotalSeconds - lastTimeInSeconds) * PlayerAnimal.Speed;
            var shift = new Vector2((float)(direction.X * coef), (float)(direction.Y * coef));

            // Принимаем во внимание, что мы можем перепрыгнуть цель
            var lenToTarget = new Vector2(player.Position.X - _target.X, player.Position.Y - _target.Y).Length();
            player.Position = lenToTarget >= shift.Length() ? player.Position + shift : _target;

            if (!maze.CanLocatedHere(player.Position.ToPoint()) && lastTimeInSeconds > TimeToStartGameAgain)
            {
                StartOver(gameTime, player, maze, lastTimeInSeconds);
            }
        }

        public static void StartOver(GameTime gameTime, PlayerAnimal player, Maze maze, double lastTimeInSeconds)
        {
            PlayMode.HaveStartedExecutingCommands = false;
            player.StartFromBeginning();
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
