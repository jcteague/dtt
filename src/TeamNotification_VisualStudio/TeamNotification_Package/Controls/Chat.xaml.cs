﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using AvenidaSoftware.TeamNotification_Package.Controls;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using SocketIOClient.Messages;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Models.UI;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Clipboard;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Http;
using TeamNotification_Library.Models;
using TeamNotification_Library.Service.LocalSystem;
using TeamNotification_Library.Service.Logging;
using TeamNotification_Library.Service.Providers;
using DataObject = System.Windows.DataObject;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Process = System.Diagnostics.Process;
using UserControl = System.Windows.Controls.UserControl;


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
        private ILog logger;

        /* We should not depend on this global state */
        private IStoreGlobalState applicationGlobalState;

        //We should make our own implementation in the future
        private TaskbarNotifierWindow taskbarNotifierWindow;

        private IHandleCodePaste codePasteEvents;
        private IHandleToolWindowEvents toolWindowEvents;
        private IHandleUserAccountEvents userAccountEvents;
        private IHandleSocketIOEvents socketIOEvents;
        private IHandleChatEvents chatEvents;

        private IHandleMixedEditorEvents mixedEditorEvents;
        private IHandleUIEvents userInterfaceEvents;

        private Dictionary<string, TableRowGroup> messagesList;
        private IProvideUser userProvider;
        private string roomId { get; set; }
        private string currentChannel { get; set; }

        private bool chatIsEnabled;
        private List<string> subscribedChannels;

        private IShowCode codeEditor;

        public Chat(IProvideUser userProvider, IListenToMessages<Action<string, string>> messageListener, IServiceChatRoomsControl chatRoomControlService, IStoreGlobalState applicationGlobalState, ICreateDteHandler dteHandlerCreator, IStoreDTE dteStore, IHandleCodePaste codePasteEvents, IHandleToolWindowEvents toolWindowEvents, IHandleUserAccountEvents userAccountEvents, IHandleVisualStudioClipboard clipboardHandler, IHandleSocketIOEvents socketIOEvents, ILog logger, IHandleMixedEditorEvents mixedEditorEvents, IHandleUIEvents userInterfaceEvents)
        {
            chatIsEnabled = true;

            dteStore.dte = ((DTE)Package.GetGlobalService(typeof(DTE)));
            this.dteStore = dteStore;
            this.codePasteEvents = codePasteEvents;
            this.toolWindowEvents = toolWindowEvents;
            this.userAccountEvents = userAccountEvents;
            this.clipboardHandler = clipboardHandler;
            this.socketIOEvents = socketIOEvents;
            this.logger = logger;
            this.mixedEditorEvents = mixedEditorEvents;
            this.userInterfaceEvents = userInterfaceEvents;
            this.applicationGlobalState = applicationGlobalState;
            this.chatRoomControlService = chatRoomControlService;
            this.messageListener = messageListener;
            this.userProvider = userProvider;
            this.dteHandlerCreator = dteHandlerCreator;
            this.subscribedChannels = new List<string>();
            this.messagesList = new Dictionary<string, TableRowGroup>();

            taskbarNotifierWindow = new TaskbarNotifierWindow(dteStore.dte)
                                        {
                                            OpeningMilliseconds = 500,
                                            HidingMilliseconds = 500,
                                            StayOpenMilliseconds = 1000
                                        };
            InitializeComponent();
            
            var mce = new ModalCodeEditor { RefControl = this.mainGrid };
            codeEditor = mce;
            var collection = chatRoomControlService.GetCollection();
            var roomLinks = FormatRooms(collection.rooms);
            
            Resources.Add("rooms", roomLinks);
            if(roomLinks.Count > 0)
                comboRooms.SelectedIndex = 0;

//            messageTextBox.Document.Blocks.Clear();

            Loaded += (s, e) => chatRoomControlService.HandleDock(GetChatUIElements());

//            DataObject.RemovePastingHandler(messageTextBox, OnPaste);
//            DataObject.AddPastingHandler(messageTextBox, OnPaste);

            mixedEditorEvents.DataWasPasted += MixedEditorDataWasPasted;


            lastStamp = "";

            codePasteEvents.Clear();
            codePasteEvents.CodePasteWasClicked += PasteCode;

            toolWindowEvents.Clear();
            toolWindowEvents.ToolWindowWasDocked += OnToolWindowWasDocked;

            userAccountEvents.Clear();
            userAccountEvents.UserHasLogout += OnUserLogout;

            socketIOEvents.Clear();
            socketIOEvents.SocketWasDisconnected += (s, e) =>
                                                        {
                                                            var room = "chat " + e.RoomId;
                                                            subscribedChannels.Remove(room);
                                                            Dispatcher.Invoke(new Action(() => btnReconnect.Visibility = Visibility.Visible));
                                                        };
        }

        private void MixedEditorDataWasPasted(object sender, DataWasPasted e)
        {
            logger.TryOrLog(() => Dispatcher.BeginInvoke(new Action(() => chatRoomControlService.HandlePaste(messageTextBox, codeEditor, e))));
        }


        private void SetResourcesForPaste(object sender, ExecutedRoutedEventArgs e)
        {
            int a = 0;
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
//            logger.TryOrLog(() => Dispatcher.BeginInvoke(new Action(() => chatRoomControlService.HandlePaste(messageTextBox, codeEditor, e))));
        }

        private void OnToolWindowWasDocked(object sender, ToolWindowWasDocked toolWindowWasDocked)
        {
            chatRoomControlService.HandleDock(GetChatUIElements());
        }

        private void OnUserLogout(object sender, UserHasLogout eventArgs)
        {
            logger.TryOrLog(() =>
                                {
                                    chatIsEnabled = false;
                                    Content = Container.GetInstance<LoginControl>();                        
                                });
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
        private extern static IntPtr GetForegroundWindow();
        
        #region Events
        public void ChatMessageArrived(string channel, string payload)
        {
            if (channel == currentChannel)
            {
                logger.TryOrLog(() => {

                    var receivedMessage = chatRoomControlService.AddReceivedMessage(GetChatUIElements(), messageScroll, payload);

                    var activeWindow = GetForegroundWindow();//GetActiveWindow();
                    var mainWindowHandle = (IntPtr)dteStore.dte.MainWindow.HWnd;
                    if ((Convert.ToInt32(receivedMessage.user_id) != userProvider.GetUser().id) && (activeWindow != mainWindowHandle))
                    {
                        taskbarNotifierWindow.Dispatcher.Invoke(new Action(() =>{
                                                                                var msg = (receivedMessage.chatMessageBody.message.Length > 8)?receivedMessage.chatMessageBody.message.Remove(8)+"...":receivedMessage.chatMessageBody.message;
                                                                                taskbarNotifierWindow.NotifyContent.Clear();
                                                                                taskbarNotifierWindow.Show();
                                                                                taskbarNotifierWindow.NotifyContent.Add(new NotifyObject(msg, receivedMessage.name+" says:"));
                                                                                taskbarNotifierWindow.Notify();
                        }));
                    }
                });
            }
        }
        void SendMessageButtonClick(object sender, RoutedEventArgs e)
        {
            logger.TryOrLog(() => SendMessage());
        }

        private void CheckKeyboard(object sender, KeyEventArgs e)
        {
            if (!applicationGlobalState.IsEditingCode && e.Key == Key.Enter && !(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
            {
                logger.TryOrLog(() =>
                                    {
                                        SendMessage();
                                        e.Handled = true;
                                    });
            }
        }

        private void LogoutUser(object sender, RoutedEventArgs e)
        {
            logger.TryOrLog(() => chatRoomControlService.LogoutUser(sender));
        }

        #endregion

        private void SendMessage()
        {
            applicationGlobalState.IsEditingCode = false;
//            chatRoomControlService.SendMessage(messageTextBox, roomId);
            chatEvents.OnSendMessageRequested(this, new SendMessageWasRequested(messageTextBox.Text, messageTextBox.Resources, roomId));
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
            
            lblReconnecting.Visibility = Visibility.Hidden;
            btnReconnect.Visibility = Visibility.Hidden;
            messageListener.ListenOnChannel(currentChannel, ChatMessageArrived, this.OnReconnectAttemptCallback, this.OnReconnectCallback);
            subscribedChannels.Add(currentChannel);
        }

        private void OnReconnectAttemptCallback()
        {
            if (subscribedChannels.Contains(currentChannel))
                subscribedChannels.Remove(currentChannel);
            lblReconnecting.Dispatcher.Invoke(new Action(() =>{
                lblReconnecting.Visibility = Visibility.Visible;
                lblReconnecting.Opacity = 1.0;
            }));
        }
        private void OnReconnectCallback()
        {
            lblReconnecting.Dispatcher.Invoke(new Action(() =>{
                lblReconnecting.Visibility = Visibility.Hidden;
                Reconnect(null, null);
            }));
        }

        private void AddMessages(string currentRoomId)
        {
            logger.TryOrLog(() => {
                this.roomId = currentRoomId;
                chatRoomControlService.AddMessages(GetChatUIElements(), messageScroll, currentRoomId);                                    
            });
        }

        private ChatUIElements GetChatUIElements()
        {
            return new ChatUIElements()
            {
                OuterGridRowDefinition3 = outerGridRowDefinition3,
                Container = messagesContainer,
                MessagesTable = messagesTable,
//                MessageInput = messageTextBox,
                MessageTextBoxGrid = messageTextBoxGrid,
                MessageContainerBorder = messageContainerBorder,
                MessageTextBoxGridSplitter = messageTextBoxGridSplitter,
                MessageGridRowDefinition1 = messageGridRowDefinition1,
                MessageGridRowDefinition2 = messageGridRowDefinition2,
                MessageGridColumnDefinition1 = messageGridColumnDefinition1,
                MessageGridColumnDefinition2 = messageGridColumnDefinition2,
                SendMessageButton = btnSendMessageButton,
                StatusBar = dteStore.dte.StatusBar,
//                InputBox = messageTextBox,
                MessagesList = messagesList,
                LastStamp = lastStamp,
                ComboRooms = comboRooms,
                codeEditor = codeEditor
            };
        }
        
        private List<Collection.Link> FormatRooms(IEnumerable<Collection.Room> unformattedRoom)
        {
            if (unformattedRoom == null)
                return new List<Collection.Link>();
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

        private void Reconnect(object sender, RoutedEventArgs e)
        {
            if (chatIsEnabled)
            {
                var value = (Collection.Link) comboRooms.SelectedValue;
                this.ChangeRoom(value.rel);    
            }
        }

        private void ClearStatusBar(object sender, RoutedEventArgs e)
        {
            dteStore.dte.StatusBar.Text = "";
        }
        
        private string lastStamp;

        private void feedback_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}