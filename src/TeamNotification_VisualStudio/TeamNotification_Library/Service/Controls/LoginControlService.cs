using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Http;

namespace TeamNotification_Library.Service.Controls
{
    public class LoginControlService : IServiceLoginControl
    {
        private readonly ISendHttpRequests httpClient;
        private readonly IProvideConfiguration<LoginConfiguration> configuration;

        public LoginControlService(ISendHttpRequests httpClient, IProvideConfiguration<LoginConfiguration> configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        public Collection GetCollection()
        {
            return httpClient.Get<Collection>(configuration.Get().HREF).Result;
        }

        public void HandleClick()
        {
            throw new System.NotImplementedException();
        }

        
    }
}