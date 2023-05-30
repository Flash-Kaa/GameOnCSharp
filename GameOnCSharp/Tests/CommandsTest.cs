using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameOnCSharp.Tests
{
    [TestFixture]
    class CommandsTest
    {
        [SetUp]
        public void SetUp()
        {
            var t = typeof(PlayMode).GetProperty("BlockSize");

            t.SetValue(null, 1);
        }


        [TestCase("doWn", 0, 1)]
        [TestCase("doWn right Up LEFT",
            0, 1,
            1, 0,
            0, -1,
            -1, 0)]
        [TestCase("doWn x6", 
            0, 1,   0, 1,
            0, 1,   0, 1,
            0, 1,   0, 1)]
        [TestCase("doWn right X1 Up LEFT x2",
            0, 1,
            1, 0,
            0, -1,
            -1, 0,
            -1, 0)]
        [TestCase("uP x0", 0, -1)]
        [TestCase("dOWn x-1", 0, 1)]
        [TestCase("dOWn x-111", 0, 1)]
        [TestCase("doWn x12",
            0, 1,   0, 1,   0, 1,   0, 1,
            0, 1,   0, 1,   0, 1,   0, 1,
            0, 1,   0, 1,   0, 1,   0, 1)]
        public void ExecuteCommands(string commands, params int[] expectedCoordinates)
        {
            Commands.SetCommands(commands);
            var expectedDirection = ConvertToVector(expectedCoordinates).ToList();

            var actualDirections = (List<Vector2>)typeof(Commands)
                .GetField("_directions", 
                    BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(null);

            Assert.AreEqual(expectedDirection.Count, actualDirections.Count);
            
            for(int i = 0; i < expectedDirection.Count; i++)
                Assert.AreEqual(expectedDirection[i], actualDirections[i]);
        }

        private IEnumerable<Vector2> ConvertToVector(int[] expectedCoordinates)
        {
            if (expectedCoordinates.Length % 2 != 0)
                throw new ArgumentException();

            for(int i = 0; i < expectedCoordinates.Length; i += 2)
                yield return new Vector2(expectedCoordinates[i], expectedCoordinates[i + 1]);
        }
    }
}
