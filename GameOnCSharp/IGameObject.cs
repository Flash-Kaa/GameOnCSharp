using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameOnCSharp
{
    public interface IGameObject
    {
        void LoadContent(ContentManager content);

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}
