using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AurelienRibon.Ui.SyntaxHighlightBox;
using EnvDTE;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Factories;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Service.LocalSystem;
using TeamNotification_Library.Service.Providers;
using TextRange = System.Windows.Documents.TextRange;

namespace TeamNotification_Library.Service.Controls
{
    public class ChatRoomsControlService : IServiceChatRoomsControl
    {
        private IProvideUser userProvider;
        private ISendHttpRequests httpClient;
        private IProvideConfiguration<ServerConfiguration> configuration;
        private IStoreClipboardData clipboardStorage;
        readonly IStoreGlobalState applicationGlobalState;

        private readonly ISendChatMessages messageSender;
        private ICreateChatMessageData chatMessageDataFactory;
        private IHandleSystemClipboard systemClipboardHandler;

        public ChatRoomsControlService(IProvideUser userProvider, ISendHttpRequests httpClient, IProvideConfiguration<ServerConfiguration> configuration, IStoreClipboardData clipboardStorage, ISendChatMessages messageSender, ICreateChatMessageData chatMessageDataFactory, IStoreGlobalState applicationGlobalState, IHandleSystemClipboard systemClipboardHandler)
        {
            this.userProvider = userProvider;
            this.httpClient = httpClient;
            this.clipboardStorage = clipboardStorage;
            this.messageSender = messageSender;
            this.chatMessageDataFactory = chatMessageDataFactory;
            this.applicationGlobalState = applicationGlobalState;
            this.systemClipboardHandler = systemClipboardHandler;
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
                var txt = dte.ActiveDocument.Object() as TextDocument;
                if (txt.IsNull()) return;
                var selection = txt.Selection;
                var clipboard = new CodeClipboardData
                {
                    solution = dte.Solution.FullName,
                    document = dte.ActiveDocument.FullName,
                    message = selection.Text,
                    line = selection.CurrentLine
                };

                clipboardStorage.Store(clipboard);
                Debug.WriteLine("CODE is in the clipboard");
            }
            else
            {
                var clipboard = new PlainClipboardData {message = systemClipboardHandler.GetText(true)};
                clipboardStorage.Store(clipboard);
                Debug.WriteLine("PLAIN is in the clipboard");
            }
            HasCopied = true;
        }

        private bool HasCopied { get; set; }

        public void HandlePaste(RichTextBox textBox, DataObjectPastingEventArgs dataObjectPastingEventArgs)
        {
            textBox.Document.Blocks.Clear();
            
            if(clipboardStorage.IsCode)
            {
                textBox.Document.Blocks.Add(new BlockUIContainer(new SyntaxHighlightBox { Text = clipboardStorage.Get<PlainClipboardData>().message, CurrentHighlighter = HighlighterManager.Instance.Highlighters["cSharp"] }));;
                HasClipboardData = true;
            }
            else
            {
                textBox.Document.Blocks.Add(new Paragraph(new Run(clipboardStorage.Get<PlainClipboardData>().message)));
            }
            dataObjectPastingEventArgs.CancelCommand();
        }

        public void SendMessage(RichTextBox textBox, string roomId)
        {
            if (HasClipboardData)
            {
                if (clipboardStorage.IsCode)
                    messageSender.SendMessage(clipboardStorage.Get<CodeClipboardData>(), roomId);
                else
                    messageSender.SendMessage(clipboardStorage.Get<PlainClipboardData>(), roomId);

                HasClipboardData = false;
            }
            else
            {
                string str = new TextRange(textBox.Document.ContentStart, textBox.Document.ContentEnd).Text;
                str = str.Remove(str.Length - 2,2);
                if (str != "")
                {
                    messageSender.SendMessage(chatMessageDataFactory.Get(str), roomId);
                }
            }
            ClearRichTextBox(textBox);
        }

        public bool HasClipboardData { get; set; }

        public void ClearRichTextBox(RichTextBox textBox)
        {
            // TODO: Find the way to be able to clear the Document with Document.Clear. SyntaxHighlighter has non-serializable properties
            textBox.Document = new FlowDocument();
        }
    }
}