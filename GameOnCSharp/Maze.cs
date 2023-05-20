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
        public Target End { get; private set; }
        public Vector2 Start { get; private set; }
        public bool HaveWin { get; private set; }
        public Trap[] Traps { get; private set; }

        private Vector2 _size;
        private SpriteFont _font;
        private Texture2D _texture;
        private List<Vector2> _walls;
        private PlayerAnimal _player;
        private Lazy<Vector2> _scale;

        private const double ShareInWidth = 0.55;

        public Maze()
        {
            var generatedMaze = new MazeGenerator(20, 20, 0.2, 0.2, 4);

            Traps = generatedMaze.Traps
                .Select(location => new Trap(
                    new Vector2(
                        location.X *PlayMode.BlockSize, 
                        location.Y * PlayMode.BlockSize)))
                .ToArray();

            _walls = generatedMaze.Walls
                .Select(x => new Vector2(
                    (int)(x.X * PlayMode.BlockSize), 
                    (int)(x.Y * PlayMode.BlockSize)))
                .ToList();

            Start = new Vector2(
                (int)(generatedMaze.Start.X * PlayMode.BlockSize), 
                (int)(generatedMaze.Start.Y * PlayMode.BlockSize));

            End = new Target(
                new Vector2(
                    generatedMaze.End.X * PlayMode.BlockSize, 
                    generatedMaze.End.Y * PlayMode.BlockSize));

            _scale = new Lazy<Vector2>(() => new Vector2(
                   PlayMode.BlockSize / _texture.Height,
                   PlayMode.BlockSize / _texture.Width));

            _size = new Vector2(
                (float)(Game1.Graphics.PreferredBackBufferWidth * ShareInWidth),
                Game1.Graphics.PreferredBackBufferHeight);

            HaveWin = false;
            _player = new PlayerAnimal(this);
        }

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>(@"Fonts/VlaShu");
            _texture = content.Load<Texture2D>(@"Sprites\fence");

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
            _walls.ForEach(x => spriteBatch.Draw(_texture, x, null, Color.White, 0f,
                Vector2.Zero, _scale.Value, SpriteEffects.None, 1f));

            foreach (var trap in Traps)
                trap.Draw(spriteBatch);

            End.Draw(spriteBatch);
            _player.Draw(spriteBatch);

            if (HaveWin)
            {
                spriteBatch.DrawString(_font, "You WIN!!!", 
                    new Vector2(100, Game1.Graphics.PreferredBackBufferHeight / 3), Color.Black, 
                    0f, Vector2.Zero, PlayMode.BlockSize / 8, SpriteEffects.None, 0f);
            }
        }

        public bool CanLocatedHere(Point position)
        {
            var inScreen =
                position.X <= _size.X
                && position.X >= 0
                && position.Y <= _size.Y
                && position.Y >= 0;

            var collideWithWall = _walls
                .Any(x => x == _player.Position);
            
            return inScreen && !collideWithWall;
        }
    }
}