using ConsoleTest.SimpleSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.ComplexSample
{
    public interface IComplexWithMultipleParameters
    {
        void TestIt();
    }

    public class ComplexWithMultipleParameter : IComplexWithMultipleParameters
    {
        readonly ISimpleForSingleton _singleton;
        readonly ISimpleForConstant _constant;
        readonly ISimpleInterface _simple;

        public ComplexWithMultipleParameter(ISimpleForSingleton singleton, ISimpleForConstant constant, ISimpleInterface simple)
        {
            _singleton = singleton;
            _constant = constant;
            _simple = simple;
        }

        public void TestIt()
        {
            var msgSingleton = "Singleton value {0}";
            var msgConstant = "Constant value {0}";
            var msgSimple = "Something from simple in transient {0}";
            Console.WriteLine(string.Format(msgSingleton, _singleton.GetTotal()));
            for (var i = 0; i< 3; i++)
            {
                _singleton.Increase();
                Console.WriteLine(string.Format(msgSingleton, _singleton.GetTotal()));
                var c = _constant.Sum(i);
                Console.WriteLine(string.Format(msgConstant, c));
                _simple.Something(string.Format(msgSimple, i));
            }
        }
    }
}
