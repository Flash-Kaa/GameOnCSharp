using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameOnCSharp
{
    public record Wall(Vector2 Position) : IGameObject
    {
        private Vector2 _scale;
        private Texture2D _texture;

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>(@"Sprites\fence");

            _scale = new Vector2(
                   PlayMode.BlockSize / _texture.Height,
                   PlayMode.BlockSize / _texture.Width);
        }

        public void Update(GameTime gameTime) 
        {
            if (Position == PlayerAnimal.Position)
                Commands.StartOver(gameTime, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f,
                Vector2.Zero, _scale, SpriteEffects.None, 1f);
        }
    }
}
