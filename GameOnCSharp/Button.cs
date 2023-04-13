using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOnCSharp
{
    public class Button : IGameObject
    {
        Texture2D _texture;
        Point _buttonPosition;

        private bool _isPressed;
        private Color _color;
        private double _pressedTime = -1;
        private double _maxSecondDelay = 0.3;

        public Button(Point buttonPosition)
        {
            _buttonPosition = buttonPosition;
            _color = Color.White;
        }

        public bool EnterButton()
        {
            return Mouse.GetState().X >= _buttonPosition.X &&
                    Mouse.GetState().Y >= _buttonPosition.Y &&
                    Mouse.GetState().X <= _buttonPosition.X + _texture.Width &&
                    Mouse.GetState().Y <= _buttonPosition.Y + _texture.Height;
        }

        public void Update(GameTime gameTime)
        {
            // Проверяем, находится ли курсор мыши над кнопкой
            if (EnterButton() && Mouse.GetState().LeftButton == ButtonState.Pressed && !_isPressed)
            {
                _isPressed = true;
                _color = Color.Gray;
                _pressedTime = gameTime.TotalGameTime.TotalSeconds;

                Game1.HaveStartedExecutingCommands = true;
            }

            if(gameTime.TotalGameTime.TotalSeconds - _pressedTime >= _maxSecondDelay)
            {
                _isPressed = false;
                _color = Color.White;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(_buttonPosition, new Point(_texture.Width, _texture.Height)), _color);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>(@"Sprites/ButtonPlay");
        }
    }
}