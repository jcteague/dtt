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
        private IFormatUserIndicator userMessageFormatter;
        private IFormatDateTime dateMessageFormatter;

        public ChatMessagesService(IFormatCodeMessages codeMessageFormatter, IFormatPlainMessages plainMessageFormatter, IFormatUserIndicator userMessageFormatter, IFormatDateTime dateMessageFormatter)
        {
            this.codeMessageFormatter = codeMessageFormatter;
            this.plainMessageFormatter = plainMessageFormatter;
            this.userMessageFormatter = userMessageFormatter;
            this.dateMessageFormatter = dateMessageFormatter;
        }

        public void AppendMessage(MessagesContainer messagesContainer, ScrollViewer scrollViewer, ChatMessageModel chatMessage)
        {
            messagesContainer.MessagesList.Dispatcher.Invoke(new Action(() =>
            {
                userMessageFormatter.GetFormattedElement(chatMessage, lastUserThatInserted).Each(messagesContainer.UsersList.Document.Blocks.Add);
                dateMessageFormatter.GetFormattedElement(chatMessage).Each(messagesContainer.DatesList.Document.Blocks.Add);

                if (chatMessage.IsCode())
                {
                    codeMessageFormatter.GetFormattedElement(chatMessage).Each(messagesContainer.MessagesList.Document.Blocks.Add);
                }
                else
                {
                    plainMessageFormatter.GetFormattedElement(chatMessage).Each(messagesContainer.MessagesList.Document.Blocks.Add);
                }
                lastUserThatInserted = chatMessage.UserId;
            }));
            messagesContainer.MessagesList.Dispatcher.Invoke(new Action(scrollViewer.ScrollToBottom));
        }

        public void ResetUser()
        {
            lastUserThatInserted = 0;
        }
    }
}