using Xunit;
using SWEN_Game;

namespace SWEN_GameTests
{
    public class SpriteCalculatorTest
    {
        [Theory]
        [InlineData("House", 324)]
        [InlineData("Tree_Big", 264)]
        [InlineData("Tree_Small", 237)]
        [InlineData("Lantern", 213)]
        [InlineData("Stump", 81)]
        [InlineData("Fence_Big", 10)]
        [InlineData("Log", 241)]
        [InlineData("Bridge", 3)]
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