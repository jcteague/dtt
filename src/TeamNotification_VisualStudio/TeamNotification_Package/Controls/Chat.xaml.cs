using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using AurelienRibon.Ui.SyntaxHighlightBox;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.Providers;
using Brushes = System.Drawing.Brushes;
using Clipboard = System.Windows.Clipboard;
using DataFormats = System.Windows.DataFormats;
using DataObject = System.Windows.DataObject;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using Pen = System.Windows.Media.Pen;
using TextBox = System.Windows.Controls.TextBox;
using UserControl = System.Windows.Controls.UserControl;

namespace AvenidaSoftware.TeamNotification_Package
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : UserControl
    {
        readonly IServiceChatRoomsControl chatRoomControlService;
        readonly IListenToMessages messageListener;
        readonly ISerializeJSON serializeJson;
        private string roomId { get; set; }
        private string currentChannel { get; set; }
        private List<string> subscribedChannels;
        private FlowDocument myFlowDoc;

        public Chat(IListenToMessages messageListener, IServiceChatRoomsControl chatRoomControlService, ISerializeJSON serializeJson)
        {
            this.chatRoomControlService = chatRoomControlService;
            this.messageListener = messageListener;
            this.serializeJson = serializeJson;
            this.subscribedChannels = new List<string>();
            InitializeComponent();
            var collection = chatRoomControlService.GetCollection();
            var roomLinks = formatRooms(collection.rooms);
            myFlowDoc = new FlowDocument();
            messageList.Document = myFlowDoc;
          //  var conn = connectionProvider.Get();

            Resources.Add("rooms", roomLinks);
            if(roomLinks.Count > 0)
                lstRooms.SelectedIndex = 0;

            DataObject.AddPastingHandler(messageTextBox, new DataObjectPastingEventHandler(OnPaste));
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            //chatRoomControlService.HandlePaste(messageTextBox, e);
        }

        #region Win32_Clipboard

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            var hwndSource = PresentationSource.CurrentSources.Cast<HwndSource>().First();

            if (hwndSource.IsNotNull())
            {
                installedHandle = hwndSource.Handle;
                viewerHandle = SetClipboardViewer(installedHandle);
                hwndSource.AddHook(new HwndSourceHook(this.hwndSourceHook));
            }

        }

        // TODO: Must do this for deregistration? Where?
//        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
//        {
//            ChangeClipboardChain(this.installedHandle, this.viewerHandle);
//            int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
//            e.Cancel = error != 0;
//
//            base.OnClosing(e);
//        }
//
//        protected override void OnClosed(EventArgs e)
//        {
//            this.viewerHandle = IntPtr.Zero;
//            this.installedHandle = IntPtr.Zero;
//            base.OnClosed(e);
//        }

        IntPtr hwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_CHANGECBCHAIN:
                    this.viewerHandle = lParam;
                    if (this.viewerHandle != IntPtr.Zero)
                    {
                        SendMessage(this.viewerHandle, msg, wParam, lParam);
                    }

                    break;

                case WM_DRAWCLIPBOARD:
                    var dte = (DTE)Package.GetGlobalService(typeof(DTE));
                    chatRoomControlService.UpdateClipboard(this, dte);

                    if (this.viewerHandle != IntPtr.Zero)
                    {
                        SendMessage(this.viewerHandle, msg, wParam, lParam);
                    }

                    break;
            }
            return IntPtr.Zero;
        }

        private void OnClipboardChanged(EventArgs clipChange)
        {
            try
            {
                if (Clipboard.ContainsData(DataFormats.Text))
                {
//                    ImageSource = (System.Windows.Interop.InteropBitmap)Clipboard.GetData(DataFormats.Bitmap);
                    Debug.WriteLine("Clipboard has changed and event has been raised");
                    
                }
            }
            catch
            {
                // Details: http://blogs.microsoft.co.il/blogs/tamir/archive/2007/10/24/clipboard-setdata-getdata-troubles-with-vpc-and-ts.aspx 
            }
        }

//        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
//        {
//            Microsoft.Win32.SaveFileDialog f = new Microsoft.Win32.SaveFileDialog();
//            f.DefaultExt = ".jpg";
//            f.Filter = "Jpeg images (.jpg)|*.jpg";
//            if (f.ShowDialog() == true)
//            {
//                using (FileStream fs = new FileStream(f.FileName, FileMode.Create, FileAccess.Write))
//                {
//                    JpegBitmapEncoder enc = new JpegBitmapEncoder();
//                    enc.Frames.Add(BitmapFrame.Create(ImageSource));
//                    enc.Save(fs);
//                    fs.Close();
//                    fs.Dispose();
//                }
//            }
//
//        }
//
//        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
//        {
//            e.CanExecute = ImageSource != default(BitmapSource);
//        }

        IntPtr viewerHandle = IntPtr.Zero;
        IntPtr installedHandle = IntPtr.Zero;

        const int WM_DRAWCLIPBOARD = 0x308;
        const int WM_CHANGECBCHAIN = 0x30D;
        
        [DllImport("user32.dll")]
        private extern static IntPtr SetClipboardViewer(IntPtr hWnd);
        [DllImport("user32.dll")]
        private extern static int ChangeClipboardChain(IntPtr hWnd, IntPtr hWndNext);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private extern static int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern int GetForegroundWindow();

        #endregion


        #region Events
        public void ChatMessageArrived(string channel, string payload)
        {
            // Here we should handle how to display the message formatted
            if (channel == currentChannel)
            {
                var m = serializeJson.Deserialize<MessageData>(payload);
                var messageBody = serializeJson.Deserialize<MessageBody>(m.body);
                AppendMessage(m.name, messageBody.message);
            }
        }
        void SendMessageButtonClick(object sender, RoutedEventArgs e)
        {
            this.SendMessage();
        }

        private void CheckKeyboard(object sender, KeyEventArgs e)
        {
          if (e.Key == Key.Enter)
                this.SendMessage();
        }
        #endregion

        private void SendMessage()
        {
            chatRoomControlService.SendMessage(messageTextBox, roomId);
        }

        ///Todo
        /// Make the text selectionable by adding something like this bellow
        /// <TextBox Background="Transparent" BorderThickness="0" Text="{Binding Text}" IsReadOnly="True" TextWrapping="Wrap"/>
        private void AppendMessage(string username, string message)
        {
            //messageList.Dispatcher.Invoke((MethodInvoker)(() => messageList.Children.Add(new Label { Content = username + ": " + message, Margin = new Thickness(5.0, 0.0, 0.0, 5.0) })));
            messageList.Dispatcher.Invoke((MethodInvoker) (() =>{
                if (message == "Code!")
                {
                    var syntaxHighlightBox = new SyntaxHighlightBox { Text = "var foo = \"sadfasf\"\nsdfguinsdgiunjsdgouinsgdoiunsgduionsdgoinsdg;\ndefoguinsweoginsdg", CurrentHighlighter = HighlighterManager.Instance.Highlighters["cSharp"] };
                    myFlowDoc.Blocks.Add(new BlockUIContainer(syntaxHighlightBox));
                }
                else
                {
                    var userMessageParagraph = new Paragraph { KeepTogether = true, LineHeight = 1.0, Margin = new Thickness(0, 0, 0, 0) };
                    userMessageParagraph.Inlines.Add(new Bold(new Run(username + ": ")));
                    userMessageParagraph.Inlines.Add(new Run(message));
                    myFlowDoc.Blocks.Add(userMessageParagraph);
                }
            }));
            messageList.Dispatcher.Invoke((MethodInvoker)(() => scrollViewer1.ScrollToBottom()));
        }

        private void ChangeRoom(string newRoomId)
        {
            currentChannel = "chat " + newRoomId;
            
            if (!subscribedChannels.Contains(currentChannel))
            {
                messageListener.ListenOnChannel(currentChannel, ChatMessageArrived);
                subscribedChannels.Add(currentChannel);
            }
            //messageList.Dispatcher.Invoke((MethodInvoker) (() => messageList.Text = "" ));
            AddMessages(newRoomId);
        }

        private void AddMessages(string currentRoomId)
        {
            this.roomId = currentRoomId;
            var collection = chatRoomControlService.GetMessagesCollection(this.roomId);
            foreach (var message in collection.messages)
            {
                var newMessage = Collection.getField(message.data, "body");
                var username = Collection.getField(message.data, "user");
                AppendMessage(username,newMessage);
            }
        }
        private List<Collection.Link> formatRooms(IEnumerable<Collection.Room> unformattedRoom)
        {
            return unformattedRoom.Select(room => new Collection.Link {name = Collection.getField(room.data, "name"), rel = Collection.getField(room.data,"id")}).ToList();
        }
        private void OnRoomSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var roomData = (Collection.Link) e.AddedItems[0];
            this.ChangeRoom(roomData.rel);
        }
    }
}