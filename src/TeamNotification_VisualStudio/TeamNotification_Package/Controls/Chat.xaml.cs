using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Factories.UI;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.LocalSystem;
using Brushes = System.Drawing.Brushes;
using Application = System.Windows.Application;
using DataObject = System.Windows.DataObject;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Label = System.Windows.Controls.Label;
using Pen = System.Windows.Media.Pen;
using UserControl = System.Windows.Controls.UserControl;
using ProgrammingLanguages = TeamNotification_Library.Configuration.Globals.ProgrammingLanguages;


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
        private ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory;
        
        private string roomId { get; set; }
        private string currentChannel { get; set; }
        private List<string> subscribedChannels;

        public Chat(IListenToMessages messageListener, IServiceChatRoomsControl chatRoomControlService, ISerializeJSON serializeJson, IStoreGlobalState applicationGlobalState, ICreateSyntaxBlockUIInstances syntaxBlockUIContainerFactory)
        {
            this.chatRoomControlService = chatRoomControlService;
            this.messageListener = messageListener;
            this.serializeJson = serializeJson;
            this.syntaxBlockUIContainerFactory = syntaxBlockUIContainerFactory;

            this.subscribedChannels = new List<string>();
            InitializeComponent();
            var collection = chatRoomControlService.GetCollection();
            var roomLinks = formatRooms(collection.rooms);
            messageList.Document = new FlowDocument();
            Application.Current.Activated += (source, e) => applicationGlobalState.Active = true;
            Application.Current.Deactivated += (source, e) => applicationGlobalState.Active = false;

            Resources.Add("rooms", roomLinks);
            if(roomLinks.Count > 0)
                lstRooms.SelectedIndex = 0;

            messageTextBox.Document.Blocks.Clear();
            DataObject.AddPastingHandler(messageTextBox, OnPaste);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            chatRoomControlService.HandlePaste(messageTextBox, e);
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
                hwndSource.AddHook(hwndSourceHook);
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
                    viewerHandle = lParam;
                    if (viewerHandle != IntPtr.Zero)
                    {
                        SendMessage(viewerHandle, msg, wParam, lParam);
                    }

                    break;

                case WM_DRAWCLIPBOARD:
                    var dte = (DTE)Package.GetGlobalService(typeof(DTE));
                    chatRoomControlService.UpdateClipboard(this, dte);

                    if (viewerHandle != IntPtr.Zero)
                    {
                        SendMessage(viewerHandle, msg, wParam, lParam);
                    }

                    break;
            }
            return IntPtr.Zero;
        }

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
                chatRoomControlService.AddReceivedMessage(messageList, scrollViewer1, channel, payload);
            }
        }
        void SendMessageButtonClick(object sender, RoutedEventArgs e)
        {
            this.SendMessage();
        }

        private void CheckKeyboard(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
            {
                this.SendMessage();
                e.Handled = true;
            }
        }

        #endregion

        private int lastUserThatInserted = -1;

        private void SendMessage()
        {
            chatRoomControlService.SendMessage(messageTextBox, roomId);
        }

        ///Todo
        /// Make the text selectionable by adding something like this bellow
        /// <TextBox Background="Transparent" BorderThickness="0" Text="{Binding Text}" IsReadOnly="True" TextWrapping="Wrap"/>

        private void ChangeRoom(string newRoomId)
        {
            lastUserThatInserted = -1;
            currentChannel = "chat " + newRoomId;
            
            if (!subscribedChannels.Contains(currentChannel))
            {
                messageListener.ListenOnChannel(currentChannel, ChatMessageArrived);
                subscribedChannels.Add(currentChannel);
            }

            // TODO: Find the way to be able to clear the Document with Document.Clear. SyntaxHighlighter has non-serializable properties
            chatRoomControlService.ClearRichTextBox(messageList);
            
            AddMessages(newRoomId);
        }

        private void AddMessages(string currentRoomId)
        {
            this.roomId = currentRoomId;
            chatRoomControlService.AddMessages(messageList, scrollViewer1, currentRoomId);
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