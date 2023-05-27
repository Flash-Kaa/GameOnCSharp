using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameOnCSharp
{
    public class Trap : IGameObject
    {
        public Vector2 Position { get; }

        private bool _touch;
        private double _touchTime;
        private Lazy<Vector2> _scale;
        private Texture2D _openTexture;
        private Texture2D _closedTexture;
        private Texture2D _currentTexture;

        private const double TimeToUpd = 0.5;

        public Trap(Vector2 position)
        {
            _touch = false;
            _touchTime = -1;
            Position = position;
            _scale = new Lazy<Vector2>(
               () => new Vector2(
                   PlayMode.BlockSize / _currentTexture.Height,
                   PlayMode.BlockSize / _currentTexture.Width));
        }

        public void LoadContent(ContentManager content)
        {
            _openTexture = content.Load<Texture2D>(@"Sprites\trap\openTrap");
            _closedTexture = content.Load<Texture2D>(@"Sprites\trap\closedTrap");

            _currentTexture = _openTexture;
        }   

        public void Update(GameTime gameTime)
        {
            if (Position == PlayerAnimal.Position)
            {
                _touch = true;
                Commands.StartOver(gameTime, 0);
            }

            if (_touch && _touchTime == -1)
            {
                _currentTexture = _closedTexture;
                _touchTime = gameTime.TotalGameTime.TotalSeconds;
            }
            else if (_touch && gameTime.TotalGameTime.TotalSeconds - _touchTime >= TimeToUpd)
            {
                _currentTexture = _openTexture;
                _touchTime = -1;
                _touch = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_currentTexture, Position, null, Color.White, 0f,
                Vector2.Zero, _scale.Value, SpriteEffects.None, 1f);
        }
    }
}
