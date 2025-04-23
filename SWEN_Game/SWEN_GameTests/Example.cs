using System.Diagnostics.CodeAnalysis;
using Xunit;
using SWEN_Game;

namespace SWEN_GameTests
{
    [ExcludeFromCodeCoverage]
    public class MyClassTest : IDisposable
    {
        public MyClassTest()
        {
            // Equivalent to [TestInitialize]
        }

        public void Dispose()
        {
            // Equivalent to [TestCleanup]
        }

        [Fact]
        public void Euklid_2PosInts_GreatCommonDivisor()
        {
            // Arrange
            var myClass = new MyClass();
            int x = 10;
            int y = 15;
            int expected = 5;

            // Act
            int actual = myClass.Euklid(x, y);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, 15, 5)]
        [InlineData(10, 0, 10)]
        [InlineData(0, 10, 10)]
        [InlineData(-10, 15, 5)]
        [InlineData(10, -15, 5)]
        public void Euklid_2Ints_GreatCommonDivisor(int x, int y, int expected)
        {
            // Arrange
            var myClass = new MyClass();

            // Act
            int actual = myClass.Euklid(x, y);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Euklid_YIs0_X()
        {
            // Arrange
            var myClass = new MyClass();
            int x = 10;
            int y = 0;
            int expected = 10;

            // Act
            int actual = myClass.Euklid(x, y);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Euklid_XIs0_Y()
        {
            // Arrange
            var myClass = new MyClass();
            int x = 0;
            int y = 10;
            int expected = 10;

            // Act
            int actual = myClass.Euklid(x, y);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}