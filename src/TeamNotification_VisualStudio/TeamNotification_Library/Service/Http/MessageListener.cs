using System.Text;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Http
{
    public class MessageListener : IListenToMessages
    {
        readonly IConnectToRedis client;

        public MessageListener(IConnectToRedis client)
        {
            this.client = client;
        }

        public void ListenOnChannel(string channel, MessageReceivedAction action)
        {
            client.Subscribe(channel,(c,bytes)=> action(c, new UTF8Encoding().GetString(bytes)));
        }
    }
}