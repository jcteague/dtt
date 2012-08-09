using System.Collections.Concurrent;
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

        public ChatRoomsControlService(IProvideUser userProvider, ISendHttpRequests httpClient, IProvideConfiguration<ServerConfiguration> configuration, ICreateClipboardArguments clipboardArgumentsFactory, IHandleClipboardEvents clipboardEvents, IStoreClipboardData clipboardStorage)
        {
            this.userProvider = userProvider;
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.clipboardArgumentsFactory = clipboardArgumentsFactory;
            this.clipboardEvents = clipboardEvents;
            this.clipboardStorage = clipboardStorage;
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
            var txt = dte.ActiveDocument.Object() as TextDocument;
            if (txt.IsNull()) return;
            var selection = txt.Selection;

            var clipboardArgs = clipboardArgumentsFactory.Get(dte.Solution.FullName, dte.ActiveDocument.FullName,
                                                              selection.Text, selection.CurrentLine);

            clipboardStorage.Store(clipboardArgs);
            clipboardEvents.OnClipboardChanged(source, clipboardArgs);
        }
    }
}