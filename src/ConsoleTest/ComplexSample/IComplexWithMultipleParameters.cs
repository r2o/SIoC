namespace ConsoleTest.ComplexSample
{
    using ConsoleTest.SimpleSample;
    using System;

    public interface IComplexWithMultipleParameters
    {
        void TestIt();
    }

    public class ComplexWithMultipleParameter : IComplexWithMultipleParameters
    {
        private readonly ISimpleForSingleton singleton;
        private readonly ISimpleForConstant constant;
        private readonly ISimpleInterface simple;

        public ComplexWithMultipleParameter(ISimpleForSingleton singleton, ISimpleForConstant constant, ISimpleInterface simple)
        {
            this.singleton = singleton;
            this.constant = constant;
            this.simple = simple;
        }

        public void TestIt()
        {
            var msgSingleton = "Singleton value {0}";
            var msgConstant = "Constant value {0}";
            var msgSimple = "Something from simple in transient {0}";
            Console.WriteLine(string.Format(msgSingleton, singleton.GetTotal()));
            for (var i = 0; i < 3; i++)
            {
                singleton.Increase();
                Console.WriteLine(string.Format(msgSingleton, singleton.GetTotal()));
                var c = constant.Sum(i);
                Console.WriteLine(string.Format(msgConstant, c));
                simple.Something(string.Format(msgSimple, i));
            }
        }
    }
}
