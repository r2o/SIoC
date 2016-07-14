using System;

namespace SIoC.core
{
    public abstract class BaseIoCModule : IIoCModule
    {
        readonly IIoCResolutionRoot _rr;
        readonly IIoCBindingRoot _br;

        protected BaseIoCModule(IIoCBindingRoot br, IIoCResolutionRoot rr)
        {
            _rr = rr;
            _br = br;
        }

        public void BindInSingleton<TInterface, TImplementation>() 
            where TImplementation : TInterface
        {
            _br.BindInSingleton<TInterface, TImplementation>();
        }

        public void BindInSingleton<TInterface>(Type timpl)
        {
            _br.BindInSingleton<TInterface>(timpl);
        }

        public void BindInSingleton(Type tint, Type timpl)
        {
            _br.BindInSingleton(tint, timpl);
        }

        public void BindInTransient<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _br.BindInTransient<TInterface, TImplementation>();
        }

        public void BindInTransient<TInterface>(Type timpl)
        {
            _br.BindInTransient<TInterface>(timpl);
        }

        public void BindInTransient(Type tint, Type timpl)
        {
            _br.BindInTransient(tint, timpl);
        }

        public void BindToMethod<TInterface>(Func<TInterface> func)
        {
            _br.BindToMethod<TInterface>(func);
        }

        public void BindToMethod(Type tint, Func<object> obj)
        {
            _br.BindToMethod(tint, obj);
        }

        public void BindToConstant<TInterface>(TInterface obj)
        {
            _br.BindToConstant<TInterface>(obj);
        }

        public void BindToConstant(Type tint, object obj)
        {
            _br.BindToConstant(tint, obj);
        }

        public bool HasBindingFor<T>()
        {
            return _br.HasBindingFor<T>();
        }

        public bool HasBindingFor(Type tint)
        {
            return _br.HasBindingFor(tint);
        }

        public T Get<T>()
        {
            return _rr.Get<T>();
        }

        public T Get<T>(Type tint)
        {
            return _rr.Get<T>(tint);
        }

        public object Get(Type tint)
        {
            return _rr.Get(tint);
        }

        public abstract void Load();
    }
}
