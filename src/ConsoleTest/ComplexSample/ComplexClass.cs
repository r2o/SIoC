using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.ComplexSample
{
    public class ComplexClass : IComplexInterface
    {
        readonly IComplexInterfaceParameter _parm;

        public ComplexClass(IComplexInterfaceParameter parm)
        {
            _parm = parm;
        }

        public void SaySomething()
        {
            Console.WriteLine(_parm.GetMessage());
        }
    }

    public class ComplexClassParameter : IComplexInterfaceParameter
    {
        public string GetMessage()
        {
            return "Hello world from ComplexClassParameter";
        }
    }
}
