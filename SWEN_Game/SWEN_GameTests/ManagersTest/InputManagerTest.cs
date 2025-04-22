using System;
using Microsoft.Xna.Framework.Input;
using SWEN_Game;
using Xunit;

namespace SWEN_GameTests.ManagersTest
{
    public class InputManagerTest
    {
        [Fact]
        public void Update_WKeyPressed_PlayerMovesUp()
        {
            // Arrange
            var player = new Player();
            Globals.Time = 1f;

            // Act
            InputManager.Update(player, new KeyboardState(Keys.W));

            // Assert
            Assert.Equal(-20, player.Position.Y);
            Assert.Equal(100, player.Position.X);
        }

        [Fact]
        public void Update_SKeyPressed_PlayerMovesDown()
        {
            // Arrange
            var player = new Player();
            Globals.Time = 1f;

            // Act
            InputManager.Update(player, new KeyboardState(Keys.S));

            // Assert
            Assert.Equal(220, player.Position.Y);
            Assert.Equal(100, player.Position.X);
        }

        [Fact]
        public void Update_AKeyPressed_PlayerMovesLeft()
        {
            // Arrange
            var player = new Player();
            Globals.Time = 1f;

            // Act
            InputManager.Update(player, new KeyboardState(Keys.A));

            // Assert
            Assert.Equal(100, player.Position.Y);
            Assert.Equal(-20, player.Position.X);
        }

        [Fact]
        public void Update_DKeyPressed_PlayerMovesRight()
        {
            // Arrange
            var player = new Player();
            Globals.Time = 1f;

            // Act
            InputManager.Update(player, new KeyboardState(Keys.D));

            // Assert
            Assert.Equal(100, player.Position.Y);
            Assert.Equal(220, player.Position.X);
        }
    }
}