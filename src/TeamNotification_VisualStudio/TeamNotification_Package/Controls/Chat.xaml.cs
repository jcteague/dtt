﻿using System;
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
using TeamNotification_Library.Service.Clipboard;
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
        private readonly IHandleVisualStudioClipboard clipboardHandler;
        readonly ICreateDteHandler dteHandlerCreator;
        readonly IListenToMessages<Action<string, string>> messageListener;
        private IStoreDTE dteStore;
        
        private IHandleCodePaste codePasteEvents;
        private IHandleToolWindowEvents toolWindowEvents;
        private IHandleUserAccountEvents userAccountEvents;

        private string roomId { get; set; }
        private string currentChannel { get; set; }

        private bool chatIsEnabled;
        private List<string> subscribedChannels;

        public Chat(IListenToMessages<Action<string, string>> messageListener, IServiceChatRoomsControl chatRoomControlService, IStoreGlobalState applicationGlobalState, ICreateDteHandler dteHandlerCreator, IStoreDTE dteStore, IHandleCodePaste codePasteEvents, IHandleToolWindowEvents toolWindowEvents, IHandleUserAccountEvents userAccountEvents, IHandleVisualStudioClipboard clipboardHandler)
        {
            chatIsEnabled = true;

            dteStore.dte = ((DTE)Package.GetGlobalService(typeof(DTE)));
            this.dteStore = dteStore;
            this.codePasteEvents = codePasteEvents;
            this.toolWindowEvents = toolWindowEvents;
            this.userAccountEvents = userAccountEvents;
            this.clipboardHandler = clipboardHandler;
            this.chatRoomControlService = chatRoomControlService;
            this.messageListener = messageListener;

            this.dteHandlerCreator = dteHandlerCreator;
            this.subscribedChannels = new List<string>();
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

            DataObject.RemovePastingHandler(messageTextBox, OnPaste);
            DataObject.AddPastingHandler(messageTextBox, OnPaste);

            codePasteEvents.CodePasteWasClicked += PasteCode;
            toolWindowEvents.ToolWindowWasDocked += OnToolWindowWasDocked;
            userAccountEvents.UserHasLogout += OnUserLogout;
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            chatRoomControlService.HandlePaste(messageTextBox, e);
        }

        private void OnToolWindowWasDocked(object sender, ToolWindowWasDocked toolWindowWasDocked)
        {
            chatRoomControlService.HandleDock(GetChatUIElements());
        }

        private void OnUserLogout(object sender, UserHasLogout eventArgs)
        {
            chatIsEnabled = false;
            Content = Container.GetInstance<LoginControl>();
        }

        #region Win32_Clipboard

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            var hwndSource = PresentationSource.CurrentSources.Cast<HwndSource>().First();
            clipboardHandler.SetUpFor(hwndSource);
        }
        
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

        private void LogoutUser(object sender, RoutedEventArgs e)
        {
            chatRoomControlService.LogoutUser(sender);
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

            if (dteHandler != null && dteHandler.CurrentSolution.IsOpen && dteHandler.CurrentSolution.FileName == message.Solution)
            {
                var document = dteHandler.OpenFile(message.Project, message.Document);
                if (document != null)
                {
                    var originalText = document.TextDocument.CreateEditPoint().GetText(document.TextDocument.EndPoint);
                    var pasteResponse = AskingPaste.Show(document, originalText, message.Message, message.Line);
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

            if (!subscribedChannels.Contains(currentChannel))
            {
                messageListener.ListenOnChannel(currentChannel, ChatMessageArrived);
                subscribedChannels.Add(currentChannel);
            }
        }

        private void AddMessages(string currentRoomId)
        {
            this.roomId = currentRoomId;
            chatRoomControlService.AddMessages(GetChatUIElements(), messageScroll, currentRoomId);
        }

        private ChatUIElements GetChatUIElements()
        {
            return new ChatUIElements
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
                StatusBar = dteStore.dte.StatusBar
            };
        }
        
        private List<Collection.Link> FormatRooms(IEnumerable<Collection.Room> unformattedRoom)
        {
            return unformattedRoom.Select(room => new Collection.Link {name = Collection.getField(room.data, "name"), rel = Collection.getField(room.data,"id")}).ToList();
        }
        
        private void OnRoomSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chatIsEnabled)
            {
                var roomData = (Collection.Link)e.AddedItems[0];
                this.ChangeRoom(roomData.rel);    
            }
        }

        private void ClearStatusBar(object sender, RoutedEventArgs e)
        {
            dteStore.dte.StatusBar.Text = "";
        }
    }
}