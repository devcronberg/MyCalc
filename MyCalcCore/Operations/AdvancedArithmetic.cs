using MyCalcCore.Attributes;

namespace MyCalcCore.Operations
{
    [OperationCategory("Advanced Math", "Advanced mathematical functions", 2)]
    public class AdvancedArithmetic
    {
        [Discover("Square", "Calculates the square of a number", "Number to square")]
        public decimal Square(decimal number)
        {
            return number * number;
        }
    }
}
