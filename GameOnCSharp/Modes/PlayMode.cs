using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;

namespace GameOnCSharp
{
    public static class PlayMode
    {
        public static float BlockSize { get; private set; }

        private static bool _start;
        public static bool HaveStartedExecutingCommands
        {
            get => _start;

            set
            {
                if (HaveWin)
                    throw new ArgumentNullException();

                _start = value;
            }
        }

        private static bool _haveWin;
        public static bool HaveWin
        {
            get => _haveWin;
            set
            {
                if (_haveWin)
                    throw new ArgumentNullException();

                _haveWin = value;
            }
        }

        private const double ShareSizeInWidth = 0.3;
        private const string winText = "You win!";

        private static List<IGameObject> _components;
        private static bool _doFirstAfterPress;
        private static SpriteFont _font;
        private static float Indent;

        private static Vector2 winTextPosition;
        private static float winTextScale;


        static PlayMode()
        {
            _doFirstAfterPress = true;
            _start = false;
            _haveWin = false;
            BlockSize = Game1.Graphics.PreferredBackBufferHeight / 20;
            Indent = BlockSize;
        }

        public static void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>(@"Fonts/VlaShu");

            winTextScale = Game1.Graphics.PreferredBackBufferHeight / 3;

            winTextPosition = new Vector2(
                Game1.Graphics.PreferredBackBufferWidth / 8 - winTextScale / _font.MeasureString(winText).X,
                Game1.Graphics.PreferredBackBufferHeight / 3 - winTextScale / _font.MeasureString(winText).Y);

            var textboxCollider = new Rectangle(
                    location: new Point(
                        (int)(Game1.Graphics.PreferredBackBufferWidth * (1 - ShareSizeInWidth)),
                        (int)Indent),
                    size: new Point(
                        (int)(Game1.Graphics.PreferredBackBufferWidth * ShareSizeInWidth - 2 * Indent),
                        (int)(Game1.Graphics.PreferredBackBufferHeight - Indent * 2)));

            var buttonForTextboxCollider = new Rectangle(
                location: new Point(
                    (int)textboxCollider.X,
                    (int)(Game1.Graphics.PreferredBackBufferHeight * 8 / 10)),
                size: new Point(
                    (int)textboxCollider.Width,
                    (int)(Game1.Graphics.PreferredBackBufferHeight * 1.5 / 10)));

            var frame = new Vector2(Indent/2, Indent/2);

            var _buttonSpriteForTextbox = content.Load<Texture2D>(@"Sprites/MyPixelButton");

            _components = new List<IGameObject>
                {
                    new TextBox(_font, textboxCollider, frame),
                    new Button(_buttonSpriteForTextbox,
                        buttonForTextboxCollider,
                        _ => HaveStartedExecutingCommands = true)
                };

            _components.ForEach(x => x.LoadContent(content));
            Maze.LoadContent(content);
        }

        public static void Update(GameTime gameTime)
        {
            if (HaveStartedExecutingCommands && _doFirstAfterPress)
            {
                Commands.SetCommands((_components[0] as TextBox).Text.ToString());
                _doFirstAfterPress = false;
            }
            else if(!HaveStartedExecutingCommands)
            {
                _doFirstAfterPress = true;
            }

            Maze.Update(gameTime);
            _components.ForEach(x => x.Update(gameTime));
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            Game1.Graphics.GraphicsDevice.Clear(Color.ForestGreen);

            if (HaveWin)
            {
                spriteBatch.DrawString(_font, winText, winTextPosition, Color.Black,
                    0f, Vector2.Zero, winTextScale / _font.MeasureString(winText).Y, SpriteEffects.None, 0f);
                return;
            }

            Maze.Draw(spriteBatch);
            _components.ForEach(x => x.Draw(spriteBatch));
        }
    }
}