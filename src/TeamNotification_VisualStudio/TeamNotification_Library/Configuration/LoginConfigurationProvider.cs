namespace TeamNotification_Library.Configuration
{
    public class LoginConfigurationProvider : IProvideConfiguration<LoginConfiguration>
    {
        public IStoreConfiguration Get()
        {
            return new LoginConfiguration();
        }
    }
}