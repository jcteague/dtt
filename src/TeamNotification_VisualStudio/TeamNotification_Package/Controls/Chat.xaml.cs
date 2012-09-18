using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using AurelienRibon.Ui.SyntaxHighlightBox;
using AvenidaSoftware.TeamNotification_Package.Controls;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Content;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.LocalSystem;
using Brushes = System.Drawing.Brushes;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using DataObject = System.Windows.DataObject;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using MessageBoxOptions = System.Windows.MessageBoxOptions;
using Pen = System.Windows.Media.Pen;
using UserControl = System.Windows.Controls.UserControl;
using ProgrammingLanguages = TeamNotification_Library.Configuration.GlobalConstants.ProgrammingLanguages;



namespace AvenidaSoftware.TeamNotification_Package
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : UserControl
    {
        readonly IServiceChatRoomsControl chatRoomControlService;
        readonly ICreateDteHandler dteHandlerCreator;
        readonly IListenToMessages<Action<string, string>> messageListener;
        private IHandleCodePaste codePasteEvents;
        private IHandleToolWindowEvents toolWindowEvents;
		private Dictionary<string, TableRowGroup> messagesList;

        
        private string roomId { get; set; }
        private string currentChannel { get; set; }
        private List<string> subscribedChannels;
        private IStoreDTE dteStore;

        public Chat(IListenToMessages<Action<string, string>> messageListener, IServiceChatRoomsControl chatRoomControlService, IStoreGlobalState applicationGlobalState, ICreateDteHandler dteHandlerCreator, IStoreDTE dteStore, IHandleCodePaste codePasteEvents, IHandleToolWindowEvents toolWindowEvents)
        {
            dteStore.dte = ((DTE)Package.GetGlobalService(typeof(DTE)));
            this.dteStore = dteStore;
            this.codePasteEvents = codePasteEvents;
            this.toolWindowEvents = toolWindowEvents;
            this.chatRoomControlService = chatRoomControlService;
            this.messageListener = messageListener;

            this.dteHandlerCreator = dteHandlerCreator;
            this.subscribedChannels = new List<string>();
            this.messagesList = new Dictionary<string, TableRowGroup>();
            InitializeComponent();
            
            var collection = chatRoomControlService.GetCollection();
            var roomLinks = FormatRooms(collection.rooms);
            
            Application.Current.Activated += (source, e) => applicationGlobalState.Active = true;
            Application.Current.Deactivated += (source, e) => applicationGlobalState.Active = false;

            Resources.Add("rooms", roomLinks);
            if(roomLinks.Count > 0)
                comboRooms.SelectedIndex = 0;

            messageTextBox.Document.Blocks.Clear();

            Loaded += (s, e) => chatRoomControlService.HandleDock(GetChatUIElements());
            
            DataObject.AddPastingHandler(messageTextBox, OnPaste);
            lastStamp = "";
            codePasteEvents.CodePasteWasClicked += PasteCode;
            toolWindowEvents.ToolWindowWasDocked += OnToolWindowWasDocked;
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            chatRoomControlService.HandlePaste(messageTextBox, e);
        }

        private void OnToolWindowWasDocked(object sender, ToolWindowWasDocked toolWindowWasDocked)
        {
            chatRoomControlService.HandleDock(GetChatUIElements());
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

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public UInt32 cbSize; //The size of the structure in bytes.
            public IntPtr hwnd; //A Handle to the Window to be Flashed. The window can be either opened or minimized.
            public UInt32 dwFlags; //The Flash Status.
            public UInt32 uCount; // number of times to flash the window
            public UInt32 dwTimeout; //The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        #region Events
        public void ChatMessageArrived(string channel, string payload)
        {
            if (channel == currentChannel)
            {
                chatRoomControlService.AddReceivedMessage(GetChatUIElements(), messageScroll, payload);
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

        private void SendMessage()
        {
            chatRoomControlService.SendMessage(messageTextBox, roomId);
        }

        private void PasteCode(object sender, EventArgs args)
        {
            var message = GetMessageBodyFromLink(sender);
            var dteHandler = dteHandlerCreator.Get(dteStore);

            if (dteHandler != null && dteHandler.CurrentSolution.IsOpen && dteHandler.CurrentSolution.FileName == message.chatMessageBody.solution)
            {
                var document = dteHandler.OpenFile(message.chatMessageBody.project, message.chatMessageBody.document);
                if (document != null)
                {
                    var originalText = document.TextDocument.CreateEditPoint().GetText(document.TextDocument.EndPoint);
                    var pasteResponse = AskingPaste.Show(document, originalText, message.chatMessageBody.message, message.chatMessageBody.line);
                    if(pasteResponse.pasteOption == PasteOptions.Abort)
                    {
                        var textDocument = document.TextDocument;
                        var editPoint = textDocument.CreateEditPoint();
                        textDocument.Selection.SelectAll();
                        textDocument.Selection.Cut();
                        editPoint.MoveToLineAndOffset(1, 1);
                        editPoint.Insert(originalText);
                    }
                }else
                {
                    CustomMessageBox.Show("The specified file does not exist.");
                }
            }
            else
            {
                CustomMessageBox.Show("You need to have the appropriate solution open in order to paste the code.");
            }
        }

        private static ChatMessageModel GetMessageBodyFromLink(object sender)
        {
            return (ChatMessageModel)((Hyperlink)sender).CommandParameter;
        }

        private void ChangeRoom(string newRoomId)
        {
            currentChannel = "chat " + newRoomId;

            chatRoomControlService.ResetContainer(GetChatUIElements());
            AddMessages(newRoomId);

            if (subscribedChannels.Contains(currentChannel)) return;
            messageListener.ListenOnChannel(currentChannel, ChatMessageArrived);
            subscribedChannels.Add(currentChannel);
        }

        private void AddMessages(string currentRoomId)
        {
            this.roomId = currentRoomId;
            chatRoomControlService.AddMessages(GetChatUIElements(), messageScroll, currentRoomId);
        }

        private ChatUIElements GetChatUIElements()
        {
            return new ChatUIElements()
            {
                OuterGridRowDefinition3 = outerGridRowDefinition3,
                Container = messagesContainer,
                MessagesTable = messagesTable,
                MessageInput = messageTextBox,
                MessageTextBoxGrid = messageTextBoxGrid,
                MessageContainerBorder = messageContainerBorder,
                MessageTextBoxGridSplitter = messageTextBoxGridSplitter,
                MessageGridRowDefinition1 = messageGridRowDefinition1,
                MessageGridRowDefinition2 = messageGridRowDefinition2,
                MessageGridColumnDefinition1 = messageGridColumnDefinition1,
                MessageGridColumnDefinition2 = messageGridColumnDefinition2,
                SendMessageButton = btnSendMessageButton,
                StatusBar = dteStore.dte.StatusBar,
                InputBox = messageTextBox,
                MessagesList = messagesList,
                LastStamp = lastStamp,
                ComboRooms = comboRooms
            };
        }
        
        private List<Collection.Link> FormatRooms(IEnumerable<Collection.Room> unformattedRoom)
        {
            return unformattedRoom.Select(room => new Collection.Link {name = Collection.getField(room.data, "name"), rel = Collection.getField(room.data,"id")}).ToList();
        }
        
        private void OnRoomSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var roomData = (Collection.Link) e.AddedItems[0];
            this.ChangeRoom(roomData.rel);
        }

        private void ClearStatusBar(object sender, RoutedEventArgs e)
        {
            dteStore.dte.StatusBar.Text = "";
        }
        private string lastStamp;
    }
}