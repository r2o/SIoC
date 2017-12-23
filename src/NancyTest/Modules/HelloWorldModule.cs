namespace NancyTest.Modules
{
    using Nancy;
    using NancyTest.Sample;

    public class HelloWorldModule : NancyModule
    {
        private readonly IHelloWorld hello;

        public HelloWorldModule(IHelloWorld hello)
        {
            this.hello = hello;
            Get["api/nancy/saysomething"] = _ =>
            {
                return Response.AsJson(this.hello.SaySomething(), HttpStatusCode.OK);
            };
        }
    }
}
