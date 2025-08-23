using MyCalcCore.Operations;
using Serilog;

class TestFinancial
{
    static void Main()
    {
        // Initialize Serilog for test
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .CreateLogger();

        try
        {
            Log.Information("Testing Bitcoin price directly...");
            var price = Financial.GetBitcoinPrice();
            Log.Information("Bitcoin price: ${Price:F2}", price);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while testing Bitcoin price");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
