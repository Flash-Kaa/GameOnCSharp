using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Linq;

namespace GameOnCSharp
{
    public class MenuMode : IGameMode
    {
        private List<Lazy<IGameObject>> _components;

        private Texture2D _startButtonTexture;
        private Texture2D _questionButtonTexture;
        private Texture2D[] _screenResolutionTextures;
        private Texture2D _fullScreenButtonTexture;

        public MenuMode()
        {
            //Graphics.PreferredBackBufferHeight = 1080;
            //Graphics.PreferredBackBufferWidth = 1920;
            //Graphics.IsFullScreen = true;   
        }

        public void Initialize()
        {
            var sizeStartButtonRect = new Point(
                    Game1.Graphics.PreferredBackBufferWidth / 3,
                    Game1.Graphics.PreferredBackBufferHeight / 5);

            var startButtonRect = new Lazy<Rectangle>( () => new Rectangle(
                location: new Point(
                    Game1.Graphics.PreferredBackBufferWidth / 2 - sizeStartButtonRect.X / 2,
                    Game1.Graphics.PreferredBackBufferHeight / 2 - sizeStartButtonRect.Y / 2),
                size: sizeStartButtonRect));

            _components = new List<Lazy<IGameObject>>()
            {
                new Lazy<IGameObject>( () => new Button(
                    _startButtonTexture, startButtonRect.Value, _ => Game1.CurrentScene = Scene.Play))
            };
        }

        public void LoadContent(ContentManager content)
        {
            _startButtonTexture = content.Load<Texture2D>(@"Sprites/MyPixelButton");
            _components.AsParallel().ForAll(x => x.Value.LoadContent(content));
        }

        public void Update(GameTime gameTime)
        {
            _components.ForEach(x => x.Value.Update(gameTime));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Game1.Graphics.GraphicsDevice.Clear(Color.DarkGreen);
            _components.ForEach(x => x.Value.Draw(spriteBatch));
        }
    }
}
