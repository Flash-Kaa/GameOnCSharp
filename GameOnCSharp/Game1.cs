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
        public static bool HaveQuestions = false;

        public static Scene CurrentScene = Scene.Menu;
        private Dictionary<Scene, Lazy<IGameMode>> _scenes;

        private SpriteBatch _spriteBatch;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _scenes = new Dictionary<Scene,Lazy<IGameMode>>
            {
                { Scene.Play, new Lazy<IGameMode>(() => new PlayMode(_spriteBatch)) },
                { Scene.Menu, new Lazy<IGameMode>(() => new MenuMode()) },
            };

            _scenes.AsParallel().ForAll(x => x.Value.Value.Initialize());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _scenes.AsParallel().ForAll(x => x.Value.Value.LoadContent(Content));
        }

        protected override void Update(GameTime gameTime)
        {
            #region[EndGame]
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            #endregion

            _scenes[CurrentScene].Value.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            #region[drawing]
            _spriteBatch.Begin();
            _scenes[CurrentScene].Value.Draw(_spriteBatch);
            _spriteBatch.End();
            #endregion 

            base.Draw(gameTime);
        }
    }

    public enum Scene
    { 
        Menu,
        Play,
        Help
    }
}