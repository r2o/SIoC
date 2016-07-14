namespace NancyTest.Sample
{
    public interface IHelloWorld
    {
        string SaySomething();
    }

    public class HelloWorld : IHelloWorld
    {
        public string SaySomething()
        {
            return "Hello I'm sending this message with the help of SIoC through nancyfx";
        }
    }
}
