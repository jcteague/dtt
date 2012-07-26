using System;

namespace TeamNotification_Library.Service.Http
{
    public class ChatMessageSender : ISendChatMessages
    {
        readonly ISendHttpRequests client;

        public ChatMessageSender(ISendHttpRequests client)
        {
            this.client = client;
        }

        public void SendMessage(string message)
        {           
            //Using this open URL so that the request doesnt make the package crash
            var uri = string.Format("http://dtt.local:3000/registration?&userName={0}&userMessage={1}", "Raymi", message);
            client.Get(uri);
        }
    }
}