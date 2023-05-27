using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;

namespace GameOnCSharp
{
    public class TextBox : IGameObject
    {
        public StringBuilder Text { get; private set; }

        private const double TimeToNextCheckInSecond = 0.15;

        private bool _isSelected;
        private SpriteFont _font;
        private float _scaleText;
        private Color _frameColor;
        private Rectangle _bounds;
        private Vector2 _position;
        private Vector2 _colliderBox;
        private Vector2 _colliderText;
        private Texture2D _whiteMask;
        private double _lastKeyPressedTime;

        public TextBox(SpriteFont font, Rectangle bounds, Vector2 frame)
        {
            Text = new StringBuilder();
            _frameColor = Color.White;
            _lastKeyPressedTime = 0;
            _isSelected = false;
            _bounds = bounds;
            _font = font;

            _colliderText = _bounds.Location.ToVector2();
            _position = _bounds.Location.ToVector2() - frame;
            _colliderBox = _bounds.Size.ToVector2() + frame * 2;
            _scaleText = PlayMode.BlockSize / _font.MeasureString("F").Y;
        }

        public void LoadContent(ContentManager content)
        {
            _whiteMask = content.Load<Texture2D>(@"Sprites\white_pixel");
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds - _lastKeyPressedTime < TimeToNextCheckInSecond)
                return;

            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && !PlayMode.HaveStartedExecutingCommands)
                _isSelected = _bounds.Contains(mouseState.Position);

            if (_isSelected && !PlayMode.HaveStartedExecutingCommands)
            {
                _frameColor = Color.LightYellow;

                if (keyboardState.GetPressedKeys().Length > 0)
                {
                    Keys keyPressed = keyboardState.GetPressedKeys()[0];
                    var input = "";

                    foreach (var key in keyboardState.GetPressedKeys())
                        input += (char)key;

                    if (keyPressed == Keys.Back && Text.Length > 0)
                    {
                        Text.Remove(Text.Length-1, 1);
                    }
                    else if (keyPressed == Keys.Space)
                    {
                        Text.Append(" ");
                    }
                    else if (keyPressed == Keys.Enter)
                    {
                        Text.Append("\n");
                    }
                    else if (input.All(x => char.IsLetterOrDigit(x)))
                    {
                        Text.Append(input);
                    }

                    _lastKeyPressedTime = gameTime.TotalGameTime.TotalSeconds;
                }
            }
            else
            {
                _frameColor = Color.White;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_whiteMask, _position, null, _frameColor, 0f, 
                Vector2.Zero, _colliderBox, SpriteEffects.None, 1f);
            
            spriteBatch.DrawString(_font, Text, _colliderText,
                _isSelected && !PlayMode.HaveStartedExecutingCommands ? Color.Red : Color.Black,
                0f, Vector2.Zero, _scaleText, SpriteEffects.None, 0f);
        }
    }
}
