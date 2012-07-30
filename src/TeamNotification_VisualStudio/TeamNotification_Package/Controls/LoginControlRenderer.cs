using TeamNotification_Library.Models;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Http;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    public class LoginControlRenderer : IBuildControls<MyControl>
    {
        private readonly ISendHttpRequests httpClient;

        public LoginControlRenderer(ISendHttpRequests httpClient)
        {
            this.httpClient = httpClient;
        }

        public MyControl GetContentFrom(string uri)
        {
            return Container.GetInstance<MyControl>();
//            var loginControl = Container.GetInstance<LoginControl>();
//            return loginControl.Render(httpClient.Get<Collection>(uri).Result);
        }
    }
}