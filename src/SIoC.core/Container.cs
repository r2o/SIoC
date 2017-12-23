namespace SIoC.core
{
    using SIoC.core.Config;
    using System;
    using System.Configuration;

    public class Container
    {
        private static readonly IContainerProvider Provider;

        private static object syncObj = new object();

        static Container()
        {
            lock (syncObj)
            {
                if (Provider != null)
                {
                    return;
                }

                var section = ConfigurationManager.GetSection("IoCSection") as IoCConfigurationSection;
                if (section == null)
                {
                    throw new ArgumentNullException("IoCConfigurationSection not defined");
                }

                if (string.IsNullOrEmpty(section.IoCModule) || string.IsNullOrEmpty(section.IoCModule.Trim()))
                {
                    throw new ArgumentNullException("IoCModule not defined");
                }

                var cp = Type.GetType(section.ContainerProvider);
                if (cp == null)
                {
                    throw new Exception(string.Format("Invalid ContainerProvider defined {0}", section.ContainerProvider));
                }

                var t = Type.GetType(section.IoCModule);
                if (t == null)
                {
                    throw new Exception(string.Format("Invalid IoCModule defined {0}", section.IoCModule));
                }

                Provider = (IContainerProvider)Activator.CreateInstance(cp);
                IIoCResolutionRoot rr;
                IIoCBindingRoot br;

                try
                {
                    rr = Provider.Get<IIoCResolutionRoot>();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error has ocurred trying to look for IIoCResolutionRoot", ex);
                }

                try
                {
                    br = Provider.Get<IIoCBindingRoot>();
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
            return Provider.Get<T>();
        }
    }
}
