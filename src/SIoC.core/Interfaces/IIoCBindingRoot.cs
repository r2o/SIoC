namespace SIoC.core
{
    using System;

    public interface IIoCBindingRoot
    {
        void BindInSingleton<TInterface, TImplementation>()
            where TImplementation : TInterface;

        void BindInSingleton<TInterface>(Type timpl);

        void BindInSingleton(Type tint, Type timpl);

        void BindInTransient<TInterface, TImplementation>()
            where TImplementation : TInterface;

        void BindInTransient<TInterface>(Type timpl);

        void BindInTransient(Type tint, Type timpl);

        void BindToMethod<TInterface>(Func<TInterface> func);

        void BindToMethod(Type tint, Func<object> obj);

        void BindToConstant<TInterface>(TInterface obj);

        void BindToConstant(Type tint, object obj);

        bool HasBindingFor<T>();

        bool HasBindingFor(Type tint);
    }
}
