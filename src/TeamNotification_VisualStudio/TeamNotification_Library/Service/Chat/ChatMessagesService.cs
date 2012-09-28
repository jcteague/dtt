using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AurelienRibon.Ui.SyntaxHighlightBox;
using TeamNotification_Library.Models;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service.Chat.Formatters;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Content;
using TeamNotification_Library.Service.Factories.UI;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Logging;

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
        private ICreateSyntaxHighlightBox syntaxHighlighterCreator;
        
public ChatMessagesService(IFormatCodeMessages codeMessageFormatter, IFormatPlainMessages plainMessageFormatter, IFormatUserIndicator userMessageFormatter, IFormatDateTime dateMessageFormatter, IBuildTable tableBuilder, ISerializeJSON jsonSerializer)
        {
            this.codeMessageFormatter = codeMessageFormatter;
            this.plainMessageFormatter = plainMessageFormatter;
            this.userMessageFormatter = userMessageFormatter;
            this.dateMessageFormatter = dateMessageFormatter;
            this.tableBuilder = tableBuilder;
            this.syntaxHighlighterCreator = syntaxHighlighterCreator;
			this.jsonSerializer = jsonSerializer;
        }

        public TableRowGroup AppendMessage(ChatUIElements messagesContainer, ScrollViewer scrollViewer, ChatMessageModel chatMessage)
        {
            TableRowGroup appendedRowGroup = null;
            messagesContainer.MessagesTable.Dispatcher.Invoke(new Action(() =>
            {
                var user = userMessageFormatter.GetFormattedElement(chatMessage, lastUserThatInserted);
                var date = dateMessageFormatter.GetFormattedElement(chatMessage);
                var message = chatMessage.chatMessageBody.IsCode
                                  ? codeMessageFormatter.GetFormattedElement(chatMessage)
                                  : plainMessageFormatter.GetFormattedElement(chatMessage);

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