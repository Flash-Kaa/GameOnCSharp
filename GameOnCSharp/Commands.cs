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

            commands.AsParallel().ForAll(
                k => 
                {
                    switch (k.ToLower())
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
                });
        }
    }
}
