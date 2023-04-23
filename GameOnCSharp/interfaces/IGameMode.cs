using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameOnCSharp
{
    public interface IGameMode
    {
        void LoadContent(ContentManager content);

        void Initialize();

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}
