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
            Assert.AreEqual(-20, player.Position.Y);
            Assert.AreEqual(100, player.Position.X);
        }
        [TestMethod]
        public void Update_SKeyPressed_PlayerMovesDown()
        {
            // Arrange
            var player = new Player();
            Globals.Time = 1f;

            InputManager.Update(player, new KeyboardState(Keys.S));
            // Assert
            Assert.AreEqual(220, player.Position.Y);
            Assert.AreEqual(100, player.Position.X);
        }
        [TestMethod]
        public void Update_AKeyPressed_PlayerMovesLeft()
        {
            // Arrange
            var player = new Player();
            Globals.Time = 1f;
            InputManager.Update(player, new KeyboardState(Keys.A));
            // Assert
            Assert.AreEqual(100, player.Position.Y);
            Assert.AreEqual(-20, player.Position.X);
        }

        [TestMethod]
        public void Update_DKeyPressed_PlayerMovesRight()
        {
            // Arrange
            var player = new Player();
            Globals.Time = 1f;
            InputManager.Update(player, new KeyboardState(Keys.D));
            // Assert
            Assert.AreEqual(100, player.Position.Y);
            Assert.AreEqual(220, player.Position.X);
        }
    }
}

