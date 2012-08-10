using System.Windows;
using System.Windows.Controls;
using EnvDTE;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.Providers;

namespace TeamNotification_Library.Service.Controls
{
    public class ChatRoomsControlService : IServiceChatRoomsControl
    {
        private IProvideUser userProvider;
        private ISendHttpRequests httpClient;
        private IProvideConfiguration<ServerConfiguration> configuration;
        private IStoreClipboardData clipboardStorage;
        private readonly ISendChatMessages messageSender;
        private ICreateChatMessageData chatMessageDataFactory;
		
        public ChatRoomsControlService(IProvideUser userProvider, ISendHttpRequests httpClient, IProvideConfiguration<ServerConfiguration> configuration, IStoreClipboardData clipboardStorage, ISendChatMessages messageSender, ICreateChatMessageData chatMessageDataFactory)
        {
            this.userProvider = userProvider;
            this.httpClient = httpClient;
            this.clipboardStorage = clipboardStorage;
            this.messageSender = messageSender;
            this.chatMessageDataFactory = chatMessageDataFactory;
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
            // IF we are copying text that is relevant solution code then store the CodeClipboardDataObject
            // ELSE just store a PlainClipboardDataObject
            var txt = dte.ActiveDocument.Object() as TextDocument;
            if (txt.IsNull()) return;
            var selection = txt.Selection;
//
//            var clipboardArgs = clipboardArgumentsFactory.Get(dte.Solution.FullName, dte.ActiveDocument.FullName,
//                                                              selection.Text, selection.CurrentLine);

            var clipboardArgs = new CodeClipboardData
                                    {
                                        solution = dte.Solution.FullName,
                                        document = dte.ActiveDocument.FullName,
                                        message = selection.Text,
                                        line = selection.CurrentLine
                                    };

            clipboardStorage.Store(clipboardArgs);
            HasClipboardData = true;
//            clipboardEvents.OnClipboardChanged(source, clipboardArgs);
        }

        public void HandlePaste(TextBox textBox, DataObjectPastingEventArgs dataObjectPastingEventArgs)
        {
            textBox.Text = clipboardStorage.Get<PlainClipboardData>().message;
            dataObjectPastingEventArgs.CancelCommand();
        }

        public void SendMessage(TextBox textBox, string roomId)
        {
            if (HasClipboardData)
            {
                if (clipboardStorage.IsCode)
                    messageSender.SendMessage(clipboardStorage.Get<CodeClipboardData>(), roomId);
                else
                    messageSender.SendMessage(clipboardStorage.Get<PlainClipboardData>(), roomId);
            }
            else
            {
                messageSender.SendMessage(chatMessageDataFactory.Get(textBox.Text), roomId);
            }
            textBox.Text = "";
            HasClipboardData = false;
        }

        public bool HasClipboardData { get; set; }
    }
}