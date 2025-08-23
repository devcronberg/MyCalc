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
        // Initialize Serilog for the Core project
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        // Initialize logger if not already done
        if (Log.Logger == Serilog.Core.Logger.None)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        Log.Information("Initializing Financial operations with crypto API configuration");

        _coinGeckoBaseUrl = configuration["CryptoApi:CoinGeckoBaseUrl"]
                           ?? "https://api.coingecko.com/api/v3/simple/price"; // Fallback

        var timeoutSeconds = configuration.GetValue<int>("CryptoApi:RequestTimeoutSeconds", 30);

        Log.Debug("Configuring CoinGecko API client with BaseUrl: {BaseUrl}, Timeout: {TimeoutSeconds}s",
            _coinGeckoBaseUrl, timeoutSeconds);

        _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyCalc-Calculator/1.0");
        // Add keep-alive to reuse connections
        _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
        // Set reasonable default headers
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        Log.Information("Financial operations initialized successfully");
    }

    [Discover("Bitcoin Price", "Gets current Bitcoin price in USD")]
    public static decimal GetBitcoinPrice()
    {
        Log.Information("Getting Bitcoin price from CoinGecko API");
        return GetCryptoPrice("bitcoin");
    }

    [Discover("Ethereum Price", "Gets current Ethereum price in USD")]
    public static decimal GetEthereumPrice()
    {
        Log.Information("Getting Ethereum price from CoinGecko API");
        return GetCryptoPrice("ethereum");
    }

    [Discover("Cardano Price", "Gets current Cardano (ADA) price in USD")]
    public static decimal GetCardanoPrice()
    {
        Log.Information("Getting Cardano price from CoinGecko API");
        return GetCryptoPrice("cardano");
    }

    [Discover("Solana Price", "Gets current Solana price in USD")]
    public static decimal GetSolanaPrice()
    {
        Log.Information("Getting Solana price from CoinGecko API");
        return GetCryptoPrice("solana");
    }

    [Discover("Polygon Price", "Gets current Polygon (MATIC) price in USD")]
    public static decimal GetPolygonPrice()
    {
        Log.Information("Getting Polygon price from CoinGecko API");
        return GetCryptoPrice("matic-network");
    }

    private static decimal GetCryptoPrice(string cryptoId)
    {
        const int maxRetries = 2; // Reduced retries to avoid hitting rate limits more
        var retryCount = 0;

        Log.Debug("Starting crypto price request for {CryptoId} with max {MaxRetries} retries", cryptoId, maxRetries);

        while (retryCount <= maxRetries)
        {
            try
            {
                var url = $"{_coinGeckoBaseUrl}?ids={cryptoId}&vs_currencies=usd";
                Log.Debug("Making API request to {Url} (attempt {AttemptNumber}/{MaxAttempts})", 
                    url, retryCount + 1, maxRetries + 1);

                // Add progressive delays to respect rate limits
                if (retryCount > 0)
                {
                    var delayMs = 5000 * retryCount; // 5s, 10s progression
                    Log.Debug("Waiting {DelayMs}ms before retry for rate limiting", delayMs);
                    Thread.Sleep(delayMs);
                }
                else
                {
                    // Always wait at least 1 second between any API calls
                    Thread.Sleep(1000);
                }

                var response = _httpClient.GetStringAsync(url).GetAwaiter().GetResult();
                Log.Debug("Received response from CoinGecko API for {CryptoId}: {ResponseLength} characters", 
                    cryptoId, response.Length);

                var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(response);

                if (data != null && data.ContainsKey(cryptoId) && data[cryptoId].ContainsKey("usd"))
                {
                    var price = data[cryptoId]["usd"];
                    Log.Information("Successfully retrieved {CryptoId} price: ${Price:F2}", cryptoId, price);
                    return price;
                }

                Log.Error("Invalid response structure from CoinGecko API for {CryptoId}", cryptoId);
                throw new InvalidOperationException($"Unable to get price for {cryptoId} - invalid response structure");
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("429") || ex.Message.Contains("Too Many Requests"))
            {
                retryCount++;
                Log.Warning("Rate limit hit for {CryptoId} (attempt {AttemptNumber}/{MaxAttempts}): {ErrorMessage}", 
                    cryptoId, retryCount, maxRetries + 1, ex.Message);
                
                if (retryCount > maxRetries)
                {
                    Log.Error("Rate limit exceeded for {CryptoId} after {MaxRetries} retries", cryptoId, maxRetries + 1);
                    throw new InvalidOperationException($"CoinGecko API rate limit exceeded for {cryptoId}. Please wait a moment before trying again. The free API has usage limits - consider waiting 1-2 minutes between multiple requests.");
                }
                // Continue to retry with longer delay
            }
            catch (HttpRequestException ex)
            {
                retryCount++;
                Log.Warning("Network error for {CryptoId} (attempt {AttemptNumber}/{MaxAttempts}): {ErrorMessage}", 
                    cryptoId, retryCount, maxRetries + 1, ex.Message);
                
                if (retryCount > maxRetries)
                {
                    Log.Error(ex, "Network error getting {CryptoId} price after {MaxRetries} attempts", cryptoId, maxRetries + 1);
                    throw new InvalidOperationException($"Network error getting {cryptoId} price after {maxRetries + 1} attempts. Please check your internet connection. Last error: {ex.Message}");
                }
                // Continue to retry
            }
            catch (TaskCanceledException ex)
            {
                retryCount++;
                Log.Warning("Request timeout for {CryptoId} (attempt {AttemptNumber}/{MaxAttempts}): {ErrorMessage}", 
                    cryptoId, retryCount, maxRetries + 1, ex.Message);
                
                if (retryCount > maxRetries)
                {
                    Log.Error(ex, "Request timeout for {CryptoId} after {MaxRetries} attempts", cryptoId, maxRetries + 1);
                    throw new InvalidOperationException($"Request timeout getting {cryptoId} price after {maxRetries + 1} attempts. The API might be slow or unavailable. Last error: {ex.Message}");
                }
                // Continue to retry
            }
            catch (JsonException ex)
            {
                // JSON errors are not retryable
                Log.Error(ex, "JSON parsing error for {CryptoId} response", cryptoId);
                throw new InvalidOperationException($"Invalid response format for {cryptoId} price. The API response format may have changed. Details: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Other unexpected errors are not retryable
                Log.Error(ex, "Unexpected error getting {CryptoId} price", cryptoId);
                throw new InvalidOperationException($"Unexpected error getting {cryptoId} price: {ex.Message}");
            }
        }

        Log.Error("Failed to get {CryptoId} price after all retry attempts due to rate limiting", cryptoId);
        throw new InvalidOperationException($"Failed to get {cryptoId} price after {maxRetries + 1} attempts due to rate limiting");
    }
}
