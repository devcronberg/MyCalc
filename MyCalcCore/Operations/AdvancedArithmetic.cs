using MyCalcCore.Attributes;
using Serilog;

namespace MyCalcCore.Operations
{
    [OperationCategory("Advanced Math", "Advanced mathematical functions", 2)]
    public class AdvancedArithmetic
    {
        [Discover("Square", "Calculates the square of a number", "Number to square")]
        public decimal Square(decimal number)
        {
            Log.Debug("Calculating square of {Number}", number);
            var result = number * number;
            Log.Debug("Square result: {Number}² = {Result}", number, result);
            return result;
        }
    }
}
