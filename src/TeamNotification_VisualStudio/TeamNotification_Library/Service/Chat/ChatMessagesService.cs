using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TeamNotification_Library.Models;
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

        public void AppendMessage(RichTextBox messageList, ScrollViewer scrollViewer, ChatMessageModel chatMessage)
        {
            messageList.Dispatcher.Invoke(new Action(() =>
            {
                if (chatMessage.IsCode())
                {
                    codeMessageFormatter.GetFormattedElement(chatMessage, lastUserThatInserted).Each(messageList.Document.Blocks.Add);
                }
                else
                {
                    plainMessageFormatter.GetFormattedElement(chatMessage, lastUserThatInserted).Each(messageList.Document.Blocks.Add);
                }
                lastUserThatInserted = chatMessage.UserId;
            }));
            messageList.Dispatcher.Invoke(new Action(scrollViewer.ScrollToBottom));
        }
    }
}