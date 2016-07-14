using System;

namespace SIoC.core.Impl
{
    public class SIoCContainer : IContainerProvider
    {
        static readonly IIoCResolutionRoot _rr;
        static readonly IIoCBindingRoot _br;

        public SIoCContainer()
        {

        }
        
        static SIoCContainer()
        {
            var r = new SIoCResRootImpl();
            _rr = r; ;
            _br = r;
            _br.BindToConstant<IIoCBindingRoot>(_br);
            _br.BindToConstant<IIoCResolutionRoot>(_rr);
        }

        public T Get<T>()
        {
            return _rr.Get<T>();
        }

        public T Get<T>(Type tInt)
        {
            return _rr.Get<T>(tInt);
        }

        public object Get(Type tint)
        {
            return _rr.Get(tint);
        }

        public bool HasBindingFor<TInterface>()
        {
            return _br.HasBindingFor<TInterface>();
        }

        public bool HasBindingFor(Type type)
        {
            return _br.HasBindingFor(type);
        }
    }
}
