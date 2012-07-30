using TeamNotification_Library.Models;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Http;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    public class LoginControlRenderer : IBuildControls<LoginControl>
    {
        private readonly ISendHttpRequests httpClient;

        public LoginControlRenderer(ISendHttpRequests httpClient)
        {
            this.httpClient = httpClient;
        }

        public LoginControl GetContentFrom(string uri)
        {
            return Container.GetInstance<LoginControl>();
        }
    }
}