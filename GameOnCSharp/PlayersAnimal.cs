using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnCSharp
{
    internal class PlayerAnimal : IGameObject
    {
        Texture2D _sprite;
        Lazy<Vector2> _scale;

        private Vector2 _position;
        private double _lastTimeInSeconds = -1;
        private const double Speed = 3;

        private Vector2 _target = new Vector2(-1, -1);

        /*public Vector2 Position 
        { 
            get => _position;
            private set
            {
                var gdm = Game1.Graphics;

                if (gdm.PreferredBackBufferHeight >= value.Y
                   || gdm.PreferredBackBufferWidth >= value.X
                   || value.X < 0 || value.Y < 0)
                    throw new ArgumentException("PlayersAnimal cann't be there");

                _position = value;
            }
        }*/

        public PlayerAnimal()
        {
            _scale = new Lazy<Vector2>(
                () => new Vector2 (
                    Game1.BrickSize / _sprite.Height,
                    Game1.BrickSize / _sprite.Width));

        }

        public void LoadContent(ContentManager content)
        {
            _sprite = content.Load<Texture2D>(@"Sprites\ship\ship_front_1");
        }

        public void Update(GameTime gameTime)
        {
            if (Game1.HaveStartedExecutingCommands)
            {
                Shift(gameTime);
            }
            _lastTimeInSeconds = gameTime.TotalGameTime.TotalSeconds;
        }

        private void Shift(GameTime gameTime)
        {
            // Vector2(-1, -1) - стандартный
            if (_target == new Vector2(-1, -1) || _target == _position)
            {
                Commands.CurrentIndex++;

                // Завершаем игру, если использовали все команды
                if (Commands.CurrentIndex >= Commands.Directions.Length)
                {
                    Game1.HaveStartedExecutingCommands = false;
                    return;
                }

                // Устанавливаем следующую точку
                _target = _position + Commands.Directions[Commands.CurrentIndex];
            }

            var direction = Commands.Directions[Commands.CurrentIndex];

            // Направление и величина движения
            var coef = (gameTime.TotalGameTime.TotalSeconds - _lastTimeInSeconds) * Speed;
            var shift = new Vector2((float)(direction.X * coef), (float)(direction.Y * coef));

            // Принимаем во внимание, что мы можем перепрыгнуть цель
            var lenToTarget = new Vector2(_position.X - _target.X, _position.Y - _target.Y).Length();
            _position = lenToTarget >= shift.Length() ? _position + shift : _target;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, _position, null, Color.White, 0f,
                Vector2.Zero, _scale.Value, SpriteEffects.None, 1f);
        }
    }
}
