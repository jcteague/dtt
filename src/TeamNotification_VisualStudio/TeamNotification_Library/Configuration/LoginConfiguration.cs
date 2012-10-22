using System.Configuration;

namespace TeamNotification_Library.Configuration
{
    public class LoginConfiguration : IStoreConfiguration
    {
        private string _href = "user/login";

        public string Uri
        {
            get
            {
                return Properties.Settings.Default.Site + _href;
            }
            set
            {
                _href = value;
            }
        }
    }
}