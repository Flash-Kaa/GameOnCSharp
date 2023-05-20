using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameOnCSharp
{
    public class PlayerAnimal : IGameObject
    {
        public const double Speed = 3;

        public Dictionary<Vector2, Texture2D> DictSprites { get; private set; }

        private Maze _maze;
        private Vector2 _position;
        private Texture2D _currentSprite;
        private Lazy<Vector2> _scaleTexture;
        private double _lastUpdTimeInSeconds;

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

        public Vector2 Position 
        { 
            get => _position;
            set
            {
                if(!_maze.CanLocatedHere(value.ToPoint()))
                    throw new IndexOutOfRangeException();

                _position = value;
            }
        }

        public PlayerAnimal(Maze maze)
        {
            _maze = maze;
            _lastUpdTimeInSeconds = -1;
            _position = _maze.Start;

            _scaleTexture = new Lazy<Vector2>(
               () => new Vector2(
                   PlayMode.BlockSize / _currentSprite.Height,
                   PlayMode.BlockSize / _currentSprite.Width));
        }

        public void StartFromBeginning()
        {
            _position = _maze.Start;
            _currentSprite = DictSprites[new Vector2(0, 0)];
        }

        public void LoadContent(ContentManager content)
        {
            _currentSprite = content.Load<Texture2D>(@"Sprites\ship\ship_down");
            
            DictSprites = new Dictionary<Vector2, Texture2D>()
            {
                [new Vector2(0, 0)] = content.Load<Texture2D>(@"Sprites\ship\ship_down"),

                [new Vector2(0, PlayMode.BlockSize)] = content.Load<Texture2D>(@"Sprites\ship\ship_down"),
                [new Vector2(PlayMode.BlockSize, 0)] = content.Load<Texture2D>(@"Sprites\ship\ship_right"),
                [new Vector2(0, -PlayMode.BlockSize)] = content.Load<Texture2D>(@"Sprites\ship\ship_up"),
                [new Vector2(-PlayMode.BlockSize, 0)] = content.Load<Texture2D>(@"Sprites\ship\ship_left")
            };
        }

        public void Update(GameTime gameTime)
        {
            if (PlayMode.HaveStartedExecutingCommands)
                Commands.ShiftPlayer(gameTime, this, _maze, _lastUpdTimeInSeconds);

            _lastUpdTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_currentSprite, _position, null, Color.White, 0f,
                    Vector2.Zero, _scaleTexture.Value, SpriteEffects.None, 1f);
        }
    }
}
