using MyCalcCore.Operations;

class TestFinancial
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Testing Bitcoin price directly...");
            var price = Financial.GetBitcoinPrice();
            Console.WriteLine($"Bitcoin price: ${price}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
        }
    }
}
