using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using TeamNotification_Library.Configuration;

namespace TeamNotification_Library.Service.Http
{
    public class ChatMessageSender : ISendChatMessages
    {
        readonly ISendHttpRequests client;
        readonly IProvideConfiguration<ServerConfiguration> serverConfiguration;
        public ChatMessageSender(ISendHttpRequests client, IProvideConfiguration<ServerConfiguration> serverConfiguration)
        {
            this.client = client;
            this.serverConfiguration = serverConfiguration;
        }

        public void SendMessage(string message, string roomId)
        {           
            //Using this open URL so that the request doesnt make the package crash
            //var uri = string.Format("http://dtt.local:3000/registration?&userName={0}&userMessage={1}", "Raymi", message);
            var sb = new StringBuilder(serverConfiguration.Get().HREF);
            var postValues = new List<KeyValuePair<string, string>>();

            sb.AppendFormat("room/{0}/messages", roomId);
            postValues.Add(new KeyValuePair<string, string>("message", message));

            client.Post(sb.ToString(), postValues.ToArray());
        }
    }
}