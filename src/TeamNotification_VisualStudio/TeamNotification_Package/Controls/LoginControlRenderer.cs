using TeamNotification_Library.Service;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    public class LoginControlRenderer : IBuildControls<MyControl>
    {
        public MyControl GetContentFrom(string uri)
        {
            return Container.GetInstance<MyControl>();
        }
    }
}