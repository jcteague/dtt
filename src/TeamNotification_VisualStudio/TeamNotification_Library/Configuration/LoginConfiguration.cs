namespace TeamNotification_Library.Configuration
{
    public class LoginConfiguration : IStoreConfiguration
    {
        private string _href = "http://dtt.local:3000/user/login";

        public string HREF
        {
            get { return _href; }
        }
    }
}