using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.SimpleSample
{
    public interface ISimpleForSingleton 
    {
        void Increase();
        int GetTotal();
    }

    public class SimpleForSingleton : ISimpleForSingleton
    {
        int _total;

        public SimpleForSingleton()
        {
            _total = 0;
        }

        public void Increase()
        {
            _total++;
        }

        public int GetTotal()
        {
            return _total;
        }
    }
}
