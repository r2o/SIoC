using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.ComplexSample
{
    public interface IComplexInterface
    {
        void SaySomething();
    }
    
    public interface IComplexInterfaceParameter
    {
        string GetMessage();
    }
}
