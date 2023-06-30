using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameOnCSharp
{
    public static class PlayerAnimal
    {
        public static Dictionary<Vector2, Texture2D> DictSprites { get; private set; }

        public const double Speed = 3;

        private static Texture2D _currentSprite;
        public static Texture2D CurrentSprite
        {
            get => _currentSprite;
            set
            {
                if(!DictSprites.ContainsValue(value))
                    throw new KeyNotFoundException();

                _currentSprite = value;
            }
        }

        private static Vector2 _position;
        public static Vector2 Position 
        { 
            get => _position;
            set
            {
                _position = value;
            }
        }

        private static Vector2 _scaleTexture;
        private static double _lastUpdTimeInSeconds;

        public static void StartFromBeginning()
        {
            _position = Maze.Start;
            _currentSprite = DictSprites[new Vector2(0, 0)];
        }

        public static void LoadContent(ContentManager content)
        {
            _lastUpdTimeInSeconds = -1;
            _position = Maze.Start;

            _currentSprite = content.Load<Texture2D>(@"Sprites\ship\ship_down");

            _scaleTexture = new Vector2(
                   PlayMode.BlockSize / _currentSprite.Height,
                   PlayMode.BlockSize / _currentSprite.Width);

            DictSprites = new Dictionary<Vector2, Texture2D>()
            {
                [new Vector2(0, 0)] = content.Load<Texture2D>(@"Sprites\ship\ship_down"),

                [new Vector2(0, PlayMode.BlockSize)] = content.Load<Texture2D>(@"Sprites\ship\ship_down"),
                [new Vector2(PlayMode.BlockSize, 0)] = content.Load<Texture2D>(@"Sprites\ship\ship_right"),
                [new Vector2(0, -PlayMode.BlockSize)] = content.Load<Texture2D>(@"Sprites\ship\ship_up"),
                [new Vector2(-PlayMode.BlockSize, 0)] = content.Load<Texture2D>(@"Sprites\ship\ship_left")
            };
        }

        public static void Update(GameTime gameTime)
        {
            if (PlayMode.HaveStartedExecutingCommands)
                Commands.ShiftPlayer(gameTime, _lastUpdTimeInSeconds);

            _lastUpdTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_currentSprite, _position, null, Color.White, 0f,
                    Vector2.Zero, _scaleTexture, SpriteEffects.None, 1f);
        }
    }
}
