namespace WebApiTestIIS.Models
{
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