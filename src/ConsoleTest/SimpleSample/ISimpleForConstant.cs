namespace ConsoleTest.SimpleSample
{
    public interface ISimpleForConstant
    {
        int Sum(int value);
    }

    public class SimpleForConstant : ISimpleForConstant
    {
        private int start;

        public SimpleForConstant(int start)
        {
            this.start = start;
        }

        public int Sum(int value)
        {
            start += value;
            return start;
        }
    }
}
