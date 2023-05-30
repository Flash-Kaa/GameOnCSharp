using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameOnCSharp
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager Graphics { get; protected set; }
        public static Scene CurrentScene { get; set; }

        private Dictionary<Scene, Type> _sceneClasses;
        private SpriteBatch _spriteBatch;
        private Scene _previous;

        public Game1() : base()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _previous = Scene.Null;
            CurrentScene = Scene.Menu;
            _sceneClasses = new Dictionary<Scene, Type>()
            {
                [Scene.Play] = typeof(PlayMode),
                [Scene.Menu] = typeof(MenuMode)
            };

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            #region[EndGame]
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            #endregion

            if (CurrentScene != _previous)
            {
                _previous = CurrentScene;
                InvokeInCurrentScene("LoadContent", Content);
            }

            InvokeInCurrentScene("Update", gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (CurrentScene != _previous)
                return;

            _spriteBatch.Begin();
            InvokeInCurrentScene("Draw", _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void InvokeInCurrentScene(string methodName, params object[] parameters)
        {
            _sceneClasses[CurrentScene].GetMethod(methodName)?.Invoke(null, parameters);
        }
    }

    public enum Scene
    { 
        Null,
        Menu,
        Play
    }
}