using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOnCSharp
{
    public class TextBox
    {
        SpriteFont font;
        Rectangle bounds;
        Vector2 frame = new Vector2(20, 20);

        private string _text = "";
        private bool _isSelected = false;
        private bool _isKeyPressed = false;
        private Keys _lastKeyPressed;

        private const double ShareInWidth = 0.7;
        private const int Indent = 30;

        public TextBox(SpriteFont font, Rectangle bounds)
        {
            this.bounds = bounds;
            this.font = font;
        }

        public TextBox(SpriteFont font, GraphicsDeviceManager gdm)
        {
            var position = new Point((int)(gdm.PreferredBackBufferWidth * ShareInWidth), Indent);
            var size = new Point(
                (int)(gdm.PreferredBackBufferWidth * (1 - ShareInWidth) - Indent), 
                gdm.PreferredBackBufferHeight - Indent * 2);

            bounds = new Rectangle(position, size);
            this.font = font;
        }

        public void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                _isSelected = bounds.Contains(mouseState.Position);
            }

            if (_isSelected)
            {
                if (keyboardState.GetPressedKeys().Length > 0)
                {
                    // получаем код последней нажатой клавиши
                    Keys keyPressed = keyboardState.GetPressedKeys()[0];
                    var input = keyPressed.ToString();

                    if (keyPressed != _lastKeyPressed)
                    {
                        if (keyPressed == Keys.Back && _text.Length > 0)
                        {
                            // Удаляем последний символ
                            _text = _text.Substring(0, _text.Length - 1); 
                        }

                        else if (keyPressed == Keys.Space)
                        {
                            _text += " ";
                        }

                        else if (keyPressed == Keys.Enter)
                        {
                            _text += "\n";
                        }

                        else if (!_isKeyPressed
                            && input.Length == 1)
                        {
                            _text += input;
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

            spriteBatch.Draw(Game1.WhiteMask, pos, null, Color.White, 0f, 
                Vector2.Zero, scale, SpriteEffects.None, 1f);
            
            spriteBatch.DrawString(font, _text, bounds.Location.ToVector2(), 
                _isSelected ? Color.Red : Color.Black);
        }

        public string Text => _text;
    }
}
