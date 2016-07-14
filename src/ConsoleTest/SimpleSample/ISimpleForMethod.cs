using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SimpleSample
{
    public interface ISimpleForMethod
    {
        void SaySomething();
    }

    public class SimpleForMethod : ISimpleForMethod
    {
        readonly string _message;

        public SimpleForMethod(string message)
        {
            _message = message;
        }

        public void SaySomething()
        {
            Console.WriteLine(_message);
        }
    }
}
