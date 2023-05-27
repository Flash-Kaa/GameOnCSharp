using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace GameOnCSharp
{
    public static class Maze
    {
        public static Target End { get; private set; }
        public static Vector2 Start { get; private set; }
        public static Trap[] Traps { get; private set; }

        private static Vector2 _size;
        private static List<Wall> _walls;
        private static List<IGameObject> _components;

        private const double ShareInWidth = 0.55;

        public static void LoadContent(ContentManager content)
        {
            var generatedMaze = new MazeGenerator(20, 20, 0.3, 0.05, 4);


            _components = generatedMaze.Traps
                .Select(location => (IGameObject)new Trap(
                    new Vector2(
                        (int)location.X * PlayMode.BlockSize,
                        (int)location.Y * PlayMode.BlockSize)))
                .ToList();

            _components.AddRange(generatedMaze.Walls
                .Select(x => (IGameObject)new Wall(new Vector2(
                    (int)(x.X * PlayMode.BlockSize),
                    (int)(x.Y * PlayMode.BlockSize)))));

            Start = new Vector2(
                (int)(generatedMaze.Start.X * PlayMode.BlockSize),
                (int)(generatedMaze.Start.Y * PlayMode.BlockSize));

            End = new Target(
                new Vector2(
                    (int)generatedMaze.End.X * PlayMode.BlockSize,
                    (int)generatedMaze.End.Y * PlayMode.BlockSize));


            _size = new Vector2(
                (float)(Game1.Graphics.PreferredBackBufferWidth * ShareInWidth),
                Game1.Graphics.PreferredBackBufferHeight);


            _components.ForEach(t => t.LoadContent(content));
            End.LoadContent(content);

            PlayerAnimal.LoadContent(content);
        }

        public static void Update(GameTime gameTime)
        {
            _components.ForEach(t => t.Update(gameTime));

            End.Update(gameTime);
            PlayerAnimal.Update(gameTime);
        }
        
        public static void Draw(SpriteBatch spriteBatch)
        {
            _components.ForEach(t => t.Draw(spriteBatch));

            End.Draw(spriteBatch);
            PlayerAnimal.Draw(spriteBatch);
        }

        public static bool CanLocatedHere(Point position)
        {
            var inScreen =
                position.X <= _size.X
                && position.X >= 0
                && position.Y <= _size.Y
                && position.Y >= 0;

            return inScreen;
        }
    }
}