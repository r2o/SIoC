using ConsoleTest.SimpleSample;
using ConsoleTest.ComplexSample;
using SIoC.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
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
