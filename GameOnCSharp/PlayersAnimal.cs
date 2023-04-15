using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameOnCSharp
{
    public class PlayerAnimal : IGameObject
    {

        public Dictionary<Vector2, Texture2D> DictSprites { get; private set; }
        Texture2D _currentSprite;

        public Texture2D CurrentSprite
        {
            get => _currentSprite;
            set
            {
                if(!DictSprites.ContainsValue(value))
                    throw new KeyNotFoundException();

                _currentSprite = value;
            }
        }

        Lazy<Vector2> _scale;

        private Maze _maze;
        private Vector2 _position;
        private double _lastTimeInSeconds = -1;

        public const double Speed = 3;


        public void StartFromBeginning()
        {
            _position = _maze.Start.ToVector2();
            _currentSprite = DictSprites[new Vector2(0, 0)];
        }

        public Vector2 Position 
        { 
            get => _position;
            set
            {
                _position = value;
            }
        }

        public PlayerAnimal(Maze maze)
        {
            _maze = maze;

            _position = _maze.Start.ToVector2();

            _scale = new Lazy<Vector2>(
               () => new Vector2(
                   Game1.BrickSize / _currentSprite.Height,
                   Game1.BrickSize / _currentSprite.Width));

        }

        public void LoadContent(ContentManager content)
        {
            _currentSprite = content.Load<Texture2D>(@"Sprites\ship\ship_down");
            DictSprites = new Dictionary<Vector2, Texture2D>()
            {
                { new Vector2(0, 0), content.Load<Texture2D>(@"Sprites\ship\ship_down") },
                { new Vector2(0, Game1.BrickSize), content.Load<Texture2D>(@"Sprites\ship\ship_down") },
                { new Vector2(Game1.BrickSize, 0), content.Load<Texture2D>(@"Sprites\ship\ship_right") },
                { new Vector2(0, -Game1.BrickSize), content.Load<Texture2D>(@"Sprites\ship\ship_up") },
                { new Vector2(-Game1.BrickSize, 0), content.Load<Texture2D>(@"Sprites\ship\ship_left") }
            };
        }

        public void Update(GameTime gameTime)
        {
            if (Game1.HaveStartedExecutingCommands)
            {
                Commands.ShiftPlayer(gameTime, this, _maze, _lastTimeInSeconds);
            }
            _lastTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_currentSprite, _position, null, Color.White, 0f,
                    Vector2.Zero, _scale.Value, SpriteEffects.None, 1f);
        }
    }
}
