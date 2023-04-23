using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameOnCSharp
{
    public class Target : IGameObject
    {
        public Vector2 Position { get; }

        const double _timeToUpd = 1;

        private double _lastUpdTime = 0;
        private Texture2D[] _grass;
        private int _currentIndex = 0;
        Lazy<Vector2> _scale;

        public Target(Vector2 position)
        {
            Position = position;

            _scale = new Lazy<Vector2>(
               () => new Vector2(
                   PlayMode.BlockSize / _grass[_currentIndex].Height,
                   PlayMode.BlockSize / _grass[_currentIndex].Width));
        }

        private void ChangeSprite()
        {
            _currentIndex++;

            if(_currentIndex == _grass.Length)
                _currentIndex = 0;
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
    }
}
