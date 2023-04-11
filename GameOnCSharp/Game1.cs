using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameOnCSharp
{
    public class Game1 : Game
    {
        public static Texture2D WhiteMask;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _textureSprite;
        private Vector2 _positionSprite = Vector2.Zero;

        private SpriteFont _spriteFont;
        private Lazy<TextBox> l_textBox;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            l_textBox = new Lazy<TextBox>(() => new TextBox(_spriteFont, _graphics));
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            WhiteMask = Content.Load<Texture2D>(@"Sprites\white_pixel");

            _textureSprite = Content.Load<Texture2D>(@"Sprites\ship\ship_front_1");
            _spriteFont = Content.Load<SpriteFont>(@"Fonts\VlaShu");
        }

        protected override void Update(GameTime gameTime)
        {
            #region[EndGame]
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            #endregion

            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();
            l_textBox.Value.Update(mouseState, keyboardState);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            #region[drawing]
            _spriteBatch.Begin();
            _spriteBatch.Draw(_textureSprite, _positionSprite, null, 
                Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
            l_textBox.Value.Draw(_spriteBatch);

            _spriteBatch.End();
            #endregion 

            base.Draw(gameTime);
        }
    }
}