using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOnCSharp
{
    public class TextBox : IGameObject
    {
        public SpriteFont Font { get; set; }
        public string Text { get; private set; } = "";

        Rectangle bounds = new Rectangle();
        Vector2 frame = new Vector2(20, 20);
        Texture2D _whiteMask;

        private bool _isSelected = false;
        private bool _isKeyPressed = false;
        private Keys _lastKeyPressed;

        private const double ShareInWidth = 0.3;
        private const int Indent = 30;

        public TextBox(SpriteFont font, Rectangle bounds)
        {
            this.bounds = bounds;
            this.Font = font;
        }

        public TextBox(SpriteFont font)
        {
            this.Font = font;

            var gdm = Game1.Graphics;

            var position = new Point((int)(gdm.PreferredBackBufferWidth * (1 - ShareInWidth)), Indent);
            var size = new Point(
                (int)(gdm.PreferredBackBufferWidth * ShareInWidth - Indent),
                gdm.PreferredBackBufferHeight - Indent * 2);

            bounds = new Rectangle(position, size);
        }

        public void LoadContent(ContentManager content)
        {
            _whiteMask = content.Load<Texture2D>(@"Sprites\white_pixel");
        }

        public void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && !Game1.HaveStartedExecutingCommands)
            {
                _isSelected = bounds.Contains(mouseState.Position);
            }

            if (_isSelected && !Game1.HaveStartedExecutingCommands)
            {
                if (keyboardState.GetPressedKeys().Length > 0)
                {
                    // получаем код последней нажатой клавиши
                    Keys keyPressed = keyboardState.GetPressedKeys()[0];
                    var input = keyPressed.ToString();

                    if (keyPressed != _lastKeyPressed)
                    {
                        if (keyPressed == Keys.Back && Text.Length > 0)
                        {
                            // Удаляем последний символ
                            Text = Text.Substring(0, Text.Length - 1); 
                        }

                        else if (keyPressed == Keys.Space)
                        {
                            Text += " ";
                        }

                        else if (keyPressed == Keys.Enter)
                        {
                            Text += "\n";
                        }

                        else if (!_isKeyPressed
                            && input.Length == 1)
                        {
                            Text += input;
                            _isKeyPressed = true;
                        }

                        _lastKeyPressed = keyPressed;
                    }
                }

                else
                {
                    _isKeyPressed = false;
                    _lastKeyPressed = Keys.None;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var pos = bounds.Location.ToVector2() - frame;
            var scale = bounds.Size.ToVector2() + frame * 2;

            #region[draw brick]
            /*spriteBatch.Draw(_whiteMask, new Vector2(0, 0), null, Color.White, 0f,
                Vector2.Zero, new Vector2(Game1.BrickSize, Game1.BrickSize), SpriteEffects.None, 1f);*/
            #endregion

            spriteBatch.Draw(_whiteMask, pos, null, Color.White, 0f, 
                Vector2.Zero, scale, SpriteEffects.None, 1f);
            
            spriteBatch.DrawString(Font, Text, bounds.Location.ToVector2(),
                _isSelected && !Game1.HaveStartedExecutingCommands ? Color.Red : Color.Black);
        }
    }
}
