using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameOnCSharp
{
    public class Target : IGameObject
    {
        public Vector2 Position { get; }

        private int _currentIndex;
        private Texture2D[] _grassTextures;
        private double _lastUpdTime;
        private Lazy<Vector2> _scaleTexture;

        private const double _timeToUpd = 4;

        public Target(Vector2 position)
        {
            _lastUpdTime = 0;
            _currentIndex = 0;
            Position = position;

            _scaleTexture = new Lazy<Vector2>(
               () => new Vector2(
                   PlayMode.BlockSize / _grassTextures[_currentIndex].Height,
                   PlayMode.BlockSize / _grassTextures[_currentIndex].Width));
        }

        public void LoadContent(ContentManager content)
        {
            _grassTextures = new Texture2D[]
            {
                content.Load<Texture2D>(@"Sprites/grass/grass1"),
                content.Load<Texture2D>(@"Sprites/grass/grass2")
            };
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds - _lastUpdTime >= _timeToUpd)
            {
                _lastUpdTime = gameTime.TotalGameTime.TotalSeconds;
                ChangeSprite();
            }

            if (Position == PlayerAnimal.Position)
                PlayMode.HaveWin = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_grassTextures[_currentIndex], Position, null, Color.White, 0f,
                Vector2.Zero, _scaleTexture.Value, SpriteEffects.None, 1f);
        }

        private void ChangeSprite()
        {
            _currentIndex++;

            if (_currentIndex == _grassTextures.Length)
                _currentIndex = 0;
        }
    }
}
