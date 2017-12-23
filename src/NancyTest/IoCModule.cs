namespace NancyTest
{
    using NancyTest.Sample;
    using SIoC.core;

    public class IoCModule : BaseIoCModule
    {
        public IoCModule(IIoCBindingRoot br, IIoCResolutionRoot rr) : base(br, rr)
        {
        }

        public override void Load()
        {
            BindInSingleton<IHelloWorld, HelloWorld>();
        }
    }
}
