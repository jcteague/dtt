using System;
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
        private IFormatNotificationMessages notifitacionMessageFormatter;

        private IFormatUserIndicator userMessageFormatter;
        private IFormatDateTime dateMessageFormatter;
        private IBuildTable tableBuilder;
        private ISerializeJSON jsonSerializer;

        public ChatMessagesService(IFormatCodeMessages codeMessageFormatter, IFormatPlainMessages plainMessageFormatter, IFormatUserIndicator userMessageFormatter, IFormatDateTime dateMessageFormatter, IBuildTable tableBuilder, ISerializeJSON jsonSerializer, IFormatNotificationMessages notifitacionMessageFormatter)
        {
            this.codeMessageFormatter = codeMessageFormatter;
            this.plainMessageFormatter = plainMessageFormatter;
            this.userMessageFormatter = userMessageFormatter;
            this.dateMessageFormatter = dateMessageFormatter;
            this.tableBuilder = tableBuilder;
            this.jsonSerializer = jsonSerializer;
            this.notifitacionMessageFormatter = notifitacionMessageFormatter;
        }

        public TableRowGroup AppendMessage(ChatUIElements messagesContainer, ScrollViewer scrollViewer, ChatMessageModel chatMessage)
        {
            TableRowGroup appendedRowGroup = null;
            messagesContainer.MessagesTable.Dispatcher.Invoke(new Action(() =>
            {
                var user = userMessageFormatter.GetFormattedElement(chatMessage, lastUserThatInserted);
                var name = user.GetText();
                var date = dateMessageFormatter.GetFormattedElement(chatMessage);
                var message = GetMessageFrom(chatMessage);

                if (!chatMessage.stamp.IsNullOrEmpty() && messagesContainer.MessagesList.ContainsKey(chatMessage.stamp))
                {
                    var editedRow = UpdateMessage(messagesContainer, chatMessage);
                    messagesContainer.MessagesList[chatMessage.stamp] = editedRow;
                    appendedRowGroup = editedRow;
                }
                else
                {
                    var columns = new Tuple<Block, Block, Block>(user, message, date);
                    appendedRowGroup = tableBuilder.GetContentFor(columns);
                    messagesContainer.MessagesTable.RowGroups.Add(appendedRowGroup);
                    lastUserThatInserted = chatMessage.user_id.ParseToInteger();
                }
            }));
            var m = stripMessage(chatMessage.chatMessageBody.message);
            messagesContainer.StatusBar.Text = chatMessage.username + " says: " + m;
            messagesContainer.MessagesTable.Dispatcher.Invoke(new Action(scrollViewer.ScrollToBottom));
            return appendedRowGroup;
        }

        private Block GetMessageFrom(ChatMessageModel chatMessage)
        {
            if (chatMessage.chatMessageBody.IsCode)
                return codeMessageFormatter.GetFormattedElement(chatMessage);

            if (!chatMessage.chatMessageBody.notification.IsNullOrWhiteSpace())
                return notifitacionMessageFormatter.GetFormattedElement(chatMessage);

            return plainMessageFormatter.GetFormattedElement(chatMessage);
        }

        private TableRowGroup UpdateMessage(ChatUIElements messagesContainer, ChatMessageModel messageModel)
        {
            var messagesTable = messagesContainer.MessagesTable;
            TableRowGroup editedRow = null;
            messagesTable.Dispatcher.Invoke(new Action(() =>
            {
                var row = messagesContainer.MessagesList[messageModel.stamp];
                var originalMessage = row.Resources["originalMessage"].Cast<Collection.Messages>();
                  
                var originalBody = jsonSerializer.Deserialize<ChatMessageBody>(Collection.getField(originalMessage.data, "body"));
                if (messageModel.chatMessageBody.IsCode)
                {
                    row.Rows[0].Cells[1] = new TableCell(codeMessageFormatter.GetFormattedElement(messageModel));

                }else{
                    row.Rows[0].Cells[1] = new TableCell(plainMessageFormatter.GetFormattedElement(messageModel));
                }
                Collection.setField(originalMessage.data, "body", jsonSerializer.Serialize(originalBody));

                row.Resources["originalMessage"] = originalMessage;
                editedRow = row;
            }));
            return editedRow;
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