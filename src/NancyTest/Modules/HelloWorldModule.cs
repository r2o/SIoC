namespace NancyTest.Modules
{
    using Nancy;
    using NancyTest.Sample;

    public class HelloWorldModule : NancyModule
    {
        readonly IHelloWorld _hello;

        public HelloWorldModule(IHelloWorld hello)
        {
            _hello = hello;
            Get["api/nancy/saysomething"] = _ =>
            {
                return Response.AsJson(_hello.SaySomething(), HttpStatusCode.OK);
            };
        }
    }
}
