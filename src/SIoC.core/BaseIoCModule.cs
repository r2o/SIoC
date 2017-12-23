namespace SIoC.core
{
    using System;

    public abstract class BaseIoCModule : IIoCModule
    {
        private readonly IIoCResolutionRoot resolutionRoot;

        private readonly IIoCBindingRoot bindingRoot;

        protected BaseIoCModule(IIoCBindingRoot br, IIoCResolutionRoot rr)
        {
            resolutionRoot = rr;
            bindingRoot = br;
        }

        public void BindInSingleton<TInterface, TImplementation>() 
            where TImplementation : TInterface
        {
            bindingRoot.BindInSingleton<TInterface, TImplementation>();
        }

        public void BindInSingleton<TInterface>(Type timpl)
        {
            bindingRoot.BindInSingleton<TInterface>(timpl);
        }

        public void BindInSingleton(Type tint, Type timpl)
        {
            bindingRoot.BindInSingleton(tint, timpl);
        }

        public void BindInTransient<TInterface, TImplementation>() where TImplementation : TInterface
        {
            bindingRoot.BindInTransient<TInterface, TImplementation>();
        }

        public void BindInTransient<TInterface>(Type timpl)
        {
            bindingRoot.BindInTransient<TInterface>(timpl);
        }

        public void BindInTransient(Type tint, Type timpl)
        {
            bindingRoot.BindInTransient(tint, timpl);
        }

        public void BindToMethod<TInterface>(Func<TInterface> func)
        {
            bindingRoot.BindToMethod(func);
        }

        public void BindToMethod(Type tint, Func<object> obj)
        {
            bindingRoot.BindToMethod(tint, obj);
        }

        public void BindToConstant<TInterface>(TInterface obj)
        {
            bindingRoot.BindToConstant(obj);
        }

        public void BindToConstant(Type tint, object obj)
        {
            bindingRoot.BindToConstant(tint, obj);
        }

        public bool HasBindingFor<T>()
        {
            return bindingRoot.HasBindingFor<T>();
        }

        public bool HasBindingFor(Type tint)
        {
            return bindingRoot.HasBindingFor(tint);
        }

        public T Get<T>()
        {
            return resolutionRoot.Get<T>();
        }

        public T Get<T>(Type tint)
        {
            return resolutionRoot.Get<T>(tint);
        }

        public object Get(Type tint)
        {
            return resolutionRoot.Get(tint);
        }

        public abstract void Load();
    }
}
