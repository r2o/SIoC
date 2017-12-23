namespace ConsoleTest.SimpleSample
{
    using System;

    public interface ISimpleInterface
    {
        void Something(string par1);
    }

    public class SimpleClass : ISimpleInterface
    {
        public void Something(string par1)
        {
            Console.WriteLine(par1);
        }
    }
}
