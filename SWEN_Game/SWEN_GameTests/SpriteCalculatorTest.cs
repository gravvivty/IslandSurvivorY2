
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWEN_Game;

namespace SWEN_GameTests
{
    [TestClass]
    public class SpriteCalculatorTest
    {
        [TestMethod]
        [DataRow("House", 324)]
        [DataRow("Tree_Big", 264)]
        [DataRow("Tree_Small", 237)]
        [DataRow("Lantern", 213)]
        [DataRow("Stump", 81)]
        [DataRow("Fence_Big", 10)]
        [DataRow("Log", 241)]
        [DataRow("Bridge", 3)]
        [DataRow("Unknown", 0)]
        public void GetAnchorTitleID_ReturnsCorrectID(string enumName, int expectedID)
        {
            // Arrange
            SpriteCalculator spriteCalculator = new SpriteCalculator(null, null);
            // Act
            int actualID = spriteCalculator.GetAnchorTileID(enumName);
            // Assert
            Assert.AreEqual(expectedID, actualID);
        }
    }
}
