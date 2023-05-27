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

        private const double MaxSecondDelay = 0.3;

        private Color _color;
        private bool _isPressed;
        private Texture2D _texture;
        private double _pressedTime;
        private Action<object> _onClick;

        public Button(Texture2D sprite, Rectangle buttonCollider, Action<object> onClick)
        {
            _texture = sprite;
            ButtonCollider = buttonCollider;
            _onClick = onClick;

            _color = Color.White;
            _pressedTime = -1;
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
            if (EnterButton() && Mouse.GetState().LeftButton == ButtonState.Pressed && !_isPressed)
            {
                _isPressed = true;
                _color = Color.Gray;
                _pressedTime = gameTime.TotalGameTime.TotalSeconds;

                _onClick(null);
            }

            if(gameTime.TotalGameTime.TotalSeconds - _pressedTime >= MaxSecondDelay)
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