using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Hosting.Self;
namespace NancyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new NancyHost(new Uri("http://localhost:2016"));
            host.Start();
            Console.ReadLine();
            host.Stop();
            
        }
    }
}
