using MyCalcCore.Attributes;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace MyCalcCore.Operations;

[OperationCategory("Financial", "Financial prices and calculations", 4)]
public class Financial
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly string _coinGeckoBaseUrl;

    static Financial()
    {
        // Read configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        _coinGeckoBaseUrl = configuration["CryptoApi:CoinGeckoBaseUrl"]
                           ?? "https://api.coingecko.com/api/v3/simple/price"; // Fallback

        var timeoutSeconds = configuration.GetValue<int>("CryptoApi:RequestTimeoutSeconds", 30);

        _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyCalc-Calculator/1.0");
        // Add keep-alive to reuse connections
        _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
        // Set reasonable default headers
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    [Discover("Bitcoin Price", "Gets current Bitcoin price in USD")]
    public static decimal GetBitcoinPrice()
    {
        return GetCryptoPrice("bitcoin");
    }

    [Discover("Ethereum Price", "Gets current Ethereum price in USD")]
    public static decimal GetEthereumPrice()
    {
        return GetCryptoPrice("ethereum");
    }

    private static decimal GetCryptoPrice(string cryptoId)
    {
        const int maxRetries = 2; // Reduced retries to avoid hitting rate limits more
        var retryCount = 0;

        while (retryCount <= maxRetries)
        {
            try
            {
                var url = $"{_coinGeckoBaseUrl}?ids={cryptoId}&vs_currencies=usd";

                // Add progressive delays to respect rate limits
                if (retryCount > 0)
                {
                    var delayMs = 5000 * retryCount; // 5s, 10s progression
                    Thread.Sleep(delayMs);
                }
                else
                {
                    // Always wait at least 1 second between any API calls
                    Thread.Sleep(1000);
                }

                var response = _httpClient.GetStringAsync(url).GetAwaiter().GetResult();

                var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(response);

                if (data != null && data.ContainsKey(cryptoId) && data[cryptoId].ContainsKey("usd"))
                {
                    return data[cryptoId]["usd"];
                }

                throw new InvalidOperationException($"Unable to get price for {cryptoId} - invalid response structure");
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("429") || ex.Message.Contains("Too Many Requests"))
            {
                retryCount++;
                if (retryCount > maxRetries)
                {
                    throw new InvalidOperationException($"CoinGecko API rate limit exceeded for {cryptoId}. Please wait a moment before trying again. The free API has usage limits - consider waiting 1-2 minutes between multiple requests.");
                }
                // Continue to retry with longer delay
            }
            catch (HttpRequestException ex)
            {
                retryCount++;
                if (retryCount > maxRetries)
                {
                    throw new InvalidOperationException($"Network error getting {cryptoId} price after {maxRetries + 1} attempts. Please check your internet connection. Last error: {ex.Message}");
                }
                // Continue to retry
            }
            catch (TaskCanceledException ex)
            {
                retryCount++;
                if (retryCount > maxRetries)
                {
                    throw new InvalidOperationException($"Request timeout getting {cryptoId} price after {maxRetries + 1} attempts. The API might be slow or unavailable. Last error: {ex.Message}");
                }
                // Continue to retry
            }
            catch (JsonException ex)
            {
                // JSON errors are not retryable
                throw new InvalidOperationException($"Invalid response format for {cryptoId} price. The API response format may have changed. Details: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Other unexpected errors are not retryable
                throw new InvalidOperationException($"Unexpected error getting {cryptoId} price: {ex.Message}");
            }
        }

        throw new InvalidOperationException($"Failed to get {cryptoId} price after {maxRetries + 1} attempts due to rate limiting");
    }
}
