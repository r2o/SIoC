namespace WebApiTestIIS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public interface IHelloWorld
    {
        string SayHello();
    }

    public class HelloWorld : IHelloWorld
    {
        public string SayHello()
        {
            return "Hello world I'm SIoC";
        }
    }
}