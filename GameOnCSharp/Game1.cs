using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameOnCSharp
{
    public class Game1 : Game
    {
        public static Dictionary<Scene, IGameMode> Scenes { get; private set; }
        public static GraphicsDeviceManager Graphics { get; private set; }

        public static Scene CurrentScene { get; set; } = Scene.Menu;
        public static bool HaveQuestions { get; set; } = false;

        private Scene _previous = Scene.Menu;
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

            Scenes = new Dictionary<Scene, IGameMode>
            { [Scene.Menu] = new MenuMode() };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Scenes[CurrentScene].LoadContent(Content);
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
                if (CurrentScene == Scene.Play)
                    Scenes[CurrentScene] = new PlayMode(_spriteBatch);
                
                _previous = CurrentScene;
                Scenes[CurrentScene].LoadContent(Content);
            }

            Scenes[CurrentScene].Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            if(Scenes.ContainsKey(CurrentScene))
                Scenes[CurrentScene].Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public enum Scene
    { 
        Menu,
        Play
    }
}