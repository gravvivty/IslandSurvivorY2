using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using SWEN_Game;

namespace SWEN_GameTests.ManagersTest
{
    [TestClass]
    public class InputManagerTest
    {
        [TestMethod]

        public void Update_WKeyPressed_PlayerMovesUp()
        {
            // Arrange
            var player = new Player();
            Globals.Time = 1f;
            // Set a fixed delta time for testing
            InputManager.Update(player, new KeyboardState(Keys.W));

            // Assert
            Assert.AreEqual(-20, player.Position.Y, "Player sollte sich nach oben bewegen.");
            Assert.AreEqual(100, player.Position.X, "Player sollte sich nicht nach links oder rechts bewegen.");
        }
        [TestMethod]
        public void Update_SKeyPressed_PlayerMovesDown()
        {
            // Arrange
            var player = new Player();
            Globals.Time = 1f;

            InputManager.Update(player, new KeyboardState(Keys.S));
            // Assert
            Assert.AreEqual(220, player.Position.Y, "Player sollte sich nach unten bewegen.");
            Assert.AreEqual(100, player.Position.X, "Player sollte sich nicht nach links oder rechts bewegen.");
        }

    }
}

