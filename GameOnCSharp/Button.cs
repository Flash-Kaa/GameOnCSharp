using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameOnCSharp
{
    public class Button : IGameObject
    {
        Texture2D _texture;
        Rectangle _buttonCollider;
        Action<bool> _onClick;

        private bool _isPressed;
        private Color _color;
        private double _pressedTime = -1;
        private double _maxSecondDelay = 0.3;

        public Button(Texture2D sprite, Rectangle buttonCollider, Action<bool> onClick)
        {
            _texture = sprite;
            _buttonCollider = buttonCollider;
            _onClick = onClick;

            _color = Color.White;
        }

        public void LoadContent(ContentManager content) { }

        public bool EnterButton()
        {
            return Mouse.GetState().X >= _buttonCollider.Location.X 
                && Mouse.GetState().Y >= _buttonCollider.Location.Y 
                && Mouse.GetState().X <= _buttonCollider.Location.X + _buttonCollider.Width 
                && Mouse.GetState().Y <= _buttonCollider.Location.Y + _buttonCollider.Height;
        }

        public void Update(GameTime gameTime)
        {
            // Проверяем, находится ли курсор мыши над кнопкой
            if (EnterButton() && Mouse.GetState().LeftButton == ButtonState.Pressed && !_isPressed)
            {
                _isPressed = true;
                _color = Color.Gray;
                _pressedTime = gameTime.TotalGameTime.TotalSeconds;

                // Совершаем заданное действие
                _onClick(true);
            }

            if(gameTime.TotalGameTime.TotalSeconds - _pressedTime >= _maxSecondDelay)
            {
                _isPressed = false;
                _color = Color.White;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _buttonCollider, _color);
        }
    }
}