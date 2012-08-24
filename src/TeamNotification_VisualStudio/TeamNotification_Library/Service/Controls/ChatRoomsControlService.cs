using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using AurelienRibon.Ui.SyntaxHighlightBox;
using EnvDTE;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Factories.UI;
using TeamNotification_Library.Service.Highlighters;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.LocalSystem;
using TeamNotification_Library.Service.Providers;
using TextRange = System.Windows.Documents.TextRange;
using ProgrammingLanguages = TeamNotification_Library.Configuration.Globals.ProgrammingLanguages;

namespace TeamNotification_Library.Service.Controls
{
    public class ChatRoomsControlService : IServiceChatRoomsControl
    {
        private IProvideUser userProvider;
        private ISendHttpRequests httpClient;
        private IProvideConfiguration<ServerConfiguration> configuration;
        private IStoreClipboardData clipboardStorage;
        private ISerializeJSON jsonSerializer;
        readonly IStoreGlobalState applicationGlobalState;
        readonly ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory;

        private readonly ISendChatMessages messageSender;
        private IHandleSystemClipboard systemClipboardHandler;

        public ChatRoomsControlService(IProvideUser userProvider, ISendHttpRequests httpClient, IProvideConfiguration<ServerConfiguration> configuration, IStoreClipboardData clipboardStorage, ISendChatMessages messageSender, IStoreGlobalState applicationGlobalState, IHandleSystemClipboard systemClipboardHandler, ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory, ISerializeJSON jsonSerializer)
        {
            this.userProvider = userProvider;
            this.httpClient = httpClient;
            this.clipboardStorage = clipboardStorage;
            this.messageSender = messageSender;
            this.applicationGlobalState = applicationGlobalState;
            this.systemClipboardHandler = systemClipboardHandler;
            this.syntaxBlockUIContainerFactory = syntaxBlockUIContainerFactory;
            this.jsonSerializer = jsonSerializer;
            this.configuration = configuration;
        }

        public Collection GetMessagesCollection(string roomId)
        {
            var uri = configuration.Get().Uri + "room/" + roomId + "/messages";
            var c = httpClient.Get<Collection>(uri).Result;
            return c;
        }

        public Collection GetCollection()
        {
            var user = userProvider.GetUser();
            var uri = configuration.Get().Uri +"user/"+ user.id;
            var c = httpClient.Get<Collection>(uri).Result;
            return c;
        }

        public void UpdateClipboard(object source, DTE dte)
        {
            if (HasCopied)
            {
                HasCopied = false;
                return;
            }

            if (applicationGlobalState.Active && dte.ActiveWindow.Document.IsNotNull())
            {
                var activeDocument = dte.ActiveDocument;
                var txt = activeDocument.Object() as TextDocument;
                if (txt.IsNull()) return;
                var selection = txt.Selection;

                var message = systemClipboardHandler.GetText(true);
                var clipboard = new CodeClipboardData
                {
                    solution = dte.Solution.FullName,
                    document = activeDocument.FullName,
                    message = message,
                    line = selection.CurrentLine,
                    column = selection.CurrentColumn,
                    programmingLanguage = activeDocument.GetProgrammingLanguage()
                };

                clipboardStorage.Store(clipboard);
            }
            else
            {
                var clipboard = new PlainClipboardData {message = systemClipboardHandler.GetText(true)};
                clipboardStorage.Store(clipboard);
            }
            HasCopied = true;
        }

        private bool HasCopied { get; set; }

        public void HandlePaste(RichTextBox textBox, DataObjectPastingEventArgs dataObjectPastingEventArgs)
        {
            if(clipboardStorage.IsCode)
            {
                var block = syntaxBlockUIContainerFactory.Get(clipboardStorage.Get<CodeClipboardData>());
                textBox.Document.Blocks.Add(block);
                textBox.CaretPosition = textBox.Document.ContentEnd;
                dataObjectPastingEventArgs.CancelCommand();
            }
        }

        public void SendMessage(RichTextBox textBox, string roomId)
        {
            messageSender.SendMessages(textBox.Document.Blocks, roomId);
            ClearRichTextBox(textBox);
        }

        public void ClearRichTextBox(RichTextBox textBox)
        {
            textBox.Document.Blocks.Clear();
        }

        public void AddMessages(RichTextBox messageList, ScrollViewer scrollviewer, string currentRoomId)
        {
            var collection = GetMessagesCollection(currentRoomId);
            foreach (var message in collection.messages)
            {
                var newMessage = jsonSerializer.Deserialize<MessageBody>(Collection.getField(message.data, "body"));
                var username = Collection.getField(message.data, "user");
                var userId = Collection.getField(message.data, "user_id");
                AppendMessage(messageList, scrollviewer, username, userId.ParseToInteger(), newMessage);
            }
        }

        public void AddReceivedMessage(RichTextBox messageList, ScrollViewer scrollviewer, string channel, string messageData)
        {
            var m = jsonSerializer.Deserialize<MessageData>(messageData);
            var messageBody = jsonSerializer.Deserialize<MessageBody>(m.body);
            AppendMessage(messageList, scrollviewer, m.name, m.GetUserId(), messageBody);
        }

        private int lastUserThatInserted = 0;

        private void AppendMessage(RichTextBox messageList, ScrollViewer scrollViewer, string username, int userId, MessageBody message)
        {
            messageList.Dispatcher.Invoke(new Action(() =>
            {
                if (!message.solution.IsNullOrEmpty())
                {
                    if (lastUserThatInserted != userId)
                    {
                        var userMessageParagraph = new Paragraph { KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0) };
                        userMessageParagraph.Inlines.Add(new Bold(new Run(username + ":")));
                        messageList.Document.Blocks.Add(userMessageParagraph);
                    }
                    var codeClipboardData = new CodeClipboardData
                    {
                        message = message.message,
                        solution = message.solution,
                        document = message.document,
                        line = message.line,
                        column = message.column,
                        programmingLanguage = message.programmingLanguage
                    };
                    var syntaxBlock = syntaxBlockUIContainerFactory.Get(codeClipboardData);
                    messageList.Document.Blocks.Add(syntaxBlock);
                }
                else
                {
                    var userMessageParagraph = new Paragraph { KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0) };

                    var lineStarter = lastUserThatInserted != userId ? username + ":" : "";
                    userMessageParagraph.Inlines.Add(new Bold(new Run(lineStarter)));

                    userMessageParagraph.Inlines.Add(new Run(message.message));
                    messageList.Document.Blocks.Add(userMessageParagraph);
                }
                lastUserThatInserted = userId;
            }));
            messageList.Dispatcher.Invoke(new Action(scrollViewer.ScrollToBottom));
        }
    }
}