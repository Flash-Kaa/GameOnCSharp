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
        public bool HaveWin = false;

        public Point Start;
        public Target End;
        public Trap[] Traps;

        private List<Rectangle> walls;
        private PlayerAnimal _player;

        private const double ShareInWidth = 0.55;

        private Vector2 _size = new Vector2(
            (float)(Game1.Graphics.PreferredBackBufferWidth * ShareInWidth),
            Game1.Graphics.PreferredBackBufferHeight - Game1.BrickSize); 

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
            Start = new Point(0, 0);
            End = new Target(new Vector2(0, 150));

            _player = new PlayerAnimal(this);

            Traps = new Trap[]
            {
                new Trap(new Vector2(100, 150)),
                new Trap(new Vector2(250, 100))
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
            foreach(var t in Traps)
            {
                if (t.Position == _player.Position)
                {
                    t.Touch = true;
                    Commands.StartOver(gameTime, _player, this, 0);
                }

                t.Update(gameTime);
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
