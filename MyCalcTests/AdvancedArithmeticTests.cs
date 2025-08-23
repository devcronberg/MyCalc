using MyCalcCore.Operations;

namespace MyCalcTests
{
    public class AdvancedArithmeticTests
    {
        private readonly AdvancedArithmetic _arithmetic = new();

        [Theory]
        [InlineData(2.0, 4.0)]
        [InlineData(3.0, 9.0)]
        [InlineData(5.5, 30.25)]
        [InlineData(10.0, 100.0)]
        [InlineData(0.5, 0.25)]
        [InlineData(1.0, 1.0)]
        [InlineData(0.0, 0.0)]
        [InlineData(12.5, 156.25)]
        [InlineData(0.1, 0.01)]
        [InlineData(100.0, 10000.0)]
        public void Square_PositiveNumbers_ReturnsCorrectSquare(decimal number, decimal expected)
        {
            // Act
            decimal result = _arithmetic.Square(number);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Square_NegativeNumber_ReturnsPositiveSquare()
        {
            // Arrange
            decimal number = -5.0m;
            decimal expected = 25.0m;

            // Act
            decimal result = _arithmetic.Square(number);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Square_VerySmallNumber_ReturnsCorrectSquare()
        {
            // Arrange
            decimal number = 0.01m;
            decimal expected = 0.0001m;

            // Act
            decimal result = _arithmetic.Square(number);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Square_LargeNumber_ReturnsCorrectSquare()
        {
            // Arrange
            decimal number = 1000m;
            decimal expected = 1000000m;

            // Act
            decimal result = _arithmetic.Square(number);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
