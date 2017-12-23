namespace ConsoleTest.SimpleSample
{
    public interface ISimpleForSingleton 
    {
        void Increase();

        int GetTotal();
    }

    public class SimpleForSingleton : ISimpleForSingleton
    {
        private int total;

        public SimpleForSingleton()
        {
            total = 0;
        }

        public void Increase()
        {
            total++;
        }

        public int GetTotal()
        {
            return total;
        }
    }
}
