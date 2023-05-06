using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOnCSharp
{
    public class Maze : IGameObject
    {
        public bool HaveWin { get; private set; } = false;

        public Point Start { get; private set; }
        public Target End { get; private set; }
        public Trap[] Traps { get; private set; }

        private List<Point> _walls;
        private PlayerAnimal _player;

        private const double ShareInWidth = 0.55;

        private Vector2 _size = new Vector2(
            (float)(Game1.Graphics.PreferredBackBufferWidth * ShareInWidth),
            Game1.Graphics.PreferredBackBufferHeight - PlayMode.BlockSize);

        Texture2D texture;
        Lazy<Vector2> _scale;

        public Maze()
        {
            var generatedMaze = new MazeGenerator(20, 20, 0.2, 0.1, 4);

            Traps = generatedMaze.Traps.Select(location => new Trap(new Vector2(location.X *PlayMode.BlockSize, location.Y * PlayMode.BlockSize))).ToArray();
            _walls = generatedMaze.Walls.Select(x => new Point((int)(x.X * PlayMode.BlockSize), (int)(x.Y * PlayMode.BlockSize))).ToList();
            Start = new Point((int)(generatedMaze.Start.X * PlayMode.BlockSize), (int)(generatedMaze.Start.X * PlayMode.BlockSize));
            End = new Target(new Vector2(generatedMaze.End.X * PlayMode.BlockSize, generatedMaze.End.Y * PlayMode.BlockSize));
            _player = new PlayerAnimal(this);


            _scale = new Lazy<Vector2>(
               () => new Vector2(
                   PlayMode.BlockSize / texture.Height,
                   PlayMode.BlockSize / texture.Width));
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"Sprites\fence");

            Traps.AsParallel().ForAll(t => t.LoadContent(content));
            End.LoadContent(content);
            _player.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            foreach(var trap in Traps)
            {
                if (trap.Position == _player.Position)
                {
                    trap.Touch = true;
                    Commands.StartOver(gameTime, _player, this, 0);
                }

                trap.Update(gameTime);
            }

            if (End.Position == _player.Position)
                HaveWin = true;

            End.Update(gameTime);
            _player.Update(gameTime);
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            //Traps.AsParallel().ForAll(t => t.Draw(spriteBatch));
            //End.Draw(spriteBatch);
            _player.Draw(spriteBatch);

            _walls.ForEach(x => spriteBatch.Draw(texture, x.ToVector2(), null, Color.White, 0f,
                Vector2.Zero, _scale.Value, SpriteEffects.None, 1f));

            foreach (var trap in Traps)
                trap.Draw(spriteBatch);

            End.Draw(spriteBatch);
        }

        public bool CanLocatedHere(Point position)
        {
            var inScreen =
                position.X <= _size.X
                && position.X >= 0
                && position.Y <= _size.Y
                && position.Y >= 0;

            var collideWithWall = _walls.Any(x => x == _player.Position.ToPoint());
            // Добавить проверку на стены, 

            return inScreen && !collideWithWall;
        }
    }
}
