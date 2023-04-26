using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        private List<Rectangle> walls;
        private PlayerAnimal _player;

        private const double ShareInWidth = 0.55;

        private Vector2 _size = new Vector2(
            (float)(Game1.Graphics.PreferredBackBufferWidth * ShareInWidth),
            Game1.Graphics.PreferredBackBufferHeight - PlayMode.BlockSize); 

        public bool CanLocatedHere(Point position)
        {
            var inScreen =
                position.X <= _size.X
                && position.X >= 0
                && position.Y <= _size.Y
                && position.Y >= 0;

            // Добавить проверку на стены, 

            return inScreen;
        }

        public Maze()
        {
            // Сделать зависимость от размера PlayModel.BlockSize
            Start = new Point(0, 0);
            End = new Target(new Vector2(0, PlayMode.BlockSize * 3));

            _player = new PlayerAnimal(this);

            Traps = new Trap[]
            {
                new Trap(new Vector2(PlayMode.BlockSize * 2, PlayMode.BlockSize * 3)),
                new Trap(new Vector2(PlayMode.BlockSize * 5, PlayMode.BlockSize * 3))
            };
        }

        public void LoadContent(ContentManager content)
        {

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
            Traps.AsParallel().ForAll(t => t.Draw(spriteBatch));
            End.Draw(spriteBatch);
            _player.Draw(spriteBatch);
        }
    }
}
