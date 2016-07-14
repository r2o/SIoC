using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SimpleSample
{
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
