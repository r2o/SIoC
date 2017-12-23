namespace SIoC.WebApi
{
    using core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Dependencies;

    public class SIoCDependenyResolver : IDependencyResolver, IDependencyScope
    {
        private readonly IIoCResolutionRoot resolutionRoot;
        private readonly IIoCBindingRoot bindingRoot;

        public SIoCDependenyResolver(IIoCResolutionRoot rr)
        {
            resolutionRoot = rr;
            bindingRoot = resolutionRoot.Get<IIoCBindingRoot>();
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            if (!bindingRoot.HasBindingFor(serviceType))
            {
                return null;
            }

            return resolutionRoot.Get(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (!bindingRoot.HasBindingFor(serviceType))
            {
                return Enumerable.Empty<object>();
            }

            return resolutionRoot.Get(serviceType) as IEnumerable<object>;
        }

        public void RegisterApiControllers()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in asm.GetTypes().Where(f => typeof(ApiController).IsAssignableFrom(f) && !f.IsAbstract))
                {
                    if (!bindingRoot.HasBindingFor(t))
                    {
                        bindingRoot.BindInTransient(t, t);
                    }
                }
            }
        }
    }
}
