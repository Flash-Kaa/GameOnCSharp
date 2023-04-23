using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameOnCSharp
{
    public class Trap : IGameObject
    {
        public Vector2 Position { get; }
        public bool Touch { get; set; } = false;

        private const double TimeToUpd = 0.5;
        private double _touchTime = -1;

        Texture2D _open;
        Texture2D _closed;

        Texture2D _currentSprite;

        Lazy<Vector2> _scale;

        public Trap(Vector2 position)
        {
            Position = position;

            _scale = new Lazy<Vector2>(
               () => new Vector2(
                   PlayMode.BlockSize / _currentSprite.Height,
                   PlayMode.BlockSize / _currentSprite.Width));
        }

        public void LoadContent(ContentManager content)
        {
            _open = content.Load<Texture2D>(@"Sprites\trap\openTrap");
            _closed = content.Load<Texture2D>(@"Sprites\trap\closedTrap");

            _currentSprite = _open;
        }   

        public void Update(GameTime gameTime)
        {
            if (Touch && _touchTime == -1)
            {
                _currentSprite = _closed;
                _touchTime = gameTime.TotalGameTime.TotalSeconds;
            }

            else if (Touch && gameTime.TotalGameTime.TotalSeconds - _touchTime >= TimeToUpd)
            {
                _currentSprite = _open;
                _touchTime = -1;
                Touch = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_currentSprite, Position, null, Color.White, 0f,
                Vector2.Zero, _scale.Value, SpriteEffects.None, 1f);
        }
    }
}
