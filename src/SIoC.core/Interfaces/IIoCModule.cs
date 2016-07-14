namespace SIoC.core
{
    public interface IIoCModule : IIoCBindingRoot, IIoCResolutionRoot
    {
        void Load();
    }
}
