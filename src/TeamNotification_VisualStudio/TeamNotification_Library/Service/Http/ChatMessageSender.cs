namespace TeamNotification_Library.Service.Http
{
    public class ChatMessageSender : ISendChatMessages
    {
        readonly ICommunicateUsingHttp client;

        public ChatMessageSender(ICommunicateUsingHttp client)
        {
            this.client = client;
        }

        public void SendMessage(string message)
        {
            var uri = string.Format("http://10.0.0.32:3000/?&userName={0}&userMessage={1}", "Raymi", message);
            client.Get(uri);
        }
    }
}