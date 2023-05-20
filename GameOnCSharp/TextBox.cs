using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOnCSharp
{
    public class TextBox : IGameObject
    {
        public string Text { get; private set; }

        private Vector2 _frame;
        private bool _isSelected;
        private SpriteFont _font;
        private Color _frameColor;
        private Rectangle _bounds;
        private bool _isKeyPressed;
        private Keys _lastKeyPressed;
        private Texture2D _whiteMask;

        public TextBox(SpriteFont font, Rectangle bounds)
        {
            _bounds = bounds;
            _font = font;

            _isSelected = false;
            _isKeyPressed = false;
            _frameColor = Color.White;
            _frame = new Vector2(20, 20);
            Text = "";
        }

        public void LoadContent(ContentManager content)
        {
            _whiteMask = content.Load<Texture2D>(@"Sprites\white_pixel");
        }

        public void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && !PlayMode.HaveStartedExecutingCommands)
            {
                _isSelected = _bounds.Contains(mouseState.Position);
            }

            if (_isSelected && !PlayMode.HaveStartedExecutingCommands)
            {
                _frameColor = Color.LightYellow;

                if (keyboardState.GetPressedKeys().Length > 0)
                {
                    // получаем код последней нажатой клавиши
                    Keys keyPressed = keyboardState.GetPressedKeys()[0];
                    var input = (char)keyPressed;

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
                            && char.IsLetterOrDigit(input))
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
            else
            {
                _frameColor = Color.White;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var pos = _bounds.Location.ToVector2() - _frame;
            var scale = _bounds.Size.ToVector2() + _frame * 2;

            #region[draw brick]
            /*spriteBatch.Draw(_whiteMask, new Vector2(0, 0), null, Color.White, 0f,
                Vector2.Zero, new Vector2(Game1.BrickSize, Game1.BrickSize), SpriteEffects.None, 1f);*/
            #endregion

            spriteBatch.Draw(_whiteMask, pos, null, _frameColor, 0f, 
                Vector2.Zero, scale, SpriteEffects.None, 1f);
            
            spriteBatch.DrawString(_font, Text, _bounds.Location.ToVector2(),
                _isSelected && !PlayMode.HaveStartedExecutingCommands ? Color.Red : Color.Black);
        }
    }
}
