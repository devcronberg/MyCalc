using MyCalcCore.Attributes;

namespace MyCalcCore.Operations
{
    [OperationCategory("Basic Arithmetic", "Fundamental mathematical operations", 1)]
    public class BasicArithmetic
    {

        [Discover("Add", "Adds two decimal numbers", "First number", "Second number")]
        public decimal Add(decimal a, decimal b)
        {
            return a + b;
        }

        [Discover("Subtract", "Subtracts second number from first", "Number to subtract from", "Number to subtract")]
        public decimal Subtract(decimal a, decimal b)
        {
            return a - b;
        }

    }
}
