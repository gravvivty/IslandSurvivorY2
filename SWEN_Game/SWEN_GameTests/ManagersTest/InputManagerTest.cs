using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SWEN_Game;
using Xunit;

namespace SWEN_GameTests.ManagersTest
{
    public class InputManagerTest
    {
        [Fact]
        public void Update_WKeyPressed_PlayerMovesUp()
        {
            Globals.Collisions = new List<Rectangle>();
            var player = new Player();
            Globals.Time = 1f;
            InputManager.Update(player, new KeyboardState(Keys.W));
            Assert.Equal(320, player.Position.Y);
            Assert.Equal(450, player.Position.X);
        }

        [Fact]
        public void Update_SKeyPressed_PlayerMovesDown()
        {
            Globals.Collisions = new List<Rectangle>();
            var player = new Player();
            Globals.Time = 1f;
            InputManager.Update(player, new KeyboardState(Keys.S));
            Assert.Equal(580, player.Position.Y);
            Assert.Equal(450, player.Position.X);
        }

        [Fact]
        public void Update_AKeyPressed_PlayerMovesLeft()
        {
            Globals.Collisions = new List<Rectangle>();
            var player = new Player();
            Globals.Time = 1f;
            InputManager.Update(player, new KeyboardState(Keys.A));
            Assert.Equal(450, player.Position.Y);
            Assert.Equal(320, player.Position.X);
        }

        [Fact]
        public void Update_DKeyPressed_PlayerMovesRight()
        {
            Globals.Collisions = new List<Rectangle>();
            var player = new Player();
            Globals.Time = 1f;
            InputManager.Update(player, new KeyboardState(Keys.D));
            Assert.Equal(450, player.Position.Y);
            Assert.Equal(580, player.Position.X);
        }
    }
}