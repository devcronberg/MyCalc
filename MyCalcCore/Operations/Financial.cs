using MyCalcCore.Attributes;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace MyCalcCore.Operations;

[OperationCategory("Financial", "Financial calculations and cryptocurrency prices", 4)]
public class Financial
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly string _coinGeckoBaseUrl;

    static Financial()
    {
        Log.Debug("Initializing Financial operations class");
        
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
        
        Log.Information("Financial operations initialized with base URL: {BaseUrl}, timeout: {TimeoutSeconds}s", 
            _coinGeckoBaseUrl, timeoutSeconds);
    }

    [Discover("Bitcoin Price", "Gets current Bitcoin price in USD")]
    public static decimal GetBitcoinPrice()
    {
        Log.Information("Fetching Bitcoin price");
        return GetCryptoPrice("bitcoin");
    }

    [Discover("Ethereum Price", "Gets current Ethereum price in USD")]
    public static decimal GetEthereumPrice()
    {
        Log.Information("Fetching Ethereum price");
        return GetCryptoPrice("ethereum");
    }

    [Discover("Cardano Price", "Gets current Cardano (ADA) price in USD")]
    public static decimal GetCardanoPrice()
    {
        Log.Information("Fetching Cardano price");
        return GetCryptoPrice("cardano");
    }

    [Discover("Solana Price", "Gets current Solana price in USD")]
    public static decimal GetSolanaPrice()
    {
        Log.Information("Fetching Solana price");
        return GetCryptoPrice("solana");
    }

    [Discover("Polygon Price", "Gets current Polygon (MATIC) price in USD")]
    public static decimal GetPolygonPrice()
    {
        Log.Information("Fetching Polygon price");
        return GetCryptoPrice("matic-network");
    }

    private static decimal GetCryptoPrice(string cryptoId)
    {
        Log.Debug("Starting crypto price fetch for '{CryptoId}'", cryptoId);
        const int maxRetries = 2; // Reduced retries to avoid hitting rate limits more
        var retryCount = 0;

        while (retryCount <= maxRetries)
        {
            try
            {
                var url = $"{_coinGeckoBaseUrl}?ids={cryptoId}&vs_currencies=usd";
                Log.Debug("Making API request to: {ApiUrl} (attempt {AttemptNumber})", url, retryCount + 1);

                // Add progressive delays to respect rate limits
                if (retryCount > 0)
                {
                    var delayMs = 5000 * retryCount; // 5s, 10s progression
                    Log.Debug("Applying retry delay of {DelayMs}ms for attempt {AttemptNumber}", delayMs, retryCount + 1);
                    Thread.Sleep(delayMs);
                }
                else
                {
                    // Always wait at least 1 second between any API calls
                    Thread.Sleep(1000);
                }

                var response = _httpClient.GetStringAsync(url).GetAwaiter().GetResult();
                Log.Debug("Received API response for '{CryptoId}': {ResponseLength} characters", 
                    cryptoId, response?.Length ?? 0);

                var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(response);

                if (data != null && data.ContainsKey(cryptoId) && data[cryptoId].ContainsKey("usd"))
                {
                    var price = data[cryptoId]["usd"];
                    Log.Information("Successfully retrieved price for '{CryptoId}': ${Price}", cryptoId, price);
                    return price;
                }

                var errorMessage = $"Unable to get price for {cryptoId} - invalid response structure";
                Log.Error("Invalid API response structure for '{CryptoId}': {ErrorMessage}", cryptoId, errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("429") || ex.Message.Contains("Too Many Requests"))
            {
                retryCount++;
                Log.Warning("Rate limit exceeded for '{CryptoId}' on attempt {AttemptNumber}: {ErrorMessage}", 
                    cryptoId, retryCount, ex.Message);
                if (retryCount > maxRetries)
                {
                    var finalError = $"CoinGecko API rate limit exceeded for {cryptoId}. Please wait a moment before trying again. The free API has usage limits - consider waiting 1-2 minutes between multiple requests.";
                    Log.Error("Final rate limit error for '{CryptoId}': {ErrorMessage}", cryptoId, finalError);
                    throw new InvalidOperationException(finalError);
                }
                // Continue to retry with longer delay
            }
            catch (HttpRequestException ex)
            {
                retryCount++;
                Log.Warning("Network error for '{CryptoId}' on attempt {AttemptNumber}: {ErrorMessage}", 
                    cryptoId, retryCount, ex.Message);
                if (retryCount > maxRetries)
                {
                    var finalError = $"Network error getting {cryptoId} price after {maxRetries + 1} attempts. Please check your internet connection. Last error: {ex.Message}";
                    Log.Error("Final network error for '{CryptoId}': {ErrorMessage}", cryptoId, finalError);
                    throw new InvalidOperationException(finalError);
                }
                // Continue to retry
            }
            catch (TaskCanceledException ex)
            {
                retryCount++;
                Log.Warning("Request timeout for '{CryptoId}' on attempt {AttemptNumber}: {ErrorMessage}", 
                    cryptoId, retryCount, ex.Message);
                if (retryCount > maxRetries)
                {
                    var finalError = $"Request timeout getting {cryptoId} price after {maxRetries + 1} attempts. The API might be slow or unavailable. Last error: {ex.Message}";
                    Log.Error("Final timeout error for '{CryptoId}': {ErrorMessage}", cryptoId, finalError);
                    throw new InvalidOperationException(finalError);
                }
                // Continue to retry
            }
            catch (JsonException ex)
            {
                // JSON errors are not retryable
                var errorMessage = $"Invalid response format for {cryptoId} price. The API response format may have changed. Details: {ex.Message}";
                Log.Error(ex, "JSON parsing error for '{CryptoId}': {ErrorMessage}", cryptoId, errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
            catch (Exception ex)
            {
                // Other unexpected errors are not retryable
                var errorMessage = $"Unexpected error getting {cryptoId} price: {ex.Message}";
                Log.Error(ex, "Unexpected error for '{CryptoId}': {ErrorMessage}", cryptoId, errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
        }

        var finalRateLimitError = $"Failed to get {cryptoId} price after {maxRetries + 1} attempts due to rate limiting";
        Log.Error("Final rate limit failure for '{CryptoId}': {ErrorMessage}", cryptoId, finalRateLimitError);
        throw new InvalidOperationException(finalRateLimitError);
    }
}
