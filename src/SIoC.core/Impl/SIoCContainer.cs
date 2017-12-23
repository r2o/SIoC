namespace SIoC.core.Impl
{
    using System;

    public class SIoCContainer : IContainerProvider
    {
        private static readonly IIoCResolutionRoot ResolutionRoot;

        private static readonly IIoCBindingRoot BindingRoot;

        static SIoCContainer()
        {
            var r = new SIoCResRootImpl();
            ResolutionRoot = r;
            BindingRoot = r;
            BindingRoot.BindToConstant<IIoCBindingRoot>(BindingRoot);
            BindingRoot.BindToConstant<IIoCResolutionRoot>(ResolutionRoot);
        }

        public SIoCContainer()
        {
        }
        
        public T Get<T>()
        {
            return ResolutionRoot.Get<T>();
        }

        public T Get<T>(Type tInt)
        {
            return ResolutionRoot.Get<T>(tInt);
        }

        public object Get(Type tint)
        {
            return ResolutionRoot.Get(tint);
        }

        public bool HasBindingFor<TInterface>()
        {
            return BindingRoot.HasBindingFor<TInterface>();
        }

        public bool HasBindingFor(Type type)
        {
            return BindingRoot.HasBindingFor(type);
        }
    }
}
