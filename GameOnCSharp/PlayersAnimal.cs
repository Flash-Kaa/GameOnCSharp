using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        private Point _lastPosition;
        private double _lastTimeInMilliseconds = -1;
        private const double Speed = 5;

        public Vector2 Position 
        { 
            get => _position;
            set
            {
                var gdm = Game1.Graphics;

                if (gdm.PreferredBackBufferHeight >= value.Y
                   || gdm.PreferredBackBufferWidth >= value.X
                   || value.X < 0 || value.Y < 0)
                    throw new ArgumentException("PlayersAnimal cann't be there");

                _position = value;
            }
        }

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
                var coef = (gameTime.TotalGameTime.TotalSeconds - _lastTimeInMilliseconds) * Speed;
                var gdResult = Commands.GetDirection(_position);

                if (gdResult != new Vector2(-1, -1))
                {
                    _position += new Vector2((float)(gdResult.X * coef), (float)(gdResult.Y * coef));
                }
            }
            _lastTimeInMilliseconds = gameTime.TotalGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, Position, null,
                Color.White, 0, _scale.Value, 2f, SpriteEffects.None, 0);
        }
    }
}
