using MyCalcCore.Attributes;
using Serilog;

namespace MyCalcCore.Operations
{
    [OperationCategory("Basic Arithmetic", "Fundamental mathematical operations", 1)]
    public class BasicArithmetic
    {

        [Discover("Add", "Adds two decimal numbers", "First number", "Second number")]
        public decimal Add(decimal a, decimal b)
        {
            Log.Debug("Performing addition: {A} + {B}", a, b);
            var result = a + b;
            Log.Debug("Addition result: {A} + {B} = {Result}", a, b, result);
            return result;
        }

        [Discover("Subtract", "Subtracts second number from first", "Number to subtract from", "Number to subtract")]
        public decimal Subtract(decimal a, decimal b)
        {
            Log.Debug("Performing subtraction: {A} - {B}", a, b);
            var result = a - b;
            Log.Debug("Subtraction result: {A} - {B} = {Result}", a, b, result);
            return result;
        }

    }
}
