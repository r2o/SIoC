using System;

namespace SIoC.core
{
    public interface IIoCResolutionRoot
    {
        T Get<T>();
        T Get<T>(Type tInt);
        object Get(Type tInt);
    }
}
