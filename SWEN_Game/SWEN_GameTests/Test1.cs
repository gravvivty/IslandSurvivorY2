namespace SWEN_GameTests
{
    using SWEN_Game;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MyClassTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void Euklid_2PosInts_GreatCommonDivisor()
        {
            // Arrange
            MyClass myClass = new MyClass();
            int x = 10;
            int y = 15;
            int expected = 5;

            // Act
            int actual = myClass.Euklid(x, y);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(10, 15, 5)]
        [DataRow(10, 0, 10)]
        [DataRow(0, 10, 10)]
        [DataRow(-10, 15, 5)]
        [DataRow(10, -15, 5)]
        public void Euklid_2Ints_GreatCommonDivisor(int x, int y, int expected)
        {
            // Arrange
            MyClass myClass = new MyClass();

            // Act
            int actual = myClass.Euklid(x, y);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Euklid_YIs0_X()
        {
            // Arrange
            MyClass myClass = new MyClass();
            int x = 10;
            int y = 0;
            int expected = 10;

            // Act
            int actual = myClass.Euklid(x, y);

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void Euklid_XIs0_Y()
        {
            // Arrange
            MyClass myClass = new MyClass();
            int x = 0;
            int y = 10;
            int expected = 10;

            // Act
            int actual = myClass.Euklid(x, y);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}