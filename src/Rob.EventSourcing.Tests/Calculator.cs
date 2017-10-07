namespace Rob.EventSourcing.Tests
{
    public class Calculator : ICalculator
    {
        public decimal Add(decimal x, decimal y)
        {
            return x + y;
        }
    }
}