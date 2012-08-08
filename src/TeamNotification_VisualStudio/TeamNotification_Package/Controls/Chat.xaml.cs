using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Models;
using Clipboard = System.Windows.Clipboard;
using DataFormats = System.Windows.DataFormats;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using TextSelection = System.Windows.Documents.TextSelection;
using UserControl = System.Windows.Controls.UserControl;

namespace AvenidaSoftware.TeamNotification_Package
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    public partial class Chat : UserControl
    {
        readonly ISendChatMessages messageSender;
        readonly IServiceChatRoomsControl chatRoomControlService;
        readonly IListenToMessages messageListener;
        readonly ISerializeJSON serializeJson;

        readonly IHandleClipboardEvents clipboardEvents;

        private string roomId { get; set; }
        private string currentChannel { get; set; }
        private List<string> subscribedChannels; 

        public Chat(ISendChatMessages messageSender, IListenToMessages messageListener, IRedisConnection connection, IServiceChatRoomsControl chatRoomControlService, ISerializeJSON serializeJson, IHandleClipboardEvents clipboardEvents)
        {
            this.chatRoomControlService = chatRoomControlService;
            this.messageSender = messageSender;
            this.messageListener = messageListener;
            this.serializeJson = serializeJson;
            this.clipboardEvents = clipboardEvents;
            this.subscribedChannels = new List<string>();

            InitializeComponent();
            connection.Open();
            var collection = chatRoomControlService.GetCollection();
            var roomLinks = this.formatRooms(collection.rooms);

            Resources.Add("rooms", roomLinks);
            if(roomLinks.Count > 0)
                lstRooms.SelectedIndex = 0;

            clipboardEvents.ClipboardHasChanged += (source, e) =>
                                                       {
                                                           Debug.WriteLine("{0} -> {1} -> {2}: {3}", e.Solution, e.Document, e.Line, e.Text);
                                                           Debug.WriteLineIf(Clipboard.ContainsData(DataFormats.Text),
                                                                             Clipboard.GetText());
                                                       };
        }

        #region Win32_Clipboard

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            var hwndSource = PresentationSource.CurrentSources.Cast<HwndSource>().First();
            if (hwndSource != null)
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
//                    EventArgs clipChange = new EventArgs();
//                    OnClipboardChanged(clipChange);
                    var dte = (DTE) Package.GetGlobalService(typeof(DTE));
                    var txt = dte.ActiveDocument.Object() as TextDocument;
                    if (txt.IsNotNull())
                    {
                        var selection = txt.Selection;

                        var clipboardArgs = new ClipboardHasChanged
                        {
                            Solution = dte.Solution.FullName,
                            Document = dte.ActiveDocument.FullName,
                            Text = selection.Text,
                            Line = selection.CurrentLine
                        };
                        clipboardEvents.OnClipboardChanged(this, clipboardArgs);    
                    }

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

        #endregion


        #region Events
        public void ChatMessageArrived(string channel, string payload)
        {
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
            var message = messageTextBox.Text;
            messageTextBox.Text = "";
            messageSender.SendMessage(message, this.roomId);
        }

        private void AppendMessage(string username, string message)
        {
            messageList.Dispatcher.Invoke((MethodInvoker)(() => messageList.Children.Add(new Label { Content = username + " : " + message })));
        }

        private void ChangeRoom(string newRoomId)
        {
            currentChannel = "chat " + newRoomId;
            
            if (!subscribedChannels.Contains(currentChannel))
            {
                messageListener.ListenOnChannel(currentChannel, ChatMessageArrived);
                subscribedChannels.Add(currentChannel);
            }
            messageList.Dispatcher.Invoke((MethodInvoker) (() => messageList.Children.Clear()));
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