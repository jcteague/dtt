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
//            messagesContainer.MessagesList.Dispatcher.Invoke(new Action(() =>
//            {              
//                userMessageFormatter.GetFormattedElement(chatMessage, lastUserThatInserted).Each(messagesContainer.UsersList.Document.Blocks.Add);
//                dateMessageFormatter.GetFormattedElement(chatMessage).Each(messagesContainer.DatesList.Document.Blocks.Add);
//
//                if (chatMessage.IsCode())
//                {
//                    codeMessageFormatter.GetFormattedElement(chatMessage).Each(messagesContainer.MessagesList.Document.Blocks.Add);
//                }
//                else
//                {
//                    plainMessageFormatter.GetFormattedElement(chatMessage).Each(messagesContainer.MessagesList.Document.Blocks.Add);
//                }
//                lastUserThatInserted = chatMessage.UserId;
//
//                
//            }));


            messagesContainer.MessagesTable.Dispatcher.Invoke(new Action(() =>
            {
                var tableRowGroup = new TableRowGroup();
                var row = GetRowFor(chatMessage);
                tableRowGroup.Rows.Add(row);

                messagesContainer.MessagesTable.RowGroups.Add(tableRowGroup);

//                messagesContainer.Container.
//                var newBlock = new FlowDocument();
//                userMessageFormatter.GetFormattedElement(chatMessage, lastUserThatInserted).Each(newBlock.Blocks.Add);
//                dateMessageFormatter.GetFormattedElement(chatMessage).Each(newBlock.Blocks.Add);
//
//                if (chatMessage.IsCode())
//                {
//                    codeMessageFormatter.GetFormattedElement(chatMessage).Each(newBlock.Blocks.Add);
//                }
//                else
//                {
//                    plainMessageFormatter.GetFormattedElement(chatMessage).Each(newBlock.Blocks.Add);
//                }
                lastUserThatInserted = chatMessage.UserId;

            }));
            messagesContainer.MessagesTable.Dispatcher.Invoke(new Action(scrollViewer.ScrollToBottom));
        }

        private TableRow GetRowFor(ChatMessageModel chatMessage)
        {
            var row = new TableRow();
            row.Cells.Add(GetCellFor(userMessageFormatter.GetFormattedElement(chatMessage, lastUserThatInserted).Value));
            
            if (chatMessage.IsCode())
            {
                row.Cells.Add(GetCellFor(codeMessageFormatter.GetFormattedElement(chatMessage).Value));
            }
            else
            {
                row.Cells.Add(GetCellFor(plainMessageFormatter.GetFormattedElement(chatMessage).Value));
            }
            row.Cells.Add(GetCellFor(dateMessageFormatter.GetFormattedElement(chatMessage).Value));

            var c1 = row.Cells[0];
            var c2 = row.Cells[1];
            var c3 = row.Cells[2];
            return row;
        }

        private TableCell GetCellFor(Block block)
        {
            return new TableCell(block);
        }

        public void ResetUser()
        {
            lastUserThatInserted = 0;
        }
    }
}