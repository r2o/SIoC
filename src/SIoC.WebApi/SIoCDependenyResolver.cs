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
        readonly IIoCResolutionRoot _rr;
        readonly IIoCBindingRoot _br;

        public SIoCDependenyResolver(IIoCResolutionRoot rr)
        {
            _rr = rr;
            _br = _rr.Get<IIoCBindingRoot>();
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
            if (!_br.HasBindingFor(serviceType))
                return null;

            return _rr.Get(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (!_br.HasBindingFor(serviceType))
                return Enumerable.Empty<object>();

            return _rr.Get(serviceType) as IEnumerable<object>;
        }

        public void RegisterApiControllers()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in asm.GetTypes().Where(f => typeof(ApiController).IsAssignableFrom(f) && !f.IsAbstract))
                {
                    if (!_br.HasBindingFor(t))
                        _br.BindInTransient(t, t);
                }
            }
        }
    }

}

