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
        public static bool HaveQuestions = false;
        public const float BrickSize = 50;

        private SpriteBatch _spriteBatch;

        private bool _doFirstAfterPress = true;
        private List<Lazy<IGameObject>> _components;

        // Для TextBox
        private SpriteFont _font;
        private Texture2D _buttonSpriteForTextbox;
        private const double ShareSizeInWidth = 0.3;
        private const int Indent = 30;

        // Info
        private Texture2D _buttonSpriteForInfo;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var textboxCollider = new Rectangle(
                location: new Point(
                    (int)(Game1.Graphics.PreferredBackBufferWidth * (1 - ShareSizeInWidth)),
                    (int)Indent),
                size: new Point(
                    (int)(Game1.Graphics.PreferredBackBufferWidth * ShareSizeInWidth - Indent),
                    (int)(Game1.Graphics.PreferredBackBufferHeight - Indent * 2)));

            var buttonForTextboxCollider = new Rectangle(
                location: new Point(
                    textboxCollider.X,
                    (int)(Game1.Graphics.PreferredBackBufferHeight * 8 / 10)),
                size: new Point(
                    textboxCollider.Width,
                    (int)(Game1.Graphics.PreferredBackBufferHeight * 1.5 / 10)));

            var buttonForInfoCollider = new Rectangle(
                location: new Point(
                    (int)(Game1.Graphics.PreferredBackBufferWidth * (1 - ShareSizeInWidth - 0.08)),
                    (int)Indent - 20),
                size: new Point(
                    (int)(Game1.Graphics.PreferredBackBufferWidth * 0.05),
                    (int)(Game1.Graphics.PreferredBackBufferWidth * 0.05)));

            _components = new List<Lazy<IGameObject>>
            {
                new Lazy<IGameObject>(() => new TextBox(_font, textboxCollider)),
                new Lazy<IGameObject>(() => new Maze()),
                new Lazy<IGameObject>(() => new Button(_buttonSpriteForTextbox, buttonForTextboxCollider, i => HaveStartedExecutingCommands = i)),
                new Lazy<IGameObject>(() => new Button(_buttonSpriteForInfo, buttonForInfoCollider, i => HaveQuestions = i))
            };

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _font = Content.Load<SpriteFont>(@"Fonts/VlaShu");
            _buttonSpriteForTextbox = Content.Load<Texture2D>(@"Sprites/MyPixelButton");
            _buttonSpriteForInfo = Content.Load<Texture2D>(@"Sprites/Questionmark col_Square Button");

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

            if(HaveStartedExecutingCommands && _doFirstAfterPress)
            {
                Commands.SetCommands((_components[0].Value as TextBox).Text);
                _doFirstAfterPress = false;
            }

            if(!HaveStartedExecutingCommands)
                _doFirstAfterPress = true;
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