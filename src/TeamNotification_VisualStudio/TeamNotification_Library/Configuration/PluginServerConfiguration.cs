namespace TeamNotification_Library.Configuration
{
    public class PluginServerConfiguration : IStoreConfiguration
    {
        private string _href = "plugin";

        public string Uri
        {
            get { return Properties.Settings.Default.Site + _href; }
            set { _href = value; }
        }
    }
}