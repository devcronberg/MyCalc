using MyCalcCore.Operations;
using Xunit;

namespace MyCalcTests;

public class FinancialTests
{
    [Fact]
    public void GetBitcoinPrice_ReturnsPositiveValue()
    {
        // Act - This will make actual API call
        var result = Financial.GetBitcoinPrice();

        // Assert
        Assert.True(result > 0, "Bitcoin price should be positive");
        Assert.True(result < 1000000, "Bitcoin price should be reasonable (less than $1M)");
    }

    [Fact]
    public void GetEthereumPrice_ReturnsPositiveValue()
    {
        // Act - This will make actual API call
        var result = Financial.GetEthereumPrice();

        // Assert
        Assert.True(result > 0, "Ethereum price should be positive");
        Assert.True(result < 100000, "Ethereum price should be reasonable (less than $100K)");
    }

    // Note: These tests make actual HTTP calls to CoinGecko API
    // The methods are now static, so no constructor testing needed
    // In a production environment, consider:
    // 1. Mocking the HTTP client for unit tests
    // 2. Creating separate integration tests for API calls
    // 3. Using test categories to separate unit tests from integration tests
}
