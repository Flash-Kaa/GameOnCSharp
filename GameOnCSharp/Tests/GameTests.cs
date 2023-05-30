using NUnit.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using static Microsoft.Xna.Framework.Graphics.GraphicsAdapter;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;

namespace GameOnCSharp
{
    public class TestGame : Game1
    {
        private GameTime gameTime = new GameTime();

        public TestGame() { }

        public void PublicInitialize()
        {
            Initialize();
        }

        public void PublicUpdate()
        {
            Update(gameTime);
        }

        public void PublicDraw()
        {
            Draw(gameTime);
        }

        public void ChangeGraphics(GraphicsDeviceManager gdm)
        {
            Graphics = gdm;
        }
    }

    //[TestFixture]
    class GameTests
    {
        private TestGame _game;
        private MouseState _mouseState;

        //[SetUp]
        public void Setup()
        {
            _game = new TestGame();

            //_game.ChangeGraphics(new GraphicsDeviceManager(_game));
            _game.PublicInitialize();


            Game1.CurrentScene = Scene.Play;
        }

        //[Test]
        public void CommandTest()
        {
            _game.PublicUpdate();
            ClearMaze();
            _game.PublicUpdate();
            WriteInTextBox("doWn");

            Assert.AreEqual(Scene.Play, Game1.CurrentScene);
        }

        private static void ClearMaze()
        {
            typeof(Maze)
                .GetField("_components", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static)
                .SetValue(null, new List<IGameObject>());
        }

        private static void WriteInTextBox(string text)
        {
            var r = (List<IGameObject>)typeof(PlayMode)
                .GetField("_components", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static)
                .GetValue(null);
            var tb = r[0] as TextBox;
            tb.GetType().GetField("Text", BindingFlags.Instance | BindingFlags.Public).SetValue(tb, text);
        }
    }
}
