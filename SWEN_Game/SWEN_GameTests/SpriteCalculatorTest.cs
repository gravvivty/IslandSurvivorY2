using Xunit;
using SWEN_Game;
using SWEN_Game._Utils;

namespace SWEN_GameTests
{
    public class SpriteCalculatorTest
    {
        [Theory]
        [InlineData("House", 0)]
        [InlineData("Tree_Big", 0)]
        [InlineData("Tree_Small", 0)]
        [InlineData("Lantern", 0)]
        [InlineData("Stump", 0)]
        [InlineData("Fence_Big", 0)]
        [InlineData("Log", 0)]
        [InlineData("Bridge", 0)]
        [InlineData("Unknown", 0)]
        public void GetAnchorTitleID_ReturnsCorrectID(string enumName, int expectedID)
        {
            // Arrange
            var spriteCalculator = new SpriteCalculator(null, null);

            // Act
            int actualID = spriteCalculator.GetAnchorTileID(enumName);

            // Assert
            Assert.Equal(expectedID, actualID);
        }
    }
}