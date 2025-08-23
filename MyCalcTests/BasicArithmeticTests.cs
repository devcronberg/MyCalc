using MyCalcCore.Operations;

namespace MyCalcTests
{
    public class BasicArithmeticTests
    {
        private readonly BasicArithmetic _arithmetic = new();

        [Theory]
        [InlineData(5.5, 3.2, 8.7)]
        [InlineData(1.0, 2.0, 3.0)]
        [InlineData(10.25, 15.75, 26.0)]
        [InlineData(0.1, 0.2, 0.3)]
        [InlineData(100.50, 200.25, 300.75)]
        [InlineData(7.777, 2.223, 10.0)]
        [InlineData(50.0, 0.01, 50.01)]
        [InlineData(999.99, 0.01, 1000.0)]
        [InlineData(123.456, 876.544, 1000.0)]
        [InlineData(0.05, 0.05, 0.1)]
        public void Add_TwoPositiveNumbers_ReturnsSum(decimal a, decimal b, decimal expected)
        {
            // Act
            decimal result = _arithmetic.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Add_PositiveAndNegativeNumbers_ReturnsCorrectResult()
        {
            // Arrange
            decimal a = 10.0m;
            decimal b = -3.5m;
            decimal expected = 6.5m;

            // Act
            decimal result = _arithmetic.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Add_TwoNegativeNumbers_ReturnsNegativeSum()
        {
            // Arrange
            decimal a = -2.5m;
            decimal b = -7.3m;
            decimal expected = -9.8m;

            // Act
            decimal result = _arithmetic.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Add_ZeroAndNumber_ReturnsNumber()
        {
            // Arrange
            decimal a = 0m;
            decimal b = 42.75m;
            decimal expected = 42.75m;

            // Act
            decimal result = _arithmetic.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Add_VeryLargeNumbers_ReturnsCorrectSum()
        {
            // Arrange
            decimal a = 999999999999999999999999999m;
            decimal b = 1m;
            decimal expected = 1000000000000000000000000000m;

            // Act
            decimal result = _arithmetic.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(10.0, 3.0, 7.0)]
        [InlineData(5.5, 2.2, 3.3)]
        [InlineData(100.75, 25.25, 75.5)]
        [InlineData(1000.0, 999.99, 0.01)]
        [InlineData(50.0, 0.01, 49.99)]
        [InlineData(7.777, 2.777, 5.0)]
        [InlineData(123.456, 23.456, 100.0)]
        [InlineData(0.3, 0.1, 0.2)]
        [InlineData(15.75, 10.25, 5.5)]
        [InlineData(200.0, 50.0, 150.0)]
        public void Subtract_TwoPositiveNumbers_ReturnsCorrectResult(decimal a, decimal b, decimal expected)
        {
            // Act
            decimal result = _arithmetic.Subtract(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Subtract_PositiveFromNegative_ReturnsNegativeResult()
        {
            // Arrange
            decimal a = -5.0m;
            decimal b = 3.0m;
            decimal expected = -8.0m;

            // Act
            decimal result = _arithmetic.Subtract(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Subtract_NegativeFromPositive_ReturnsPositiveResult()
        {
            // Arrange
            decimal a = 10.0m;
            decimal b = -5.0m;
            decimal expected = 15.0m;

            // Act
            decimal result = _arithmetic.Subtract(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Subtract_TwoNegativeNumbers_ReturnsCorrectResult()
        {
            // Arrange
            decimal a = -2.5m;
            decimal b = -7.0m;
            decimal expected = 4.5m;

            // Act
            decimal result = _arithmetic.Subtract(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Subtract_SameNumbers_ReturnsZero()
        {
            // Arrange
            decimal a = 42.75m;
            decimal b = 42.75m;
            decimal expected = 0m;

            // Act
            decimal result = _arithmetic.Subtract(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Subtract_FromZero_ReturnsNegative()
        {
            // Arrange
            decimal a = 0m;
            decimal b = 25.5m;
            decimal expected = -25.5m;

            // Act
            decimal result = _arithmetic.Subtract(a, b);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}