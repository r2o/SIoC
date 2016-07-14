using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SimpleSample
{
    public interface ISimpleForConstant
    {
        int Sum(int value);
        
    }

    public class SimpleForConstant : ISimpleForConstant
    {
        int _start;

        public SimpleForConstant(int start)
        {
            _start = start;
        }

        public int Sum(int value)
        {
            _start += value;
            return _start;
        }
    }
}
