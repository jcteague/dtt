using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Controls
{
    public class ChatRoomsControlService : IServiceChatRoomsControl
    {
        private IProvideUser userProvider;

        public ChatRoomsControlService(IProvideUser userProvider)
        {
            this.userProvider = userProvider;
        }

        public Collection GetCollection()
        {
            var user = userProvider.GetUser();

            return null;
        }
    }
}