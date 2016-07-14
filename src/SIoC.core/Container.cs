using SIoC.core.Config;
using System;
using System.Configuration;

namespace SIoC.core
{
    public class Container
    {
        static readonly IContainerProvider _provider;
        static object syncObj = new object();

        static Container()
        {
            lock (syncObj)
            {
                if (_provider != null)
                    return;

                var section = ConfigurationManager.GetSection("IoCSection") as IoCConfigurationSection;
                if (section == null)
                    throw new ArgumentNullException("IoCConfigurationSection not defined");

                if (string.IsNullOrEmpty(section.IoCModule) || string.IsNullOrEmpty(section.IoCModule.Trim()))
                    throw new ArgumentNullException("IoCModule not defined");


                var cp = Type.GetType(section.ContainerProvider);
                if (cp == null)
                    throw new Exception(string.Format("Invalid ContainerProvider defined {0}", section.ContainerProvider));
                var t = Type.GetType(section.IoCModule);
                if (t == null)
                    throw new Exception(string.Format("Invalid IoCModule defined {0}", section.IoCModule));

                _provider = (IContainerProvider)Activator.CreateInstance(cp);
                IIoCResolutionRoot rr;
                IIoCBindingRoot br;

                try
                {
                    rr = _provider.Get<IIoCResolutionRoot>();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error has ocurred trying to look for IIoCResolutionRoot", ex);
                }

                try
                {
                    br = _provider.Get<IIoCBindingRoot>();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error has ocurred trying to look for IIoCBindingRoot", ex);
                }

                var mod = (BaseIoCModule)Activator.CreateInstance(t, br, rr);
                br.BindToConstant<IIoCModule>(mod);
                mod.Load();
            }
        }

        public static T Get<T>()
        {
            return _provider.Get<T>();
        }
    }
}
