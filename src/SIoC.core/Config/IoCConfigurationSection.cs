using System.Configuration;

namespace SIoC.core.Config
{
    public sealed class IoCConfigurationSection : ConfigurationSection
    {
        const string _IoCModule = "IoCModule";
        const string _ContainerProvider = "ContainerProvider";

        [ConfigurationProperty(_IoCModule)]
        public string IoCModule
        {
            get { return ((string)(base[_IoCModule])); }
            set { base[_IoCModule] = value; }
        }

        [ConfigurationProperty(_ContainerProvider)]
        public string ContainerProvider
        {
            get { return (string)base[_ContainerProvider]; }
            set { base[_ContainerProvider] = value; }
        }
    }
}
