using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace GameOnCSharp
{
    public static class Commands
    {
        public static Vector2[] Directions;
        public static int CurrentIndex;

        private static string[] CommandsList 
            = new string[]
            {
                "right",
                "left",
                "up",
                "down"
            };

        static void Initialize()
        {
            CurrentIndex = -1;
        }

        public static void SetCommands(string text)
        {
            Initialize();

            var commands = text.Split(' ', '\n').Where(x => CommandsList.Contains(x.ToLower()));
            Directions = new Vector2[commands.Count()];

            var index = 0;
            foreach (var command in commands)
            {
                switch (command.ToLower())
                {
                    case "right":
                        Directions[index++] = new Vector2(Game1.BrickSize, 0);
                        break;
                    case "left":
                        Directions[index++] = new Vector2(-Game1.BrickSize, 0);
                        break;
                    case "up":
                        Directions[index++] = new Vector2(0, -Game1.BrickSize);
                        break;
                    case "down":
                        Directions[index++] = new Vector2(0, Game1.BrickSize);
                        break;
                }
            }
        }



        private static double _lastDieTimeInSecond = -1;
        private static Vector2 _target;
        public const double TimeToStartGameAgain = 2;

        public static void ShiftPlayer(GameTime gameTime, PlayerAnimal player, Maze maze, double lastTimeInSeconds)
        {
            var timeAfterDie = gameTime.TotalGameTime.TotalSeconds - _lastDieTimeInSecond;

            if (player.Position == maze.Start.ToVector2() || _target == player.Position)
            {
                Commands.CurrentIndex++;

                // Завершаем игру, если использовали все команды
                if (Commands.CurrentIndex >= Commands.Directions.Length)
                {
                    if(lastTimeInSeconds > TimeToStartGameAgain)
                        StartOver(gameTime, player, maze, lastTimeInSeconds);

                    return;
                }

                // Устанавливаем следующую точку
                _target = player.Position + Commands.Directions[Commands.CurrentIndex];
            }

            var direction = Commands.Directions[Commands.CurrentIndex];

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

        // Начать с начала
        public static void StartOver(GameTime gameTime, PlayerAnimal player, Maze maze, double lastTimeInSeconds)
        {
            Game1.HaveStartedExecutingCommands = false;
            player.StartFromBeginning();
        }
    }
}
