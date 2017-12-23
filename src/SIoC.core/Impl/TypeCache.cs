namespace SIoC.core.Impl
{
    using System;
    using System.Reflection;

    internal class TypeCache
    {
        internal Type TImpl { get; set; }

        internal ParameterInfo[] CtorParms { get; set; }

        internal TypeCache()
        {
            CtorParms = new ParameterInfo[0];
        }
    }
}
