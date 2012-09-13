using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Chat.Formatters;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Content;
using TeamNotification_Library.Service.Http;

namespace TeamNotification_Library.Service.Chat
{
    public class ChatMessagesService : IHandleChatMessages
    {
        private int lastUserThatInserted = 0;
        private IFormatCodeMessages codeMessageFormatter;
        private IFormatPlainMessages plainMessageFormatter;
        private IFormatUserIndicator userMessageFormatter;
        private IFormatDateTime dateMessageFormatter;
        private IBuildTable tableBuilder;
        private ISerializeJSON jsonSerializer;

        public ChatMessagesService(IFormatCodeMessages codeMessageFormatter, IFormatPlainMessages plainMessageFormatter, IFormatUserIndicator userMessageFormatter, IFormatDateTime dateMessageFormatter, IBuildTable tableBuilder)
        {
            this.codeMessageFormatter = codeMessageFormatter;
            this.plainMessageFormatter = plainMessageFormatter;
            this.userMessageFormatter = userMessageFormatter;
            this.dateMessageFormatter = dateMessageFormatter;
            this.tableBuilder = tableBuilder;
            this.jsonSerializer = new JSONSerializer();
        }

        public void AppendMessage(MessagesContainer messagesContainer, ScrollViewer scrollViewer, ChatMessageModel chatMessage)
        {
            //OVER HERE
            messagesContainer.MessagesTable.Dispatcher.Invoke(new Action(() =>
            {
                var user = userMessageFormatter.GetFormattedElement(chatMessage, lastUserThatInserted);
                var date = dateMessageFormatter.GetFormattedElement(chatMessage);
                var message = chatMessage.chatMessageBody.IsCode
                                  ? codeMessageFormatter.GetFormattedElement(chatMessage)
                                  : plainMessageFormatter.GetFormattedElement(chatMessage);

                if (!chatMessage.stamp.IsNullOrEmpty() && messagesContainer.MessagesList.ContainsKey(chatMessage.stamp))
                {
                    UpdateMessage(messagesContainer.MessagesTable, chatMessage);
                    messagesContainer.MessagesList[chatMessage.stamp] = chatMessage.stamp;//chatMessage.chatMessageBody.message;
                }else
                {
                    var columns = new Tuple<Block, Block, Block>(user, message, date);
                    messagesContainer.MessagesTable.RowGroups.Add(tableBuilder.GetContentFor(columns));
                    messagesContainer.MessagesList.Add(chatMessage.stamp, chatMessage.stamp);//chatMessage.chatMessageBody.message);
                }
                lastUserThatInserted = chatMessage.user_id.ParseToInteger();

            }));
            var m = stripMessage(chatMessage.chatMessageBody.message);
            messagesContainer.StatusBar.Text = chatMessage.username + " says: " + m;
            messagesContainer.MessagesTable.Dispatcher.Invoke(new Action(scrollViewer.ScrollToBottom));
        }

        private void UpdateMessage(Table messagesTable, ChatMessageModel messageModel)
        {
            messagesTable.Dispatcher.Invoke(new Action(() =>
            {
                foreach (var row in messagesTable.RowGroups)
                {
                    var originalMessage = row.Resources["originalMessage"].Cast<Collection.Messages>();
                    var stamp = Collection.getField(originalMessage.data, "stamp");
                  
                    if (stamp != messageModel.stamp) continue;
                    var originalBody = jsonSerializer.Deserialize<ChatMessageBody>(Collection.getField(originalMessage.data, "body"));
                    row.Rows[0].Cells[1] = new TableCell(new Paragraph(new Run(messageModel.chatMessageBody.message)));
                    Collection.setField(originalMessage.data, "body", jsonSerializer.Serialize(originalBody));

                    row.Resources["originalMessage"] = originalMessage;
                }
            }));
        }
        private string stripMessage(string message)
        {
            var m = message;
            if (m.Length > 15)
                m = m.Remove(14) + "...";
            return m;
        }

        public void ResetUser()
        {
            lastUserThatInserted = 0;
        }
    }
}