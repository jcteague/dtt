using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Chat.Formatters;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Content;

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

        public ChatMessagesService(IFormatCodeMessages codeMessageFormatter, IFormatPlainMessages plainMessageFormatter, IFormatUserIndicator userMessageFormatter, IFormatDateTime dateMessageFormatter, IBuildTable tableBuilder)
        {
            this.codeMessageFormatter = codeMessageFormatter;
            this.plainMessageFormatter = plainMessageFormatter;
            this.userMessageFormatter = userMessageFormatter;
            this.dateMessageFormatter = dateMessageFormatter;
            this.tableBuilder = tableBuilder;
        }

        public void AppendMessage(MessagesContainer messagesContainer, ScrollViewer scrollViewer, ChatMessageModel chatMessage)
        {
            messagesContainer.MessagesTable.Dispatcher.Invoke(new Action(() =>
            {
                var user = userMessageFormatter.GetFormattedElement(chatMessage, lastUserThatInserted).Value;
                var date = dateMessageFormatter.GetFormattedElement(chatMessage).Value;
                var message = chatMessage.IsCode()
                                  ? codeMessageFormatter.GetFormattedElement(chatMessage).Value
                                  : plainMessageFormatter.GetFormattedElement(chatMessage).Value;

                var columns = new Tuple<Block, Block, Block>(user, message, date);
                messagesContainer.MessagesTable.RowGroups.Add(tableBuilder.GetContentFor(columns));
                lastUserThatInserted = chatMessage.UserId;
            }));
            messagesContainer.MessagesTable.Dispatcher.Invoke(new Action(scrollViewer.ScrollToBottom));
        }

        public void ResetUser()
        {
            lastUserThatInserted = 0;
        }
    }
}