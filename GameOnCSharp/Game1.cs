using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOnCSharp
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager Graphics { get; private set; }
        public static bool HaveStartedExecutingCommands = false;
        public const int BrickSize = 10;

        private SpriteBatch _spriteBatch;

        public SpriteFont Font;

        private List<Lazy<IGameObject>> _components;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var buttonPosition = new Point(
                Game1.Graphics.PreferredBackBufferWidth * 7 / 10, 
                Game1.Graphics.PreferredBackBufferHeight * 8 / 10);

            _components = new List<Lazy<IGameObject>>
            {
                new Lazy<IGameObject>(() => new TextBox(Font)),
                new Lazy<IGameObject>(() => new PlayerAnimal()),
                new Lazy<IGameObject>(() => new Button(buttonPosition))
            };

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Font = Content.Load<SpriteFont>(@"Fonts\VlaShu");

            _components.AsParallel().ForAll(x => x.Value.LoadContent(Content));
        }

        protected override void Update(GameTime gameTime)
        {
            #region[EndGame]
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            #endregion

            _components.ForEach(x => x.Value.Update(gameTime));

            if(HaveStartedExecutingCommands)
            {
                Commands.SetCommands((_components[0].Value as TextBox).Text);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            #region[drawing]
            _spriteBatch.Begin();

            _components.ForEach(x => x.Value.Draw(_spriteBatch));

            _spriteBatch.End();
            #endregion 

            base.Draw(gameTime);
        }
    }
}