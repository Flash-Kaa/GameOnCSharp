using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameOnCSharp
{
    public class Button : IGameObject
    {
        public Rectangle ButtonCollider { get; set; }

        Texture2D _texture;
        Action<bool> _onClick;

        private bool _isPressed;
        private Color _color;
        private double _pressedTime = -1;
        private double _maxSecondDelay = 0.3;

        public Button(Texture2D sprite, Rectangle buttonCollider, Action<bool> onClick)
        {
            _texture = sprite;
            ButtonCollider = buttonCollider;
            _onClick = onClick;

            _color = Color.White;
        }

        public void LoadContent(ContentManager content) { }

        public bool EnterButton()
        {
            return Mouse.GetState().X >= ButtonCollider.Location.X 
                && Mouse.GetState().Y >= ButtonCollider.Location.Y 
                && Mouse.GetState().X <= ButtonCollider.Location.X + ButtonCollider.Width 
                && Mouse.GetState().Y <= ButtonCollider.Location.Y + ButtonCollider.Height;
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
            spriteBatch.Draw(_texture, ButtonCollider, _color);
        }
    }
}