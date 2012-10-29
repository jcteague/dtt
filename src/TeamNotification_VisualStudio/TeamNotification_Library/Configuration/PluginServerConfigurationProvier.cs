namespace TeamNotification_Library.Configuration
{
    public class PluginServerConfigurationProvier : IProvideConfiguration<PluginServerConfiguration>
    {
        public IStoreConfiguration Get()
        {
            return new PluginServerConfiguration();
        }
    }
}