namespace ConsoleTest
{
    using ConsoleTest.ComplexSample;
    using ConsoleTest.SimpleSample;
    using SIoC.core;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            var rr = Container.Get<IIoCResolutionRoot>();
            var simpleInterface = rr.Get<ISimpleInterface>();
            simpleInterface.Something("hello world");
            var complex = rr.Get<IComplexInterface>();
            complex.SaySomething();
            var simpleForMethod = rr.Get<ISimpleForMethod>();
            simpleForMethod.SaySomething();
            var simpleForConstant = rr.Get<ISimpleForConstant>();
            var msg = "Constant value after sum 10 - {0}";
            Console.WriteLine(string.Format(msg, simpleForConstant.Sum(10)));
            Console.WriteLine(string.Format(msg, simpleForConstant.Sum(10)));
            for (var i = 0; i < 3; i++)
            {
                var complexWithParameters = rr.Get<IComplexWithMultipleParameters>();
                complexWithParameters.TestIt();
                Console.WriteLine(string.Empty);
            }

            Console.ReadLine();
        }
    }
}
