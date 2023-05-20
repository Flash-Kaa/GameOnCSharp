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
        private Texture2D[] _grass;
        private double _lastUpdTime;
        private Lazy<Vector2> _scale;

        private const double _timeToUpd = 4;

        public Target(Vector2 position)
        {
            _lastUpdTime = 0;
            _currentIndex = 0;
            Position = position;

            _scale = new Lazy<Vector2>(
               () => new Vector2(
                   PlayMode.BlockSize / _grass[_currentIndex].Height,
                   PlayMode.BlockSize / _grass[_currentIndex].Width));
        }

        public void LoadContent(ContentManager content)
        {
            _grass = new Texture2D[]
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_grass[_currentIndex], Position, null, Color.White, 0f,
                Vector2.Zero, _scale.Value, SpriteEffects.None, 1f);
        }

        private void ChangeSprite()
        {
            _currentIndex++;

            if (_currentIndex == _grass.Length)
                _currentIndex = 0;
        }
    }
}
