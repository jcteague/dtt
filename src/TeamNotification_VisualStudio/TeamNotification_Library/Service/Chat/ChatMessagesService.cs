using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Chat.Formatters;
using TeamNotification_Library.Extensions;

namespace TeamNotification_Library.Service.Chat
{
    public class ChatMessagesService : IHandleChatMessages
    {
        private int lastUserThatInserted = 0;
        private IFormatCodeMessages codeMessageFormatter;
        private IFormatPlainMessages plainMessageFormatter;

        public ChatMessagesService(IFormatCodeMessages codeMessageFormatter, IFormatPlainMessages plainMessageFormatter)
        {
            this.codeMessageFormatter = codeMessageFormatter;
            this.plainMessageFormatter = plainMessageFormatter;
        }

        public void AppendMessage(MessagesContainer messagesContainer, ScrollViewer scrollViewer, ChatMessageModel chatMessage)
        {
            var user = "";
            if (lastUserThatInserted != chatMessage.UserId)
            {
                user = chatMessage.UserName;
            }
            messagesContainer.UsersList.Document.Blocks.Add(new Paragraph(new Bold(new Run(user))));

            messagesContainer.MessagesList.Dispatcher.Invoke(new Action(() =>
            {
                if (chatMessage.IsCode())
                {
                    codeMessageFormatter.GetFormattedElement(chatMessage, lastUserThatInserted).Each(messagesContainer.MessagesList.Document.Blocks.Add);
                }
                else
                {
                    plainMessageFormatter.GetFormattedElement(chatMessage, lastUserThatInserted).Each(messagesContainer.MessagesList.Document.Blocks.Add);
                }
                lastUserThatInserted = chatMessage.UserId;
            }));
            messagesContainer.MessagesList.Dispatcher.Invoke(new Action(scrollViewer.ScrollToBottom));
        }
    }
}