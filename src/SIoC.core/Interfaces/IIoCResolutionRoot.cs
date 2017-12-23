namespace SIoC.core
{
    using System;

    public interface IIoCResolutionRoot
    {
        T Get<T>();

        T Get<T>(Type tInt);

        object Get(Type tInt);
    }
}
