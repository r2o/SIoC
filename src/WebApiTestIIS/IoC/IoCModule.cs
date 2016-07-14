namespace WebApiTestIIS.IoC
{
    using Models;
    using SIoC.core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class IoCModule : BaseIoCModule
    {
        public IoCModule(IIoCBindingRoot br, IIoCResolutionRoot rr) 
            : base(br, rr)
        {
        }

        public override void Load()
        {
            BindInSingleton<IHelloWorld, HelloWorld>();
        }
    }
}