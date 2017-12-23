namespace ConsoleTest
{
    using ConsoleTest.ComplexSample;
    using ConsoleTest.SimpleSample;
    using SIoC.core;

    public class IoCModule : BaseIoCModule
    {
        public IoCModule(IIoCBindingRoot br, IIoCResolutionRoot rr) 
            : base(br, rr)
        {
        }

        public override void Load()
        {
            BindInTransient<ISimpleInterface, SimpleClass>();
            BindInTransient<IComplexInterfaceParameter, ComplexClassParameter>();
            BindInTransient<IComplexInterface, ComplexClass>();
            BindToMethod<ISimpleForMethod>(() => new SimpleForMethod("I am from BindToMethod"));
            BindToConstant<ISimpleForConstant>(new SimpleForConstant(1));
            BindInSingleton<ISimpleForSingleton, SimpleForSingleton>();
            BindInSingleton<IComplexWithMultipleParameters, ComplexWithMultipleParameter>();
        }
    }
}
