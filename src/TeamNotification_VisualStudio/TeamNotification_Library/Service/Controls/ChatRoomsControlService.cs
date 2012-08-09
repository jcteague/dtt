using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Controls;
using EnvDTE;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Providers;
using System.Collections.Generic;

namespace TeamNotification_Library.Service.Controls
{
    public class ChatRoomsControlService : IServiceChatRoomsControl
    {
        private IProvideUser userProvider;
        private ISendHttpRequests httpClient;
        private IProvideConfiguration<ServerConfiguration> configuration;
        private ICreateClipboardArguments clipboardArgumentsFactory;
        private IHandleClipboardEvents clipboardEvents;
        private IStoreClipboardData clipboardStorage;
        private ISerializeJSON serializer;
        private readonly ISendChatMessages messageSender;
        private ICreateChatMessageData chatMessageDataFactory;

        public ChatRoomsControlService(IProvideUser userProvider, ISendHttpRequests httpClient, IProvideConfiguration<ServerConfiguration> configuration, ICreateClipboardArguments clipboardArgumentsFactory, IHandleClipboardEvents clipboardEvents, IStoreClipboardData clipboardStorage, ISendChatMessages messageSender, ISerializeJSON serializer, ICreateChatMessageData chatMessageDataFactory)
        {
            this.userProvider = userProvider;
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.clipboardArgumentsFactory = clipboardArgumentsFactory;
            this.clipboardEvents = clipboardEvents;
            this.clipboardStorage = clipboardStorage;
            this.messageSender = messageSender;
            this.serializer = serializer;
            this.chatMessageDataFactory = chatMessageDataFactory;
            this.configuration = configuration;
        }

        public Collection GetMessagesCollection(string roomId)
        {
            var uri = configuration.Get().HREF + "room/" + roomId + "/messages";
            var c = httpClient.Get<Collection>(uri).Result;
            return c;
        }

        public Collection GetCollection()
        {
            var user = userProvider.GetUser();
            var uri = configuration.Get().HREF +"user/"+ user.id;
            var c = httpClient.Get<Collection>(uri).Result;
            return c;
        }

        public void UpdateClipboard(object source, DTE dte)
        {
            // IF we are copying text that is relevant solution code then store the CodeClipboardDataObject
            // ELSE just store a PlainClipboardDataObject
            var txt = dte.ActiveDocument.Object() as TextDocument;
            if (txt.IsNull()) return;
            var selection = txt.Selection;

            var clipboardArgs = clipboardArgumentsFactory.Get(dte.Solution.FullName, dte.ActiveDocument.FullName,
                                                              selection.Text, selection.CurrentLine);

            clipboardStorage.Store(clipboardArgs);
            clipboardEvents.OnClipboardChanged(source, clipboardArgs);
        }

        public void HandlePaste(TextBox textBox, DataObjectPastingEventArgs dataObjectPastingEventArgs)
        {
            // IF Clipboard Data is code then do use the clipboard data to send to the backend
            // ELSE use the normal text on the clipboard
            //            messageTextBox.Text = clipboardStorage.Data.Text;
            
            
//            var clipboard = serializer.Deserialize<ClipboardHasChanged>(Clipboard.GetText());
//
////             TODO: Code should not be modifiable and pretty printed
//            //messageTextBox.Text = "Pasted Code";
//            textBox.Text = clipboard.message;
            textBox.Text = clipboardStorage.Get<PlainClipboardData>().message;
            dataObjectPastingEventArgs.CancelCommand();
        }

        public void SendMessage(TextBox textBox, string roomId)
        {
            ChatMessageData message;
            if (HasClipboardData)
            {
                message = GetClipboardMessage();
            }
            else
            {
                message = chatMessageDataFactory.Get(textBox.Text);
            }
            messageSender.SendMessage(message, roomId);
            textBox.Text = "";
            HasClipboardData = false;
        }

        private ChatMessageData GetClipboardMessage()
        {
            if (clipboardStorage.IsCode())
                return clipboardStorage.Get<CodeClipboardData>();
            
            return clipboardStorage.Get<PlainClipboardData>();
        }

        public bool HasClipboardData { get; set; }
    }
}