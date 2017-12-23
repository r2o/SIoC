namespace SIoC.core.Config
{
    using System.Configuration;

    public sealed class IoCConfigurationSection : ConfigurationSection
    {
        private const string IoCModuleName = "IoCModule";
        private const string ContainerProviderName = "ContainerProvider";

        [ConfigurationProperty(IoCModuleName)]
        public string IoCModule
        {
            get { return (string)base[IoCModuleName]; }
            set { base[IoCModuleName] = value; }
        }

        [ConfigurationProperty(ContainerProviderName)]
        public string ContainerProvider
        {
            get { return (string)base[ContainerProviderName]; }
            set { base[ContainerProviderName] = value; }
        }
    }
}
