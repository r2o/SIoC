namespace ConsoleTest.SimpleSample
{
    using System;

    public interface ISimpleForMethod
    {
        void SaySomething();
    }

    public class SimpleForMethod : ISimpleForMethod
    {
        private readonly string message;

        public SimpleForMethod(string message)
        {
            this.message = message;
        }

        public void SaySomething()
        {
            Console.WriteLine(message);
        }
    }
}
