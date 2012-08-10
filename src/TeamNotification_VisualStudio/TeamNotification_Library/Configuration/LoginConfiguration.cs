namespace TeamNotification_Library.Configuration
{
    public class LoginConfiguration : IStoreConfiguration
    {
        private string _href = "http://dtt.local:3000/user/login";

        public string Uri
        {
            get { return _href; }
            set { _href = value; }
        }
    }
}