namespace NancyTest
{
    using Nancy.Hosting.Self;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new NancyHost(new Uri("http://localhost:2016"));
            host.Start();
            Console.ReadLine();
            host.Stop();
        }
    }
}
