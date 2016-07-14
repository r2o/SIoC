using ConsoleTest.ComplexSample;
using ConsoleTest.SimpleSample;
using SIoC.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
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
                Console.WriteLine("");
            }
            Console.ReadLine();
        }
    }
}
